// Controllers/ClaimController.cs
using Claiming_System.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Claiming_System.Controllers
{
    public class ClaimController : Controller
    {
        private const string connectionString = "Server=localhost;Database=claimsystem;User=root;Password=Passw0rd;";

        // GET: Claim/Index
        public ActionResult Index()
        {
            var claims = LoadClaimsList();
            return View(claims);
        }

        // Register a new user
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO users (fullname, email, password) VALUES (@FullName, @Email, @Password)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", user.FullName);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);  // Use proper password hashing

                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }


        // Login action
        // Login action
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM users WHERE email = @Email AND password = @Password";  // Insecure: Implement password hashing in production
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);  // Use proper password hashing

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Authentication successful, redirect to the claim index
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            // Authentication failed
                            ModelState.AddModelError("", "Invalid login attempt.");
                        }
                    }
                }
            }
            return View();
        }


        // Submit a claim
        // Submit a claim
        [HttpPost]
        public ActionResult SubmitClaim(Claim claim)
        {
            if (ModelState.IsValid)
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO claims (lecturer_id, hours_worked, total_claim, status) " +
                                   "VALUES (@LecturerId, @HoursWorked, @TotalClaim, @Status)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LecturerId", claim.Id);  // Assuming LecturerId exists in claim
                        cmd.Parameters.AddWithValue("@HoursWorked", claim.HoursWorked);
                        cmd.Parameters.AddWithValue("@TotalClaim", claim.TotalClaim);
                        cmd.Parameters.AddWithValue("@Status", "Pending");  // Set initial status to Pending

                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(claim);
        }


        private List<Claim> LoadClaimsList()
        {
            var claims = new List<Claim>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT claims.id, lecturer.name AS LecturerName, lecturer.department AS LecturerDepartment, " +
                               "claims.hours_worked AS HoursWorked, claims.total_claim AS TotalClaim, claims.status AS Status " +
                               "FROM claims " +
                               "JOIN lecturer ON claims.lecturer_id = lecturer.id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            claims.Add(new Claim
                            {
                                Id = reader.GetInt32("id"),
                                LecturerName = reader.GetString("LecturerName"),
                                LecturerDepartment = reader.GetString("LecturerDepartment"),
                                HoursWorked = reader.GetDecimal("HoursWorked"),
                                TotalClaim = reader.GetDecimal("TotalClaim"),
                                Status = reader.GetString("Status")
                            });
                        }
                    }
                }
            }

            return claims;
        }

        [HttpPost]
        public ActionResult UploadDocument(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var filePath = Path.Combine(uploadDir, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Optionally, store the file path in the database
                // Example: Store filePath in the claims table for reference

                return RedirectToAction("Index");
            }
            return View();
        }

    }

}
