using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace empoweraidgroup.Controllers
{
    public class AdminController : Controller
    {
        // Action method to display the Admin Dashboard
        public IActionResult Index()
        {
            // Fetch the username from the session
            string username = HttpContext.Session.GetString("loggedUser");

            // If the username is not found in the session, redirect to login
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("AdminLogin", "Account");
            }

            // Pass the username to the view
            ViewBag.Username = username;

            // Explicitly render the Admin Dashboard view (AdminDashboard.cshtml in Views/Account)
            return View("~/Views/Account/AdminDashboard.cshtml");
        }

        // Action method to display AssignTasks view
        public IActionResult AssignTasks()
        {
            // Render the AssignTasks view (AssignTasks.cshtml in Views/Admin)
            return View("~/Views/Admin/AssignTasks.cshtml");
        }

        // Action method to display ManageEmployee view
        public IActionResult ManageEmployee()
        {
            // Fetch the username from the session
            string username = HttpContext.Session.GetString("loggedUser");

            // If the username is not found in the session, redirect to login
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("AdminLogin", "Account");
            }

            // Return the ManageEmployee view (ManageEmployee.cshtml in Views/Account)
            return View("~/Views/Account/ManageEmployee.cshtml");
        }

        // Action method to display the ViewProgress view
        public IActionResult ViewProgress()
        {
            // Fetch tasks data here if needed (or in ViewProgress.cshtml if using ViewBag/Model)
            // e.g., ViewBag.Tasks = taskService.GetTasks();

            // Return the ViewProgress view (ViewProgress.cshtml in Views/Account)
            return View("~/Views/Account/ViewProgress.cshtml");
        }

        // Action method to handle logout
        public IActionResult Logout()
        {
            // Clear session on logout
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home"); // Redirect to Home page after logout
        }
    }
}
