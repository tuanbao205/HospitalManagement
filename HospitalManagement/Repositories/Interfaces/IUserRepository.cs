using HospitalManagement.Models;
using System.Collections.Generic;

namespace HospitalManagement.Repositories.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
        bool UsernameExists(string username, int? excludeId = null);
    }
}