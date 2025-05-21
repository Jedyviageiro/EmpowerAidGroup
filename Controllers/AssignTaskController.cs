using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace empoweraidgroup.Controllers
{
    public class AssignTaskController : Controller
    {
        // Action method to display the Assign Tasks view
        public IActionResult AssignTasks()
        {
            // Fetch the username from the session (if needed)
            string username = HttpContext.Session.GetString("loggedUser");

            // If the username is not found in the session, redirect to login
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("AdminLogin", "Account");
            }

            // Pass the username to the view (optional)
            ViewBag.Username = username;

            // Explicitly render the AssignTasks view (AssignTasks.cshtml in Views/Account)
            return View("~/Views/Account/AssignTasks.cshtml");
        }

        // Action method to handle task assignment logic (could be a POST method if needed)
        [HttpPost]
        public IActionResult AssignTaskToEmployee(int employeeId, string username, string taskDescription, string dueDate)
        {
            // Logic for assigning a task (add your database code here to insert the task)
            // Redirect to a different view upon success
            return RedirectToAction("AssignTasks", "AssignTask"); // Or another view after successful task assignment
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
