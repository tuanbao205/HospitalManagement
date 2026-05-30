using HospitalManagement.Models;
using System;
using System.Collections.Generic;

namespace HospitalManagement.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        IEnumerable<Appointment> GetAll();
        IEnumerable<Appointment> GetByDoctorId(int doctorId);
        Appointment GetById(int id);
        void Add(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(int id);
        bool IsTimeSlotTaken(int doctorId, DateTime date, int? excludeId = null);
    }
}