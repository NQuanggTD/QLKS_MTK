namespace QLKS.Models
{
    public enum BookingStatus
    {
        Pending,        // Chờ xác nhận
        Confirmed,      // Đã xác nhận
        CheckedIn,      // Đã nhận phòng
        CheckedOut,     // Đã trả phòng
        Cancelled       // Đã hủy
    }

    public enum PaymentStatus
    {
        Pending,        // Chờ thanh toán
        Paid,           // Đã thanh toán
        Refunded        // Đã hoàn tiền
    }

    public class Booking
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public BookingStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal Deposit { get; set; }
        public int NumberOfGuests { get; set; }
        public List<string> Services { get; set; } = new();
        public string Notes { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public DateTime? ActualCheckInTime { get; set; }
        public DateTime? ActualCheckOutTime { get; set; }

        public Booking()
        {
            Id = Guid.NewGuid().ToString();
            BookingDate = DateTime.Now;
            Status = BookingStatus.Pending;
            PaymentStatus = PaymentStatus.Pending;
        }

        public int GetNumberOfNights()
        {
            return (CheckOutDate.Date - CheckInDate.Date).Days;
        }
    }
}
