using empoweraidgroup.Models;  // Ensure this is your model namespace
using Microsoft.AspNetCore.Mvc;  // MVC namespace for controllers
using MySql.Data.MySqlClient;  // Correct MySQL library

namespace empoweraidgroup.Controllers
{
    public class ManageEmployeeController : Controller
    {
        private readonly DatabaseConnection _dbConnection = new DatabaseConnection();

        // GET: /ManageEmployee
        public IActionResult Index()
        {
            return View("~/Views/Account/ManageEmployee.cshtml");  // Ensure correct view path
        }

        // POST: /ManageEmployee/RetrieveEmployee
        [HttpPost]
        public IActionResult RetrieveEmployee(int employee_id)
        {
            Employee employee = GetEmployeeById(employee_id);
            string message = "";
            string messageType = "error";

            if (employee != null)
            {
                message = "Employee found.";
                messageType = "success";
            }
            else
            {
                message = "No employee found. Please enter a valid Employee ID.";
            }

            ViewData["Message"] = message;
            ViewData["MessageType"] = messageType;
            ViewData["Employee"] = employee;

            return View("~/Views/Account/ManageEmployee.cshtml");  // Ensure correct view path
        }

        // POST: /ManageEmployee/RemoveEmployee
        [HttpPost("RemoveEmployee")]
        public IActionResult RemoveEmployee(int employee_id)
        {
            bool result = RemoveEmployeeById(employee_id);
            string message = "";
            string messageType = "error";

            if (result)
            {
                message = "Employee removed successfully.";
                messageType = "success";
            }
            else
            {
                message = "Failed to remove employee. Please try again.";
            }

            ViewData["Message"] = message;
            ViewData["MessageType"] = messageType;

            return View("~/Views/Account/ManageEmployee.cshtml");  // Ensure correct view path
        }

        // POST: /ManageEmployee/EditEmployee
        [HttpPost("EditEmployee")]
        public IActionResult EditEmployee(Employee employee)
        {
            bool result = EditEmployeeDetails(employee);
            string message = "";
            string messageType = "error";

            if (result)
            {
                message = "Employee details updated successfully.";
                messageType = "success";
            }
            else
            {
                message = "Failed to update employee details. Please try again.";
            }

            ViewData["Message"] = message;
            ViewData["MessageType"] = messageType;

            return View("~/Views/Account/ManageEmployee.cshtml");  // Ensure correct view path
        }

        // Helper method to remove employee by ID
        private bool RemoveEmployeeById(int employeeId)
        {
            bool success = false;

            try
            {
                using (MySqlConnection conn = _dbConnection.Connect())
                {
                    string query = "DELETE FROM employee WHERE id = @employeeId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = "Error removing employee: " + ex.Message;
                ViewData["MessageType"] = "error";
            }

            return success;
        }

        // Helper method to update employee details
        private bool EditEmployeeDetails(Employee employee)
        {
            bool success = false;

            try
            {
                using (MySqlConnection conn = _dbConnection.Connect())
                {
                    string query = @"UPDATE employee 
                                     SET name = @name, email = @email, age = @age, gender = @gender, marital_status = @maritalStatus, 
                                         phone = @phone, hire_date = @hireDate, position = @position
                                     WHERE id = @employeeId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@employeeId", employee.Id);
                    cmd.Parameters.AddWithValue("@name", employee.Name);
                    cmd.Parameters.AddWithValue("@email", employee.Email);
                    cmd.Parameters.AddWithValue("@age", employee.Age);
                    cmd.Parameters.AddWithValue("@gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@maritalStatus", employee.MaritalStatus);
                    cmd.Parameters.AddWithValue("@phone", employee.Phone);
                    cmd.Parameters.AddWithValue("@hireDate", employee.HireDate);
                    cmd.Parameters.AddWithValue("@position", employee.Position);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = "Error updating employee details: " + ex.Message;
                ViewData["MessageType"] = "error";
            }

            return success;
        }

        // Helper method to retrieve employee by ID
        private Employee GetEmployeeById(int employeeId)
        {
            Employee employee = null;

            try
            {
                using (MySqlConnection conn = _dbConnection.Connect())
                {
                    string query = "SELECT * FROM employee WHERE id = @employeeId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@employeeId", employeeId);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Age = reader.GetInt32(3),
                            Gender = reader.GetString(4),
                            MaritalStatus = reader.GetString(5),
                            Phone = reader.GetString(6),
                            HireDate = reader.GetDateTime(7),
                            Position = reader.GetString(8)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = "Error retrieving employee: " + ex.Message;
                ViewData["MessageType"] = "error";
            }

            return employee;
        }
    }
}
