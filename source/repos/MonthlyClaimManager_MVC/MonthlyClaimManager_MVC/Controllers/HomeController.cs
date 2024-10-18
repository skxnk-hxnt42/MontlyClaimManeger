using Microsoft.AspNetCore.Mvc;
using MonthlyClaimManager.Models;

namespace MonthlyClaimManager.Controllers
{
    public class HomeController : Controller
    {
        // Display the Index Page
        public IActionResult Index()
        {
            return View();
        }

        // Display Lecturer Login Page
        [HttpGet]
        public IActionResult LecturerLogin()
        {
            return View(new LoginModel());
        }

        // Handle Lecturer Login
        [HttpPost]
        public IActionResult LecturerLogin(LoginModel model)
        {
            if (model.Username == "lecturer" && model.Password == "lecturer")
            {
                // Redirect to Submit Claims Page
                return RedirectToAction("SubmitClaim", "Claim");
            }

            // Login failed, show error
            model.ErrorMessage = "Invalid username or password.";
            return View(model);
        }

        // Display Admin Login Page
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View(new LoginModel());
        }

        // Handle Admin Login
        [HttpPost]
        public IActionResult AdminLogin(LoginModel model)
        {
            if (model.Username == "admin" && model.Password == "admin")
            {
                // Redirect to Coordinator/Manager Dashboard
                return RedirectToAction("ClaimsList", "Claim");
            }

            // Login failed, show error
            model.ErrorMessage = "Invalid username or password.";
            return View(model);
        }
    }
}
