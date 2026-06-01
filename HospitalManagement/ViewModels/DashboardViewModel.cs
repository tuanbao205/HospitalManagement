using HospitalManagement.Data;
using HospitalManagement.Helpers;
using HospitalManagement.ViewModels.Base;
using System;
using System.Linq;

namespace HospitalManagement.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly HospitalDbContext _db;

        private int _totalPatients;
        public int TotalPatients
        {
            get => _totalPatients;
            set => SetProperty(ref _totalPatients, value);
        }

        private int _totalDoctors;
        public int TotalDoctors
        {
            get => _totalDoctors;
            set => SetProperty(ref _totalDoctors, value);
        }

        private int _todayAppointments;
        public int TodayAppointments
        {
            get => _todayAppointments;
            set => SetProperty(ref _todayAppointments, value);
        }

        private int _totalMedicalRecords;
        public int TotalMedicalRecords
        {
            get => _totalMedicalRecords;
            set => SetProperty(ref _totalMedicalRecords, value);
        }

        private int _scheduledAppointments;
        public int ScheduledAppointments
        {
            get => _scheduledAppointments;
            set => SetProperty(ref _scheduledAppointments, value);
        }

        private int _doneAppointments;
        public int DoneAppointments
        {
            get => _doneAppointments;
            set => SetProperty(ref _doneAppointments, value);
        }

        private string _welcomeText;
        public string WelcomeText
        {
            get => _welcomeText;
            set => SetProperty(ref _welcomeText, value);
        }

        public DashboardViewModel()
        {
            _db = new HospitalDbContext();
            WelcomeText = $"Xin chào, {SessionHelper.CurrentUser?.Username}!";
            LoadStats();
        }

        private void LoadStats()
        {
            TotalPatients = _db.Patients.Count();
            TotalDoctors = _db.Doctors.Count();
            TotalMedicalRecords = _db.MedicalRecords.Count();

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            TodayAppointments = _db.Appointments
                .Count(a => a.Date >= today && a.Date < tomorrow);

            ScheduledAppointments = _db.Appointments
                .Count(a => a.Status == "Scheduled");

            DoneAppointments = _db.Appointments
                .Count(a => a.Status == "Done");
        }
    }
}