using HospitalManagement.Data;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using HospitalManagement.Services;
using HospitalManagement.ViewModels.Base;
using HospitalManagement.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagement.ViewModels
{
    public class AppointmentViewModel : BaseViewModel
    {
        private readonly AppointmentRepository _appointmentRepo;
        private readonly PatientRepository _patientRepo;
        private readonly DoctorRepository _doctorRepo;
        private readonly AppointmentService _service;

        private ObservableCollection<Patient> _patients;
        public ObservableCollection<Patient> Patients
        {
            get => _patients;
            set => SetProperty(ref _patients, value);
        }

        private ObservableCollection<Doctor> _doctors;
        public ObservableCollection<Doctor> Doctors
        {
            get => _doctors;
            set => SetProperty(ref _doctors, value);
        }

        private ObservableCollection<Appointment> _appointments;
        public ObservableCollection<Appointment> Appointments
        {
            get => _appointments;
            set => SetProperty(ref _appointments, value);
        }

        private Appointment _selectedAppointment;
        public Appointment SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {
                SetProperty(ref _selectedAppointment, value);
                if (value != null)
                {
                    LoadForm(value);
                    IsEditing = true;
                }
                else
                {
                    IsEditing = false;
                }
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                SetProperty(ref _isEditing, value);
                OnPropertyChanged(nameof(BookButtonText));
            }
        }

        public string BookButtonText => _isEditing ? "✏️ Cập nhật lịch" : "📅 Đặt lịch mới";

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set => SetProperty(ref _selectedPatient, value);
        }

        private Doctor _selectedDoctor;
        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set => SetProperty(ref _selectedDoctor, value);
        }

        private DateTime _appointmentDate = DateTime.Today.AddDays(1);
        public DateTime AppointmentDate
        {
            get => _appointmentDate;
            set => SetProperty(ref _appointmentDate, value);
        }

        private string _appointmentHour = "08";
        public string AppointmentHour
        {
            get => _appointmentHour;
            set => SetProperty(ref _appointmentHour, value);
        }

        private string _appointmentMinute = "00";
        public string AppointmentMinute
        {
            get => _appointmentMinute;
            set => SetProperty(ref _appointmentMinute, value);
        }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }

        public bool IsAdmin => SessionHelper.IsAdmin;

        public ObservableCollection<string> Hours { get; }
        public ObservableCollection<string> Minutes { get; }
        public ObservableCollection<string> StatusOptions { get; }

        public ICommand BookCommand { get; }
        public ICommand UpdateStatusCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public AppointmentViewModel()
        {
            var db = new HospitalDbContext();
            _appointmentRepo = new AppointmentRepository(db);
            _patientRepo = new PatientRepository(db);
            _doctorRepo = new DoctorRepository(db);
            _service = new AppointmentService(_appointmentRepo);

            Hours = new ObservableCollection<string>();
            Minutes = new ObservableCollection<string> { "00", "15", "30", "45" };
            StatusOptions = new ObservableCollection<string>
                { "Scheduled", "Done", "Cancelled" };

            for (int i = 7; i <= 17; i++)
                Hours.Add(i.ToString("D2"));

            LoadData();

            BookCommand = new RelayCommand(_ => Book(), _ => CanBook());
            UpdateStatusCommand = new RelayCommand(_ => UpdateStatus(), _ => SelectedAppointment != null);
            CancelCommand = new RelayCommand(_ => Cancel(), _ => SelectedAppointment != null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedAppointment != null);
            ClearCommand = new RelayCommand(_ => ClearForm());
        }

        private void LoadData()
        {
            Patients = new ObservableCollection<Patient>(_patientRepo.GetAll());
            Doctors = new ObservableCollection<Doctor>(_doctorRepo.GetAll());
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            if (SessionHelper.IsAdmin)
            {
                Appointments = new ObservableCollection<Appointment>(
                    _appointmentRepo.GetAll());
            }
            else
            {
                var doctorId = SessionHelper.CurrentUser.DoctorId ?? 0;
                Appointments = new ObservableCollection<Appointment>(
                    _appointmentRepo.GetByDoctorId(doctorId));
            }
        }

        private void LoadForm(Appointment a)
        {
            SelectedPatient = a.Patient;
            SelectedDoctor = a.Doctor;
            AppointmentDate = a.Date.Date;
            AppointmentHour = a.Date.Hour.ToString("D2");
            AppointmentMinute = a.Date.Minute.ToString("D2");
            SelectedStatus = a.Status;
        }

        private bool CanBook()
            => SelectedPatient != null && SelectedDoctor != null;

        private void Book()
        {
            var date = AppointmentDate.Date
                .AddHours(int.Parse(AppointmentHour))
                .AddMinutes(int.Parse(AppointmentMinute));

            if (IsEditing && SelectedAppointment != null)
            {
                if (_appointmentRepo.IsTimeSlotTaken(
                        SelectedDoctor.DoctorId, date, SelectedAppointment.AppointmentId))
                {
                    MessageBox.Show("Bác sĩ đã có lịch khám vào thời điểm này!",
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SelectedAppointment.PatientId = SelectedPatient.PatientId;
                SelectedAppointment.DoctorId = SelectedDoctor.DoctorId;
                SelectedAppointment.Date = date;
                _appointmentRepo.Update(SelectedAppointment);
                LoadAppointments();
                ClearForm();
                MessageBox.Show("Cập nhật lịch thành công!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var appointment = new Appointment
                {
                    PatientId = SelectedPatient.PatientId,
                    DoctorId = SelectedDoctor.DoctorId,
                    Date = date,
                    Status = "Scheduled"
                };

                if (_service.Book(appointment, out string error))
                {
                    LoadAppointments();
                    ClearForm();
                    MessageBox.Show("Đặt lịch thành công!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(error, "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void UpdateStatus()
        {
            SelectedAppointment.Status = SelectedStatus;
            _appointmentRepo.Update(SelectedAppointment);
            LoadAppointments();
            SelectedStatus = SelectedAppointment.Status;
            MessageBox.Show("Cập nhật trạng thái thành công!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Cancel()
        {
            var result = MessageBox.Show(
                "Hủy lịch khám này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            SelectedAppointment.Status = "Cancelled";
            _appointmentRepo.Update(SelectedAppointment);
            LoadAppointments();
            SelectedStatus = "Cancelled";
        }

        private void Delete()
        {
            var result = MessageBox.Show(
                $"Xóa lịch khám của '{SelectedAppointment.Patient.FullName}' vào lúc {SelectedAppointment.Date:dd/MM/yyyy HH:mm}?",
                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            _appointmentRepo.Delete(SelectedAppointment.AppointmentId);
            LoadAppointments();
            ClearForm();
            MessageBox.Show("Xóa lịch khám thành công!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearForm()
        {
            SelectedPatient = null;
            SelectedDoctor = null;
            AppointmentDate = DateTime.Today.AddDays(1);
            AppointmentHour = "08";
            AppointmentMinute = "00";
            SelectedStatus = null;
            SelectedAppointment = null;
            IsEditing = false;
        }
    }
}