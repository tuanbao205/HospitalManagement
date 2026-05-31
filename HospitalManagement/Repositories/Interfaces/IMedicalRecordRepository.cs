using HospitalManagement.Models;
using System.Collections.Generic;

namespace HospitalManagement.Repositories.Interfaces
{
    public interface IMedicalRecordRepository
    {
        IEnumerable<MedicalRecord> GetAll();
        IEnumerable<MedicalRecord> GetByDoctorId(int doctorId);
        MedicalRecord GetByAppointmentId(int appointmentId);
        void Add(MedicalRecord record);
        void Update(MedicalRecord record);
        void Delete(int id);
        bool ExistsByAppointmentId(int appointmentId);
    }
}