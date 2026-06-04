using HospitalManagement.ViewModels;
using System.Windows;

namespace HospitalManagement.Views
{
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
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