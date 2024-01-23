using System.ComponentModel.DataAnnotations;

namespace EmployeeMgmtSystem.Models.Admin
{
    public class AdminViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? ProfilePath { get; set; }
        public FormFile? Profile { get; set; }
    }
}
