using Microsoft.AspNetCore.Mvc;
using QLKS.Models;
using QLKS.Services;

namespace QLKS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public IActionResult GetAllBookings()
        {
            var bookings = _bookingService.GetAllBookings();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookingById(string id)
        {
            var booking = _bookingService.GetBookingById(id);
            if (booking == null)
                return NotFound(new { message = "Booking không tồn tại" });

            return Ok(booking);
        }

        [HttpGet("customer/{customerId}")]
        public IActionResult GetBookingsByCustomer(string customerId)
        {
            var bookings = _bookingService.GetBookingsByCustomer(customerId);
            return Ok(bookings);
        }

        [HttpGet("room/{roomId}")]
        public IActionResult GetBookingsByRoom(string roomId)
        {
            var bookings = _bookingService.GetBookingsByRoom(roomId);
            return Ok(bookings);
        }

        [HttpGet("status/{status}")]
        public IActionResult GetBookingsByStatus(BookingStatus status)
        {
            var bookings = _bookingService.GetBookingsByStatus(status);
            return Ok(bookings);
        }

        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            try
            {
                var newBooking = _bookingService.CreateBooking(booking);
                return CreatedAtAction(nameof(GetBookingById), new { id = newBooking.Id }, newBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBooking(string id, [FromBody] Booking booking)
        {
            try
            {
                booking.Id = id;
                var updatedBooking = _bookingService.UpdateBooking(booking);
                
                if (updatedBooking == null)
                    return NotFound(new { message = "Booking không tồn tại" });

                return Ok(updatedBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult CancelBooking(string id)
        {
            try
            {
                var result = _bookingService.CancelBooking(id);
                if (!result)
                    return NotFound(new { message = "Booking không tồn tại" });

                return Ok(new { message = "Hủy booking thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/checkin")]
        public IActionResult CheckIn(string id)
        {
            try
            {
                var result = _bookingService.CheckIn(id);
                if (!result)
                    return NotFound(new { message = "Booking không tồn tại" });

                return Ok(new { message = "Check-in thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/checkout")]
        public IActionResult CheckOut(string id)
        {
            try
            {
                var result = _bookingService.CheckOut(id);
                if (!result)
                    return NotFound(new { message = "Booking không tồn tại" });

                return Ok(new { message = "Check-out thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
