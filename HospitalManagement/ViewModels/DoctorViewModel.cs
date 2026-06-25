using HospitalManagement.Data;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using HospitalManagement.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagement.ViewModels
{
    public class DoctorViewModel : BaseViewModel
    {
        private readonly DoctorRepository _repo;

        private readonly DepartmentRepository _departmentRepo;

        private ObservableCollection<Doctor> _doctors;

        public ObservableCollection<Doctor> Doctors
        {
            get => _doctors;
            set => SetProperty(ref _doctors, value);
        }

        public ObservableCollection<Department> Departments { get; set; }

        private Doctor _selectedDoctor;
        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                SetProperty(ref _selectedDoctor, value);
                if (value != null) LoadForm(value);
            }
        }

        private Department _selectedDepartment;

        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set => SetProperty(ref _selectedDepartment, value);
        }

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        private string _specialty;
        public string Specialty
        {
            get => _specialty;
            set => SetProperty(ref _specialty, value);
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set => SetProperty(ref _searchKeyword, value);
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }

        public DoctorViewModel()
        {
            _repo = new DoctorRepository(new HospitalDbContext());
            _departmentRepo = new DepartmentRepository(new HospitalDbContext());

            LoadDepartments();
            LoadDoctors();

            AddCommand = new RelayCommand(_ => Add(), _ => CanSave());
            UpdateCommand = new RelayCommand(_ => Update(), _ => SelectedDoctor != null && CanSave());
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedDoctor != null);
            SearchCommand = new RelayCommand(_ => Search());
            ClearCommand = new RelayCommand(_ => ClearForm());
        }

        private void LoadDoctors()
        {
            Doctors = new ObservableCollection<Doctor>(_repo.GetAll());
        }

        private void LoadDepartments()
        {
            Departments = new ObservableCollection<Department>(
                _departmentRepo.GetAll());

            OnPropertyChanged(nameof(Departments));
        }

        private void LoadForm(Doctor d)
        {
            FullName = d.FullName;
            Specialty = d.Specialty;
            Phone = d.Phone;
            Email = d.Email;

            SelectedDepartment =
                Departments.FirstOrDefault(
                    x => x.DepartmentId == d.DepartmentId);
        }

        private bool CanSave()
            => !string.IsNullOrWhiteSpace(FullName) &&
               !string.IsNullOrWhiteSpace(Specialty);

        private bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                error = "Vui lòng nhập họ và tên bác sĩ!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Specialty))
            {
                error = "Vui lòng nhập chuyên khoa!";
                return false;
            }

            if (SelectedDepartment == null)
            {
                error = "Vui lòng chọn khoa!";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Phone) &&
                (Phone.Length < 10 || !Phone.All(char.IsDigit)))
            {
                error = "Số điện thoại không hợp lệ (10 chữ số)!";
                return false;
            }
            if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains("@"))
            {
                error = "Email không hợp lệ!";
                return false;
            }
            error = null;
            return true;
        }

        private void Add()
        {
            if (!Validate(out string error))
            {
                MessageBox.Show(error, "Lỗi nhập liệu",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var doctor = new Doctor
            {
                FullName = FullName,
                Specialty = Specialty,
                Phone = Phone,
                Email = Email,
                DepartmentId = SelectedDepartment.DepartmentId
            };

            _repo.Add(doctor);
            LoadDoctors();
            ClearForm();
            MessageBox.Show("Thêm bác sĩ thành công!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Update()
        {
            if (!Validate(out string error))
            {
                MessageBox.Show(error, "Lỗi nhập liệu",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SelectedDoctor.FullName = FullName;
            SelectedDoctor.Specialty = Specialty;
            SelectedDoctor.Phone = Phone;
            SelectedDoctor.Email = Email;
            SelectedDoctor.DepartmentId = SelectedDepartment.DepartmentId;
            _repo.Update(SelectedDoctor);
            LoadDoctors();
            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Delete()
        {
            var result = MessageBox.Show(
                $"Xóa bác sĩ '{SelectedDoctor.FullName}'?",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            _repo.Delete(SelectedDoctor.DoctorId);
            LoadDoctors();
            ClearForm();
        }

        private void Search()
        {
            var result = _repo.Search(SearchKeyword);
            Doctors = new ObservableCollection<Doctor>(result);
        }

        private void ClearForm()
        {
            FullName = string.Empty;
            Specialty = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;

            SelectedDepartment = null;

            SelectedDoctor = null;
            SearchKeyword = string.Empty;

            LoadDoctors();
        }
    }
}