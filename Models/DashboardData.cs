namespace QLKS.Models
{
    public class DashboardData
    {
        public int TotalRooms { get; set; }
        public int AvailableRooms { get; set; }
        public int OccupiedRooms { get; set; }
        public int MaintenanceRooms { get; set; }
        public int ReservedRooms { get; set; }
        public int TotalBookings { get; set; }
        public int PendingBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public int CheckedInBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal EstimatedRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public int NewBookingsToday { get; set; }
        public List<BookingSummary> RecentBookings { get; set; } = new();
        public List<RoomOccupancy> RoomOccupancyByType { get; set; } = new();
    }

    public class BookingSummary
    {
        public string BookingId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class RoomOccupancy
    {
        public string RoomType { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Occupied { get; set; }
        public double OccupancyRate { get; set; }
    }
}
