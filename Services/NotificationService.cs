using QLKS.Models;
using QLKS.Utils;

namespace QLKS.Services
{
    public interface INotificationService
    {
        List<Notification> GetAllNotifications();
        List<Notification> GetUnreadNotifications();
        Notification? GetNotificationById(string id);
        Notification AddNotification(Notification notification);
        bool MarkAsRead(string id);
        bool MarkAllAsRead();
        bool DeleteNotification(string id);
        bool DeleteAllRead();
        int GetUnreadCount();
        void CreateBookingNotification(string bookingId, string customerName, string roomNumber);
        void CreateCheckInNotification(string bookingId, string customerName);
        void CreateCheckOutNotification(string bookingId, string customerName);
    }

    public class NotificationService : INotificationService
    {
        private readonly IDataStore _dataStore;
        private const string DataKey = "notifications";

        public NotificationService(IDataStore dataStore)
        {
            _dataStore = dataStore;
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            var notifications = _dataStore.Load<List<Notification>>(DataKey);
            if (notifications == null || !notifications.Any())
            {
                notifications = new List<Notification>
                {
                    new Notification
                    {
                        Type = NotificationType.Booking,
                        Title = "Booking mới",
                        Message = "Có 1 booking mới từ khách hàng Nguyễn Văn An",
                        Icon = "fa-calendar-check",
                        Link = "/Home/Bookings"
                    },
                    new Notification
                    {
                        Type = NotificationType.Info,
                        Title = "Check-in hôm nay",
                        Message = "Có 2 khách sẽ check-in trong hôm nay",
                        Icon = "fa-sign-in-alt",
                        Link = "/Home/Bookings"
                    },
                    new Notification
                    {
                        Type = NotificationType.Warning,
                        Title = "Phòng cần bảo trì",
                        Message = "Phòng 103 cần được bảo trì định kỳ",
                        Icon = "fa-tools",
                        Link = "/Home/Rooms"
                    }
                };

                _dataStore.Save(DataKey, notifications);
                Logger.Info("Initialized sample notification data");
            }
        }

        public List<Notification> GetAllNotifications()
        {
            var notifications = _dataStore.Load<List<Notification>>(DataKey) ?? new List<Notification>();
            return notifications.OrderByDescending(n => n.CreatedDate).ToList();
        }

        public List<Notification> GetUnreadNotifications()
        {
            var notifications = GetAllNotifications();
            return notifications.Where(n => !n.IsRead).ToList();
        }

        public Notification? GetNotificationById(string id)
        {
            var notifications = GetAllNotifications();
            return notifications.FirstOrDefault(n => n.Id == id);
        }

        public Notification AddNotification(Notification notification)
        {
            var notifications = GetAllNotifications();
            notification.Id = Guid.NewGuid().ToString();
            notification.CreatedDate = DateTime.Now;
            notification.IsRead = false;
            
            notifications.Insert(0, notification);
            _dataStore.Save(DataKey, notifications);
            
            Logger.Info($"Added new notification: {notification.Title}");
            return notification;
        }

        public bool MarkAsRead(string id)
        {
            var notifications = GetAllNotifications();
            var notification = notifications.FirstOrDefault(n => n.Id == id);
            
            if (notification == null)
                return false;

            notification.IsRead = true;
            _dataStore.Save(DataKey, notifications);
            
            return true;
        }

        public bool MarkAllAsRead()
        {
            var notifications = GetAllNotifications();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            _dataStore.Save(DataKey, notifications);
            return true;
        }

        public bool DeleteNotification(string id)
        {
            var notifications = GetAllNotifications();
            var notification = notifications.FirstOrDefault(n => n.Id == id);
            
            if (notification == null)
                return false;

            notifications.Remove(notification);
            _dataStore.Save(DataKey, notifications);
            
            Logger.Info($"Deleted notification: {notification.Title}");
            return true;
        }

        public bool DeleteAllRead()
        {
            var notifications = GetAllNotifications();
            var unread = notifications.Where(n => !n.IsRead).ToList();
            _dataStore.Save(DataKey, unread);
            return true;
        }

        public int GetUnreadCount()
        {
            return GetUnreadNotifications().Count;
        }

        public void CreateBookingNotification(string bookingId, string customerName, string roomNumber)
        {
            var notification = new Notification
            {
                Type = NotificationType.Booking,
                Title = "Booking mới",
                Message = $"Khách hàng {customerName} đã đặt phòng {roomNumber}",
                Icon = "fa-calendar-check",
                Link = "/Home/Bookings",
                RelatedId = bookingId
            };
            AddNotification(notification);
        }

        public void CreateCheckInNotification(string bookingId, string customerName)
        {
            var notification = new Notification
            {
                Type = NotificationType.Success,
                Title = "Check-in thành công",
                Message = $"Khách hàng {customerName} đã check-in",
                Icon = "fa-sign-in-alt",
                Link = "/Home/Bookings",
                RelatedId = bookingId
            };
            AddNotification(notification);
        }

        public void CreateCheckOutNotification(string bookingId, string customerName)
        {
            var notification = new Notification
            {
                Type = NotificationType.Info,
                Title = "Check-out",
                Message = $"Khách hàng {customerName} đã check-out",
                Icon = "fa-sign-out-alt",
                Link = "/Home/Bookings",
                RelatedId = bookingId
            };
            AddNotification(notification);
        }
    }
}
