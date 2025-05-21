using Microsoft.AspNetCore.Mvc;
using empoweraidgroup.Models;
using MySql.Data.MySqlClient;

namespace empoweraidgroup.Controllers
{
    // Remove the "Admin" prefix and map directly to /AddEmployee URL
    [Route("[controller]")]  // This directly maps to /AddEmployee
    public class AddEmployeeController : Controller
    {
        private readonly DatabaseConnection _dbConnection;

        public AddEmployeeController()
        {
            _dbConnection = new DatabaseConnection();
        }

        // Action method to display the Add Employee form
        [HttpGet]
        public IActionResult Index()
        {
            // Ensure the view is being accessed from the correct location in /Views/Account/
            return View("~/Views/Account/AddEmployee.cshtml");
        }

        // Action method to handle Add Employee form submission
        [HttpPost]
        public IActionResult AddEmployee(string name, string email, int age, string gender, string maritalStatus, string phone, string hireDate, string position)
        {
            MySqlConnection conn = _dbConnection.Connect();

            try
            {
                // Check if the email or phone already exists in the database
                string checkQuery = "SELECT COUNT(*) FROM employee WHERE email = @Email OR phone = @Phone";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);
                checkCmd.Parameters.AddWithValue("@Phone", phone);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    // Return error if email or phone already exists
                    ViewBag.Error = "Email or phone number already exists.";
                    return View("Index");
                }

                // Insert new employee into the database
                string query = "INSERT INTO employee (name, email, age, gender, marital_status, phone, hire_date, position) " +
                               "VALUES (@Name, @Email, @Age, @Gender, @MaritalStatus, @Phone, @HireDate, @Position)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@MaritalStatus", maritalStatus);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@HireDate", hireDate);
                cmd.Parameters.AddWithValue("@Position", position);

                cmd.ExecuteNonQuery();

                // Redirect to a success page or another view after successful insertion
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log the error and show a message
                Console.WriteLine("Error: " + ex.Message);
                ViewBag.Error = "An error occurred while adding the employee.";
                return View("Index");
            }
            finally
            {
                _dbConnection.Disconnect(conn);
            }
        }
    }
}
