# Hotel's Wangg - Hệ Thống Quản Lý Khách Sạn

<div align="center">

![Hotel's Wangg](https://img.shields.io/badge/Hotel's%20Wangg-Management%20System-1a2f4a?style=for-the-badge&logo=hotel&logoColor=d4af37)

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=flat&logo=.net)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=c-sharp)](https://docs.microsoft.com/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green.svg?style=flat)](LICENSE)
[![Status](https://img.shields.io/badge/Status-Active-success?style=flat)](https://github.com)

**Hệ thống quản lý khách sạn hiện đại với giao diện đẹp mắt Navy Blue & Gold**

[Tính Năng](#-tính-năng) • [Cài Đặt](#-cài-đặt) • [Hướng Dẫn](#-hướng-dẫn-sử-dụng) • [API Docs](#-api-documentation) • [Chi Tiết](CHI_TIET.md)

</div>

---

## 📋 Mục Lục

- [Giới Thiệu](#-giới-thiệu)
- [Tính Năng](#-tính-năng)
- [Công Nghệ](#-công-nghệ)
- [Cài Đặt](#-cài-đặt)
- [Hướng Dẫn Sử Dụng](#-hướng-dẫn-sử-dụng)
- [API Documentation](#-api-documentation)
- [Design Patterns](#-design-patterns)
- [Screenshots](#-screenshots)
- [FAQ](#-faq)
- [Tài Liệu Chi Tiết](CHI_TIET.md)

---

## 🏨 Giới Thiệu

**Hotel's Wangg Management System** là một hệ thống quản lý khách sạn toàn diện, được xây dựng với công nghệ hiện đại ASP.NET Core 8.0. Hệ thống cung cấp giao diện người dùng đẹp mắt với theme Navy Blue & Gold, kết hợp các design patterns tiên tiến và bảo mật cao.

### ✨ Điểm Nổi Bật

- 🎨 **Giao diện hiện đại**: Theme Navy Blue (#1a2f4a) & Gold (#d4af37)
  - 📁 `wwwroot/css/style.css` - 1500+ dòng CSS với gradient, animation
  - 🎯 CSS Variables cho theme colors (lines 1-20)
  
- 🔐 **Bảo mật cao**: Mã hóa PBKDF2 + SHA256, 100k iterations
  - 📁 `Services/AuthService.cs` - Hàm `HashPassword()` (lines 124-138)
  - 🔑 Salt ngẫu nhiên 32 bytes cho mỗi password
  
- 📱 **Responsive**: Hoạt động mượt mà trên mọi thiết bị
  - 📁 `wwwroot/css/style.css` - Media queries (lines 1200-1500)
  
- ⚡ **Realtime**: Cập nhật dữ liệu ngay lập tức
  - 📁 `wwwroot/js/admin.js` - Hàm `loadSidebarUserInfo()` (lines 10-21)
  
- 🎯 **Design Patterns**: Decorator, Strategy, Singleton, Repository
  - 📁 `Services/Pricing/` - 4 Decorators + 5 Strategies
  - � `Services/DataStore.cs` - Singleton Pattern (lines 8-15)
  
- �💾 **No Database Required**: Lưu trữ JSON, không cần SQL Server
  - 📁 `Data/*.json` - users.json, rooms.json, bookings.json, customers.json
  
- 🌐 **RESTful API**: 30+ endpoints đầy đủ
  - 📁 `Controllers/AuthController.cs` - 8 endpoints authentication
  - 📁 `Controllers/RoomController.cs` - 6 endpoints quản lý phòng
  - 📁 `Controllers/BookingController.cs` - 8 endpoints booking
  
- 🔔 **Thông báo đẹp**: Toast notifications với 7 loại
  - 📁 `wwwroot/js/app.js` - Hàm `showNotification()` (lines 48-83)

---

## ⚡ Tính Năng

### 🔐 Xác Thực & Bảo Mật
**📁 Code Location**: `Controllers/AuthController.cs`, `Services/AuthService.cs`

- ✅ **Đăng nhập/Đăng ký** với mã hóa PBKDF2
  - 📍 `AuthController.cs` → `Login()` action (lines 26-74)
  - 📍 `AuthService.cs` → `Authenticate()` method (lines 33-60)
  - 🔧 Chức năng: Xác thực user, tạo session, lưu login history
  
- ✅ **Quản lý hồ sơ cá nhân**
  - 📍 `AuthController.cs` → `UpdateProfile()` action (lines 211-244)
  - 📍 `wwwroot/js/admin.js` → `saveAdminProfile()` (lines 140-179)
  - 🔧 Chức năng: Cập nhật fullName, email, phone và reload sidebar
  
- ✅ **Đổi mật khẩu an toàn**
  - 📍 `AuthController.cs` → `ChangePassword()` action (lines 246-282)
  - 📍 `Services/AuthService.cs` → `ChangePassword()` (lines 225-250)
  - 🔧 Chức năng: Validate mật khẩu cũ, hash mật khẩu mới với PBKDF2
  
- ✅ **Upload avatar**
  - 📍 `wwwroot/js/admin.js` → `changeAvatar()` (lines 181-210)
  - 🔧 Chức năng: Đọc file ảnh, convert base64, lưu localStorage, cập nhật sidebar
  
- ✅ **Lịch sử đăng nhập** (IP, thiết bị, trình duyệt)
  - 📍 `AuthController.cs` → `GetLoginHistory()` (lines 284-308)
  - 📍 `Services/AuthService.cs` → `GetLoginHistory()` (lines 252-268)
  - 🔧 Chức năng: Lưu mỗi lần login với timestamp, IP, user-agent
  
- ✅ **Session management** 24h
  - 📍 `Program.cs` → Session configuration (lines 15-20)
  - 🔧 Chức năng: Timeout 24h, HttpOnly cookies, Sliding expiration
  
- ✅ **Phân quyền**: Admin, Manager, Staff
  - 📍 `Models/User.cs` → `Role` enum (lines 25-30)
  - 🔧 Chức năng: Role-based access control

### 📊 Dashboard
**📁 Code Location**: `Controllers/HomeController.cs`, `Services/DashboardService.cs`

- ✅ **Tổng quan doanh thu realtime**
  - 📍 `DashboardService.cs` → `GetDashboardData()` (lines 15-85)
  - 🔧 Chức năng: Tính tổng revenue từ bookings, group by month
  
- ✅ **Thống kê phòng** (trống, đã đặt, bảo trì)
  - 📍 `DashboardService.cs` → Group rooms by status (lines 40-55)
  - 🔧 Chức năng: Count rooms theo RoomStatus enum
  
- ✅ **Biểu đồ doanh thu theo tháng**
  - 📍 `Views/Home/Dashboard.cshtml` → Chart rendering (lines 150-200)
  - 📍 `wwwroot/js/app.js` → Chart data processing
  - 🔧 Chức năng: Aggregate revenue by month, render với Chart.js-like API

### 🛏️ Quản Lý Phòng
**📁 Code Location**: `Controllers/RoomController.cs`, `Services/RoomService.cs`

- ✅ **CRUD phòng đầy đủ**
  - 📍 `RoomController.cs` → 6 endpoints (lines 15-200)
    - `GetRooms()` - GET all rooms
    - `CreateRoom()` - POST new room  
    - `UpdateRoom()` - PUT room by ID
    - `DeleteRoom()` - DELETE room by ID
  - 🔧 Chức năng: Full CRUD operations với validation
  
- ✅ **4 loại phòng**: Standard, Deluxe, Suite, Presidential
  - 📍 `Models/Room.cs` → `RoomType` enum (lines 30-36)
  - 🔧 Chức năng: Phân loại phòng với giá khác nhau
  
- ✅ **4 trạng thái**: Available, Occupied, Maintenance, Reserved
  - 📍 `Models/Room.cs` → `RoomStatus` enum (lines 38-44)
  - 🔧 Chức năng: Quản lý trạng thái phòng realtime

### 📅 Đặt Phòng
**📁 Code Location**: `Controllers/BookingController.cs`, `Services/BookingService.cs`

- ✅ **Tạo booking mới**
  - 📍 `BookingController.cs` → `CreateBooking()` (lines 45-85)
  - 🔧 Chức năng: Validate dates, check room available, create booking
  
- ✅ **Check-in / Check-out**
  - 📍 `BookingController.cs` → `CheckIn()`, `CheckOut()` (lines 120-180)
  - 🔧 Chức năng: Update booking status, update room status
  
- ✅ **Áp dụng decorator** (dịch vụ bổ sung)
  - 📍 `Services/Pricing/Decorators/` → 4 decorator classes
  - 🔧 Chức năng: Wrap booking với additional services (xem Decorator Pattern)

### 🎯 Decorator Pattern (4 Decorators)
**📁 Code Location**: `Services/Pricing/Decorators/`

- ✅ **BaseBookingDecorator** - Abstract base class
  - 📍 `Decorators/BaseBookingDecorator.cs` (lines 5-25)
  - 🔧 Chức năng: Wrap IBooking interface, forward CalculateTotalPrice()
  
- ✅ **BreakfastDecorator**: +100,000 VND
  - 📍 `Decorators/BreakfastDecorator.cs` (lines 5-20)
  - 🔧 Chức năng: `booking.CalculateTotalPrice() + 100000`
  
- ✅ **SpaServiceDecorator**: +500,000 VND
  - 📍 `Decorators/SpaServiceDecorator.cs` (lines 5-20)
  - 🔧 Chức năng: `booking.CalculateTotalPrice() + 500000`
  
- ✅ **AirportTransferDecorator**: +300,000 VND
  - 📍 `Decorators/AirportTransferDecorator.cs` (lines 5-20)
  - 🔧 Chức năng: `booking.CalculateTotalPrice() + 300000`
  
- ✅ **LaundryServiceDecorator**: +50,000 VND
  - 📍 `Decorators/LaundryServiceDecorator.cs` (lines 5-20)
  - 🔧 Chức năng: `booking.CalculateTotalPrice() + 50000`

**💡 Cách sử dụng Decorator Pattern**:
```csharp
// Ví dụ: Booking với breakfast + spa
IBooking booking = new Booking(...);
booking = new BreakfastDecorator(booking);    // +100k
booking = new SpaServiceDecorator(booking);    // +500k
decimal total = booking.CalculateTotalPrice(); // Original + 600k
```

### 💰 Strategy Pattern (5 Strategies)
**📁 Code Location**: `Services/Pricing/Strategies/`

- ✅ **IDiscountStrategy** - Interface
  - 📍 `Strategies/IDiscountStrategy.cs` (lines 3-8)
  - 🔧 Chức năng: Define contract `ApplyDiscount(decimal originalPrice)`
  
- ✅ **NoDiscountStrategy**: 0%
  - 📍 `Strategies/NoDiscountStrategy.cs` (lines 5-12)
  - 🔧 Chức năng: Return original price unchanged
  
- ✅ **VIPDiscountStrategy**: 10%
  - 📍 `Strategies/VIPDiscountStrategy.cs` (lines 5-12)
  - 🔧 Chức năng: `originalPrice * 0.90`
  
- ✅ **SeasonalDiscountStrategy**: 15%
  - 📍 `Strategies/SeasonalDiscountStrategy.cs` (lines 5-12)
  - 🔧 Chức năng: `originalPrice * 0.85`
  
- ✅ **LoyaltyDiscountStrategy**: 5-20%
  - 📍 `Strategies/LoyaltyDiscountStrategy.cs` (lines 5-25)
  - 🔧 Chức năng: Discount based on customer loyalty points
  
- ✅ **EarlyBirdDiscountStrategy**: 10-20%
  - 📍 `Strategies/EarlyBirdDiscountStrategy.cs` (lines 5-25)
  - 🔧 Chức năng: Higher discount for earlier bookings

**💡 Cách sử dụng Strategy Pattern**:
```csharp
// Ví dụ: Áp dụng VIP discount
IDiscountStrategy strategy = new VIPDiscountStrategy();
decimal finalPrice = strategy.ApplyDiscount(1000000); // 900,000 VND
```
- ✅ **Early Bird**: 10-20%

### 🔔 Thông Báo
- ✅ 7 loại: Success, Error, Warning, Info, Booking, Payment, System
- ✅ Toast notifications đẹp mắt
- ✅ Realtime updates

### ⚙️ Cài Đặt
- ✅ Quản lý hồ sơ admin
- ✅ Đổi mật khẩu
- ✅ Bật 2FA (demo)
- ✅ Xem lịch sử đăng nhập
- ✅ Xuất dữ liệu JSON
- ✅ Sao lưu hệ thống

### 🏠 Trang Chủ
- ✅ Giới thiệu khách sạn đẹp mắt
- ✅ Thông tin liên hệ
- ✅ Thống kê thành tựu
- ✅ Tính năng nổi bật

---

## 🛠️ Công Nghệ

### Backend
- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12
- **Architecture**: MVC
- **Storage**: JSON File System
- **Authentication**: PBKDF2 + SHA256
- **Session**: ASP.NET Core Session (24h)

### Frontend
- **View Engine**: Razor Pages
- **JavaScript**: Vanilla JS (ES6+)
- **CSS**: CSS3 with Grid & Flexbox
- **Icons**: Font Awesome 6.4.0
- **Animations**: CSS Transitions & Keyframes

### Design Patterns
- ✅ **MVC**: Model-View-Controller
- ✅ **Singleton**: Service lifetime management
- ✅ **Repository**: Data access abstraction
- ✅ **Decorator**: Dynamic service additions
- ✅ **Strategy**: Flexible discount algorithms
- ✅ **Dependency Injection**: Built-in DI container

### Dependencies
```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

---

## 🚀 Cài Đặt

### Yêu Cầu Hệ Thống
- ✅ .NET 8.0 SDK
- ✅ Visual Studio 2022 hoặc VS Code
- ✅ Windows 10/11, Linux, hoặc macOS
- ✅ 2GB RAM (tối thiểu)
- ✅ 500MB dung lượng ổ cứng

### Cài Đặt Nhanh

**Bước 1**: Clone hoặc tải dự án
```bash
cd D:\QLKS
```

**Bước 2**: Restore dependencies
```bash
dotnet restore
```

**Bước 3**: Build project
```bash
dotnet build
```

**Bước 4**: Chạy ứng dụng
```bash
dotnet run
```

**Bước 5**: Truy cập
```
URL: http://localhost:5000
Username: admin
Password: admin123
```

### Tài Khoản Mặc Định

| Username | Password   | Role    | Email                  |
|----------|------------|---------|------------------------|
| admin    | admin123   | Admin   | admin@hotelwangg.com   |
| manager  | manager123 | Manager | manager@hotelwangg.com |

---

## 📖 Hướng Dẫn Sử Dụng

### 1️⃣ Đăng Nhập
1. Truy cập http://localhost:5000
2. Nhập `admin` / `admin123`
3. Click "Đăng nhập"

### 2️⃣ Quản Lý Hồ Sơ
1. Click nút **👤 Hồ sơ** ở sidebar
2. Sửa thông tin (Họ tên, Email, SĐT)
3. Click "📷 Đổi ảnh" để upload avatar
4. Click "💾 Lưu thay đổi"
5. **Thông tin và ảnh sẽ hiển thị ngay trên sidebar**

### 3️⃣ Đổi Mật Khẩu
1. Click nút **🛠️ Cài đặt**
2. Click "🔑 Đổi mật khẩu"
3. Nhập mật khẩu hiện tại
4. Nhập mật khẩu mới (min 6 ký tự)
5. Xác nhận mật khẩu
6. Click "🔑 Đổi mật khẩu"

### 4️⃣ Xem Lịch Sử Đăng Nhập
1. Click **🛠️ Cài đặt**
2. Click "📋 Xem lịch sử đăng nhập"
3. Xem bảng với thông tin: Thời gian, IP, Trình duyệt, Thiết bị

### 5️⃣ Quản Lý Phòng
1. Vào **Quản lý phòng**
2. Click "➕ Thêm phòng"
3. Điền thông tin
4. Click "Lưu"

### 6️⃣ Đặt Phòng
1. Vào **Đặt phòng**
2. Click "➕ Tạo booking"
3. Chọn khách hàng, phòng, ngày
4. Chọn dịch vụ bổ sung (Decorator)
5. Áp dụng chiết khấu (Strategy)
6. Click "Tạo booking"

### 7️⃣ Về Trang Chủ
- **Click vào logo "Hotel's Wangg"** ở sidebar để xem trang giới thiệu

### 8️⃣ Đăng Xuất
1. Click **🚪 Đăng xuất**
2. Xác nhận
3. Tự động về trang Login

---

## 📡 API Documentation

### 🔐 Authentication APIs
**📁 Controller**: `Controllers/AuthController.cs`

```http
POST   /api/auth/login           # Đăng nhập
```
- 📍 Code: `AuthController.cs` → `Login()` action (lines 26-74)
- 📥 Request: `{ username, password }`
- 📤 Response: `{ success, message, user: { id, username, fullName, role } }`
- 🔧 Chức năng: Hash password với PBKDF2, so sánh với DB, tạo session, lưu login history

```http
POST   /api/auth/register        # Đăng ký
```
- 📍 Code: `AuthController.cs` → `Register()` action (lines 76-125)
- 📥 Request: `{ username, password, fullName, email, phone, role }`
- 📤 Response: `{ success, message }`
- 🔧 Chức năng: Validate input, hash password, tạo user mới trong users.json

```http
POST   /api/auth/logout          # Đăng xuất
```
- 📍 Code: `AuthController.cs` → `Logout()` action (lines 127-145)
- 📤 Response: `{ success, message }`
- 🔧 Chức năng: Clear session, redirect về login

```http
GET    /api/auth/me              # Thông tin user hiện tại
```
- 📍 Code: `AuthController.cs` → `GetCurrentUser()` action (lines 175-208)
- 📤 Response: `{ success, user: { id, username, fullName, email, phone, role, avatarUrl } }`
- 🔧 Chức năng: Lấy userId từ session, query user từ DB

```http
PUT    /api/auth/profile         # Cập nhật hồ sơ
```
- 📍 Code: `AuthController.cs` → `UpdateProfile()` action (lines 211-244)
- 📥 Request: `{ fullName, email, phone }`
- 📤 Response: `{ success, message }`
- 🔧 Chức năng: Validate email, cập nhật user trong users.json

```http
POST   /api/auth/change-password # Đổi mật khẩu
```
- 📍 Code: `AuthController.cs` → `ChangePassword()` action (lines 246-282)
- 📥 Request: `{ currentPassword, newPassword }`
- 📤 Response: `{ success, message }`
- 🔧 Chức năng: Verify current password, hash new password, update DB

```http
GET    /api/auth/login-history   # Lịch sử đăng nhập
```
- 📍 Code: `AuthController.cs` → `GetLoginHistory()` action (lines 284-308)
- 📤 Response: `{ success, history: [{ timestamp, ipAddress, userAgent, deviceInfo }] }`
- 🔧 Chức năng: Lấy 20 login gần nhất của user từ session

---

### 🛏️ Room APIs
**📁 Controller**: `Controllers/RoomController.cs`

```http
GET    /api/room                 # Danh sách phòng
```
- 📍 Code: `RoomController.cs` → `GetRooms()` (lines 18-35)
- 📤 Response: `{ success, rooms: [{ id, number, type, status, price }] }`
- 🔧 Chức năng: Load tất cả rooms từ rooms.json

```http
POST   /api/room                 # Tạo phòng mới
```
- 📍 Code: `RoomController.cs` → `CreateRoom()` (lines 37-70)
- 📥 Request: `{ number, type, status, price, floor, description }`
- 📤 Response: `{ success, message, room }`
- 🔧 Chức năng: Validate unique number, tạo room ID, save to rooms.json

```http
PUT    /api/room/{id}            # Cập nhật phòng
```
- 📍 Code: `RoomController.cs` → `UpdateRoom()` (lines 72-110)
- 📥 Request: `{ number, type, status, price, floor, description }`
- 📤 Response: `{ success, message }`
- 🔧 Chức năng: Find room by ID, update properties, save

```http
DELETE /api/room/{id}            # Xóa phòng
```
- 📍 Code: `RoomController.cs` → `DeleteRoom()` (lines 112-140)
- 📤 Response: `{ success, message }`
- 🔧 Chức năng: Remove room from rooms.json

---

### 📅 Booking APIs
**📁 Controller**: `Controllers/BookingController.cs`

```http
GET    /api/booking              # Danh sách booking
```
- 📍 Code: `BookingController.cs` → `GetBookings()` (lines 20-38)
- 📤 Response: `{ success, bookings: [...] }`
- 🔧 Chức năng: Load tất cả bookings từ bookings.json

```http
POST   /api/booking              # Tạo booking mới
```
- 📍 Code: `BookingController.cs` → `CreateBooking()` (lines 40-85)
- 📥 Request: `{ customerId, roomId, checkIn, checkOut, services: [], discountType }`
- 📤 Response: `{ success, message, booking }`
- 🔧 Chức năng: 
  1. Validate dates (checkIn < checkOut)
  2. Check room availability
  3. Calculate base price
  4. Apply decorators (services)
  5. Apply discount strategy
  6. Save booking
  7. Update room status

```http
POST   /api/booking/{id}/checkin # Check-in
```
- 📍 Code: `BookingController.cs` → `CheckIn()` (lines 120-150)
- 📤 Response: `{ success, message }`
- 🔧 Chức năng: Update booking status → CheckedIn, room status → Occupied

```http
POST   /api/booking/{id}/checkout # Check-out
```
- 📍 Code: `BookingController.cs` → `CheckOut()` (lines 152-180)
- 📤 Response: `{ success, message }`
- 🔧 Chức năng: Update booking status → CheckedOut, room status → Available

---

### 💰 Pricing APIs
**📁 Controller**: `Controllers/PricingController.cs`

```http
POST   /api/pricing/calculate    # Tính giá với decorators
```
- 📍 Code: `PricingController.cs` → `CalculatePrice()` (lines 15-60)
- 📥 Request: `{ basePrice, services: ["breakfast", "spa"] }`
- 📤 Response: `{ success, basePrice, services: [...], totalPrice }`
- 🔧 Chức năng: Wrap booking với decorators, return itemized breakdown

```http
POST   /api/pricing/discount     # Áp dụng chiết khấu
```
- 📍 Code: `PricingController.cs` → `ApplyDiscount()` (lines 62-95)
- 📥 Request: `{ originalPrice, discountType: "vip" }`
- 📤 Response: `{ success, originalPrice, discountPercent, finalPrice }`
- 🔧 Chức năng: Select strategy by type, calculate discount

```http
GET    /api/pricing/strategies   # Danh sách strategies
```
- 📍 Code: `PricingController.cs` → `GetStrategies()` (lines 97-120)
- 📤 Response: `{ success, strategies: [{ name, description, discount }] }`
- 🔧 Chức năng: Return metadata về các discount strategies

```http
GET    /api/pricing/services     # Danh sách decorators
```
- 📍 Code: `PricingController.cs` → `GetServices()` (lines 122-145)
- 📤 Response: `{ success, services: [{ name, description, price }] }`
- 🔧 Chức năng: Return metadata về các additional services

**Xem chi tiết**: [CHI_TIET.md](CHI_TIET.md)

---

## 🎨 Design Patterns

Hệ thống áp dụng **6 Design Patterns** tiên tiến:

### 1. MVC Pattern
Tách biệt Model-View-Controller để dễ bảo trì và mở rộng.

### 2. Singleton Pattern
Đảm bảo chỉ có 1 instance của Services và DataStore trong toàn ứng dụng.

### 3. Repository Pattern
Tách biệt logic truy xuất dữ liệu khỏi business logic.

```csharp
public interface IDataStore
{
    T? Load<T>(string key);
    void Save<T>(string key, T data);
}

public class JsonDataStore : IDataStore
{
    public T? Load<T>(string key) { /* Implementation */ }
    public void Save<T>(string key, T data) { /* Implementation */ }
}
```

### 4. Dependency Injection
Quản lý dependencies thông qua DI container của ASP.NET Core.

```csharp
builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
```

### 5. Decorator Pattern 🎯

**Mục đích:** Thêm dịch vụ bổ sung cho phòng một cách linh hoạt

**Ví dụ:**
```csharp
IServiceDecorator service = new BasicServiceDecorator(1000000);
service = new BreakfastDecorator(service);      // +100k
service = new SpaServiceDecorator(service);     // +500k
service = new AirportTransferDecorator(service); // +300k

decimal total = service.CalculatePrice(); // 1,900,000 VND
```

**Các Decorator có sẵn:**
- `BreakfastDecorator` - Bữa sáng (+100,000 VND)
- `SpaServiceDecorator` - Spa & Massage (+500,000 VND)
- `AirportTransferDecorator` - Đưa đón sân bay (+300,000 VND)
- `LaundryServiceDecorator` - Giặt ủi (+50,000 VND)

### 6. Strategy Pattern 💰

**Mục đích:** Áp dụng các chiến lược giảm giá khác nhau

**Ví dụ:**
```csharp
// VIP discount
IDiscountStrategy strategy = new VIPDiscountStrategy();
decimal finalPrice = strategy.ApplyDiscount(1000000); // 900,000

// Seasonal discount
strategy = new SeasonalDiscountStrategy();
finalPrice = strategy.ApplyDiscount(1000000); // 850,000

// Early bird discount
strategy = new EarlyBirdDiscountStrategy(30); // 30 days before
finalPrice = strategy.ApplyDiscount(1000000); // 800,000
```

**Các Strategy có sẵn:**
- `NoDiscountStrategy` - Không giảm giá (0%)
- `VIPDiscountStrategy` - Khách VIP (10%)
- `SeasonalDiscountStrategy` - Theo mùa (15%)
- `LoyaltyDiscountStrategy` - Khách thân thiết (5-20%)
- `EarlyBirdDiscountStrategy` - Đặt sớm (10-20%)

---

## 📸 Screenshots

### 🏠 Trang Chủ (About)
- Hero section với gradient navy blue
- Giới thiệu Hotel's Wangg
- 6 tính năng nổi bật
- Thống kê thành tựu
- Thông tin liên hệ

### 🔐 Đăng Nhập
- Gradient background
- Form đẹp với shadow
- Auto-fill demo (admin/admin123)
- Remember me checkbox

### 📊 Dashboard
- Cards thống kê realtime
- Biểu đồ doanh thu
- Danh sách booking gần đây
- Thông báo quan trọng

### 🛏️ Quản Lý Phòng
- Grid layout responsive
- Card cho mỗi phòng
- Badge trạng thái
- Actions: View, Edit, Delete

### 👤 Admin Profile
- Modal popup đẹp
- Upload avatar với preview
- Form validation realtime
- Update thông tin ngay lập tức

### ⚙️ Admin Settings
- 4 sections: Security, Notifications, History, Data
- Đổi mật khẩu an toàn
- Xem lịch sử đăng nhập
- Xuất dữ liệu & backup

---

## 🔒 Bảo Mật

### Mã Hóa Mật Khẩu
- **Thuật toán**: PBKDF2 + SHA256
- **Salt size**: 16 bytes (random)
- **Hash size**: 32 bytes
- **Iterations**: 100,000
- **No plain text**: Không lưu password dạng text

### Session Management
- **Timeout**: 24 giờ
- **HttpOnly**: true
- **IsEssential**: true
- **Secure cookies**: Production

### Validation
- **Email**: RFC 2822 compliant
- **Phone**: 10-11 digits
- **Username**: 4-20 chars, alphanumeric + underscore
- **Password**: Min 6 chars (recommend 8+)

---

## ❓ FAQ

### Q: Làm sao để thay đổi port?
**A**: Sửa `applicationUrl` trong `Properties/launchSettings.json`

### Q: Dữ liệu được lưu ở đâu?
**A**: Trong `bin/Debug/net8.0/Data/*.json`

### Q: Làm sao để reset mật khẩu admin?
**A**: Xóa file `users.json`, hệ thống tạo lại admin mặc định

### Q: Avatar được lưu như thế nào?
**A**: Base64 trong localStorage (client-side). Production nên upload lên server/cloud

### Q: Hệ thống có hỗ trợ SQL Server không?
**A**: Hiện tại dùng JSON. Có thể thêm Entity Framework Core để dùng SQL Server

### Q: Làm sao để deploy production?
**A**: `dotnet publish -c Release`, output trong `bin/Release/net8.0/publish/`

**Xem thêm**: [CHI_TIET.md](CHI_TIET.md)

---

## 📚 Cấu Trúc Dự Án

```
QLKS/
├── Controllers/          # API & MVC Controllers (11 files)
├── Models/              # Data models (8 files)
├── Services/            # Business logic (10 services)
│   ├── Decorators/      # 7 decorator files
│   └── Strategies/      # 5 strategy files
├── Utils/               # Utilities (4 files)
├── Views/               # Razor views (10 views)
│   ├── Home/
│   └── Shared/
├── wwwroot/             # Static files
│   ├── css/style.css    # 2000+ lines
│   └── js/admin.js      # 500+ lines
├── Program.cs           # Entry point
├── README.md            # File này
└── CHI_TIET.md         # Tài liệu chi tiết đầy đủ
```

**Total**: 60+ files, 10,000+ lines of code

---

## 📝 Changelog

### Version 2.3 (10/10/2025) - Current ✨
✅ Hoàn thiện hệ thống xác thực đầy đủ  
✅ Thêm đăng nhập/đăng ký/đăng xuất  
✅ Quản lý hồ sơ với upload avatar  
✅ Avatar hiển thị ngay trên sidebar  
✅ Đổi mật khẩu an toàn  
✅ Lịch sử đăng nhập chi tiết  
✅ Trang About đẹp mắt với Hero section  
✅ Click logo → Về trang chủ  
✅ Tích hợp API thực hoàn chỉnh  
✅ Fix tất cả lỗi admin.js  
✅ Giảm file .md xuống 2 file (README + CHI_TIET)  

### Version 2.2 (09/10/2025)
✅ Đổi tên Hotel NQ → Hotel's Wangg  
✅ Admin panel với 3 buttons (Profile, Settings, Logout)  
✅ Modal system hoàn chỉnh  
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

## 🎓 Tài Liệu

### Trong Dự Án
- **README.md** (file này): Tổng quan và hướng dẫn nhanh
- **[CHI_TIET.md](CHI_TIET.md)**: Tài liệu chi tiết đầy đủ (500+ dòng)

### Tài Liệu Bên Ngoài
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [C# Documentation](https://docs.microsoft.com/dotnet/csharp/)
- [Design Patterns](https://refactoring.guru/design-patterns)

---

## 🤝 Đóng Góp

Mọi đóng góp đều được chào đón! Các bước:

1. Fork repository
2. Tạo branch mới (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

---

## 📞 Liên Hệ

- **Email**: admin@hotelwangg.com
- **Hotline**: 1900 1234
- **Website**: http://localhost:5000/Home/About
- **Documentation**: [CHI_TIET.md](CHI_TIET.md)

---

## 📄 License

MIT License - Tự do sử dụng cho mục đích cá nhân và thương mại.

---

## 🌟 Đánh Giá

Nếu bạn thấy dự án hữu ích, hãy cho 1 ⭐!

---

<div align="center">

**Hotel's Wangg Management System v2.3**  
*Hệ thống quản lý khách sạn hiện đại, an toàn và dễ sử dụng*

Made with ❤️ using ASP.NET Core 8.0

[⬆ Về đầu trang](#hotels-wangg---hệ-thống-quản-lý-khách-sạn)

</div>
