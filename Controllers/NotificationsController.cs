using Microsoft.AspNetCore.Mvc;
using QLKS.Services;

namespace QLKS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult GetAllNotifications()
        {
            var notifications = _notificationService.GetAllNotifications();
            return Ok(notifications);
        }

        [HttpGet("unread")]
        public IActionResult GetUnreadNotifications()
        {
            var notifications = _notificationService.GetUnreadNotifications();
            return Ok(notifications);
        }

        [HttpGet("count")]
        public IActionResult GetUnreadCount()
        {
            var count = _notificationService.GetUnreadCount();
            return Ok(new { count });
        }

        [HttpPatch("{id}/read")]
        public IActionResult MarkAsRead(string id)
        {
            var result = _notificationService.MarkAsRead(id);
            if (!result)
                return NotFound(new { message = "Thông báo không tồn tại" });

            return Ok(new { message = "Đã đánh dấu đã đọc" });
        }

        [HttpPatch("read-all")]
        public IActionResult MarkAllAsRead()
        {
            _notificationService.MarkAllAsRead();
            return Ok(new { message = "Đã đánh dấu tất cả là đã đọc" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(string id)
        {
            var result = _notificationService.DeleteNotification(id);
            if (!result)
                return NotFound(new { message = "Thông báo không tồn tại" });

            return Ok(new { message = "Đã xóa thông báo" });
        }

        [HttpDelete("read")]
        public IActionResult DeleteAllRead()
        {
            _notificationService.DeleteAllRead();
            return Ok(new { message = "Đã xóa tất cả thông báo đã đọc" });
        }
    }
}
