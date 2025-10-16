using QLKS.Models;
using QLKS.Utils;

namespace QLKS.Services
{
    public interface IDashboardService
    {
        DashboardData GetDashboardData();
        List<BookingSummary> GetRecentBookings(int count = 10);
        Dictionary<string, decimal> GetRevenueByMonth(int year);
        Dictionary<string, int> GetBookingStatsByStatus();
    }

    public class DashboardService : IDashboardService
    {
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;
        private readonly ICustomerService _customerService;

        public DashboardService(
            IRoomService roomService,
            IBookingService bookingService,
            ICustomerService customerService)
        {
            _roomService = roomService;
            _bookingService = bookingService;
            _customerService = customerService;
        }

        public DashboardData GetDashboardData()
        {
            var rooms = _roomService.GetAllRooms();
            var bookings = _bookingService.GetAllBookings();
            var customers = _customerService.GetAllCustomers();

            var today = DateTime.Now.Date;

            var data = new DashboardData
            {
                TotalRooms = rooms.Count,
                AvailableRooms = rooms.Count(r => r.Status == RoomStatus.Available),
                OccupiedRooms = rooms.Count(r => r.Status == RoomStatus.Occupied),
                MaintenanceRooms = rooms.Count(r => r.Status == RoomStatus.Maintenance),
                ReservedRooms = rooms.Count(r => r.Status == RoomStatus.Reserved),
                
                TotalBookings = bookings.Count,
                PendingBookings = bookings.Count(b => b.Status == BookingStatus.Pending),
                ConfirmedBookings = bookings.Count(b => b.Status == BookingStatus.Confirmed),
                CheckedInBookings = bookings.Count(b => b.Status == BookingStatus.CheckedIn),
                
                TotalCustomers = customers.Count,
                NewBookingsToday = bookings.Count(b => b.BookingDate.Date == today),
                
                TotalRevenue = bookings
                    .Where(b => b.Status == BookingStatus.CheckedOut)
                    .Sum(b => b.TotalAmount),
                
                EstimatedRevenue = bookings
                    .Where(b => b.Status == BookingStatus.CheckedIn || b.Status == BookingStatus.Confirmed)
                    .Sum(b => b.TotalAmount)
            };

            // Recent bookings
            data.RecentBookings = GetRecentBookings(5);

            // Room occupancy by type
            var roomTypes = Enum.GetValues(typeof(RoomType)).Cast<RoomType>();
            foreach (var type in roomTypes)
            {
                var roomsOfType = rooms.Where(r => r.Type == type).ToList();
                if (roomsOfType.Any())
                {
                    var occupied = roomsOfType.Count(r => r.Status == RoomStatus.Occupied || r.Status == RoomStatus.Reserved);
                    data.RoomOccupancyByType.Add(new RoomOccupancy
                    {
                        RoomType = GetRoomTypeName(type),
                        Total = roomsOfType.Count,
                        Occupied = occupied,
                        OccupancyRate = (double)occupied / roomsOfType.Count * 100
                    });
                }
            }

            Logger.Info("Generated dashboard data");
            return data;
        }

        public List<BookingSummary> GetRecentBookings(int count = 10)
        {
            var bookings = _bookingService.GetAllBookings()
                .OrderByDescending(b => b.BookingDate)
                .Take(count)
                .ToList();

            var summaries = new List<BookingSummary>();
            foreach (var booking in bookings)
            {
                var customer = _customerService.GetCustomerById(booking.CustomerId);
                var room = _roomService.GetRoomById(booking.RoomId);

                if (customer != null && room != null)
                {
                    summaries.Add(new BookingSummary
                    {
                        BookingId = booking.Id,
                        CustomerName = customer.FullName,
                        RoomNumber = room.RoomNumber,
                        CheckInDate = booking.CheckInDate,
                        CheckOutDate = booking.CheckOutDate,
                        Status = GetBookingStatusName(booking.Status),
                        Amount = booking.TotalAmount
                    });
                }
            }

            return summaries;
        }

        public Dictionary<string, decimal> GetRevenueByMonth(int year)
        {
            var bookings = _bookingService.GetAllBookings()
                .Where(b => b.CheckOutDate.Year == year && b.Status == BookingStatus.CheckedOut)
                .ToList();

            var revenueByMonth = new Dictionary<string, decimal>();
            for (int month = 1; month <= 12; month++)
            {
                var monthName = new DateTime(year, month, 1).ToString("MM/yyyy");
                var revenue = bookings
                    .Where(b => b.CheckOutDate.Month == month)
                    .Sum(b => b.TotalAmount);
                revenueByMonth[monthName] = revenue;
            }

            return revenueByMonth;
        }

        public Dictionary<string, int> GetBookingStatsByStatus()
        {
            var bookings = _bookingService.GetAllBookings();
            var stats = new Dictionary<string, int>();

            var statuses = Enum.GetValues(typeof(BookingStatus)).Cast<BookingStatus>();
            foreach (var status in statuses)
            {
                var count = bookings.Count(b => b.Status == status);
                stats[GetBookingStatusName(status)] = count;
            }

            return stats;
        }

        private string GetRoomTypeName(RoomType type)
        {
            return type switch
            {
                RoomType.Standard => "Phòng tiêu chuẩn",
                RoomType.Deluxe => "Phòng Deluxe",
                RoomType.Suite => "Suite",
                RoomType.Presidential => "Phòng Tổng thống",
                _ => type.ToString()
            };
        }

        private string GetBookingStatusName(BookingStatus status)
        {
            return status switch
            {
                BookingStatus.Pending => "Chờ xác nhận",
                BookingStatus.Confirmed => "Đã xác nhận",
                BookingStatus.CheckedIn => "Đã nhận phòng",
                BookingStatus.CheckedOut => "Đã trả phòng",
                BookingStatus.Cancelled => "Đã hủy",
                _ => status.ToString()
            };
        }
    }
}
