using HospitalManagement.Data;
using HospitalManagement.Helpers;
using HospitalManagement.ViewModels.Base;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagement.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _hasError;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password);
        }
        private void ExecuteLogin(object parameter)
        {
            using (var db = new HospitalDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == Username);

                if (user == null || user.Password != Password)
                {
                    ErrorMessage = "Sai tên đăng nhập hoặc mật khẩu!";
                    HasError = true;
                    return;
                }

                HasError = false;
                SessionHelper.CurrentUser = user;

                if (SessionHelper.IsAdmin)
                    Application.Current.MainWindow = new Views.AdminWindow();
                else
                    Application.Current.MainWindow = new Views.DoctorWindow();

                Application.Current.MainWindow.Show();

                foreach (Window w in Application.Current.Windows)
                {
                    if (w is Views.LoginWindow)
                    {
                        w.Close();
                        break;
                    }
                }
            }
        }
    }
}