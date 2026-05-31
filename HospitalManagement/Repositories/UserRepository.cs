using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HospitalManagement.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HospitalDbContext _db;

        public UserRepository(HospitalDbContext db)
        {
            _db = db;
        }

        public IEnumerable<User> GetAll()
            => _db.Users.Include(u => u.Doctor).ToList();

        public User GetById(int id)
            => _db.Users.Include(u => u.Doctor).FirstOrDefault(u => u.UserId == id);

        public void Add(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public void Update(User user)
        {
            var existing = _db.Users.Find(user.UserId);
            if (existing == null) return;
            existing.Username = user.Username;
            existing.Password = user.Password;
            existing.Role = user.Role;
            existing.DoctorId = user.DoctorId;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null) return;
            _db.Users.Remove(user);
            _db.SaveChanges();
        }

        public bool UsernameExists(string username, int? excludeId = null)
            => _db.Users.Any(u => u.Username == username &&
                (excludeId == null || u.UserId != excludeId));
    }
}