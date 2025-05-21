using empoweraidgroup.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace empoweraidgroup.Controllers
{
    public class EmployeeDashboardController : Controller
    {
        private readonly DatabaseConnection _dbConnection;

        // Constructor
        public EmployeeDashboardController()
        {
            _dbConnection = new DatabaseConnection();
        }

        // Index action to load employee dashboard
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("loggedUser");

            // If the username is not found in the session, redirect to login
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("EmployeeDashboardController", "Account");
            }

            // Pass the username to the view
            ViewBag.Username = username;

            // Explicitly specify the path to the view
            return View("~/Views/Account/EmployeeDashboard.cshtml"); // Return the populated model to the view
        }

        public IActionResult ViewTasks()
        {
            string username = HttpContext.Session.GetString("loggedUser");

            // Check if user is logged in
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("EmployeeDashboardController", "Account"); // Redirect if not logged in
            }

            // Get the tasks assigned to this employee
            List<Models.Task> tasks = GetEmployeeTasks();

            // Pass the tasks list to the view
            return View("~/Views/Account/ViewTasks.cshtml", tasks);
        }


        // CompleteTask action to mark a task as complete
        [HttpPost]
        public IActionResult CompleteTask(int task_id)
        {
            // Check if user is logged in
            string username = HttpContext.Session.GetString("loggedUser");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("EmployeeDashboardController", "Account"); // Redirect if not logged in
            }

            // Update the task status to "Awaiting Approval" in the database
            using (var connection = _dbConnection.Connect())
            {
                string query = "UPDATE tasks SET status = 'Awaiting Approval' WHERE id = @task_id AND employee_id = @employee_id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@task_id", task_id);
                cmd.Parameters.AddWithValue("@employee_id", GetEmployeeIdByUsername(username));

                cmd.ExecuteNonQuery(); // Execute the update
            }

            // Redirect to the ViewTasks page to show updated task status
            return RedirectToAction("ViewTasks");
        }

        // Method to fetch employee profile based on username
        private EmployeeProfile GetEmployeeProfile(string username)
        {
            EmployeeProfile profile = null;

            using (var connection = _dbConnection.Connect())
            {
                // Query to fetch profile details from the employee table based on username
                string query = @"
                    SELECT email, age, marital_status, phone
                    FROM employee
                    WHERE name = @username"; // Directly using the 'employee' table

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);

                MySqlDataReader reader = cmd.ExecuteReader();

                // Check if data was returned
                if (reader.Read())
                {
                    profile = new EmployeeProfile
                    {
                        Email = reader["email"].ToString(),
                        Age = Convert.ToInt32(reader["age"]),
                        MaritalStatus = reader["marital_status"].ToString(),
                        Phone = reader["phone"]?.ToString() // Safely handle null values
                    };
                }
            }

            return profile; // Return the profile info
        }

        // Method to fetch all tasks for an employee based on employee ID
        private List<Models.Task> GetEmployeeTasks()
        {
            List<Models.Task> tasks = new List<Models.Task>();
            string username = HttpContext.Session.GetString("loggedUser");

            using (var connection = _dbConnection.Connect())
            {
                int employeeId = GetEmployeeIdByUsername(username);

                string query = "SELECT id, task_description, due_date, status, assigned_at FROM tasks WHERE employee_id = @employee_id";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@employee_id", employeeId);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Models.Task
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            TaskDescription = reader["task_description"].ToString(),
                            DueDate = Convert.ToDateTime(reader["due_date"]),
                            Status = reader["status"].ToString(),
                            AssignedAt = Convert.ToDateTime(reader["assigned_at"])
                        });
                    }
                }
            }

            return tasks;
        }


        // Method to get the employee's ID using their username
        private int GetEmployeeIdByUsername(string username)
        {
            int employeeId = 0;

            using (var connection = _dbConnection.Connect())
            {
                // Query to fetch the employee ID from the employee table based on username
                string query = "SELECT id FROM employee WHERE name = @username"; // Directly query 'employee' table

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);

                var result = cmd.ExecuteScalar(); // Execute the query and get the result
                if (result != null)
                {
                    employeeId = Convert.ToInt32(result); // Convert result to integer
                }
            }

            return employeeId; // Return the employee ID
        }
    }
}
