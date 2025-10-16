using Microsoft.AspNetCore.Mvc;
using QLKS.Models;
using QLKS.Services;

namespace QLKS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceManagementService _serviceManagementService;

        public ServicesController(IServiceManagementService serviceManagementService)
        {
            _serviceManagementService = serviceManagementService;
        }

        [HttpGet]
        public IActionResult GetAllServices()
        {
            var services = _serviceManagementService.GetAllServices();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public IActionResult GetServiceById(string id)
        {
            var service = _serviceManagementService.GetServiceById(id);
            if (service == null)
                return NotFound(new { message = "Dịch vụ không tồn tại" });

            return Ok(service);
        }

        [HttpGet("category/{category}")]
        public IActionResult GetServicesByCategory(ServiceCategory category)
        {
            var services = _serviceManagementService.GetServicesByCategory(category);
            return Ok(services);
        }

        [HttpGet("available")]
        public IActionResult GetAvailableServices()
        {
            var services = _serviceManagementService.GetAvailableServices();
            return Ok(services);
        }

        [HttpPost]
        public IActionResult AddService([FromBody] Service service)
        {
            try
            {
                var newService = _serviceManagementService.AddService(service);
                return CreatedAtAction(nameof(GetServiceById), new { id = newService.Id }, newService);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateService(string id, [FromBody] Service service)
        {
            try
            {
                service.Id = id;
                var updatedService = _serviceManagementService.UpdateService(service);
                
                if (updatedService == null)
                    return NotFound(new { message = "Dịch vụ không tồn tại" });

                return Ok(updatedService);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteService(string id)
        {
            var result = _serviceManagementService.DeleteService(id);
            if (!result)
                return NotFound(new { message = "Dịch vụ không tồn tại" });

            return Ok(new { message = "Xóa dịch vụ thành công" });
        }
    }
}
