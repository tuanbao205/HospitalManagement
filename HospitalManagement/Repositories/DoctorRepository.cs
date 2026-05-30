using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagement.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalDbContext _db;

        public DoctorRepository(HospitalDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Doctor> GetAll() => _db.Doctors.ToList();

        public Doctor GetById(int id) => _db.Doctors.Find(id);

        public void Add(Doctor doctor)
        {
            _db.Doctors.Add(doctor);
            _db.SaveChanges();
        }

        public void Update(Doctor doctor)
        {
            var existing = _db.Doctors.Find(doctor.DoctorId);
            if (existing == null) return;
            existing.FullName = doctor.FullName;
            existing.Specialty = doctor.Specialty;
            existing.Phone = doctor.Phone;
            existing.Email = doctor.Email;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var doctor = _db.Doctors.Find(id);
            if (doctor == null) return;
            _db.Doctors.Remove(doctor);
            _db.SaveChanges();
        }

        public IEnumerable<Doctor> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAll();
            keyword = keyword.ToLower();
            return _db.Doctors
                .Where(d => d.FullName.ToLower().Contains(keyword)
                         || d.Specialty.ToLower().Contains(keyword))
                .ToList();
        }
    }
}