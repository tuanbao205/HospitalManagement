using HospitalManagement.ViewModels;
using System.Windows;

namespace HospitalManagement.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void PwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
                vm.Password = PwdBox.Password;
        }
    }
}