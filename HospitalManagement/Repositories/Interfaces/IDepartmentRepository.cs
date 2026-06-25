using HospitalManagement.Models;
using System.Collections.Generic;

namespace HospitalManagement.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAll();
        Department GetById(int id);
        void Add(Department department);
        void Update(Department department);
        void Delete(int id);
        IEnumerable<Department> Search(string keyword);
    }
}