using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        public int? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}