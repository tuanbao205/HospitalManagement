using HospitalManagement.Data;
using HospitalManagement.Helpers;
using HospitalManagement.ViewModels.Base;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagement.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set => SetProperty(ref _oldPassword, value);
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public ICommand ChangePasswordCommand { get; }
        public ICommand CancelCommand { get; }

        public ChangePasswordViewModel()
        {
            ChangePasswordCommand = new RelayCommand(_ => ChangePassword(), _ => CanChange());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private bool CanChange()
            => !string.IsNullOrWhiteSpace(OldPassword) &&
               !string.IsNullOrWhiteSpace(NewPassword) &&
               !string.IsNullOrWhiteSpace(ConfirmPassword);

        private void ChangePassword()
        {
            // Kiểm tra mật khẩu cũ
            if (OldPassword != SessionHelper.CurrentUser.Password)
            {
                ErrorMessage = "Mật khẩu cũ không đúng!";
                HasError = true;
                return;
            }

            // Kiểm tra mật khẩu mới
            if (NewPassword.Length < 6)
            {
                ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự!";
                HasError = true;
                return;
            }

            // Kiểm tra xác nhận mật khẩu
            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Mật khẩu xác nhận không khớp!";
                HasError = true;
                return;
            }

            // Cập nhật mật khẩu
            using (var db = new HospitalDbContext())
            {
                var user = db.Users.FirstOrDefault(
                    u => u.UserId == SessionHelper.CurrentUser.UserId);
                if (user == null) return;

                user.Password = NewPassword;
                db.SaveChanges();

                // Cập nhật session
                SessionHelper.CurrentUser.Password = NewPassword;
            }

            HasError = false;
            MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);

            // Đóng cửa sổ
            Cancel();
        }

        private void Cancel()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w is Views.ChangePasswordWindow)
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}