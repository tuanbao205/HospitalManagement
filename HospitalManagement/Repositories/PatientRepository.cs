using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagement.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HospitalDbContext _db;

        public PatientRepository(HospitalDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Patient> GetAll() => _db.Patients.ToList();

        public Patient GetById(int id) => _db.Patients.Find(id);

        public void Add(Patient patient)
        {
            _db.Patients.Add(patient);
            _db.SaveChanges();
        }

        public void Update(Patient patient)
        {
            var existing = _db.Patients.Find(patient.PatientId);
            if (existing == null) return;
            existing.FullName = patient.FullName;
            existing.DOB = patient.DOB;
            existing.Gender = patient.Gender;
            existing.Phone = patient.Phone;
            existing.Address = patient.Address;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var patient = _db.Patients.Find(id);
            if (patient == null) return;
            _db.Patients.Remove(patient);
            _db.SaveChanges();
        }

        public IEnumerable<Patient> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAll();
            keyword = keyword.ToLower();
            return _db.Patients
                .Where(p => p.FullName.ToLower().Contains(keyword)
                         || p.Phone.Contains(keyword))
                .ToList();
        }
    }
} 