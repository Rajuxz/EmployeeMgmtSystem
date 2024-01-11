namespace EmployeeMgmtSystem.Models.Domain
{
    public class AssignedWork
    {
        public int Id { get; set; }
        public string? Description{ get; set; }
        public bool? IsUrgent{ get; set; } = false;
        public string? Status { get; set; } //Normal/Urgent
        public bool? IsCompleted { get; set; } = false;

        public int EmployeeId { get; set; }
        public EmployeeModel? Employee { get; set; }
    }
}
