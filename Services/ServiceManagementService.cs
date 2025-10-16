using QLKS.Models;
using QLKS.Utils;

namespace QLKS.Services
{
    public interface IServiceManagementService
    {
        List<Service> GetAllServices();
        Service? GetServiceById(string id);
        List<Service> GetServicesByCategory(ServiceCategory category);
        List<Service> GetAvailableServices();
        Service AddService(Service service);
        Service? UpdateService(Service service);
        bool DeleteService(string id);
    }

    public class ServiceManagementService : IServiceManagementService
    {
        private readonly IDataStore _dataStore;
        private const string DataKey = "services";

        public ServiceManagementService(IDataStore dataStore)
        {
            _dataStore = dataStore;
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            var services = _dataStore.Load<List<Service>>(DataKey);
            if (services == null || !services.Any())
            {
                services = new List<Service>
                {
                    new Service
                    {
                        Name = "Bữa sáng buffet",
                        Description = "Buffet sáng đa dạng món Á - Âu",
                        Category = ServiceCategory.FoodBeverage,
                        Price = 150000,
                        Status = ServiceStatus.Available,
                        Unit = "suất",
                        ImageUrl = "/images/services/breakfast.jpg",
                        EstimatedDuration = 60,
                        IsPopular = true
                    },
                    new Service
                    {
                        Name = "Đưa đón sân bay",
                        Description = "Dịch vụ đưa đón sân bay bằng xe riêng",
                        Category = ServiceCategory.Transportation,
                        Price = 300000,
                        Status = ServiceStatus.BookingRequired,
                        Unit = "chuyến",
                        ImageUrl = "/images/services/airport-transfer.jpg",
                        EstimatedDuration = 60,
                        IsPopular = true
                    },
                    new Service
                    {
                        Name = "Massage toàn thân",
                        Description = "Massage thư giãn toàn thân với tinh dầu thiên nhiên",
                        Category = ServiceCategory.Spa,
                        Price = 500000,
                        Status = ServiceStatus.Available,
                        Unit = "giờ",
                        ImageUrl = "/images/services/massage.jpg",
                        EstimatedDuration = 90,
                        IsPopular = true
                    },
                    new Service
                    {
                        Name = "Giặt ủi quần áo",
                        Description = "Dịch vụ giặt ủi nhanh chóng, chuyên nghiệp",
                        Category = ServiceCategory.Laundry,
                        Price = 50000,
                        Status = ServiceStatus.Available,
                        Unit = "kg",
                        ImageUrl = "/images/services/laundry.jpg",
                        EstimatedDuration = 120
                    },
                    new Service
                    {
                        Name = "Room Service 24/7",
                        Description = "Phục vụ đồ ăn uống tận phòng",
                        Category = ServiceCategory.RoomService,
                        Price = 0,
                        Status = ServiceStatus.Available,
                        Unit = "lần",
                        ImageUrl = "/images/services/room-service.jpg",
                        EstimatedDuration = 30,
                        IsPopular = true
                    },
                    new Service
                    {
                        Name = "Karaoke",
                        Description = "Phòng karaoke hiện đại với âm thanh đỉnh cao",
                        Category = ServiceCategory.Entertainment,
                        Price = 200000,
                        Status = ServiceStatus.BookingRequired,
                        Unit = "giờ",
                        ImageUrl = "/images/services/karaoke.jpg",
                        EstimatedDuration = 120
                    },
                    new Service
                    {
                        Name = "Phòng họp",
                        Description = "Phòng họp với đầy đủ thiết bị hiện đại",
                        Category = ServiceCategory.Conference,
                        Price = 500000,
                        Status = ServiceStatus.BookingRequired,
                        Unit = "giờ",
                        ImageUrl = "/images/services/meeting-room.jpg",
                        EstimatedDuration = 240
                    }
                };

                _dataStore.Save(DataKey, services);
                Logger.Info("Initialized sample service data");
            }
        }

        public List<Service> GetAllServices()
        {
            return _dataStore.Load<List<Service>>(DataKey) ?? new List<Service>();
        }

        public Service? GetServiceById(string id)
        {
            var services = GetAllServices();
            return services.FirstOrDefault(s => s.Id == id);
        }

        public List<Service> GetServicesByCategory(ServiceCategory category)
        {
            var services = GetAllServices();
            return services.Where(s => s.Category == category).ToList();
        }

        public List<Service> GetAvailableServices()
        {
            var services = GetAllServices();
            return services.Where(s => s.Status != ServiceStatus.Unavailable).ToList();
        }

        public Service AddService(Service service)
        {
            if (string.IsNullOrWhiteSpace(service.Name))
            {
                throw new InvalidOperationException("Tên dịch vụ không được để trống");
            }

            if (!Validator.IsValidPrice(service.Price))
            {
                throw new InvalidOperationException("Giá dịch vụ không hợp lệ");
            }

            var services = GetAllServices();
            service.Id = Guid.NewGuid().ToString();
            services.Add(service);
            _dataStore.Save(DataKey, services);
            
            Logger.Info($"Added new service: {service.Name}");
            return service;
        }

        public Service? UpdateService(Service service)
        {
            var services = GetAllServices();
            var existingService = services.FirstOrDefault(s => s.Id == service.Id);
            
            if (existingService == null)
                return null;

            existingService.Name = service.Name;
            existingService.Description = service.Description;
            existingService.Category = service.Category;
            existingService.Price = service.Price;
            existingService.Status = service.Status;
            existingService.Unit = service.Unit;
            existingService.ImageUrl = service.ImageUrl;
            existingService.EstimatedDuration = service.EstimatedDuration;
            existingService.IsPopular = service.IsPopular;

            _dataStore.Save(DataKey, services);
            Logger.Info($"Updated service: {service.Name}");
            
            return existingService;
        }

        public bool DeleteService(string id)
        {
            var services = GetAllServices();
            var service = services.FirstOrDefault(s => s.Id == id);
            
            if (service == null)
                return false;

            services.Remove(service);
            _dataStore.Save(DataKey, services);
            
            Logger.Info($"Deleted service: {service.Name}");
            return true;
        }
    }
}
