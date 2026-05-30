using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HospitalManagement.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HospitalDbContext _db;

        public AppointmentRepository(HospitalDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Appointment> GetAll()
            => _db.Appointments
                  .Include(a => a.Patient)
                  .Include(a => a.Doctor)
                  .ToList();

        public IEnumerable<Appointment> GetByDoctorId(int doctorId)
            => _db.Appointments
                  .Include(a => a.Patient)
                  .Include(a => a.Doctor)
                  .Where(a => a.DoctorId == doctorId)
                  .ToList();

        public Appointment GetById(int id)
            => _db.Appointments
                  .Include(a => a.Patient)
                  .Include(a => a.Doctor)
                  .FirstOrDefault(a => a.AppointmentId == id);

        public void Add(Appointment appointment)
        {
            _db.Appointments.Add(appointment);
            _db.SaveChanges();
        }

        public void Update(Appointment appointment)
        {
            var existing = _db.Appointments.Find(appointment.AppointmentId);
            if (existing == null) return;
            existing.PatientId = appointment.PatientId;
            existing.DoctorId = appointment.DoctorId;
            existing.Date = appointment.Date;
            existing.Status = appointment.Status;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var appointment = _db.Appointments.Find(id);
            if (appointment == null) return;
            _db.Appointments.Remove(appointment);
            _db.SaveChanges();
        }

        public bool IsTimeSlotTaken(int doctorId, DateTime date, int? excludeId = null)
            => _db.Appointments.Any(a =>
                a.DoctorId == doctorId &&
                a.Date == date &&
                (excludeId == null || a.AppointmentId != excludeId));
    }
}