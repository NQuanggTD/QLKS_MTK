using Microsoft.AspNetCore.Mvc;
using QLKS.Models;
using QLKS.Services;
using QLKS.Utils;

namespace QLKS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login
        /// POST /api/auth/login
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { success = false, message = "Vui lòng nhập đầy đủ thông tin" });
                }

                var user = _authService.Login(request.Username, request.Password);

                if (user == null)
                {
                    return Unauthorized(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không đúng" });
                }

                // Add login history
                var loginHistory = new LoginHistory
                {
                    LoginTime = DateTime.Now,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                    Device = Request.Headers["User-Agent"].ToString(),
                    Browser = GetBrowserName(Request.Headers["User-Agent"].ToString()),
                    Success = true
                };
                _authService.AddLoginHistory(user.Id, loginHistory);

                // Create session (simplified - in production use proper session management)
                HttpContext.Session.SetString("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                return Ok(new
                {
                    success = true,
                    message = "Đăng nhập thành công",
                    user = new
                    {
                        id = user.Id,
                        username = user.Username,
                        fullName = user.FullName,
                        email = user.Email,
                        phone = user.Phone,
                        role = user.Role,
                        avatarUrl = user.AvatarUrl
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in Login: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi đăng nhập" });
            }
        }

        /// <summary>
        /// Register
        /// POST /api/auth/register
        /// </summary>
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { success = false, message = "Vui lòng nhập đầy đủ thông tin" });
                }

                if (request.Username.Length < 4)
                {
                    return BadRequest(new { success = false, message = "Tên đăng nhập phải có ít nhất 4 ký tự" });
                }

                if (request.Password.Length < 6)
                {
                    return BadRequest(new { success = false, message = "Mật khẩu phải có ít nhất 6 ký tự" });
                }

                if (request.Password != request.ConfirmPassword)
                {
                    return BadRequest(new { success = false, message = "Mật khẩu xác nhận không khớp" });
                }

                if (!Validator.IsValidEmail(request.Email))
                {
                    return BadRequest(new { success = false, message = "Email không hợp lệ" });
                }

                var user = _authService.Register(request);

                if (user == null)
                {
                    return BadRequest(new { success = false, message = "Tên đăng nhập hoặc email đã tồn tại" });
                }

                return Ok(new
                {
                    success = true,
                    message = "Đăng ký thành công",
                    user = new
                    {
                        id = user.Id,
                        username = user.Username,
                        fullName = user.FullName,
                        email = user.Email
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in Register: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi đăng ký" });
            }
        }

        /// <summary>
        /// Logout
        /// POST /api/auth/logout
        /// </summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return Ok(new { success = true, message = "Đăng xuất thành công" });
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in Logout: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi đăng xuất" });
            }
        }

        /// <summary>
        /// Get current user
        /// GET /api/auth/me
        /// </summary>
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, message = "Chưa đăng nhập" });
                }

                var user = _authService.GetUserById(userId);

                if (user == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy người dùng" });
                }

                return Ok(new
                {
                    success = true,
                    user = new
                    {
                        id = user.Id,
                        username = user.Username,
                        fullName = user.FullName,
                        email = user.Email,
                        phone = user.Phone,
                        role = user.Role,
                        avatarUrl = user.AvatarUrl,
                        createdDate = user.CreatedDate,
                        lastLoginDate = user.LastLoginDate
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in GetCurrentUser: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi" });
            }
        }

        /// <summary>
        /// Update profile
        /// PUT /api/auth/profile
        /// </summary>
        [HttpPut("profile")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, message = "Chưa đăng nhập" });
                }

                if (!Validator.IsValidEmail(request.Email))
                {
                    return BadRequest(new { success = false, message = "Email không hợp lệ" });
                }

                var success = _authService.UpdateProfile(userId, request);

                if (!success)
                {
                    return BadRequest(new { success = false, message = "Cập nhật thất bại" });
                }

                return Ok(new { success = true, message = "Cập nhật hồ sơ thành công" });
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in UpdateProfile: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi" });
            }
        }

        /// <summary>
        /// Change password
        /// POST /api/auth/change-password
        /// </summary>
        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, message = "Chưa đăng nhập" });
                }

                if (request.NewPassword.Length < 6)
                {
                    return BadRequest(new { success = false, message = "Mật khẩu mới phải có ít nhất 6 ký tự" });
                }

                if (request.NewPassword != request.ConfirmPassword)
                {
                    return BadRequest(new { success = false, message = "Mật khẩu xác nhận không khớp" });
                }

                var success = _authService.ChangePassword(userId, request.CurrentPassword, request.NewPassword);

                if (!success)
                {
                    return BadRequest(new { success = false, message = "Mật khẩu hiện tại không đúng" });
                }

                return Ok(new { success = true, message = "Đổi mật khẩu thành công" });
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in ChangePassword: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi" });
            }
        }

        /// <summary>
        /// Get login history
        /// GET /api/auth/login-history
        /// </summary>
        [HttpGet("login-history")]
        public IActionResult GetLoginHistory()
        {
            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, message = "Chưa đăng nhập" });
                }

                var history = _authService.GetLoginHistory(userId);

                return Ok(new { success = true, history });
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in GetLoginHistory: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi" });
            }
        }

        private string GetBrowserName(string userAgent)
        {
            if (userAgent.Contains("Chrome")) return "Chrome";
            if (userAgent.Contains("Firefox")) return "Firefox";
            if (userAgent.Contains("Safari")) return "Safari";
            if (userAgent.Contains("Edge")) return "Edge";
            if (userAgent.Contains("Opera")) return "Opera";
            return "Unknown";
        }
    }
}
