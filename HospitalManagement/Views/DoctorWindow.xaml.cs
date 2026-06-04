using HospitalManagement.Helpers;
using System.Windows;

namespace HospitalManagement.Views
{
    public partial class DoctorWindow : Window
    {
        public DoctorWindow()
        {
            InitializeComponent();
            lblUsername.Text = SessionHelper.CurrentUser?.Username;
            MainFrame.Navigate(new Pages.AppointmentPage());
        }

        private void BtnMyAppointments_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Pages.AppointmentPage());

        private void BtnMedicalRecords_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Pages.MedicalRecordPage());

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Logout();
            new LoginWindow().Show();
            Close();
        }
        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Pages.ChangePasswordPage());
    }
}