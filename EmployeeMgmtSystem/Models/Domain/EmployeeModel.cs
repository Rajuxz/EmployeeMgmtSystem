namespace EmployeeMgmtSystem.Models.Domain
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Salary { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }

        public string? Image { get; set; }

        //Establishing relationship between employees and assigned works.
        public ICollection<AssignedWork>? AssignedWork { get; set; }
    }
}
