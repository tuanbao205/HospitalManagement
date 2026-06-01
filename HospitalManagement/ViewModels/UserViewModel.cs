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
    public class UserViewModel : BaseViewModel
    {
        private readonly UserRepository _userRepo;
        private readonly DoctorRepository _doctorRepo;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value);
                if (value != null) LoadForm(value);
            }
        }

        private ObservableCollection<Doctor> _doctors;
        public ObservableCollection<Doctor> Doctors
        {
            get => _doctors;
            set => SetProperty(ref _doctors, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _selectedRole = "Admin";
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                SetProperty(ref _selectedRole, value);
                OnPropertyChanged(nameof(IsDoctorRole));
            }
        }

        private Doctor _selectedDoctor;
        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set => SetProperty(ref _selectedDoctor, value);
        }

        public bool IsDoctorRole => SelectedRole == "Doctor";

        public List<string> RoleOptions { get; } = new List<string> { "Admin", "Doctor" };

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public UserViewModel()
        {
            var db = new HospitalDbContext();
            _userRepo = new UserRepository(db);
            _doctorRepo = new DoctorRepository(db);

            LoadData();

            AddCommand = new RelayCommand(_ => Add(), _ => CanSave());
            UpdateCommand = new RelayCommand(_ => Update(), _ => SelectedUser != null && CanSave());
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedUser != null);
            ClearCommand = new RelayCommand(_ => ClearForm());
        }

        private void LoadData()
        {
            Users = new ObservableCollection<User>(_userRepo.GetAll());
            LoadAvailableDoctors(null);
        }

        private void LoadAvailableDoctors(int? currentDoctorId)
        {
            var usedDoctorIds = _userRepo.GetAll()
                .Where(u => u.DoctorId.HasValue)
                .Select(u => u.DoctorId.Value)
                .ToList();

            var availableDoctors = _doctorRepo.GetAll()
                .Where(d => !usedDoctorIds.Contains(d.DoctorId)
                         || d.DoctorId == currentDoctorId)
                .ToList();

            Doctors = new ObservableCollection<Doctor>(availableDoctors);
        }

        private void LoadForm(User u)
        {
            Username = u.Username;
            Password = u.Password;
            SelectedRole = u.Role;
            LoadAvailableDoctors(u.DoctorId);
            SelectedDoctor = u.DoctorId.HasValue
                ? _doctorRepo.GetById(u.DoctorId.Value)
                : null;
        }

        private bool CanSave()
            => !string.IsNullOrWhiteSpace(Username) &&
               !string.IsNullOrWhiteSpace(Password);

        private bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                error = "Vui lòng nhập tên đăng nhập!";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
            {
                error = "Mật khẩu phải có ít nhất 6 ký tự!";
                return false;
            }
            if (SelectedRole == "Doctor" && SelectedDoctor == null)
            {
                error = "Vui lòng chọn bác sĩ cho tài khoản Doctor!";
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
            if (_userRepo.UsernameExists(Username))
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var user = new User
            {
                Username = Username,
                Password = Password,
                Role = SelectedRole,
                DoctorId = SelectedRole == "Doctor" ? SelectedDoctor?.DoctorId : null
            };
            _userRepo.Add(user);
            LoadData();
            ClearForm();
            MessageBox.Show("Thêm tài khoản thành công!", "Thông báo",
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
            if (_userRepo.UsernameExists(Username, SelectedUser.UserId))
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SelectedUser.Username = Username;
            SelectedUser.Password = Password;
            SelectedUser.Role = SelectedRole;
            SelectedUser.DoctorId = SelectedRole == "Doctor" ? SelectedDoctor?.DoctorId : null;
            _userRepo.Update(SelectedUser);
            LoadData();
            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Delete()
        {
            var result = MessageBox.Show(
                $"Xóa tài khoản '{SelectedUser.Username}'?",
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            _userRepo.Delete(SelectedUser.UserId);
            LoadData();
            ClearForm();
        }

        private void ClearForm()
        {
            Username = string.Empty;
            Password = string.Empty;
            SelectedRole = "Admin";
            SelectedDoctor = null;
            SelectedUser = null;
            LoadAvailableDoctors(null);
        }
    }
}