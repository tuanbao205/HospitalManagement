using HospitalManagement.Data;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using HospitalManagement.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagement.ViewModels
{
    public class PatientViewModel : BaseViewModel
    {
        private readonly PatientRepository _repo;

        private ObservableCollection<Patient> _patients;
        public ObservableCollection<Patient> Patients
        {
            get => _patients;
            set => SetProperty(ref _patients, value);
        }

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                SetProperty(ref _selectedPatient, value);
                if (value != null) LoadForm(value);
            }
        }

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private DateTime _dob = DateTime.Today.AddYears(-30);
        public DateTime DOB
        {
            get => _dob;
            set => SetProperty(ref _dob, value);
        }

        private string _gender = "Nam";
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set => SetProperty(ref _searchKeyword, value);
        }

        // Danh sách giới tính cho ComboBox
        public List<string> GenderOptions { get; } = new List<string> { "Nam", "Nữ", "Khác" };

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }

        public PatientViewModel()
        {
            _repo = new PatientRepository(new HospitalDbContext());
            LoadPatients();

            AddCommand = new RelayCommand(_ => Add(), _ => CanSave());
            UpdateCommand = new RelayCommand(_ => Update(), _ => SelectedPatient != null && CanSave());
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedPatient != null);
            SearchCommand = new RelayCommand(_ => Search());
            ClearCommand = new RelayCommand(_ => ClearForm());
        }

        private void LoadPatients()
        {
            Patients = new ObservableCollection<Patient>(_repo.GetAll());
        }

        private void LoadForm(Patient p)
        {
            FullName = p.FullName;
            DOB = p.DOB;
            Gender = p.Gender;
            Phone = p.Phone;
            Address = p.Address;
        }

        private bool CanSave()
            => !string.IsNullOrWhiteSpace(FullName) &&
               !string.IsNullOrWhiteSpace(Phone);

        private void Add()
        {
            var patient = new Patient
            {
                FullName = FullName,
                DOB = DOB,
                Gender = Gender,
                Phone = Phone,
                Address = Address
            };
            _repo.Add(patient);
            LoadPatients();
            ClearForm();
            MessageBox.Show("Thêm bệnh nhân thành công!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Update()
        {
            SelectedPatient.FullName = FullName;
            SelectedPatient.DOB = DOB;
            SelectedPatient.Gender = Gender;
            SelectedPatient.Phone = Phone;
            SelectedPatient.Address = Address;
            _repo.Update(SelectedPatient);
            LoadPatients();
            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Delete()
        {
            var result = MessageBox.Show(
                $"Xóa bệnh nhân '{SelectedPatient.FullName}'?",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            _repo.Delete(SelectedPatient.PatientId);
            LoadPatients();
            ClearForm();
        }

        private void Search()
        {
            var result = _repo.Search(SearchKeyword);
            Patients = new ObservableCollection<Patient>(result);
        }

        private void ClearForm()
        {
            FullName = string.Empty;
            DOB = DateTime.Today.AddYears(-30);
            Gender = "Nam";
            Phone = string.Empty;
            Address = string.Empty;
            SelectedPatient = null;
            SearchKeyword = string.Empty;
            LoadPatients();
        }
    }
}