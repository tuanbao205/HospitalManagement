namespace HospitalManagement.Migrations
{
    using HospitalManagement.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HospitalManagement.Data.HospitalDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HospitalManagement.Data.HospitalDbContext context)
        {
            try
            {
                context.Doctors.AddOrUpdate(d => d.DoctorId,
                    new Doctor { DoctorId = 1, FullName = "BS. Nguyễn Văn An", Specialty = "Nội khoa", Phone = "0901000001", Email = "an@hospital.vn" },
                    new Doctor { DoctorId = 2, FullName = "BS. Trần Thị Bình", Specialty = "Nhi khoa", Phone = "0901000002", Email = "binh@hospital.vn" },
                    new Doctor { DoctorId = 3, FullName = "BS. Lê Minh Châu", Specialty = "Tim mạch", Phone = "0901000003", Email = "chau@hospital.vn" }
                );
                context.SaveChanges();

                context.Patients.AddOrUpdate(p => p.PatientId,
                    new Patient { PatientId = 1, FullName = "Phạm Văn Dũng", DOB = new DateTime(1990, 5, 15), Gender = "Nam", Phone = "0912000001", Address = "Hà Nội" },
                    new Patient { PatientId = 2, FullName = "Hoàng Thị Em", DOB = new DateTime(1985, 3, 20), Gender = "Nữ", Phone = "0912000002", Address = "Hà Nội" },
                    new Patient { PatientId = 3, FullName = "Vũ Quốc Hùng", DOB = new DateTime(2000, 8, 10), Gender = "Nam", Phone = "0912000003", Address = "Hà Nội" }
                );
                context.SaveChanges();

                context.Users.AddOrUpdate(u => u.UserId,
                     new User { UserId = 1, Username = "admin", Password = BCrypt.Net.BCrypt.HashPassword("admin123"), Role = "Admin" },
                     new User { UserId = 2, Username = "doctor1", Password = BCrypt.Net.BCrypt.HashPassword("doctor123"), Role = "Doctor", DoctorId = 1 },
                     new User { UserId = 3, Username = "doctor2", Password = BCrypt.Net.BCrypt.HashPassword("doctor123"), Role = "Doctor", DoctorId = 2 }
                 );
                context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SEED ERROR: " + ex.Message);
                throw;
            }
        }
    }
}
