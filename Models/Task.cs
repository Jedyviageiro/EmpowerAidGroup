namespace empoweraidgroup.Models
{
    public class Task
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; }
        public string TaskDescription { get; set; }
        public DateTime DueDate { get; set; }
        public int Id { get; internal set; }
        public string Status { get; internal set; }
        public DateTime AssignedAt { get; internal set; }
    }
}
