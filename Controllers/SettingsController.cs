using Microsoft.AspNetCore.Mvc;
using QLKS.Models;
using QLKS.Services;

namespace QLKS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public IActionResult GetSettings()
        {
            var settings = _settingsService.GetSettings();
            return Ok(settings);
        }

        [HttpPut]
        public IActionResult UpdateSettings([FromBody] AppSettings settings)
        {
            try
            {
                var updatedSettings = _settingsService.UpdateSettings(settings);
                return Ok(updatedSettings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("reset")]
        public IActionResult ResetToDefault()
        {
            _settingsService.ResetToDefault();
            return Ok(new { message = "Đã khôi phục cài đặt mặc định" });
        }
    }
}
