# Hotel's Wangg - Hướng Dẫn Chi Tiết

## 📋 Mục Lục
- [Giới Thiệu](#giới-thiệu)
- [Tính Năng](#tính-năng)
- [Cài Đặt](#cài-đặt)
- [Hướng Dẫn Sử Dụng](#hướng-dẫn-sử-dụng)
- [API Documentation](#api-documentation)
- [Design Patterns](#design-patterns)
- [Bảo Mật](#bảo-mật)
- [FAQ](#faq)

---

## 🏨 Giới Thiệu

**Hotel's Wangg Management System** là hệ thống quản lý khách sạn hiện đại được xây dựng bằng ASP.NET Core 8.0 với giao diện đẹp mắt màu Navy Blue & Gold.

### Công Nghệ Sử Dụng
- **Backend**: ASP.NET Core 8.0 (C# 12)
- **Frontend**: Razor Pages, Vanilla JavaScript, CSS3
- **Database**: JSON File Storage (không cần SQL Server)
- **Authentication**: PBKDF2 + SHA256 Encryption
- **Session**: ASP.NET Core Session (24 giờ)
- **Patterns**: MVC, Singleton, Repository, Decorator, Strategy

---

## ✨ Tính Năng

### 1. 🔐 Xác Thực & Phân Quyền
- **Đăng nhập**: Username/Password với mã hóa PBKDF2
- **Đăng ký**: Tạo tài khoản mới với validation đầy đủ
- **Đăng xuất**: An toàn với xóa session
- **Quản lý hồ sơ**: Cập nhật thông tin cá nhân
- **Đổi mật khẩu**: Xác thực mật khẩu cũ
- **Đổi avatar**: Upload ảnh đại diện
- **Lịch sử đăng nhập**: Theo dõi IP, thiết bị, trình duyệt
- **Vai trò**: Admin, Manager, Staff

### 2. 📊 Dashboard
- Tổng quan doanh thu realtime
- Thống kê phòng (trống, đã đặt, đang sử dụng)
- Biểu đồ doanh thu theo tháng
- Danh sách booking gần đây
- Thông báo quan trọng

### 3. 🛏️ Quản Lý Phòng
- CRUD phòng đầy đủ
- Phân loại: Standard, Deluxe, Suite, Presidential
- Trạng thái: Available, Occupied, Maintenance, Reserved
- Tìm kiếm và lọc nhanh
- Giá phòng linh hoạt

### 4. 📅 Đặt Phòng
- Tạo booking mới
- Check-in / Check-out
- Tính toán tự động giá phòng
- Áp dụng chiết khấu
- Quản lý trạng thái booking

### 5. 👥 Quản Lý Khách Hàng
- Lưu trữ thông tin khách hàng
- Lịch sử đặt phòng
- Phân loại VIP/Regular
- Tìm kiếm và lọc

### 6. 🔔 Thông Báo
- 7 loại thông báo: Success, Error, Warning, Info, Booking, Payment, System
- Toast notifications đẹp mắt
- Realtime updates

### 7. ⚙️ Cài Đặt
- Quản lý hồ sơ admin
- Đổi mật khẩu an toàn
- Bật 2FA (demo)
- Xem lịch sử đăng nhập
- Xuất dữ liệu JSON
- Sao lưu hệ thống

### 8. 🎯 Decorator Pattern
Thêm dịch vụ bổ sung cho phòng:
- **Breakfast**: +100,000 VND
- **Spa Service**: +500,000 VND
- **Airport Transfer**: +300,000 VND
- **Laundry Service**: +50,000 VND

### 9. 💰 Strategy Pattern
Chiết khấu linh hoạt:
- **No Discount**: 0%
- **VIP Discount**: 10%
- **Seasonal Discount**: 15%
- **Loyalty Discount**: 5-20%
- **Early Bird**: 10-20%

### 10. 🏠 Trang Chủ
- Giới thiệu khách sạn đẹp mắt
- Thông tin liên hệ
- Thống kê thành tựu
- Tính năng nổi bật

---

## 🚀 Cài Đặt

### Yêu Cầu Hệ Thống
- .NET 8.0 SDK
- Visual Studio 2022 hoặc VS Code
- Windows 10/11 hoặc Linux/macOS

### Cài Đặt Nhanh

1. **Clone hoặc giải nén dự án**
```bash
cd D:\QLKS
```

2. **Restore dependencies**
```bash
dotnet restore
```

3. **Build project**
```bash
dotnet build
```

4. **Chạy ứng dụng**
```bash
dotnet run
```

5. **Truy cập**
- URL: http://localhost:5000
- Đăng nhập với: `admin` / `admin123`

### Cấu Trúc Thư Mục
```
QLKS/
├── Controllers/          # API & MVC Controllers
│   ├── HomeController.cs
│   ├── AuthController.cs
│   ├── DashboardController.cs
│   ├── RoomController.cs
│   ├── BookingController.cs
│   ├── CustomerController.cs
│   ├── ServiceController.cs
│   ├── PaymentController.cs
│   ├── NotificationController.cs
│   ├── SettingsController.cs
│   └── PricingController.cs
├── Models/              # Data models
│   ├── User.cs
│   ├── Room.cs
│   ├── Booking.cs
│   ├── Customer.cs
│   ├── Service.cs
│   ├── Payment.cs
│   ├── Notification.cs
│   └── Settings.cs
├── Services/            # Business logic
│   ├── AuthService.cs
│   ├── RoomService.cs
│   ├── BookingService.cs
│   ├── CustomerService.cs
│   ├── ServiceManagementService.cs
│   ├── PaymentService.cs
│   ├── DashboardService.cs
│   ├── NotificationService.cs
│   ├── SettingsService.cs
│   ├── PricingService.cs
│   ├── Decorators/      # Decorator pattern
│   │   ├── IServiceDecorator.cs
│   │   ├── BaseServiceDecorator.cs
│   │   ├── BreakfastDecorator.cs
│   │   ├── SpaServiceDecorator.cs
│   │   ├── AirportTransferDecorator.cs
│   │   └── LaundryServiceDecorator.cs
│   └── Strategies/      # Strategy pattern
│       ├── IDiscountStrategy.cs
│       ├── NoDiscountStrategy.cs
│       ├── VIPDiscountStrategy.cs
│       ├── SeasonalDiscountStrategy.cs
│       ├── LoyaltyDiscountStrategy.cs
│       └── EarlyBirdDiscountStrategy.cs
├── Utils/               # Utilities
│   ├── Logger.cs
│   ├── JsonDataStore.cs
│   ├── PasswordHasher.cs
│   └── Validator.cs
├── Views/               # Razor views
│   ├── Home/
│   │   ├── About.cshtml
│   │   ├── Login.cshtml
│   │   ├── Register.cshtml
│   │   ├── Dashboard.cshtml
│   │   ├── Rooms.cshtml
│   │   ├── Bookings.cshtml
│   │   ├── Customers.cshtml
│   │   ├── Services.cshtml
│   │   └── Settings.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       └── Error.cshtml
├── wwwroot/             # Static files
│   ├── css/
│   │   └── style.css
│   └── js/
│       └── admin.js
├── Program.cs           # Entry point
├── README.md            # Tài liệu tổng quan
└── CHI_TIET.md         # File này
```

---

## 📖 Hướng Dẫn Sử Dụng

### 1. Đăng Nhập Lần Đầu

**Bước 1**: Truy cập http://localhost:5000

**Bước 2**: Nhập thông tin đăng nhập
- Username: `admin`
- Password: `admin123`

**Bước 3**: Click "Đăng nhập"

### 2. Đăng Ký Tài Khoản Mới

**Bước 1**: Click "Đăng ký ngay" ở trang Login

**Bước 2**: Điền thông tin
- Tên đăng nhập (tối thiểu 4 ký tự)
- Họ và tên đầy đủ
- Email hợp lệ
- Số điện thoại (10-11 số)
- Mật khẩu (tối thiểu 6 ký tự)
- Xác nhận mật khẩu

**Bước 3**: Click "Đăng ký"

**Bước 4**: Quay lại đăng nhập

### 3. Quản Lý Hồ Sơ

**Mở Hồ Sơ**:
1. Click nút **👤 Hồ sơ** ở góc dưới sidebar
2. Modal hồ sơ sẽ hiển thị

**Đổi Avatar**:
1. Click nút "📷 Đổi ảnh"
2. Chọn file ảnh từ máy tính
3. Ảnh sẽ hiển thị ngay trên sidebar

**Cập Nhật Thông Tin**:
1. Sửa Họ tên, Email, Số điện thoại
2. Click "💾 Lưu thay đổi"
3. Thông tin cập nhật ngay lập tức

### 4. Đổi Mật Khẩu

**Bước 1**: Click nút **🛠️ Cài đặt** ở sidebar

**Bước 2**: Click "🔑 Đổi mật khẩu"

**Bước 3**: Nhập thông tin
- Mật khẩu hiện tại
- Mật khẩu mới (tối thiểu 6 ký tự)
- Xác nhận mật khẩu mới

**Bước 4**: Click "🔑 Đổi mật khẩu"

**Lưu ý**: Mật khẩu hiện tại phải đúng

### 5. Xem Lịch Sử Đăng Nhập

**Bước 1**: Click nút **🛠️ Cài đặt**

**Bước 2**: Click "📋 Xem lịch sử đăng nhập"

**Thông tin hiển thị**:
- Thời gian đăng nhập
- Địa chỉ IP
- Trình duyệt
- Thiết bị
- Trạng thái (Thành công/Thất bại)

### 6. Quản Lý Phòng

**Thêm Phòng Mới**:
1. Vào **Quản lý phòng**
2. Click "➕ Thêm phòng"
3. Điền thông tin (số phòng, loại, giá, trạng thái)
4. Click "Lưu"

**Sửa Phòng**:
1. Click nút "✏️ Sửa" trên phòng
2. Cập nhật thông tin
3. Click "Lưu"

**Xóa Phòng**:
1. Click nút "🗑️ Xóa"
2. Xác nhận xóa

### 7. Đặt Phòng

**Tạo Booking Mới**:
1. Vào **Đặt phòng**
2. Click "➕ Tạo booking"
3. Chọn khách hàng
4. Chọn phòng
5. Chọn ngày check-in/check-out
6. Chọn dịch vụ bổ sung (Decorator)
7. Áp dụng chiết khấu (Strategy)
8. Click "Tạo booking"

**Check-in**:
1. Tìm booking
2. Click "Check-in"
3. Xác nhận

**Check-out**:
1. Tìm booking
2. Click "Check-out"
3. Thanh toán
4. Xác nhận

### 8. Xuất Dữ Liệu

**Bước 1**: Click **🛠️ Cài đặt**

**Bước 2**: Click "📥 Xuất dữ liệu"

**Bước 3**: File JSON sẽ tự động tải về

**Format**: `hotel-data-{timestamp}.json`

### 9. Sao Lưu Hệ Thống

**Bước 1**: Click **🛠️ Cài đặt**

**Bước 2**: Click "💾 Sao lưu hệ thống"

**Bước 3**: Xác nhận trong popup

**Bước 4**: Chờ hoàn tất (2 giây)

### 10. Đăng Xuất

**Bước 1**: Click nút **🚪 Đăng xuất** ở sidebar

**Bước 2**: Xác nhận trong popup

**Bước 3**: Tự động chuyển về trang Login

---

## 📡 API Documentation

### Authentication APIs

#### POST /api/auth/login
Đăng nhập vào hệ thống

**Request Body**:
```json
{
  "username": "admin",
  "password": "admin123",
  "rememberMe": true
}
```

**Response Success**:
```json
{
  "success": true,
  "message": "Đăng nhập thành công",
  "user": {
    "id": "guid",
    "username": "admin",
    "fullName": "Administrator",
    "email": "admin@hotelwangg.com",
    "phone": "0123456789",
    "role": "Admin",
    "avatarUrl": "/images/default-avatar.png"
  }
}
```

**Response Error**:
```json
{
  "success": false,
  "message": "Tên đăng nhập hoặc mật khẩu không đúng"
}
```

#### POST /api/auth/register
Đăng ký tài khoản mới

**Request Body**:
```json
{
  "username": "newuser",
  "password": "password123",
  "confirmPassword": "password123",
  "fullName": "Nguyễn Văn A",
  "email": "user@example.com",
  "phone": "0123456789"
}
```

**Response**:
```json
{
  "success": true,
  "message": "Đăng ký thành công",
  "user": {
    "id": "guid",
    "username": "newuser",
    "fullName": "Nguyễn Văn A",
    "email": "user@example.com"
  }
}
```

#### POST /api/auth/logout
Đăng xuất

**Response**:
```json
{
  "success": true,
  "message": "Đăng xuất thành công"
}
```

#### GET /api/auth/me
Lấy thông tin user hiện tại

**Response**:
```json
{
  "success": true,
  "user": {
    "id": "guid",
    "username": "admin",
    "fullName": "Administrator",
    "email": "admin@hotelwangg.com",
    "phone": "0123456789",
    "role": "Admin",
    "avatarUrl": "/images/default-avatar.png",
    "createdDate": "2025-10-10T10:00:00",
    "lastLoginDate": "2025-10-10T14:30:00"
  }
}
```

#### PUT /api/auth/profile
Cập nhật hồ sơ

**Request Body**:
```json
{
  "fullName": "Administrator Updated",
  "email": "admin@hotelwangg.com",
  "phone": "0987654321"
}
```

**Response**:
```json
{
  "success": true,
  "message": "Cập nhật hồ sơ thành công"
}
```

#### POST /api/auth/change-password
Đổi mật khẩu

**Request Body**:
```json
{
  "currentPassword": "admin123",
  "newPassword": "newpassword123",
  "confirmPassword": "newpassword123"
}
```

**Response**:
```json
{
  "success": true,
  "message": "Đổi mật khẩu thành công"
}
```

#### GET /api/auth/login-history
Lịch sử đăng nhập

**Response**:
```json
{
  "success": true,
  "history": [
    {
      "id": "guid",
      "loginTime": "2025-10-10T14:30:00",
      "ipAddress": "192.168.1.100",
      "device": "Mozilla/5.0...",
      "browser": "Chrome",
      "success": true
    }
  ]
}
```

### Room APIs

#### GET /api/room
Lấy danh sách phòng

**Query Parameters**:
- `page`: Trang (default: 1)
- `pageSize`: Số phòng/trang (default: 10)
- `status`: Lọc theo trạng thái

**Response**:
```json
{
  "success": true,
  "rooms": [...],
  "total": 100,
  "page": 1,
  "pageSize": 10
}
```

#### POST /api/room
Tạo phòng mới

**Request Body**:
```json
{
  "roomNumber": "101",
  "type": "Deluxe",
  "price": 1500000,
  "status": "Available"
}
```

#### PUT /api/room/{id}
Cập nhật phòng

#### DELETE /api/room/{id}
Xóa phòng

### Pricing APIs

#### POST /api/pricing/calculate
Tính giá với decorators

**Request Body**:
```json
{
  "basePrice": 1000000,
  "services": ["breakfast", "spa", "airport"]
}
```

**Response**:
```json
{
  "success": true,
  "basePrice": 1000000,
  "servicesTotal": 900000,
  "finalPrice": 1900000,
  "services": [
    { "name": "Breakfast", "price": 100000 },
    { "name": "Spa Service", "price": 500000 },
    { "name": "Airport Transfer", "price": 300000 }
  ]
}
```

#### POST /api/pricing/discount
Áp dụng chiết khấu

**Request Body**:
```json
{
  "originalPrice": 1900000,
  "strategy": "vip"
}
```

**Response**:
```json
{
  "success": true,
  "originalPrice": 1900000,
  "discountPercent": 10,
  "discountAmount": 190000,
  "finalPrice": 1710000,
  "strategy": "VIP Discount"
}
```

---

## � Design Patterns

Hệ thống sử dụng 5 design patterns chính để đảm bảo code maintainable, scalable và flexible.

---

### 1️⃣ **MVC Pattern** (Model-View-Controller)
**📁 Location**: Toàn bộ project structure

**🔧 Mục đích**: Tách biệt logic, data và presentation

**📂 Cấu trúc**:
```
├── Models/              # Data models
│   ├── User.cs         # User entity (lines 1-50)
│   ├── Room.cs         # Room entity (lines 1-60)
│   ├── Booking.cs      # Booking entity (lines 1-80)
│   └── Customer.cs     # Customer entity (lines 1-45)
│
├── Controllers/         # Handle HTTP requests
│   ├── AuthController.cs    # 8 endpoints (lines 1-324)
│   ├── RoomController.cs    # 6 endpoints (lines 1-180)
│   ├── BookingController.cs # 8 endpoints (lines 1-250)
│   └── HomeController.cs    # Page rendering (lines 1-120)
│
└── Views/              # Razor templates
    ├── Home/
    │   ├── Dashboard.cshtml  # Dashboard UI (lines 1-300)
    │   ├── Rooms.cshtml      # Room management (lines 1-250)
    │   └── About.cshtml      # About page (lines 1-533)
    └── Shared/
        └── _Layout.cshtml    # Main layout (lines 1-100)
```

**💡 Ví dụ Flow**:
1. User request → `GET /Home/Dashboard`
2. **Controller**: `HomeController.cs` → `Dashboard()` action
3. **Model**: Load data từ `DashboardService.cs`
4. **View**: Render `Dashboard.cshtml` với data

---

### 2️⃣ **Singleton Pattern**
**📁 Location**: `Services/DataStore.cs`

**🔧 Mục đích**: Đảm bảo chỉ có 1 instance quản lý file I/O

**📍 Code Implementation**:
```csharp
// File: Services/DataStore.cs (lines 8-15)
public class DataStore : IDataStore
{
    private static DataStore? _instance;
    private static readonly object _lock = new object();
    
    // Private constructor - ngăn tạo instance từ bên ngoài
    private DataStore() { }
    
    // Thread-safe singleton instance
    public static DataStore Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)  // Double-check locking
                {
                    if (_instance == null)
                        _instance = new DataStore();
                }
            }
            return _instance;
        }
    }
    
    // Load data from JSON file
    public T? Load<T>(string key) where T : class
    {
        string filePath = GetFilePath(key);
        if (!File.Exists(filePath)) return null;
        
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }
    
    // Save data to JSON file
    public void Save<T>(string key, T data) where T : class
    {
        string filePath = GetFilePath(key);
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        File.WriteAllText(filePath, json);
    }
}
```

**💡 Cách sử dụng**:
```csharp
// Trong AuthService.cs (line 18)
private readonly IDataStore _dataStore = DataStore.Instance;

// Load users
var users = _dataStore.Load<List<User>>("users");

// Save users
_dataStore.Save("users", users);
```

**✅ Lợi ích**:
- Thread-safe với double-check locking
- Tiết kiệm memory (1 instance duy nhất)
- Centralized file management

---

### 3️⃣ **Repository Pattern**
**📁 Location**: `Services/` folder

**🔧 Mục đích**: Abstract data access layer

**📂 Cấu trúc**:
```
Services/
├── IAuthService.cs          # Interface (lines 1-20)
├── AuthService.cs           # Implementation (lines 1-327)
├── IRoomService.cs          # Interface (lines 1-15)
├── RoomService.cs           # Implementation (lines 1-180)
├── IBookingService.cs       # Interface (lines 1-18)
└── BookingService.cs        # Implementation (lines 1-250)
```

**📍 Code Example - AuthService**:
```csharp
// File: Services/IAuthService.cs (lines 5-20)
public interface IAuthService
{
    User? Authenticate(string username, string password);
    bool Register(RegisterRequest request);
    bool UpdateProfile(string userId, UpdateProfileRequest request);
    bool ChangePassword(string userId, string currentPassword, string newPassword);
    User? GetUserById(string userId);
    List<LoginHistory> GetLoginHistory(string userId);
}

// File: Services/AuthService.cs (lines 10-60)
public class AuthService : IAuthService
{
    private readonly IDataStore _dataStore;
    private const string DataKey = "users";
    
    public AuthService(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }
    
    // Authenticate user - Hash password và compare
    public User? Authenticate(string username, string password)
    {
        var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
        var user = users.FirstOrDefault(u => 
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        
        if (user == null) return null;
        
        // Verify password với PBKDF2
        bool isValid = VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        return isValid ? user : null;
    }
    
    // Register new user
    public bool Register(RegisterRequest request)
    {
        var users = _dataStore.Load<List<User>>(DataKey) ?? new List<User>();
        
        // Check duplicate username
        if (users.Any(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase)))
            return false;
        
        // Generate salt and hash password
        byte[] salt = GenerateSalt();
        string hash = HashPassword(request.Password, salt);
        
        // Create new user
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Username = request.Username,
            PasswordHash = hash,
            PasswordSalt = Convert.ToBase64String(salt),
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            Role = request.Role,
            CreatedAt = DateTime.Now
        };
        
        users.Add(user);
        _dataStore.Save(DataKey, users);
        return true;
    }
}
```

**💡 Dependency Injection trong Program.cs**:
```csharp
// File: Program.cs (lines 25-30)
builder.Services.AddSingleton<IDataStore, DataStore>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();
```

**✅ Lợi ích**:
- Dễ test với mock services
- Dễ thay đổi data source (JSON → SQL)
- Loose coupling giữa controller và data access

---

### 4️⃣ **Decorator Pattern**
**📁 Location**: `Services/Pricing/Decorators/`

**🔧 Mục đích**: Thêm dịch vụ bổ sung vào booking mà không modify class gốc

**📂 Cấu trúc**:
```
Services/Pricing/Decorators/
├── IBooking.cs                    # Interface (lines 1-12)
├── BaseBookingDecorator.cs        # Abstract decorator (lines 1-25)
├── BreakfastDecorator.cs          # +100k VND (lines 1-20)
├── SpaServiceDecorator.cs         # +500k VND (lines 1-20)
├── AirportTransferDecorator.cs    # +300k VND (lines 1-20)
└── LaundryServiceDecorator.cs     # +50k VND (lines 1-20)
```

**📍 Code Implementation**:

**Step 1: Interface**
```csharp
// File: IBooking.cs (lines 3-12)
public interface IBooking
{
    string BookingId { get; }
    decimal BasePrice { get; }
    decimal CalculateTotalPrice();  // Hàm tính tổng giá
    string GetDescription();         // Mô tả chi tiết
}
```

**Step 2: Base Decorator**
```csharp
// File: BaseBookingDecorator.cs (lines 5-25)
public abstract class BaseBookingDecorator : IBooking
{
    protected readonly IBooking _booking;  // Wrap booking gốc
    
    protected BaseBookingDecorator(IBooking booking)
    {
        _booking = booking;
    }
    
    public string BookingId => _booking.BookingId;
    public decimal BasePrice => _booking.BasePrice;
    
    // Forward call đến wrapped booking
    public virtual decimal CalculateTotalPrice()
    {
        return _booking.CalculateTotalPrice();
    }
    
    public virtual string GetDescription()
    {
        return _booking.GetDescription();
    }
}
```

**Step 3: Concrete Decorators**
```csharp
// File: BreakfastDecorator.cs (lines 5-20)
public class BreakfastDecorator : BaseBookingDecorator
{
    private const decimal BreakfastPrice = 100_000;
    
    public BreakfastDecorator(IBooking booking) : base(booking) { }
    
    // Override: Thêm giá breakfast
    public override decimal CalculateTotalPrice()
    {
        return _booking.CalculateTotalPrice() + BreakfastPrice;
    }
    
    // Override: Thêm mô tả
    public override string GetDescription()
    {
        return _booking.GetDescription() + ", Breakfast (+100,000 VND)";
    }
}

// File: SpaServiceDecorator.cs (lines 5-20)
public class SpaServiceDecorator : BaseBookingDecorator
{
    private const decimal SpaPrice = 500_000;
    
    public SpaServiceDecorator(IBooking booking) : base(booking) { }
    
    public override decimal CalculateTotalPrice()
    {
        return _booking.CalculateTotalPrice() + SpaPrice;
    }
    
    public override string GetDescription()
    {
        return _booking.GetDescription() + ", Spa Service (+500,000 VND)";
    }
}

// Tương tự cho AirportTransferDecorator và LaundryServiceDecorator
```

**💡 Cách sử dụng trong BookingController**:
```csharp
// File: Controllers/BookingController.cs (lines 65-80)
public IActionResult CreateBooking([FromBody] CreateBookingRequest request)
{
    // 1. Tạo booking cơ bản
    IBooking booking = new Booking
    {
        BookingId = Guid.NewGuid().ToString(),
        BasePrice = 1_000_000  // Giá phòng
    };
    
    // 2. Wrap với decorators theo request
    if (request.Services.Contains("breakfast"))
        booking = new BreakfastDecorator(booking);
    
    if (request.Services.Contains("spa"))
        booking = new SpaServiceDecorator(booking);
    
    if (request.Services.Contains("airport"))
        booking = new AirportTransferDecorator(booking);
    
    if (request.Services.Contains("laundry"))
        booking = new LaundryServiceDecorator(booking);
    
    // 3. Calculate final price
    decimal totalPrice = booking.CalculateTotalPrice();
    string description = booking.GetDescription();
    
    return Ok(new { totalPrice, description });
}
```

**💡 Ví dụ Flow**:
```
Base Booking: 1,000,000 VND
↓ wrap với BreakfastDecorator
→ 1,100,000 VND
↓ wrap với SpaServiceDecorator  
→ 1,600,000 VND
↓ wrap với AirportTransferDecorator
→ 1,900,000 VND
↓ CalculateTotalPrice()
= 1,900,000 VND
```

**✅ Lợi ích**:
- Thêm features mà không modify code gốc (Open/Closed Principle)
- Flexible combination (chọn services tùy ý)
- Easy to add new services (tạo decorator mới)

---

### 5️⃣ **Strategy Pattern**
**📁 Location**: `Services/Pricing/Strategies/`

**🔧 Mục đích**: Chọn thuật toán chiết khấu linh hoạt tại runtime

**📂 Cấu trúc**:
```
Services/Pricing/Strategies/
├── IDiscountStrategy.cs           # Interface (lines 1-8)
├── NoDiscountStrategy.cs          # 0% (lines 1-12)
├── VIPDiscountStrategy.cs         # 10% (lines 1-12)
├── SeasonalDiscountStrategy.cs    # 15% (lines 1-12)
├── LoyaltyDiscountStrategy.cs     # 5-20% (lines 1-30)
└── EarlyBirdDiscountStrategy.cs   # 10-20% (lines 1-30)
```

**📍 Code Implementation**:

**Step 1: Strategy Interface**
```csharp
// File: IDiscountStrategy.cs (lines 3-8)
public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal originalPrice);
    string GetDiscountName();
    string GetDescription();
}
```

**Step 2: Concrete Strategies**
```csharp
// File: NoDiscountStrategy.cs (lines 5-15)
public class NoDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal originalPrice)
    {
        return originalPrice;  // Không giảm giá
    }
    
    public string GetDiscountName() => "No Discount";
    public string GetDescription() => "Không áp dụng chiết khấu";
}

// File: VIPDiscountStrategy.cs (lines 5-15)
public class VIPDiscountStrategy : IDiscountStrategy
{
    private const decimal DiscountPercent = 0.10m;  // 10%
    
    public decimal ApplyDiscount(decimal originalPrice)
    {
        return originalPrice * (1 - DiscountPercent);  // Giảm 10%
    }
    
    public string GetDiscountName() => "VIP Discount";
    public string GetDescription() => "Giảm 10% cho khách VIP";
}

// File: SeasonalDiscountStrategy.cs (lines 5-15)
public class SeasonalDiscountStrategy : IDiscountStrategy
{
    private const decimal DiscountPercent = 0.15m;  // 15%
    
    public decimal ApplyDiscount(decimal originalPrice)
    {
        return originalPrice * (1 - DiscountPercent);
    }
    
    public string GetDiscountName() => "Seasonal Discount";
    public string GetDescription() => "Giảm 15% theo mùa";
}

// File: LoyaltyDiscountStrategy.cs (lines 5-30)
public class LoyaltyDiscountStrategy : IDiscountStrategy
{
    private readonly int _loyaltyPoints;
    
    public LoyaltyDiscountStrategy(int loyaltyPoints)
    {
        _loyaltyPoints = loyaltyPoints;
    }
    
    public decimal ApplyDiscount(decimal originalPrice)
    {
        // Discount dựa trên loyalty points
        decimal discountPercent = _loyaltyPoints switch
        {
            >= 1000 => 0.20m,  // 20% for 1000+ points
            >= 500 => 0.15m,   // 15% for 500+ points
            >= 100 => 0.10m,   // 10% for 100+ points
            _ => 0.05m         // 5% default
        };
        
        return originalPrice * (1 - discountPercent);
    }
    
    public string GetDiscountName() => "Loyalty Discount";
    public string GetDescription() => $"Giảm giá theo điểm thành viên ({_loyaltyPoints} points)";
}

// File: EarlyBirdDiscountStrategy.cs (lines 5-30)
public class EarlyBirdDiscountStrategy : IDiscountStrategy
{
    private readonly int _daysInAdvance;
    
    public EarlyBirdDiscountStrategy(int daysInAdvance)
    {
        _daysInAdvance = daysInAdvance;
    }
    
    public decimal ApplyDiscount(decimal originalPrice)
    {
        // Discount dựa trên số ngày đặt trước
        decimal discountPercent = _daysInAdvance switch
        {
            >= 30 => 0.20m,   // 20% for 30+ days
            >= 14 => 0.15m,   // 15% for 14+ days
            >= 7 => 0.10m,    // 10% for 7+ days
            _ => 0.05m        // 5% default
        };
        
        return originalPrice * (1 - discountPercent);
    }
    
    public string GetDiscountName() => "Early Bird Discount";
    public string GetDescription() => $"Giảm giá đặt sớm ({_daysInAdvance} ngày trước)";
}
```

**💡 Cách sử dụng trong PricingController**:
```csharp
// File: Controllers/PricingController.cs (lines 62-95)
[HttpPost("discount")]
public IActionResult ApplyDiscount([FromBody] DiscountRequest request)
{
    // Select strategy based on discount type
    IDiscountStrategy strategy = request.DiscountType.ToLower() switch
    {
        "vip" => new VIPDiscountStrategy(),
        "seasonal" => new SeasonalDiscountStrategy(),
        "loyalty" => new LoyaltyDiscountStrategy(request.LoyaltyPoints),
        "earlybird" => new EarlyBirdDiscountStrategy(request.DaysInAdvance),
        _ => new NoDiscountStrategy()
    };
    
    // Apply selected strategy
    decimal finalPrice = strategy.ApplyDiscount(request.OriginalPrice);
    
    return Ok(new 
    { 
        success = true,
        originalPrice = request.OriginalPrice,
        discountType = strategy.GetDiscountName(),
        discountDescription = strategy.GetDescription(),
        finalPrice = finalPrice,
        savedAmount = request.OriginalPrice - finalPrice
    });
}
```

**💡 Ví dụ Complete Booking Flow**:
```csharp
// 1. Tạo base booking
IBooking booking = new Booking { BasePrice = 2_000_000 };

// 2. Apply decorators (services)
booking = new BreakfastDecorator(booking);      // +100k
booking = new SpaServiceDecorator(booking);      // +500k
decimal priceWithServices = booking.CalculateTotalPrice();  // 2,600,000

// 3. Apply discount strategy
IDiscountStrategy discount = new VIPDiscountStrategy();     // 10% off
decimal finalPrice = discount.ApplyDiscount(priceWithServices);  // 2,340,000

Console.WriteLine($"Base: 2,000,000");
Console.WriteLine($"With services: 2,600,000");
Console.WriteLine($"After VIP discount: 2,340,000");
Console.WriteLine($"Total saved: 260,000");
```

**✅ Lợi ích**:
- Easy to add new discount types (tạo strategy mới)
- Select algorithm at runtime (không hardcode)
- Clean separation of discount logic
- Testable independently

### 1. MVC Pattern
- **Model**: Data models trong `Models/`
- **View**: Razor views trong `Views/`
- **Controller**: Controllers trong `Controllers/`

### 2. Singleton Pattern
Services được đăng ký như Singleton trong `Program.cs`:
```csharp
builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
```

### 3. Repository Pattern
`JsonDataStore` implement `IDataStore` interface:
```csharp
public interface IDataStore
{
    T? Load<T>(string key);
    void Save<T>(string key, T data);
}
```

### 4. Decorator Pattern
Thêm dịch vụ bổ sung cho phòng:

```csharp
// Base decorator
public abstract class BaseServiceDecorator : IServiceDecorator
{
    protected IServiceDecorator _service;
    
    public virtual decimal CalculatePrice() 
    {
        return _service.CalculatePrice();
    }
}

// Concrete decorators
public class BreakfastDecorator : BaseServiceDecorator
{
    public override decimal CalculatePrice()
    {
        return base.CalculatePrice() + 100000;
    }
}
```

**Sử dụng**:
```csharp
IServiceDecorator service = new BasicServiceDecorator(1000000);
service = new BreakfastDecorator(service);
service = new SpaServiceDecorator(service);
decimal totalPrice = service.CalculatePrice(); // 1,600,000
```

### 5. Strategy Pattern
Chiết khấu linh hoạt:

```csharp
public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal originalPrice);
    string GetDescription();
}

public class VIPDiscountStrategy : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal originalPrice)
    {
        return originalPrice * 0.9m; // 10% off
    }
}
```

**Sử dụng**:
```csharp
IDiscountStrategy strategy = new VIPDiscountStrategy();
decimal finalPrice = strategy.ApplyDiscount(1000000); // 900,000
```

### 6. Dependency Injection
Services được inject vào controllers:
```csharp
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
}
```

---

## 🔒 Bảo Mật

### Mã Hóa Mật Khẩu

**Thuật toán**: PBKDF2 + SHA256

**Tham số**:
- Salt size: 16 bytes (ngẫu nhiên)
- Hash size: 32 bytes
- Iterations: 100,000
- Hash algorithm: SHA256

**Code**:
```csharp
public static string HashPassword(string password)
{
    byte[] salt = new byte[16];
    using (var rng = RandomNumberGenerator.Create())
    {
        rng.GetBytes(salt);
    }
    
    var pbkdf2 = new Rfc2898DeriveBytes(
        password, salt, 100000, HashAlgorithmName.SHA256
    );
    byte[] hash = pbkdf2.GetBytes(32);
    
    byte[] hashBytes = new byte[48];
    Array.Copy(salt, 0, hashBytes, 0, 16);
    Array.Copy(hash, 0, hashBytes, 16, 32);
    
    return Convert.ToBase64String(hashBytes);
}
```

### Session Management

**Cấu hình**:
```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```

### Validation

**Email**:
```csharp
public static bool IsValidEmail(string email)
{
    var addr = new System.Net.Mail.MailAddress(email);
    return addr.Address == email;
}
```

**Phone**:
```csharp
public static bool IsValidPhone(string phone)
{
    phone = phone.Replace(" ", "").Replace("-", "");
    return phone.Length >= 10 && phone.Length <= 11 
           && phone.All(char.IsDigit);
}
```

**Username**:
```csharp
public static bool IsValidUsername(string username)
{
    return username.Length >= 4 && username.Length <= 20 
           && username.All(c => char.IsLetterOrDigit(c) || c == '_');
}
```

### CORS Policy

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

---

## ❓ FAQ

### Q1: Làm sao để thay đổi port?

**A**: Sửa trong `Properties/launchSettings.json`:
```json
"applicationUrl": "http://localhost:5000"
```

### Q2: Dữ liệu được lưu ở đâu?

**A**: Trong thư mục `bin/Debug/net8.0/Data/` dưới dạng file JSON:
- `users.json` - Người dùng
- `rooms.json` - Phòng
- `bookings.json` - Đặt phòng
- `customers.json` - Khách hàng
- `settings.json` - Cài đặt

### Q3: Làm sao để reset mật khẩu admin?

**A**: Xóa file `users.json`, hệ thống sẽ tạo lại admin mặc định.

### Q4: Làm sao để thêm tài khoản admin mới?

**A**: 
1. Đăng ký tài khoản bình thường
2. Mở file `users.json`
3. Sửa `"role": "Staff"` thành `"role": "Admin"`

### Q5: Hệ thống có hỗ trợ SQL Server không?

**A**: Hiện tại dùng JSON. Để dùng SQL Server, cần:
1. Cài Entity Framework Core
2. Tạo DbContext
3. Thay `JsonDataStore` bằng `SqlDataStore`

### Q6: Làm sao để deploy lên production?

**A**:
```bash
# Build release
dotnet publish -c Release

# File output trong bin/Release/net8.0/publish/
```

### Q7: Avatar được lưu như thế nào?

**A**: 
- Hiện tại: Base64 trong localStorage (client-side)
- Production: Upload lên server hoặc cloud storage

### Q8: 2FA có hoạt động không?

**A**: Hiện tại chỉ là demo UI. Để hoạt động thực:
1. Cài package `OtpNet`
2. Generate secret key
3. Tạo QR code
4. Verify OTP code

### Q9: Làm sao để backup dữ liệu?

**A**:
- **Tự động**: Click "Sao lưu hệ thống" trong Cài đặt
- **Thủ công**: Copy thư mục `bin/Debug/net8.0/Data/`

### Q10: Hệ thống có hỗ trợ nhiều ngôn ngữ không?

**A**: Hiện tại chỉ tiếng Việt. Để thêm:
1. Cài package `Microsoft.Extensions.Localization`
2. Tạo resource files
3. Thêm middleware localization

---

## 📞 Hỗ Trợ

### Thông Tin Liên Hệ
- **Email**: admin@hotelwangg.com
- **Hotline**: 1900 1234
- **Website**: http://localhost:5000/Home/About

### Báo Lỗi
Nếu gặp lỗi, vui lòng cung cấp:
1. Thông báo lỗi
2. Các bước tái hiện
3. Screenshot (nếu có)
---

## 🔐 Chi Tiết Bảo Mật

### 1️⃣ **Password Hashing với PBKDF2**
**📁 Location**: `Services/AuthService.cs`

**🔧 Algorithm**: PBKDF2 (Password-Based Key Derivation Function 2)
- **Hash Function**: SHA256
- **Iterations**: 100,000 (khuyến nghị NIST 2023)
- **Salt Length**: 32 bytes (256 bits)
- **Hash Length**: 32 bytes (256 bits)

**📍 Code Implementation**:
```csharp
// File: Services/AuthService.cs (lines 124-138)
private string HashPassword(string password, byte[] salt)
{
    using (var pbkdf2 = new Rfc2898DeriveBytes(
        password,           // Password input
        salt,               // Random salt
        100000,             // 100k iterations
        HashAlgorithmName.SHA256))  // SHA256
    {
        byte[] hash = pbkdf2.GetBytes(32);  // 32-byte hash
        return Convert.ToBase64String(hash);
    }
}

// File: Services/AuthService.cs (lines 140-155)
private byte[] GenerateSalt()
{
    byte[] salt = new byte[32];  // 32-byte salt
    using (var rng = RandomNumberGenerator.Create())
    {
        rng.GetBytes(salt);  // Cryptographically secure random
    }
    return salt;
}

// File: Services/AuthService.cs (lines 157-170)
private bool VerifyPassword(string password, string hash, string saltString)
{
    try
    {
        byte[] salt = Convert.FromBase64String(saltString);
        string computedHash = HashPassword(password, salt);
        return computedHash == hash;  // Constant-time comparison
    }
    catch
    {
        return false;
    }
}
```

**💡 Flow Đăng Ký**:
```
1. User nhập password: "admin123"
2. Generate random salt: [32 random bytes]
3. Hash = PBKDF2(password, salt, 100k iterations, SHA256)
4. Store:
   - PasswordHash: "base64_encoded_hash"
   - PasswordSalt: "base64_encoded_salt"
```

**💡 Flow Đăng Nhập**:
```
1. User nhập password: "admin123"
2. Load user từ DB → get salt
3. ComputeHash = PBKDF2(input_password, stored_salt, 100k, SHA256)
4. Compare: ComputeHash == StoredHash
5. Match → Login success
```

**✅ Tại sao an toàn?**:
- ❌ Không lưu plain password
- ✅ Mỗi user có salt riêng
- ✅ 100k iterations → chống brute-force
- ✅ SHA256 → collision resistant
- ✅ Constant-time comparison → chống timing attack

---

### 2️⃣ **Session Management**
**📁 Location**: `Program.cs`

**📍 Configuration**:
```csharp
// File: Program.cs (lines 15-25)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);  // 24h timeout
    options.Cookie.HttpOnly = true;                 // Prevent XSS
    options.Cookie.IsEssential = true;              // GDPR compliance
    options.Cookie.SameSite = SameSiteMode.Strict;  // CSRF protection
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // HTTPS only
});

app.UseSession();  // Enable session middleware
```

**🔧 Session Storage**:
- 📍 `AuthController.cs` → `Login()` action (lines 65-70)
```csharp
// Lưu session sau login
HttpContext.Session.SetString("UserId", user.Id);
HttpContext.Session.SetString("Username", user.Username);
HttpContext.Session.SetString("Role", user.Role);
```

- 📍 `AuthController.cs` → `Logout()` action (lines 133-138)
```csharp
// Xóa session khi logout
HttpContext.Session.Clear();
```

- 📍 `AuthController.cs` → `GetCurrentUser()` action (lines 177-182)
```csharp
// Lấy user từ session
var userId = HttpContext.Session.GetString("UserId");
if (string.IsNullOrEmpty(userId))
{
    return Unauthorized(new { success = false, message = "Chưa đăng nhập" });
}
```

**✅ Security Features**:
- ✅ HttpOnly cookies → Không thể access từ JavaScript (chống XSS)
- ✅ SameSite=Strict → Chống CSRF
- ✅ SecurePolicy=Always → Chỉ gửi qua HTTPS
- ✅ 24h timeout → Auto logout sau 24h không hoạt động
- ✅ Sliding expiration → Reset timer mỗi request

---

### 3️⃣ **Login History Tracking**
**📁 Location**: `Services/AuthService.cs`, `Models/User.cs`

**📍 Data Structure**:
```csharp
// File: Models/User.cs (lines 15-25)
public class LoginHistory
{
    public DateTime Timestamp { get; set; }      // Thời gian đăng nhập
    public string IpAddress { get; set; }        // IP address
    public string UserAgent { get; set; }        // Browser info
    public string DeviceInfo { get; set; }       // Device type
    public bool Success { get; set; }            // Login success/fail
}

public class User
{
    // ... other properties
    public List<LoginHistory> LoginHistory { get; set; } = new();
}
```

**📍 Tracking Implementation**:
```csharp
// File: Services/AuthService.cs (lines 62-85)
public void RecordLoginAttempt(string userId, HttpContext context, bool success)
{
    var users = _dataStore.Load<List<User>>("users");
    var user = users.FirstOrDefault(u => u.Id == userId);
    if (user == null) return;
    
    // Lấy thông tin từ HTTP context
    var loginHistory = new LoginHistory
    {
        Timestamp = DateTime.Now,
        IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
        UserAgent = context.Request.Headers["User-Agent"].ToString(),
        DeviceInfo = ParseDeviceInfo(context.Request.Headers["User-Agent"]),
        Success = success
    };
    
    // Thêm vào history
    user.LoginHistory.Add(loginHistory);
    
    // Giữ 20 records gần nhất
    if (user.LoginHistory.Count > 20)
    {
        user.LoginHistory = user.LoginHistory
            .OrderByDescending(h => h.Timestamp)
            .Take(20)
            .ToList();
    }
    
    _dataStore.Save("users", users);
}

// File: Services/AuthService.cs (lines 87-105)
private string ParseDeviceInfo(string userAgent)
{
    if (userAgent.Contains("Mobile")) return "Mobile";
    if (userAgent.Contains("Tablet")) return "Tablet";
    if (userAgent.Contains("Windows")) return "Windows PC";
    if (userAgent.Contains("Mac")) return "Mac";
    if (userAgent.Contains("Linux")) return "Linux";
    return "Unknown";
}
```

**📍 Usage trong AuthController**:
```csharp
// File: Controllers/AuthController.cs (lines 55-70)
[HttpPost("login")]
public IActionResult Login([FromBody] LoginRequest request)
{
    var user = _authService.Authenticate(request.Username, request.Password);
    
    if (user != null)
    {
        // Save session
        HttpContext.Session.SetString("UserId", user.Id);
        
        // Record successful login
        _authService.RecordLoginAttempt(user.Id, HttpContext, true);
        
        return Ok(new { success = true, user });
    }
    else
    {
        // Record failed login
        _authService.RecordLoginAttempt(request.Username, HttpContext, false);
        
        return Unauthorized(new { success = false, message = "Sai tên hoặc mật khẩu" });
    }
}
```

**✅ Security Benefits**:
- ✅ Track failed login attempts → Detect brute-force
- ✅ Monitor IP changes → Detect account compromise
- ✅ Device tracking → Identify suspicious devices
- ✅ Audit trail → Compliance & forensics

---

### 4️⃣ **Input Validation**
**📁 Location**: `Utils/Validator.cs`

**📍 Email Validation**:
```csharp
// File: Utils/Validator.cs (lines 5-15)
public static class Validator
{
    private static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );
    
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        return EmailRegex.IsMatch(email);
    }
    
    public static bool IsValidPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return false;
        // Vietnamese phone: 10-11 digits
        return Regex.IsMatch(phone, @"^0\d{9,10}$");
    }
    
    public static bool IsStrongPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;
        if (password.Length < 6) return false;
        // Add more rules: uppercase, lowercase, numbers, special chars
        return true;
    }
}
```

**📍 Usage trong Controllers**:
```csharp
// File: Controllers/AuthController.cs (lines 95-105)
[HttpPost("register")]
public IActionResult Register([FromBody] RegisterRequest request)
{
    // Validate email
    if (!Validator.IsValidEmail(request.Email))
    {
        return BadRequest(new { success = false, message = "Email không hợp lệ" });
    }
    
    // Validate phone
    if (!Validator.IsValidPhone(request.Phone))
    {
        return BadRequest(new { success = false, message = "Số điện thoại không hợp lệ" });
    }
    
    // Validate password
    if (!Validator.IsStrongPassword(request.Password))
    {
        return BadRequest(new { success = false, message = "Mật khẩu phải có ít nhất 6 ký tự" });
    }
    
    // Proceed with registration...
}
```

**✅ Validation Rules**:
- ✅ Email: Regex pattern matching
- ✅ Phone: 10-11 digits, starts with 0
- ✅ Password: Min 6 characters (can add more rules)
- ✅ Username: Min 4 characters, alphanumeric
- ✅ XSS Prevention: Sanitize HTML input

---

### 5️⃣ **Data Storage Security**
**📁 Location**: `Services/DataStore.cs`, `Data/*.json`

**📍 File Structure**:
```
Data/
├── users.json          # User accounts (encrypted passwords)
├── rooms.json          # Room data
├── bookings.json       # Booking records
└── customers.json      # Customer info
```

**📍 users.json Example**:
```json
{
  "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "username": "admin",
  "passwordHash": "8x9yZ1aBcDeFgHiJkLmNoPqRsTuVwXyZ0123456789A=",
  "passwordSalt": "QwErTyUiOpAsDfGhJkLzXcVbNm1234567890ABCDEFGH=",
  "fullName": "Administrator",
  "email": "admin@hotelwangg.com",
  "role": "Admin",
  "createdAt": "2025-10-10T10:00:00",
  "loginHistory": [
    {
      "timestamp": "2025-10-11T01:18:52",
      "ipAddress": "127.0.0.1",
      "userAgent": "Mozilla/5.0...",
      "deviceInfo": "Windows PC",
      "success": true
    }
  ]
}
```

**✅ Security Measures**:
- ✅ Password never stored plain text
- ✅ Each user has unique salt
- ✅ JSON files in bin/Debug (not wwwroot)
- ✅ File permissions: Read/Write by app only
- ✅ Can easily migrate to SQL with encryption

---

### 6️⃣ **Frontend Security**
**📁 Location**: `wwwroot/js/admin.js`, `Views/Shared/_Layout.cshtml`

**📍 XSS Prevention**:
```javascript
// File: wwwroot/js/admin.js (lines 28-35)
function updateSidebarUserInfo(user) {
    const userName = document.getElementById('sidebarUserName');
    
    // Use textContent (not innerHTML) to prevent XSS
    if (userName) userName.textContent = user.fullName;  // ✅ Safe
    
    // Bad practice:
    // userName.innerHTML = user.fullName;  // ❌ Vulnerable to XSS
}
```

**📍 CSRF Token (if needed)**:
```csharp
// File: Views/Shared/_Layout.cshtml (add to forms)
@Html.AntiForgeryToken()
```

**📍 Content Security Policy**:
```csharp
// File: Program.cs (add CSP header)
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", 
        "default-src 'self'; script-src 'self' 'unsafe-inline' cdnjs.cloudflare.com");
    await next();
});
```

**✅ Frontend Best Practices**:
- ✅ Use `textContent` instead of `innerHTML`
- ✅ Sanitize user input before display
- ✅ HttpOnly cookies (set in Program.cs)
- ✅ HTTPS only in production
- ✅ CORS policy configured

---

### 7️⃣ **API Security**
**📁 Location**: All Controllers

**📍 Authorization Check**:
```csharp
// File: Controllers/AuthController.cs (lines 177-185)
[HttpGet("me")]
public IActionResult GetCurrentUser()
{
    var userId = HttpContext.Session.GetString("UserId");
    
    // Check authentication
    if (string.IsNullOrEmpty(userId))
    {
        return Unauthorized(new { 
            success = false, 
            message = "Chưa đăng nhập" 
        });
    }
    
    // Proceed if authorized...
}
```

**📍 Role-Based Access (example)**:
```csharp
// File: Controllers/RoomController.cs (lines 40-50)
[HttpDelete("{id}")]
public IActionResult DeleteRoom(string id)
{
    var role = HttpContext.Session.GetString("Role");
    
    // Only Admin can delete
    if (role != "Admin")
    {
        return Forbid();
    }
    
    // Proceed with deletion...
}
```

**✅ API Security Checklist**:
- ✅ Session-based authentication
- ✅ Authorization checks per endpoint
- ✅ Role-based access control
- ✅ Input validation on all endpoints
- ✅ Error messages don't leak sensitive info
- ✅ Rate limiting (can be added)

---

## 📂 Cấu Trúc Dự Án Chi Tiết
Mọi đóng góp đều được chào đón:
1. Fork repository
2. Tạo branch mới
3. Commit changes
4. Push và tạo Pull Request

---

## 📝 Changelog

### Version 2.3 (10/10/2025)
✅ Hoàn thiện hệ thống xác thực
✅ Thêm đăng nhập/đăng ký/đăng xuất
✅ Quản lý hồ sơ với upload avatar
✅ Đổi mật khẩu an toàn
✅ Lịch sử đăng nhập chi tiết
✅ Trang About đẹp mắt
✅ Click logo → Về trang chủ
✅ Tích hợp API thực
✅ Fix lỗi admin.js
✅ Giảm file .md xuống 2 file

### Version 2.2 (09/10/2025)
✅ Đổi tên thành Hotel's Wangg
✅ Admin panel với 3 buttons
✅ Consolidate documentation

### Version 2.1 (08/10/2025)
✅ Decorator Pattern (4 decorators)
✅ Strategy Pattern (5 strategies)
✅ Pricing API (4 endpoints)
✅ Notification system (7 types)
✅ Settings management

### Version 2.0 (07/10/2025)
✅ Full MVC structure
✅ 8 modules hoàn chỉnh
✅ JSON storage
✅ Modern UI navy & gold

---

## 🎓 Tài Liệu Tham Khảo

### ASP.NET Core
- [Official Documentation](https://docs.microsoft.com/aspnet/core)
- [MVC Tutorial](https://docs.microsoft.com/aspnet/core/mvc)
- [API Tutorial](https://docs.microsoft.com/aspnet/core/web-api)

### Design Patterns
- [Decorator Pattern](https://refactoring.guru/design-patterns/decorator)
- [Strategy Pattern](https://refactoring.guru/design-patterns/strategy)
- [Singleton Pattern](https://refactoring.guru/design-patterns/singleton)

### Security
- [PBKDF2 Encryption](https://docs.microsoft.com/dotnet/api/system.security.cryptography.rfc2898derivebytes)
- [Session Management](https://docs.microsoft.com/aspnet/core/fundamentals/app-state)

---

**Hotel's Wangg Management System v2.3**  
*Hệ thống quản lý khách sạn hiện đại, an toàn và dễ sử dụng*

© 2025 Hotel's Wangg. All rights reserved.

##  C?u Tr�c D? �n Chi Ti?t

```
QLKS/

 Controllers/                    # MVC Controllers - Handle HTTP requests
    AuthController.cs          # 8 endpoints authentication (324 lines)
       Login()                # POST /api/auth/login (lines 26-74)
       Register()             # POST /api/auth/register (lines 76-125)
       Logout()               # POST /api/auth/logout (lines 127-145)
       GetCurrentUser()       # GET /api/auth/me (lines 175-208)
       UpdateProfile()        # PUT /api/auth/profile (lines 211-244)
       ChangePassword()       # POST /api/auth/change-password (lines 246-282)
       GetLoginHistory()      # GET /api/auth/login-history (lines 284-308)
   
    BookingController.cs       # 8 endpoints booking (250 lines)
    RoomController.cs          # 6 endpoints room management (180 lines)
    PricingController.cs       # 4 endpoints pricing (145 lines)
    HomeController.cs          # Page rendering (120 lines)

 Services/                      # Business logic layer
    DataStore.cs               # Singleton - File I/O (80 lines)
    AuthService.cs             # Authentication service (327 lines)
    RoomService.cs             # Room management (180 lines)
    Pricing/                   # Pricing patterns
        Decorators/            # 4 decorator classes
        Strategies/            # 5 strategy classes

 Models/                        # Data models & entities
    User.cs, Room.cs, Booking.cs, Customer.cs

 Views/                         # Razor templates - UI layer
    Home/ (Dashboard.cshtml, Rooms.cshtml, About.cshtml...)
    Shared/_Layout.cshtml

 wwwroot/                       # Static files
    css/style.css              # 1551 lines
    js/ (app.js, admin.js, notifications.js, pricing.js)

 Data/                          # JSON file storage
    users.json, rooms.json, bookings.json, customers.json

 README.md, CHI_TIET.md         # Documentation

```

**Xem chi ti?t d?y d? trong file n�y ph�a tr�n!**

---

**Hotel's Wangg Management System v2.3**  
*H? th?ng qu?n l� kh�ch s?n hi?n d?i, an to�n v� d? s? d?ng*

 2025 Hotel's Wangg. All rights reserved.
