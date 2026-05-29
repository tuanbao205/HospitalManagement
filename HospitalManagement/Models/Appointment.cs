using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } // Scheduled / Done / Cancelled
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; }
    }
}