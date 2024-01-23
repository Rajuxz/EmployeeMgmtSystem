using System.ComponentModel.DataAnnotations;

namespace EmployeeMgmtSystem.Models.Domain
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
