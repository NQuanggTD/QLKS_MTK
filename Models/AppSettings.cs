namespace QLKS.Models
{
    public class AppSettings
    {
        public string HotelName { get; set; } = "Hotel Management System";
        public string HotelAddress { get; set; } = "";
        public string HotelPhone { get; set; } = "";
        public string HotelEmail { get; set; } = "";
        public string Currency { get; set; } = "VNĐ";
        public int CheckInTime { get; set; } = 14; // 14:00
        public int CheckOutTime { get; set; } = 12; // 12:00
        public decimal VIPDiscount { get; set; } = 0.1m; // 10%
        public bool EnableEmailNotifications { get; set; } = false;
        public bool EnableSMSNotifications { get; set; } = false;
        public int AutoRefreshDashboard { get; set; } = 30; // seconds
        public string Theme { get; set; } = "default"; // default, dark
        public string Language { get; set; } = "vi"; // vi, en
        public bool ShowWelcomeScreen { get; set; } = true;
        public int BookingExpiryHours { get; set; } = 24;
        public decimal DepositPercentage { get; set; } = 0.3m; // 30%
    }
}
