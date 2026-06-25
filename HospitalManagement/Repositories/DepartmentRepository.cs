using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagement.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HospitalDbContext _db;

        public DepartmentRepository(HospitalDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Department> GetAll()
        {
            return _db.Departments.ToList();
        }

        public Department GetById(int id)
        {
            return _db.Departments.Find(id);
        }

        public void Add(Department department)
        {
            _db.Departments.Add(department);
            _db.SaveChanges();
        }

        public void Update(Department department)
        {
            var existing = _db.Departments.Find(department.DepartmentId);

            if (existing == null) return;

            existing.DepartmentName = department.DepartmentName;
            existing.Description = department.Description;

            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var department = _db.Departments.Find(id);

            if (department == null) return;

            _db.Departments.Remove(department);
            _db.SaveChanges();
        }

        public IEnumerable<Department> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetAll();

            keyword = keyword.ToLower();

            return _db.Departments
                .Where(d =>
                    d.DepartmentName.ToLower().Contains(keyword) ||
                    d.Description.ToLower().Contains(keyword))
                .ToList();
        }
    }
}