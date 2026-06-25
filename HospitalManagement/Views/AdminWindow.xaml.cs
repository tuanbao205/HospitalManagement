using HospitalManagement.Helpers;
using System.Windows;

namespace HospitalManagement.Views
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            lblUsername.Text = SessionHelper.CurrentUser?.Username;
            MainFrame.Navigate(new Pages.DashboardPage());
        }
        private void BtnDepartment_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Pages.DepartmentPage());

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Pages.DashboardPage());
        private void BtnPatients_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Pages.PatientPage());

        private void BtnDoctors_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Pages.DoctorPage());

        private void BtnAppointments_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Pages.AppointmentPage());

        private void BtnMedicalRecords_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Pages.MedicalRecordPage());
        private void BtnUsers_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new Pages.UserPage());

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionHelper.Logout();
            new LoginWindow().Show();
            Close();
        }
    }
}