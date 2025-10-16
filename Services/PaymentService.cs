using QLKS.Models;
using QLKS.Utils;

namespace QLKS.Services
{
    public interface IPaymentService
    {
        decimal CalculateRoomCharge(Booking booking);
        decimal CalculateServiceCharge(List<string> serviceIds, List<int> quantities);
        decimal CalculateTotalAmount(Booking booking, List<string> serviceIds, List<int> quantities);
        PaymentSummary GeneratePaymentSummary(string bookingId);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly IServiceManagementService _serviceManagementService;
        private readonly ICustomerService _customerService;

        public PaymentService(
            IBookingService bookingService,
            IRoomService roomService,
            IServiceManagementService serviceManagementService,
            ICustomerService customerService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _serviceManagementService = serviceManagementService;
            _customerService = customerService;
        }

        public decimal CalculateRoomCharge(Booking booking)
        {
            var room = _roomService.GetRoomById(booking.RoomId);
            if (room == null)
            {
                throw new InvalidOperationException("Phòng không tồn tại");
            }

            var numberOfNights = booking.GetNumberOfNights();
            return room.PricePerNight * numberOfNights;
        }

        public decimal CalculateServiceCharge(List<string> serviceIds, List<int> quantities)
        {
            if (serviceIds.Count != quantities.Count)
            {
                throw new ArgumentException("Số lượng dịch vụ và số lượng không khớp");
            }

            decimal total = 0;
            for (int i = 0; i < serviceIds.Count; i++)
            {
                var service = _serviceManagementService.GetServiceById(serviceIds[i]);
                if (service != null)
                {
                    total += service.Price * quantities[i];
                }
            }

            return total;
        }

        public decimal CalculateTotalAmount(Booking booking, List<string> serviceIds, List<int> quantities)
        {
            var roomCharge = CalculateRoomCharge(booking);
            var serviceCharge = CalculateServiceCharge(serviceIds, quantities);
            return roomCharge + serviceCharge;
        }

        public PaymentSummary GeneratePaymentSummary(string bookingId)
        {
            var booking = _bookingService.GetBookingById(bookingId);
            if (booking == null)
            {
                throw new InvalidOperationException("Booking không tồn tại");
            }

            var customer = _customerService.GetCustomerById(booking.CustomerId);
            var room = _roomService.GetRoomById(booking.RoomId);

            if (customer == null || room == null)
            {
                throw new InvalidOperationException("Dữ liệu không hợp lệ");
            }

            var summary = new PaymentSummary
            {
                BookingId = booking.Id,
                CustomerName = customer.FullName,
                RoomNumber = room.RoomNumber,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                NumberOfNights = booking.GetNumberOfNights(),
                RoomPricePerNight = room.PricePerNight,
                RoomCharge = CalculateRoomCharge(booking),
                PaymentMethod = booking.PaymentMethod,
                Deposit = booking.Deposit
            };

            // Add services
            var quantities = new List<int>();
            if (booking.Services != null && booking.Services.Count > 0)
            {
                foreach (var serviceName in booking.Services)
                {
                    summary.Services.Add(new ServiceCharge
                    {
                        ServiceName = serviceName,
                        Quantity = 1,
                        UnitPrice = 0, // Service price not tracked in this version
                        Total = 0
                    });
                    quantities.Add(1);
                }
            }

            summary.ServiceCharge = 0; // Services are now strings, not priced entities
            summary.SubTotal = summary.RoomCharge + summary.ServiceCharge;
            
            // Apply discount for VIP customers
            if (customer.CustomerType == CustomerType.VIP)
            {
                summary.Discount = summary.SubTotal * 0.1m; // 10% discount
                summary.DiscountReason = "Khách hàng VIP - Giảm 10%";
            }

            summary.TotalAmount = summary.SubTotal - summary.Discount;
            summary.AmountDue = summary.TotalAmount - summary.Deposit;

            Logger.Info($"Generated payment summary for booking: {bookingId}");
            return summary;
        }
    }

    public class PaymentSummary
    {
        public string BookingId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        public decimal RoomPricePerNight { get; set; }
        public decimal RoomCharge { get; set; }
        public List<ServiceCharge> Services { get; set; } = new();
        public decimal ServiceCharge { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public string DiscountReason { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal Deposit { get; set; }
        public decimal AmountDue { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
    }

    public class ServiceCharge
    {
        public string ServiceName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}
