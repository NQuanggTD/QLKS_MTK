using Microsoft.AspNetCore.Mvc;
using QLKS.Models;
using QLKS.Services;

namespace QLKS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _customerService.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(string id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
                return NotFound(new { message = "Khách hàng không tồn tại" });

            return Ok(customer);
        }

        [HttpGet("search")]
        public IActionResult SearchCustomers([FromQuery] string keyword)
        {
            var customers = _customerService.SearchCustomers(keyword);
            return Ok(customers);
        }

        [HttpGet("{id}/bookings")]
        public IActionResult GetCustomerBookingHistory(string id)
        {
            var bookings = _customerService.GetCustomerBookingHistory(id);
            return Ok(bookings);
        }

        [HttpPost]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            try
            {
                var newCustomer = _customerService.AddCustomer(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.Id }, newCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(string id, [FromBody] Customer customer)
        {
            try
            {
                customer.Id = id;
                var updatedCustomer = _customerService.UpdateCustomer(customer);
                
                if (updatedCustomer == null)
                    return NotFound(new { message = "Khách hàng không tồn tại" });

                return Ok(updatedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(string id)
        {
            var result = _customerService.DeleteCustomer(id);
            if (!result)
                return NotFound(new { message = "Khách hàng không tồn tại" });

            return Ok(new { message = "Xóa khách hàng thành công" });
        }
    }
}
