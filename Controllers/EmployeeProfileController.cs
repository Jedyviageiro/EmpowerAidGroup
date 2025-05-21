using empoweraidgroup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;

namespace empoweraidgroup.Controllers
{
    public class EmployeeProfileController : Controller
    {
        private readonly DatabaseConnection _dbConnection;

        public EmployeeProfileController()
        {
            _dbConnection = new DatabaseConnection();
        }

        public IActionResult ShowProfile()
        {
            // Get the logged-in user's username from the session
            string username = HttpContext.Session.GetString("loggedUser");

            if (username == null)
            {
                // Redirect to dashboard if user is not logged in
                return RedirectToAction("Index", "EmployeeDashboard");
            }

            Employee employee = null;

            // Connect to the database and retrieve employee profile data
            using (var connection = _dbConnection.Connect())
            {
                string sql = "SELECT id, name, email, age, gender, marital_status, phone, hire_date, position FROM employee WHERE name = @username";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Email = reader.GetString("email"),
                            Age = reader.GetInt32("age"),
                            Gender = reader.GetString("gender"),
                            MaritalStatus = reader.GetString("marital_status"),
                            Phone = reader.GetString("phone"),
                            HireDate = reader.GetDateTime("hire_date"),
                            Position = reader.GetString("position")
                        };
                    }
                }
            }

            if (employee == null)
            {
                ViewBag.Message = "No profile information available. Click the button to view your profile information.";
            }

            return View("~/Views/Account/EmployeeProfile.cshtml", employee);
        }
    }
}
