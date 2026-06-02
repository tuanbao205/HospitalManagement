namespace HospitalManagement.Migrations
{
    using HospitalManagement.Models;
    using System;
    using System.Data.Entity.Migrations;

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
                // Seed Doctors
                context.Doctors.AddOrUpdate(d => d.DoctorId,
                    new Doctor { DoctorId = 1, FullName = "BS. Nguyễn Văn An", Specialty = "Nội khoa", Phone = "0901000001", Email = "an@hospital.vn" },
                    new Doctor { DoctorId = 2, FullName = "BS. Trần Thị Bình", Specialty = "Nhi khoa", Phone = "0901000002", Email = "binh@hospital.vn" },
                    new Doctor { DoctorId = 3, FullName = "BS. Lê Minh Châu", Specialty = "Tim mạch", Phone = "0901000003", Email = "chau@hospital.vn" },
                    new Doctor { DoctorId = 4, FullName = "BS. Phạm Thị Dung", Specialty = "Da liễu", Phone = "0901000004", Email = "dung@hospital.vn" },
                    new Doctor { DoctorId = 5, FullName = "BS. Hoàng Văn Em", Specialty = "Thần kinh", Phone = "0901000005", Email = "em@hospital.vn" }
                );
                context.SaveChanges();

                // Seed Patients
                context.Patients.AddOrUpdate(p => p.PatientId,
                    new Patient { PatientId = 1, FullName = "Phạm Văn Dũng", DOB = new DateTime(1990, 5, 15), Gender = "Nam", Phone = "0912000001", Address = "Hà Nội" },
                    new Patient { PatientId = 2, FullName = "Hoàng Thị Em", DOB = new DateTime(1985, 3, 20), Gender = "Nữ", Phone = "0912000002", Address = "Hà Nội" },
                    new Patient { PatientId = 3, FullName = "Vũ Quốc Hùng", DOB = new DateTime(2000, 8, 10), Gender = "Nam", Phone = "0912000003", Address = "Hà Nội" },
                    new Patient { PatientId = 4, FullName = "Nguyễn Thị Lan", DOB = new DateTime(1995, 2, 14), Gender = "Nữ", Phone = "0912000004", Address = "Hà Nội" },
                    new Patient { PatientId = 5, FullName = "Trần Văn Minh", DOB = new DateTime(1978, 11, 30), Gender = "Nam", Phone = "0912000005", Address = "Hà Nội" }
                );
                context.SaveChanges();

                // Seed Users
                context.Users.AddOrUpdate(u => u.UserId,
                    new User { UserId = 1, Username = "admin", Password = "admin123", Role = "Admin" },
                    new User { UserId = 2, Username = "doctor1", Password = "doctor123", Role = "Doctor", DoctorId = 1 },
                    new User { UserId = 3, Username = "doctor2", Password = "doctor123", Role = "Doctor", DoctorId = 2 },
                    new User { UserId = 4, Username = "doctor3", Password = "doctor123", Role = "Doctor", DoctorId = 3 }
                );
                context.SaveChanges();

                // Seed Appointments
                context.Appointments.AddOrUpdate(a => a.AppointmentId,
                    new Appointment { AppointmentId = 1, PatientId = 1, DoctorId = 1, Date = new DateTime(2026, 6, 1, 8, 0, 0), Status = "Done" },
                    new Appointment { AppointmentId = 2, PatientId = 3, DoctorId = 2, Date = new DateTime(2026, 6, 2, 10, 0, 0), Status = "Done" },
                    new Appointment { AppointmentId = 3, PatientId = 4, DoctorId = 3, Date = new DateTime(2026, 6, 3, 8, 30, 0), Status = "Scheduled" },
                    new Appointment { AppointmentId = 4, PatientId = 5, DoctorId = 3, Date = new DateTime(2026, 6, 3, 9, 30, 0), Status = "Scheduled" },
                    new Appointment { AppointmentId = 5, PatientId = 2, DoctorId = 4, Date = new DateTime(2026, 6, 4, 8, 0, 0), Status = "Cancelled" }
                );
                context.SaveChanges();

                // Seed MedicalRecords
                context.MedicalRecords.AddOrUpdate(m => m.RecordId,
                    new MedicalRecord
                    {
                        RecordId = 1,
                        Diagnosis = "Viêm họng cấp tính",
                        Prescription = "Amoxicillin 500mg x 2 lần/ngày x 7 ngày\nParacetamol 500mg khi sốt",
                        Note = "Uống nhiều nước, nghỉ ngơi",
                        CreatedDate = new DateTime(2026, 6, 1, 8, 30, 0)
                    },
                    new MedicalRecord
                    {
                        RecordId = 2,
                        Diagnosis = "Tăng huyết áp độ 1",
                        Prescription = "Amlodipine 5mg x 1 lần/ngày\nTái khám sau 2 tuần",
                        Note = "Hạn chế muối, tập thể dục nhẹ",
                        CreatedDate = new DateTime(2026, 6, 2, 9, 30, 0)
                    },
                    new MedicalRecord
                    {
                        RecordId = 3,
                        Diagnosis = "Viêm phế quản",
                        Prescription = "Azithromycin 250mg x 1 lần/ngày x 5 ngày\nSalbutamol khi khó thở",
                        Note = "Tránh khói bụi, không hút thuốc",
                        CreatedDate = new DateTime(2026, 6, 2, 10, 30, 0)
                    }
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