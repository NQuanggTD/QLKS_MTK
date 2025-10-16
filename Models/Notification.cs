namespace QLKS.Models
{
    public enum NotificationType
    {
        Info,           // Thông tin
        Success,        // Thành công
        Warning,        // Cảnh báo
        Error,          // Lỗi
        Booking,        // Liên quan đến booking
        Payment,        // Liên quan đến thanh toán
        System          // Hệ thống
    }

    public class Notification
    {
        public string Id { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public string Link { get; set; } = string.Empty;
        public string RelatedId { get; set; } = string.Empty; // BookingId, CustomerId, etc.

        public Notification()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            IsRead = false;
        }
    }
}
