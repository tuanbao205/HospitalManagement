using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HospitalManagement.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly HospitalDbContext _db;

        public MedicalRecordRepository(HospitalDbContext db)
        {
            _db = db;
        }

        public IEnumerable<MedicalRecord> GetAll()
            => _db.MedicalRecords
                  .Include(m => m.Appointment)
                  .Include(m => m.Appointment.Patient)
                  .Include(m => m.Appointment.Doctor)
                  .ToList();

        public IEnumerable<MedicalRecord> GetByDoctorId(int doctorId)
            => _db.MedicalRecords
                  .Include(m => m.Appointment)
                  .Include(m => m.Appointment.Patient)
                  .Include(m => m.Appointment.Doctor)
                  .Where(m => m.Appointment.DoctorId == doctorId)
                  .ToList();

        public MedicalRecord GetByAppointmentId(int appointmentId)
            => _db.MedicalRecords
                  .Include(m => m.Appointment)
                  .Include(m => m.Appointment.Patient)
                  .Include(m => m.Appointment.Doctor)
                  .FirstOrDefault(m => m.RecordId == appointmentId);

        public void Add(MedicalRecord record)
        {
            record.CreatedDate = DateTime.Now;
            _db.MedicalRecords.Add(record);
            _db.SaveChanges();
        }

        public void Update(MedicalRecord record)
        {
            var existing = _db.MedicalRecords.Find(record.RecordId);
            if (existing == null) return;
            existing.Diagnosis = record.Diagnosis;
            existing.Prescription = record.Prescription;
            existing.Note = record.Note;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var record = _db.MedicalRecords.Find(id);
            if (record == null) return;
            _db.MedicalRecords.Remove(record);
            _db.SaveChanges();
        }

        public bool ExistsByAppointmentId(int appointmentId)
            => _db.MedicalRecords.Any(m => m.RecordId == appointmentId);
    }
}