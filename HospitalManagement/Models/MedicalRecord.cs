using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordId { get; set; }
        public int AppointmentId { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Appointment Appointment { get; set; }
    }
}