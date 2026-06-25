using HospitalManagement.Models;
using System.Collections.Generic;

namespace HospitalManagement.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}