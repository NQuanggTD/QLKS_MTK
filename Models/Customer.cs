namespace QLKS.Models
{
    public enum CustomerType
    {
        Regular,    // Thường
        VIP         // VIP
    }

    public class Customer
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string IdCard { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; } = "Việt Nam";
        public CustomerType CustomerType { get; set; } = CustomerType.Regular;
        public int LoyaltyPoints { get; set; }
        public int TotalBookings { get; set; }
        public string SpecialNotes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<string> BookingHistory { get; set; } = new();

        public Customer()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }
    }
}
