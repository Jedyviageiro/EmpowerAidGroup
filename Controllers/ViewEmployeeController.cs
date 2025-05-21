using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using empoweraidgroup.Models;

namespace empoweraidgroup.Controllers
{
    public class ViewEmployeeController : Controller
    {
        // Assuming you have a DatabaseConnection class to handle DB connection
        private readonly DatabaseConnection _dbConnection;

        public ViewEmployeeController()
        {
            _dbConnection = new DatabaseConnection(); // Ensure the connection is properly initialized
        }

        public IActionResult ViewEmployees()
        {
            List<Employee> employeesList = new List<Employee>();

            using (MySqlConnection conn = _dbConnection.Connect())
            {
                if (conn.State != ConnectionState.Open)
                {
                    ViewBag.ErrorMessage = "Failed to connect to the database.";
                    return View("~/Views/Account/ViewEmployees.cshtml"); // Correct path to view
                }

                string query = "SELECT id, name, email, age, gender, marital_status, phone, hire_date, position FROM employee";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee employee = new Employee
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
                            employeesList.Add(employee);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    ViewBag.ErrorMessage = "Database error: " + ex.Message;
                }
            }

            // Return the employees list to the View in the correct path
            return View("~/Views/Account/ViewEmployees.cshtml", employeesList);
        }
    }
}
