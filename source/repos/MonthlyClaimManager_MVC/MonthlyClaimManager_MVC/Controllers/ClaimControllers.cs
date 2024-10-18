using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonthlyClaimManager.Hubs;
using MonthlyClaimManager.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MonthlyClaimManager.Controllers
{
    public class ClaimController : Controller
    {
        private static List<Claim> _claims = new List<Claim>(); // Simulating the database
        private readonly IHubContext<ClaimHub> _hubContext;
        private readonly ILogger<ClaimController> _logger; // Logger for logging errors

        public ClaimController(IHubContext<ClaimHub> hubContext, ILogger<ClaimController> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        // Display the lecturer claim submission form
        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile uploadedFile)
        {
            // Log any validation errors
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError(error.ErrorMessage); // Log errors instead of using Console.WriteLine
            }

            if (ModelState.IsValid)
            {
                // Calculate the claim amount
                claim.ClaimAmount = claim.HoursWorked * claim.HourlyRate;

                // Set ClaimStatus programmatically
                claim.ClaimStatus = "Pending";

                // Define the path where the file will be saved
                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Check if the uploads directory exists, if not, create it
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Handle file upload with validation
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    if (uploadedFile.Length > 10485760) // 10MB file size limit
                    {
                        ModelState.AddModelError(string.Empty, "The file size should not exceed 10MB.");
                        return View(claim);
                    }

                    var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                    var extension = Path.GetExtension(uploadedFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError(string.Empty, "Only .pdf, .docx, and .xlsx file types are allowed.");
                        return View(claim);
                    }

                    // Generate a unique filename to avoid collisions
                    var uniqueFileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                    var filePath = Path.Combine(uploadsPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(stream);
                    }

                    // Set DocumentPath here after file upload is successful
                    claim.DocumentPath = $"/uploads/{uniqueFileName}";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Please upload a supporting document.");
                    return View(claim);
                }

                // Add the claim to the list (simulating saving to a database)
                _claims.Add(claim);

                // Notify all clients about the new claim via SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveClaimUpdate");

                return RedirectToAction("LecturerClaims", new { lecturerId = claim.LecturerID });
            }

            return View(claim);
        }

        // View for coordinators and managers to approve/reject claims
        public IActionResult ClaimsList()
        {
            var pendingClaims = _claims.Where(c => c.ClaimStatus == "Pending").ToList();
            return View(_claims);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.LecturerID == id);
            if (claim != null)
            {
                claim.ClaimStatus = "Approved";

                // Notify all clients about the claim status change
                await _hubContext.Clients.All.SendAsync("ReceiveStatusUpdate");
            }

            return RedirectToAction("ClaimsList");
        }

        [HttpPost]
        public async Task<IActionResult> RejectClaim(int id)
        {
            var claim = _claims.FirstOrDefault(c => c.LecturerID == id);
            if (claim != null)
            {
                claim.ClaimStatus = "Rejected";

                // Notify all clients about the claim status change
                await _hubContext.Clients.All.SendAsync("ReceiveStatusUpdate");
            }

            return RedirectToAction("ClaimsList");
        }
        // Display a view where the lecturer can see their own claims
        public IActionResult LecturerClaims(int lecturerId)
        {
            var lecturerClaims = _claims.Where(c => c.LecturerID == lecturerId).ToList();
            return View(lecturerClaims);
        }

    }
}
