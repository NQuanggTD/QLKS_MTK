using Microsoft.AspNetCore.Mvc;
using QLKS.Services;

namespace QLKS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult GetDashboardData()
        {
            try
            {
                var data = _dashboardService.GetDashboardData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("recent-bookings")]
        public IActionResult GetRecentBookings([FromQuery] int count = 10)
        {
            try
            {
                var bookings = _dashboardService.GetRecentBookings(count);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("revenue/{year}")]
        public IActionResult GetRevenueByMonth(int year)
        {
            try
            {
                var revenue = _dashboardService.GetRevenueByMonth(year);
                return Ok(revenue);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("booking-stats")]
        public IActionResult GetBookingStats()
        {
            try
            {
                var stats = _dashboardService.GetBookingStatsByStatus();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
