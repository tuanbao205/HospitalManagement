using HospitalManagement.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HospitalManagement.Pages
{
    public partial class ChangePasswordPage : Page
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private void PwdOld_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ChangePasswordViewModel vm)
                vm.OldPassword = PwdOld.Password;
        }

        private void PwdNew_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ChangePasswordViewModel vm)
                vm.NewPassword = PwdNew.Password;
        }

        private void PwdConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ChangePasswordViewModel vm)
                vm.ConfirmPassword = PwdConfirm.Password;
        }
    }
}