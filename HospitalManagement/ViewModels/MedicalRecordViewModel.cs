using HospitalManagement.Data;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using HospitalManagement.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagement.ViewModels
{
    public class MedicalRecordViewModel : BaseViewModel
    {
        private readonly MedicalRecordRepository _recordRepo;
        private readonly AppointmentRepository _appointmentRepo;

        private ObservableCollection<MedicalRecord> _records;
        public ObservableCollection<MedicalRecord> Records
        {
            get => _records;
            set => SetProperty(ref _records, value);
        }

        private MedicalRecord _selectedRecord;
        public MedicalRecord SelectedRecord
        {
            get => _selectedRecord;
            set
            {
                SetProperty(ref _selectedRecord, value);
                if (value != null) LoadForm(value);
            }
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
                    PatientInfo = value.Patient?.FullName;
                    DoctorInfo = value.Doctor?.FullName;
                    AppointmentDate = value.Date.ToString("dd/MM/yyyy HH:mm");
                    // Reset editing mode khi chọn lịch khám mới
                    SelectedRecord = null;
                    IsEditing = false;
                    Diagnosis = string.Empty;
                    Prescription = string.Empty;
                    Note = string.Empty;
                }
            }
        }

        private string _diagnosis;
        public string Diagnosis
        {
            get => _diagnosis;
            set => SetProperty(ref _diagnosis, value);
        }

        private string _prescription;
        public string Prescription
        {
            get => _prescription;
            set => SetProperty(ref _prescription, value);
        }

        private string _note;
        public string Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }

        private string _patientInfo;
        public string PatientInfo
        {
            get => _patientInfo;
            set => SetProperty(ref _patientInfo, value);
        }

        private string _doctorInfo;
        public string DoctorInfo
        {
            get => _doctorInfo;
            set => SetProperty(ref _doctorInfo, value);
        }

        private string _appointmentDate;
        public string AppointmentDate
        {
            get => _appointmentDate;
            set => SetProperty(ref _appointmentDate, value);
        }

        public bool IsDoctor => SessionHelper.IsDoctor;

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                SetProperty(ref _isEditing, value);
                OnPropertyChanged(nameof(SaveButtonText));
            }
        }

        public string SaveButtonText => _isEditing ? "✏️ Cập nhật hồ sơ" : "💾 Lưu hồ sơ";

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand PrintCommand { get; }

        public MedicalRecordViewModel()
        {
            var db = new HospitalDbContext();
            _recordRepo = new MedicalRecordRepository(db);
            _appointmentRepo = new AppointmentRepository(db);

            LoadData();

            SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedRecord != null && IsDoctor);
            ClearCommand = new RelayCommand(_ => ClearForm());
            PrintCommand = new RelayCommand(_ => Print(), _ => SelectedRecord != null);
        }

        private void LoadData()
        {
            LoadRecords();
            LoadAppointments();
        }

        private void LoadRecords()
        {
            if (SessionHelper.IsAdmin)
                Records = new ObservableCollection<MedicalRecord>(_recordRepo.GetAll());
            else
            {
                var doctorId = SessionHelper.CurrentUser.DoctorId ?? 0;
                Records = new ObservableCollection<MedicalRecord>(
                    _recordRepo.GetByDoctorId(doctorId));
            }
        }

        private void LoadAppointments()
        {
            var allAppointments = SessionHelper.IsAdmin
                ? _appointmentRepo.GetAll()
                : _appointmentRepo.GetByDoctorId(
                    SessionHelper.CurrentUser.DoctorId ?? 0);

            var available = new ObservableCollection<Appointment>();
            foreach (var a in allAppointments)
            {
                if (!_recordRepo.ExistsByAppointmentId(a.AppointmentId))
                    available.Add(a);
            }
            Appointments = available;
        }

        private void LoadForm(MedicalRecord r)
        {
            Diagnosis = r.Diagnosis;
            Prescription = r.Prescription;
            Note = r.Note;
            PatientInfo = r.Appointment?.Patient?.FullName;
            DoctorInfo = r.Appointment?.Doctor?.FullName;
            AppointmentDate = r.Appointment?.Date.ToString("dd/MM/yyyy HH:mm");
            IsEditing = true;
        }

        private bool CanSave()
        {
            if (IsEditing) return !string.IsNullOrWhiteSpace(Diagnosis);
            return SelectedAppointment != null &&
                   !string.IsNullOrWhiteSpace(Diagnosis);
        }

        private void Save()
        {
            if (IsEditing && SelectedRecord != null)
            {
                SelectedRecord.Diagnosis = Diagnosis;
                SelectedRecord.Prescription = Prescription;
                SelectedRecord.Note = Note;
                _recordRepo.Update(SelectedRecord);
                LoadData();
                MessageBox.Show("Cập nhật hồ sơ thành công!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var record = new MedicalRecord
                {
                    RecordId = SelectedAppointment.AppointmentId,
                    Diagnosis = Diagnosis,
                    Prescription = Prescription,
                    Note = Note,
                    CreatedDate = DateTime.Now
                };
                _recordRepo.Add(record);

                SelectedAppointment.Status = "Done";
                _appointmentRepo.Update(SelectedAppointment);

                LoadData();
                ClearForm();
                MessageBox.Show("Tạo hồ sơ bệnh án thành công!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Delete()
        {
            var result = MessageBox.Show(
                "Xóa hồ sơ bệnh án này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            _recordRepo.Delete(SelectedRecord.RecordId);
            LoadData();
            ClearForm();
        }

        private void ClearForm()
        {
            IsEditing = false;
            Diagnosis = string.Empty;
            Prescription = string.Empty;
            Note = string.Empty;
            PatientInfo = string.Empty;
            DoctorInfo = string.Empty;
            AppointmentDate = string.Empty;
            SelectedRecord = null;
            SelectedAppointment = null;
        }
        private void Print()
        {
            if (SelectedRecord == null) return;
            PrintHelper.PrintMedicalRecord(SelectedRecord);
        }
    }
}