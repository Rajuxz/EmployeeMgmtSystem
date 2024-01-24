using EmployeeMgmtSystem.Models.Domain;

namespace EmployeeMgmtSystem.Repository.IRepository
{
    public interface IDepartmentRepository: IRepository<Department>
    {
        void Update(Department model);
        void Save();
    }
}
