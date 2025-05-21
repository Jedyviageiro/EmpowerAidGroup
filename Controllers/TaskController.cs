using Microsoft.AspNetCore.Mvc;
using empoweraidgroup.Models;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;


namespace empoweraidgroup.Controllers
{
    public class TaskController : Controller
    {
        private readonly DatabaseConnection _dbConnection;

        public TaskController()
        {
            _dbConnection = new DatabaseConnection();
        }

        // View tasks (equivalent of ViewProgressServlet)
        [HttpGet]
        public IActionResult ViewTasks()
        {
            List<empoweraidgroup.Models.Task> tasks = new List<empoweraidgroup.Models.Task>();

            using (var conn = _dbConnection.Connect())
            {
                string sql = "SELECT id, employee_id, username, task_description, due_date, status, assigned_at FROM tasks";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new empoweraidgroup.Models.Task
                            {
                                Id = reader.GetInt32("id"),
                                EmployeeId = reader.GetInt32("employee_id"),
                                Username = reader.GetString("username"),
                                TaskDescription = reader.GetString("task_description"),
                                DueDate = reader.GetDateTime("due_date"),
                                Status = reader.GetString("status"),
                                AssignedAt = reader.GetDateTime("assigned_at")
                            });
                        }
                    }
                }
            }

            return View("~/Views/Account/ViewProgress.cshtml", tasks);
        }

        // Approve Task (equivalent of ApproveTaskServlet)
        [HttpPost]
        public IActionResult ApproveTask(int taskId)
        {
            using (var conn = _dbConnection.Connect())
            {
                string sql = "UPDATE tasks SET status = 'Completed' WHERE id = @taskId";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskId", taskId);
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("ViewTasks");
        }

        // Reject Task (equivalent of RejectTaskServlet)
        [HttpPost]
        public IActionResult RejectTask(int taskId)
        {
            using (var conn = _dbConnection.Connect())
            {
                string sql = "UPDATE tasks SET status = 'Rejected' WHERE id = @taskId";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@taskId", taskId);
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("ViewTasks");
        }
    }
}
