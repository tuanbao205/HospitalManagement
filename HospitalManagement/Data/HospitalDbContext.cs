using HospitalManagement.Models;
using System;
using System.Data.Entity;

namespace HospitalManagement.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext() : base("name=HospitalDB")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Appointment: 1 bác sĩ không có 2 lịch cùng thời điểm
            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.DoctorId, a.Date })
                .IsUnique();

            // Quan hệ Appointment - MedicalRecord (1-1)
            modelBuilder.Entity<MedicalRecord>()
                .HasRequired(m => m.Appointment)
                .WithOptional(a => a.MedicalRecord);

            base.OnModelCreating(modelBuilder);
        }
    }
}