using QLKS.Models;
using QLKS.Utils;

namespace QLKS.Services
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        Customer? GetCustomerById(string id);
        List<Customer> SearchCustomers(string keyword);
        Customer AddCustomer(Customer customer);
        Customer? UpdateCustomer(Customer customer);
        bool DeleteCustomer(string id);
        List<Booking> GetCustomerBookingHistory(string customerId);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IDataStore _dataStore;
        private const string DataKey = "customers";

        public CustomerService(IDataStore dataStore)
        {
            _dataStore = dataStore;
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            var customers = _dataStore.Load<List<Customer>>(DataKey);
            if (customers == null || !customers.Any())
            {
                customers = new List<Customer>
                {
                    new Customer
                    {
                        FullName = "Nguyễn Văn An",
                        Email = "nguyenvanan@email.com",
                        Phone = "0912345678",
                        IdCard = "001234567890",
                        Address = "123 Đường ABC, Quận 1, TP.HCM",
                        DateOfBirth = new DateTime(1985, 5, 15),
                        Nationality = "Việt Nam",
                        SpecialNotes = "Khách VIP, ưu tiên phòng view biển",
                        CustomerType = CustomerType.VIP,
                        LoyaltyPoints = 1500,
                        TotalBookings = 12,
                        CreatedAt = DateTime.Now.AddYears(-2)
                    },
                    new Customer
                    {
                        FullName = "Trần Thị Bình",
                        Email = "tranthib@email.com",
                        Phone = "0987654321",
                        IdCard = "001234567891",
                        Address = "456 Đường XYZ, Quận 3, TP.HCM",
                        DateOfBirth = new DateTime(1990, 8, 20),
                        Nationality = "Việt Nam",
                        SpecialNotes = "Dị ứng hải sản",
                        CustomerType = CustomerType.Regular,
                        LoyaltyPoints = 320,
                        TotalBookings = 3,
                        CreatedAt = DateTime.Now.AddMonths(-8)
                    },
                    new Customer
                    {
                        FullName = "John Smith",
                        Email = "johnsmith@email.com",
                        Phone = "+84123456789",
                        IdCard = "P1234567",
                        Address = "New York, USA",
                        DateOfBirth = new DateTime(1982, 3, 10),
                        Nationality = "USA",
                        SpecialNotes = "Vegetarian",
                        CustomerType = CustomerType.Regular,
                        LoyaltyPoints = 500,
                        TotalBookings = 5,
                        CreatedAt = DateTime.Now.AddMonths(-6)
                    }
                };

                _dataStore.Save(DataKey, customers);
                Logger.Info("Initialized sample customer data");
            }
        }

        public List<Customer> GetAllCustomers()
        {
            return _dataStore.Load<List<Customer>>(DataKey) ?? new List<Customer>();
        }

        public Customer? GetCustomerById(string id)
        {
            var customers = GetAllCustomers();
            return customers.FirstOrDefault(c => c.Id == id);
        }

        public List<Customer> SearchCustomers(string keyword)
        {
            var customers = GetAllCustomers();
            keyword = keyword.ToLower();
            
            return customers.Where(c =>
                c.FullName.ToLower().Contains(keyword) ||
                c.Email.ToLower().Contains(keyword) ||
                c.Phone.Contains(keyword) ||
                c.IdCard.Contains(keyword)
            ).ToList();
        }

        public Customer AddCustomer(Customer customer)
        {
            var validation = Validator.ValidateCustomer(customer);
            if (!validation.IsValid)
            {
                throw new InvalidOperationException(validation.GetErrorMessage());
            }

            var customers = GetAllCustomers();
            
            // Check for duplicate email or identity number
            if (customers.Any(c => c.Email == customer.Email))
            {
                throw new InvalidOperationException("Email đã tồn tại");
            }
            
            if (customers.Any(c => c.IdCard == customer.IdCard))
            {
                throw new InvalidOperationException("Số CMND/CCCD đã tồn tại");
            }

            customer.Id = Guid.NewGuid().ToString();
            customer.CreatedAt = DateTime.Now;
            customer.TotalBookings = 0;
            customer.LoyaltyPoints = 0;
            customers.Add(customer);
            _dataStore.Save(DataKey, customers);
            
            Logger.Info($"Added new customer: {customer.FullName}");
            return customer;
        }

        public Customer? UpdateCustomer(Customer customer)
        {
            var validation = Validator.ValidateCustomer(customer);
            if (!validation.IsValid)
            {
                throw new InvalidOperationException(validation.GetErrorMessage());
            }

            var customers = GetAllCustomers();
            var existingCustomer = customers.FirstOrDefault(c => c.Id == customer.Id);
            
            if (existingCustomer == null)
                return null;

            // Update properties
            existingCustomer.FullName = customer.FullName;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.IdCard = customer.IdCard;
            existingCustomer.Address = customer.Address;
            existingCustomer.DateOfBirth = customer.DateOfBirth;
            existingCustomer.Nationality = customer.Nationality;
            existingCustomer.SpecialNotes = customer.SpecialNotes;
            existingCustomer.CustomerType = customer.CustomerType;
            existingCustomer.LoyaltyPoints = customer.LoyaltyPoints;
            existingCustomer.TotalBookings = customer.TotalBookings;

            _dataStore.Save(DataKey, customers);
            Logger.Info($"Updated customer: {customer.FullName}");
            
            return existingCustomer;
        }

        public bool DeleteCustomer(string id)
        {
            var customers = GetAllCustomers();
            var customer = customers.FirstOrDefault(c => c.Id == id);
            
            if (customer == null)
                return false;

            customers.Remove(customer);
            _dataStore.Save(DataKey, customers);
            
            Logger.Info($"Deleted customer: {customer.FullName}");
            return true;
        }

        public List<Booking> GetCustomerBookingHistory(string customerId)
        {
            var bookings = _dataStore.Load<List<Booking>>("bookings") ?? new List<Booking>();
            return bookings.Where(b => b.CustomerId == customerId)
                          .OrderByDescending(b => b.BookingDate)
                          .ToList();
        }
    }
}
