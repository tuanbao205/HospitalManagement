using HospitalManagement.Data;
using HospitalManagement.Helpers;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using HospitalManagement.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagement.ViewModels
{
    public class DepartmentViewModel : BaseViewModel
    {
        private readonly DepartmentRepository _repo;

        public ObservableCollection<Department> Departments { get; set; }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                SetProperty(ref _selectedDepartment, value);

                if (value != null)
                {
                    DepartmentName = value.DepartmentName;
                    Description = value.Description;
                }
            }
        }

        private string _departmentName;
        public string DepartmentName
        {
            get => _departmentName;
            set => SetProperty(ref _departmentName, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
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

        public DepartmentViewModel()
        {
            _repo = new DepartmentRepository(new HospitalDbContext());

            LoadDepartments();

            AddCommand = new RelayCommand(_ => Add());
            UpdateCommand = new RelayCommand(_ => Update(), _ => SelectedDepartment != null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedDepartment != null);
            SearchCommand = new RelayCommand(_ => Search());
            ClearCommand = new RelayCommand(_ => ClearForm());
        }

        private void LoadDepartments()
        {
            Departments =
                new ObservableCollection<Department>(_repo.GetAll());

            OnPropertyChanged(nameof(Departments));
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(DepartmentName))
            {
                MessageBox.Show("Vui lòng nhập tên phòng ban!");
                return;
            }

            _repo.Add(new Department
            {
                DepartmentName = DepartmentName,
                Description = Description
            });

            LoadDepartments();
            ClearForm();
        }

        private void Update()
        {
            SelectedDepartment.DepartmentName = DepartmentName;
            SelectedDepartment.Description = Description;

            _repo.Update(SelectedDepartment);

            LoadDepartments();

            MessageBox.Show("Cập nhật thành công!");
        }

        private void Delete()
        {
            if (MessageBox.Show(
                "Bạn có chắc muốn xóa?",
                "Xác nhận",
                MessageBoxButton.YesNo)
                != MessageBoxResult.Yes)
                return;

            try
            {
                _repo.Delete(SelectedDepartment.DepartmentId);
                LoadDepartments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            LoadDepartments();
            ClearForm();
        }

        private void Search()
        {
            Departments =
                new ObservableCollection<Department>(
                    _repo.Search(SearchKeyword));

            OnPropertyChanged(nameof(Departments));
        }

        private void ClearForm()
        {
            DepartmentName = "";
            Description = "";
            SearchKeyword = "";

            SelectedDepartment = null;

            LoadDepartments();
        }
    }
}