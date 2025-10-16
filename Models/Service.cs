namespace QLKS.Models
{
    public enum ServiceCategory
    {
        FoodBeverage,   // Ăn uống
        Transportation, // Đưa đón
        Spa,            // Spa
        Laundry,        // Giặt ủi
        RoomService,    // Dịch vụ phòng
        Entertainment,  // Giải trí
        Conference      // Hội nghị
    }

    public enum ServiceStatus
    {
        Available,      // Có sẵn
        Unavailable,    // Không có sẵn
        BookingRequired // Yêu cầu đặt trước
    }

    public class Service
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ServiceCategory Category { get; set; }
        public decimal Price { get; set; }
        public ServiceStatus Status { get; set; }
        public string Unit { get; set; } = string.Empty; // "lần", "giờ", "suất"
        public string ImageUrl { get; set; } = string.Empty;
        public int EstimatedDuration { get; set; } // Thời gian ước tính (phút)
        public bool IsPopular { get; set; }

        public Service()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
