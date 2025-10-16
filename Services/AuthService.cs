using QLKS.Models;
using QLKS.Utils;

namespace QLKS.Services
{
    public interface IAuthService
    {
        User? Login(string username, string password);
        User? Register(RegisterRequest request);
        bool ChangePassword(string userId, string currentPassword, string newPassword);
        User? GetUserById(string userId);
        User? GetUserByUsername(string username);
        bool UpdateProfile(string userId, UpdateProfileRequest request);
        bool UpdateAvatar(string userId, string avatarUrl);
        void AddLoginHistory(string userId, LoginHistory history);
        List<LoginHistory> GetLoginHistory(string userId);
        List<User> GetAllUsers();
        bool DeleteUser(string userId);
        bool ToggleUserStatus(string userId);
    }

    public class AuthService : IAuthService
    {
        private readonly IDataStore _dataStore;
        private const string DataKey = "users";

        public AuthService(IDataStore dataStore)
        {
            _dataStore = dataStore;
            InitializeDefaultAdmin();
        }

        private void InitializeDefaultAdmin()
        {
            var users = _dataStore.Load<List<User>>(DataKey);
            
            if (users == null || !users.Any())
            {
                users = new List<User>
                {
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Username = "admin",
                        Password = PasswordHasher.HashPassword("admin123"),
                        FullName = "Administrator",
                        Email = "admin@hotelwangg.com",
                        Phone = "0123456789",
                        Role = "Admin",
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    },
                    new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Username = "manager",
                        Password = PasswordHasher.HashPassword("manager123"),
                        FullName = "Manager",
                        Email = "manager@hotelwangg.com",
                        Phone = "0987654321",
                        Role = "Manager",
                        IsActive = true,
                        CreatedDate = DateTime.Now
                    }
                };

                _dataStore.Save(DataKey, users);
                Logger.Info("Initialized default users");
            }
        }

        public User? Login(string username, string password)
        {
            try
            {
                var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
                var user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    Logger.Warning($"Login failed: User not found - {username}");
                    return null;
                }

                if (!user.IsActive)
                {
                    Logger.Warning($"Login failed: User inactive - {username}");
                    return null;
                }

                if (!PasswordHasher.VerifyPassword(password, user.Password))
                {
                    Logger.Warning($"Login failed: Wrong password - {username}");
                    return null;
                }

                user.LastLoginDate = DateTime.Now;
                _dataStore.Save(DataKey, users);

                Logger.Info($"User logged in successfully: {username}");
                return user;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during login: {ex.Message}");
                return null;
            }
        }

        public User? Register(RegisterRequest request)
        {
            try
            {
                var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();

                // Check if username exists
                if (users.Any(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    Logger.Warning($"Registration failed: Username exists - {request.Username}");
                    return null;
                }

                // Check if email exists
                if (users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    Logger.Warning($"Registration failed: Email exists - {request.Email}");
                    return null;
                }

                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = request.Username,
                    Password = PasswordHasher.HashPassword(request.Password),
                    FullName = request.FullName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Role = "Staff", // Default role
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                users.Add(newUser);
                _dataStore.Save(DataKey, users);

                Logger.Info($"User registered successfully: {request.Username}");
                return newUser;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during registration: {ex.Message}");
                return null;
            }
        }

        public bool ChangePassword(string userId, string currentPassword, string newPassword)
        {
            try
            {
                var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
                var user = users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    Logger.Warning($"Change password failed: User not found - {userId}");
                    return false;
                }

                if (!PasswordHasher.VerifyPassword(currentPassword, user.Password))
                {
                    Logger.Warning($"Change password failed: Wrong current password - {userId}");
                    return false;
                }

                user.Password = PasswordHasher.HashPassword(newPassword);
                _dataStore.Save(DataKey, users);

                Logger.Info($"Password changed successfully: {user.Username}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error changing password: {ex.Message}");
                return false;
            }
        }

        public User? GetUserById(string userId)
        {
            var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
            return users.FirstOrDefault(u => u.Id == userId);
        }

        public User? GetUserByUsername(string username)
        {
            var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public bool UpdateProfile(string userId, UpdateProfileRequest request)
        {
            try
            {
                var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
                var user = users.FirstOrDefault(u => u.Id == userId);

                if (user == null) return false;

                user.FullName = request.FullName;
                user.Email = request.Email;
                user.Phone = request.Phone;

                _dataStore.Save(DataKey, users);
                Logger.Info($"Profile updated: {user.Username}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error updating profile: {ex.Message}");
                return false;
            }
        }

        public bool UpdateAvatar(string userId, string avatarUrl)
        {
            try
            {
                var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
                var user = users.FirstOrDefault(u => u.Id == userId);

                if (user == null) return false;

                user.AvatarUrl = avatarUrl;
                _dataStore.Save(DataKey, users);

                Logger.Info($"Avatar updated: {user.Username}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error updating avatar: {ex.Message}");
                return false;
            }
        }

        public void AddLoginHistory(string userId, LoginHistory history)
        {
            try
            {
                var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
                var user = users.FirstOrDefault(u => u.Id == userId);

                if (user == null) return;

                user.LoginHistory.Insert(0, history);
                
                // Keep only last 20 login history entries
                if (user.LoginHistory.Count > 20)
                {
                    user.LoginHistory = user.LoginHistory.Take(20).ToList();
                }

                _dataStore.Save(DataKey, users);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error adding login history: {ex.Message}");
            }
        }

        public List<LoginHistory> GetLoginHistory(string userId)
        {
            var user = GetUserById(userId);
            return user?.LoginHistory ?? new List<LoginHistory>();
        }

        public List<User> GetAllUsers()
        {
            return _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
        }

        public bool DeleteUser(string userId)
        {
            try
            {
                var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
                var user = users.FirstOrDefault(u => u.Id == userId);

                if (user == null) return false;

                users.Remove(user);
                _dataStore.Save(DataKey, users);

                Logger.Info($"User deleted: {user.Username}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error deleting user: {ex.Message}");
                return false;
            }
        }

        public bool ToggleUserStatus(string userId)
        {
            try
            {
                var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
                var user = users.FirstOrDefault(u => u.Id == userId);

                if (user == null) return false;

                user.IsActive = !user.IsActive;
                _dataStore.Save(DataKey, users);

                Logger.Info($"User status toggled: {user.Username} - Active: {user.IsActive}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error toggling user status: {ex.Message}");
                return false;
            }
        }
    }
}
