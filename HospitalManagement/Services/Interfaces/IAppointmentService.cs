using HospitalManagement.Models;
using System;
using System.Collections.Generic;

namespace HospitalManagement.Services.Interfaces
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> GetAll();
        IEnumerable<Appointment> GetByDoctorId(int doctorId);
        bool Book(Appointment appointment, out string error);
    }
}