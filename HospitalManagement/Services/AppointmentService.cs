using HospitalManagement.Models;
using HospitalManagement.Repositories.Interfaces;
using HospitalManagement.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace HospitalManagement.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Appointment> GetAll() => _repo.GetAll();

        public IEnumerable<Appointment> GetByDoctorId(int doctorId)
            => _repo.GetByDoctorId(doctorId);

        public bool Book(Appointment appointment, out string error)
        {
            // Kiểm tra trùng lịch
            if (_repo.IsTimeSlotTaken(appointment.DoctorId, appointment.Date))
            {
                error = "Bác sĩ đã có lịch khám vào thời điểm này!";
                return false;
            }

            // Không đặt lịch trong quá khứ
            if (appointment.Date < DateTime.Now)
            {
                error = "Không thể đặt lịch trong quá khứ!";
                return false;
            }

            appointment.Status = "Scheduled";
            _repo.Add(appointment);
            error = null;
            return true;
        }
    }
}