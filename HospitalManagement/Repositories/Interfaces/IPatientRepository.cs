using HospitalManagement.Models;
using System.Collections.Generic;

namespace HospitalManagement.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetAll();
        Patient GetById(int id);
        void Add(Patient patient);
        void Update(Patient patient);
        void Delete(int id);
        IEnumerable<Patient> Search(string keyword);
    }
}