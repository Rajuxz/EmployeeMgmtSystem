﻿using EmployeeMgmtSystem.Models.Domain;

namespace EmployeeMgmtSystem.Repository.IRepository
{
    public interface IEmployeeRepository : IRepository<EmployeeModel>
    {
        void Update(EmployeeModel model);
        void Save();
        public string? SaveFileAndReturnName(string path, IFormFile formFile);
    }
}
