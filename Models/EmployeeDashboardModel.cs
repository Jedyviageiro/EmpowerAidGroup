namespace empoweraidgroup.Models
{
    public class EmployeeDashboardModel
    {
        // The employee's username for personalized greeting
        public string Username { get; set; }

        // The profile information for the employee
        public EmployeeProfile ProfileInfo { get; set; }

        // A list of tasks assigned to the employee
        public List<Task> Tasks { get; set; }
    }

    // A simple class for employee profile details
    public class EmployeeProfile
    {
        public string Email { get; set; }
        public int Age { get; set; }
        public string MaritalStatus { get; set; }
        public string Phone { get; set; }
    }

    // Assuming the Task model is already defined similarly
 
}
