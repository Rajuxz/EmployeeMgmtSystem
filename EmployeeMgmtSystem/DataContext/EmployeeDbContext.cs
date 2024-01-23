using EmployeeMgmtSystem.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMgmtSystem.DataContext
{
    public class EmployeeDbContext:DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }


        public DbSet<AdminModel>? Admins { get; set; }
        public DbSet<EmployeeModel>? Employees { get; set; }
        public DbSet<AssignedWork>? AssignedWorks { get; set; }
        public DbSet<Department>? Department { get; set; }
    }
}
