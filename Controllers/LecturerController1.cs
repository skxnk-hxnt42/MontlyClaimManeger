using Microsoft.AspNetCore.Mvc;
using Claiming_System.Models;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Claiming_System.Controllers
{
    public class LecturerController : Controller
    {
        private string connectionString = "server=localhost;database=claimsystem;uid=root;password=;";

        // Register Action
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Lecturer lecturer)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Insert into the User table first (plain text password)
                var cmdUser = new MySqlCommand("INSERT INTO users (email, password) VALUES (@Email, @Password); SELECT LAST_INSERT_ID();", conn);
                cmdUser.Parameters.AddWithValue("@Email", lecturer.Email);
                cmdUser.Parameters.AddWithValue("@Password", lecturer.Password); // No hashing, plain text password

                // Get the UserId from the inserted record
                var userId = Convert.ToInt32(cmdUser.ExecuteScalar());

                // Now, insert into the Lecturer table using the UserId
                var cmdLecturer = new MySqlCommand("INSERT INTO lecturer (name, department, email, user_id) VALUES (@Name, @Department, @Email, @UserId)", conn);
                cmdLecturer.Parameters.AddWithValue("@Name", lecturer.Name);
                cmdLecturer.Parameters.AddWithValue("@Department", lecturer.Department);
                cmdLecturer.Parameters.AddWithValue("@Email", lecturer.Email);
                cmdLecturer.Parameters.AddWithValue("@UserId", userId);

                int result = cmdLecturer.ExecuteNonQuery();

                if (result > 0)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to register lecturer.");
                    return View();
                }
            }
        }

        // Login Action
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM users WHERE email = @Email AND password = @Password", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    HttpContext.Session.SetString("Email", email);
                    return RedirectToAction("Dashboard");
                }
            }

            ModelState.AddModelError("", "Invalid login credentials.");
            return View();
        }

        // Dashboard Action
        public IActionResult Dashboard()
        {
            string email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            string userName = "";
            string lecturerEmail = email;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT name FROM lecturer WHERE email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userName = reader["name"].ToString();
                }
            }

            ViewData["UserName"] = userName;
            ViewData["LecturerEmail"] = lecturerEmail;
            ViewData["SuccessMessage"] = TempData["SuccessMessage"]; // Add this

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile supportingDocument)
        {
            // Step 1: Validate the claim data
            if (claim == null || claim.HoursWorked <= 0 || claim.HourlyRate <= 0 || string.IsNullOrWhiteSpace(claim.Month))
            {
                ModelState.AddModelError("", "Invalid claim data. Please check your inputs.");
                return View(claim);
            }

            string email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    // Step 2: Get Lecturer ID from the database
                    var cmdLecturerId = new MySqlCommand("SELECT id FROM lecturer WHERE email = @Email", conn);
                    cmdLecturerId.Parameters.AddWithValue("@Email", email);
                    var lecturerId = await cmdLecturerId.ExecuteScalarAsync();

                    if (lecturerId == null)
                    {
                        ModelState.AddModelError("", "Lecturer not found.");
                        return View(claim);
                    }

                    // Step 3: Calculate the total claim amount
                    decimal totalClaim = claim.HoursWorked * claim.HourlyRate;

                    // Step 4: Handle file upload
                    string filePath = null;
                    string fileName = null;
                    if (supportingDocument != null && supportingDocument.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                        Directory.CreateDirectory(uploadsFolder);  // Ensure directory exists
                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(supportingDocument.FileName);
                        filePath = Path.Combine(uploadsFolder, fileName);

                        // Save the file to the server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await supportingDocument.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No supporting document uploaded.");
                    }

                    // Step 5: Insert the claim into the claims table with the document details
                    string sqlQuery = @"
                INSERT INTO claims (lecturer_id, hours_worked, hourly_rate, total_claim, status, month, file_name, file_path) 
                VALUES (@LecturerId, @HoursWorked, @HourlyRate, @TotalClaim, @Status, @Month, @FileName, @FilePath)";

                    var cmd = new MySqlCommand(sqlQuery, conn);
                    cmd.Parameters.AddWithValue("@LecturerId", lecturerId);
                    cmd.Parameters.AddWithValue("@HoursWorked", claim.HoursWorked);
                    cmd.Parameters.AddWithValue("@HourlyRate", claim.HourlyRate);
                    cmd.Parameters.AddWithValue("@TotalClaim", totalClaim);
                    cmd.Parameters.AddWithValue("@Status", "Pending"); // Default status
                    cmd.Parameters.AddWithValue("@Month", claim.Month);
                    cmd.Parameters.AddWithValue("@FileName", fileName); // Pass file name
                    cmd.Parameters.AddWithValue("@FilePath", filePath); // Pass file path

                    int result = await cmd.ExecuteNonQueryAsync();

                    if (result > 0)
                    {
                        TempData["SuccessMessage"] = "Claim submitted successfully!";
                        return RedirectToAction("Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to submit claim. Please try again.");
                        Console.WriteLine("Failed to insert claim into database.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Step 6: Handle any errors that occur
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                Console.WriteLine("Exception: " + ex.ToString());
            }

            return View(claim); // Return the view with the error message
        }

    }
}




