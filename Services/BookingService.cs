using QLKS.Models;
using QLKS.Utils;

namespace QLKS.Services
{
    public interface IBookingService
    {
        List<Booking> GetAllBookings();
        Booking? GetBookingById(string id);
        List<Booking> GetBookingsByCustomer(string customerId);
        List<Booking> GetBookingsByRoom(string roomId);
        List<Booking> GetBookingsByStatus(BookingStatus status);
        List<Booking> GetBookingsByDateRange(DateTime startDate, DateTime endDate);
        Booking CreateBooking(Booking booking);
        Booking? UpdateBooking(Booking booking);
        bool CancelBooking(string id);
        bool CheckIn(string id);
        bool CheckOut(string id);
    }

    public class BookingService : IBookingService
    {
        private readonly IDataStore _dataStore;
        private readonly IRoomService _roomService;
        private readonly ICustomerService _customerService;
        private readonly INotificationService _notificationService;
        private const string DataKey = "bookings";

        public BookingService(IDataStore dataStore, IRoomService roomService, ICustomerService customerService, INotificationService notificationService)
        {
            _dataStore = dataStore;
            _roomService = roomService;
            _customerService = customerService;
            _notificationService = notificationService;
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            var bookings = _dataStore.Load<List<Booking>>(DataKey);
            if (bookings == null || !bookings.Any())
            {
                var customers = _customerService.GetAllCustomers();
                var rooms = _roomService.GetAllRooms();

                if (customers.Any() && rooms.Any())
                {
                    bookings = new List<Booking>
                    {
                        new Booking
                        {
                            CustomerId = customers[0].Id,
                            RoomId = rooms[2].Id, // Suite room
                            CheckInDate = DateTime.Now,
                            CheckOutDate = DateTime.Now.AddDays(3),
                            Status = BookingStatus.CheckedIn,
                            PaymentMethod = "CreditCard",
                            PaymentStatus = PaymentStatus.Paid,
                            NumberOfGuests = 2,
                            TotalAmount = rooms[2].PricePerNight * 3,
                            Deposit = rooms[2].PricePerNight,
                            Notes = "Phòng tầng cao, view đẹp",
                            Services = new List<string> { "Bữa sáng buffet", "Đưa đón sân bay" },
                            ActualCheckInTime = DateTime.Now
                        },
                        new Booking
                        {
                            CustomerId = customers[1].Id,
                            RoomId = rooms[1].Id, // Standard room
                            CheckInDate = DateTime.Now.AddDays(2),
                            CheckOutDate = DateTime.Now.AddDays(5),
                            Status = BookingStatus.Confirmed,
                            PaymentMethod = "Cash",
                            PaymentStatus = PaymentStatus.Pending,
                            NumberOfGuests = 2,
                            TotalAmount = rooms[1].PricePerNight * 3,
                            Deposit = rooms[1].PricePerNight * 0.5m,
                            Notes = "Không hút thuốc",
                            Services = new List<string> { "Bữa sáng" }
                        }
                    };

                    _dataStore.Save(DataKey, bookings);
                    Logger.Info("Initialized sample booking data");
                }
            }
        }

        public List<Booking> GetAllBookings()
        {
            return _dataStore.Load<List<Booking>>(DataKey) ?? new List<Booking>();
        }

        public Booking? GetBookingById(string id)
        {
            var bookings = GetAllBookings();
            return bookings.FirstOrDefault(b => b.Id == id);
        }

        public List<Booking> GetBookingsByCustomer(string customerId)
        {
            var bookings = GetAllBookings();
            return bookings.Where(b => b.CustomerId == customerId).ToList();
        }

        public List<Booking> GetBookingsByRoom(string roomId)
        {
            var bookings = GetAllBookings();
            return bookings.Where(b => b.RoomId == roomId).ToList();
        }

        public List<Booking> GetBookingsByStatus(BookingStatus status)
        {
            var bookings = GetAllBookings();
            return bookings.Where(b => b.Status == status).ToList();
        }

        public List<Booking> GetBookingsByDateRange(DateTime startDate, DateTime endDate)
        {
            var bookings = GetAllBookings();
            return bookings.Where(b => 
                b.CheckInDate <= endDate && b.CheckOutDate >= startDate
            ).ToList();
        }

        public Booking CreateBooking(Booking booking)
        {
            var validation = Validator.ValidateBooking(booking);
            if (!validation.IsValid)
            {
                throw new InvalidOperationException(validation.GetErrorMessage());
            }

            // Verify customer exists
            var customer = _customerService.GetCustomerById(booking.CustomerId);
            if (customer == null)
            {
                throw new InvalidOperationException("Khách hàng không tồn tại");
            }

            // Verify room exists and is available
            var room = _roomService.GetRoomById(booking.RoomId);
            if (room == null)
            {
                throw new InvalidOperationException("Phòng không tồn tại");
            }

            if (room.Status != RoomStatus.Available)
            {
                throw new InvalidOperationException("Phòng không khả dụng");
            }

            // Check room capacity
            if (booking.NumberOfGuests > room.MaxGuests)
            {
                throw new InvalidOperationException($"Phòng chỉ chứa tối đa {room.MaxGuests} người");
            }

            // Calculate total amount
            var numberOfNights = booking.GetNumberOfNights();
            booking.TotalAmount = room.PricePerNight * numberOfNights;

            booking.Id = Guid.NewGuid().ToString();
            booking.BookingDate = DateTime.Now;
            booking.Status = BookingStatus.Pending;

            var bookings = GetAllBookings();
            bookings.Add(booking);
            _dataStore.Save(DataKey, bookings);

            // Update room status to Reserved
            _roomService.UpdateRoomStatus(room.Id, RoomStatus.Reserved);

            // Add to customer booking history
            customer.BookingHistory.Add(booking.Id);
            _customerService.UpdateCustomer(customer);

            // Create notification
            _notificationService.CreateBookingNotification(booking.Id, customer.FullName, room.RoomNumber);

            Logger.Info($"Created new booking: {booking.Id} for customer {customer.FullName}");
            return booking;
        }

        public Booking? UpdateBooking(Booking booking)
        {
            var validation = Validator.ValidateBooking(booking);
            if (!validation.IsValid)
            {
                throw new InvalidOperationException(validation.GetErrorMessage());
            }

            var bookings = GetAllBookings();
            var existingBooking = bookings.FirstOrDefault(b => b.Id == booking.Id);
            
            if (existingBooking == null)
                return null;

            // Update properties
            existingBooking.CheckInDate = booking.CheckInDate;
            existingBooking.CheckOutDate = booking.CheckOutDate;
            existingBooking.NumberOfGuests = booking.NumberOfGuests;
            existingBooking.PaymentMethod = booking.PaymentMethod;
            existingBooking.PaymentStatus = booking.PaymentStatus;
            existingBooking.Notes = booking.Notes;
            existingBooking.Services = booking.Services;
            existingBooking.Deposit = booking.Deposit;

            // Recalculate total amount
            var room = _roomService.GetRoomById(booking.RoomId);
            if (room != null)
            {
                var numberOfNights = booking.GetNumberOfNights();
                existingBooking.TotalAmount = room.PricePerNight * numberOfNights;
            }

            _dataStore.Save(DataKey, bookings);
            Logger.Info($"Updated booking: {booking.Id}");
            
            return existingBooking;
        }

        public bool CancelBooking(string id)
        {
            var bookings = GetAllBookings();
            var booking = bookings.FirstOrDefault(b => b.Id == id);
            
            if (booking == null)
                return false;

            booking.Status = BookingStatus.Cancelled;
            _dataStore.Save(DataKey, bookings);

            // Update room status back to Available
            _roomService.UpdateRoomStatus(booking.RoomId, RoomStatus.Available);

            Logger.Info($"Cancelled booking: {id}");
            return true;
        }

        public bool CheckIn(string id)
        {
            var bookings = GetAllBookings();
            var booking = bookings.FirstOrDefault(b => b.Id == id);
            
            if (booking == null)
                return false;

            if (booking.Status != BookingStatus.Confirmed && booking.Status != BookingStatus.Pending)
            {
                throw new InvalidOperationException("Không thể check-in cho booking này");
            }

            booking.Status = BookingStatus.CheckedIn;
            booking.ActualCheckInTime = DateTime.Now;
            _dataStore.Save(DataKey, bookings);

            // Update room status to Occupied
            _roomService.UpdateRoomStatus(booking.RoomId, RoomStatus.Occupied);

            // Create notification
            var customer = _customerService.GetCustomerById(booking.CustomerId);
            if (customer != null)
            {
                _notificationService.CreateCheckInNotification(booking.Id, customer.FullName);
            }

            Logger.Info($"Checked in booking: {id}");
            return true;
        }

        public bool CheckOut(string id)
        {
            var bookings = GetAllBookings();
            var booking = bookings.FirstOrDefault(b => b.Id == id);
            
            if (booking == null)
                return false;

            if (booking.Status != BookingStatus.CheckedIn)
            {
                throw new InvalidOperationException("Không thể check-out cho booking này");
            }

            booking.Status = BookingStatus.CheckedOut;
            booking.ActualCheckOutTime = DateTime.Now;
            _dataStore.Save(DataKey, bookings);

            // Update room status back to Available
            _roomService.UpdateRoomStatus(booking.RoomId, RoomStatus.Available);

            // Create notification
            var customer = _customerService.GetCustomerById(booking.CustomerId);
            if (customer != null)
            {
                _notificationService.CreateCheckOutNotification(booking.Id, customer.FullName);
            }

            Logger.Info($"Checked out booking: {id}");
            return true;
        }
    }
}
