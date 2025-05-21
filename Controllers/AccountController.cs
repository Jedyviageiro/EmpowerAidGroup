using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using empoweraidgroup.Models;
using Microsoft.AspNetCore.Http;

namespace empoweraidgroup.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseConnection dbConnection = new DatabaseConnection();

        // GET action to render Admin Login page
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        // POST action to process Admin Login
        [HttpPost]
        public IActionResult AdminLogin(string email, string password)
        {
            using (MySqlConnection conn = dbConnection.Connect())
            {
                if (conn.State != ConnectionState.Open)
                {
                    ViewBag.ErrorMessage = "Failed to connect to the database.";
                    return View();
                }

                string query = "SELECT username, role FROM users WHERE email = @Email AND password = @Password";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader["role"].ToString();
                            if (!role.Equals("admin", System.StringComparison.OrdinalIgnoreCase))
                            {
                                ViewBag.ErrorMessage = "Access denied. Employee login is not allowed here.";
                                return View();
                            }

                            HttpContext.Session.SetString("loggedEmail", email);
                            HttpContext.Session.SetString("loggedUser", reader["username"].ToString());
                            HttpContext.Session.SetString("userRole", "admin");

                            return RedirectToAction("Index", "Admin"); // Redirect to Admin Dashboard
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Invalid email or password.";
                            return View();
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    ViewBag.ErrorMessage = "Database error occurred: " + ex.Message;
                    return View();
                }
            }
        }

        // GET action to render Employee Login page
        [HttpGet]
        public IActionResult EmployeeLogin()
        {
            return View();
        }

        // POST action to process Employee Login
        [HttpPost]
        public IActionResult EmployeeLogin(string email, string password)
        {
            using (MySqlConnection conn = dbConnection.Connect())
            {
                if (conn.State != ConnectionState.Open)
                {
                    ViewBag.ErrorMessage = "Failed to connect to the database.";
                    return View();
                }

                string query = "SELECT username, role FROM users WHERE email = @Email AND password = @Password";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string role = reader["role"].ToString();
                            if (!role.Equals("employee", System.StringComparison.OrdinalIgnoreCase))
                            {
                                ViewBag.ErrorMessage = "Access denied. Admin login is not allowed here.";
                                return View();
                            }

                            HttpContext.Session.SetString("loggedEmail", email);
                            HttpContext.Session.SetString("loggedUser", reader["username"].ToString());
                            HttpContext.Session.SetString("userRole", "employee");

                            return RedirectToAction("Index", "EmployeeDashboard"); // Redirect to Employee Dashboard
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Invalid email or password.";
                            return View();
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    ViewBag.ErrorMessage = "Database error occurred: " + ex.Message;
                    return View();
                }
            }
        }
    }
}
