namespace QLKS.Models
{
    public enum RoomStatus
    {
        Available,      // Trống
        Occupied,       // Đang sử dụng
        Maintenance,    // Bảo trì
        Reserved        // Đã đặt
    }

    public enum RoomType
    {
        Standard,       // Tiêu chuẩn
        Deluxe,         // Cao cấp
        Suite,          // Suite
        Presidential    // Tổng thống
    }

    public class Room
    {
        public string Id { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public RoomType Type { get; set; }
        public RoomStatus Status { get; set; }
        public decimal PricePerNight { get; set; }
        public int Floor { get; set; }
        public int MaxGuests { get; set; }
        public string BedType { get; set; } = string.Empty;
        public double Size { get; set; } // m2
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Amenities { get; set; } = new();
        public DateTime? LastMaintenanceDate { get; set; }

        public Room()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
