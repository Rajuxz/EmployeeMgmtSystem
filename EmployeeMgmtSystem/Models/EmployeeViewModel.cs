using EmployeeMgmtSystem.Models.Domain;

namespace EmployeeMgmtSystem.Models
{
    public class EmployeeViewModel
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Salary { get; set; }
        public string? Phone { get; set; }
        public string? Position { get; set; }
        public string? Email { get; set; }
        public List<Department>? Departments { get; set; }
        public int SelectedDepartmentId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
