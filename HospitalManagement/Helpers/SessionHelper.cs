using HospitalManagement.Models;

namespace HospitalManagement.Helpers
{
    public static class SessionHelper
    {
        public static User CurrentUser { get; set; }

        public static bool IsAdmin => CurrentUser?.Role == "Admin";
        public static bool IsDoctor => CurrentUser?.Role == "Doctor";

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}