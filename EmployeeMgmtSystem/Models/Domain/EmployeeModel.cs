using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string? Image { get; set; }

        public ICollection<AssignedWork>? AssignedWork { get; set; }


        [Required]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
       
    }
    //making one to one relationship between employees and department
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
