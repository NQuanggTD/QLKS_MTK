# 🏨 Wangg's Hotel - Hệ Thống Quản Lý Khách Sạn

<div align="center">

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=flat&logo=.net)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=c-sharp)](https://docs.microsoft.com/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-MIT-green.svg?style=flat)](LICENSE)

**Hệ thống quản lý khách sạn hiện đại - Navy Blue & Gold Theme**

[Chạy Nhanh](#-chạy-nhanh) • [Tính Năng](#-tính-năng) • [Design Patterns](#-design-patterns) • [Tài Liệu Chi Tiết](CHI_TIET.md)

</div>

---

## 🎯 Giới Thiệu

**Wangg's Hotel Management System** - Hệ thống quản lý khách sạn toàn diện với ASP.NET Core 8.0, áp dụng Design Patterns tiên tiến và bảo mật cao.

### ✨ Điểm Nổi Bật

- 🎨 **Giao diện đẹp**: Theme Navy Blue & Gold với CSS gradient, animations
- 🔐 **Bảo mật cao**: PBKDF2 + SHA256 (100k iterations), salt ngẫu nhiên
- 📱 **Responsive**: Tương thích mọi thiết bị
- ⚡ **Realtime**: Cập nhật dữ liệu tức thì
- 🎯 **Design Patterns**: MVC, Decorator, Strategy, Singleton, Repository, DI
- 💾 **JSON Storage**: Không cần SQL Server
- 🌐 **RESTful API**: 30+ endpoints
- 🔔 **Notifications**: Toast messages với 7 loại

---

## ⚡ Chạy Nhanh

### 🪟 Windows (Khuyên dùng)

**Cách đơn giản nhất - Double-click:**
1. Tìm file **`run.bat`** trong thư mục dự án
2. **Double-click** vào file
3. Đợi 5 giây - Trình duyệt tự động mở
4. Đăng nhập: `admin` / `admin123`

**Hoặc chạy từ PowerShell/CMD:**
```powershell
cd D:\TieuLuan_MTK\QLKS
.\run.bat
```

### 🐧 Linux/Mac

```bash
cd /path/to/QLKS
chmod +x run.sh
./run.sh
```

### 📝 Thông tin đăng nhập

```
URL:      http://localhost:5000
Username: admin
Password: admin123
Role:     Admin
```

### 🛑 Dừng ứng dụng

Nhấn **Ctrl + C** trong terminal để dừng.

---

## 🚀 Cài Đặt Thủ Công

### Yêu cầu
- .NET 8.0 SDK
- 2GB RAM
- 500MB dung lượng

### Các bước

```bash
# Clone/tải dự án
cd D:\TieuLuan_MTK\QLKS

# Restore dependencies
dotnet restore

# Build
dotnet build

# Chạy
dotnet run

# Truy cập: http://localhost:5000
```

---

## 🎨 Tính Năng

### 🔐 Xác Thực & Bảo Mật
- ✅ Đăng nhập/Đăng ký với mã hóa PBKDF2
- ✅ Quản lý hồ sơ cá nhân (fullName, email, phone)
- ✅ Upload avatar (base64)
- ✅ Đổi mật khẩu an toàn
- ✅ Lịch sử đăng nhập (IP, device, browser)
- ✅ Session 24h với sliding expiration
- ✅ Phân quyền: Admin, Manager, Staff

### 📊 Dashboard
- ✅ Tổng quan doanh thu realtime
- ✅ Thống kê phòng (Available, Occupied, Maintenance, Reserved)
- ✅ Biểu đồ doanh thu theo tháng
- ✅ Danh sách booking gần đây
- ✅ Thông báo quan trọng

### 🛏️ Quản Lý Phòng
- ✅ CRUD phòng đầy đủ
- ✅ 4 loại phòng: Standard, Deluxe, Suite, Presidential
- ✅ 4 trạng thái: Available, Occupied, Maintenance, Reserved
- ✅ Tìm kiếm & lọc phòng
- ✅ Hiển thị grid responsive với cards

### 📅 Đặt Phòng (Bookings)
- ✅ Tạo booking với validation dates
- ✅ Check availability tự động
- ✅ Thêm dịch vụ bổ sung (Decorator Pattern)
- ✅ Áp dụng chiết khấu (Strategy Pattern)
- ✅ Check-in / Check-out
- ✅ Hủy booking
- ✅ Tính giá tự động

### 👥 Quản Lý Khách Hàng
- ✅ CRUD khách hàng
- ✅ Lưu thông tin: name, email, phone, address, ID card
- ✅ Lịch sử booking của khách
- ✅ Tìm kiếm khách hàng

### 🎁 Quản Lý Dịch Vụ
- ✅ CRUD dịch vụ khách sạn
- ✅ Pricing cho từng dịch vụ
- ✅ Áp dụng Decorator Pattern
- ✅ Tính giá tự động khi thêm dịch vụ

### 💰 Thanh Toán
- ✅ Xem danh sách booking cần thanh toán
- ✅ Lọc theo trạng thái payment
- ✅ Chi tiết hóa đơn (room charge, services, discount)
- ✅ Xác nhận thanh toán
- ✅ 3 phương thức: Cash, Credit Card, Bank Transfer

### 🔔 Thông Báo
- ✅ 7 loại: Success, Error, Warning, Info, Booking, Payment, System
- ✅ Toast notifications đẹp mắt
- ✅ Auto-dismiss sau 5s
- ✅ Click để đóng

### ⚙️ Cài Đặt Hệ Thống
- ✅ Quản lý hồ sơ admin
- ✅ Đổi mật khẩu
- ✅ Xem lịch sử đăng nhập
- ✅ Xuất dữ liệu JSON
- ✅ Sao lưu hệ thống

---

## 🛠️ Công Nghệ

### Backend
- **Framework**: ASP.NET Core 8.0 MVC
- **Language**: C# 12
- **Storage**: JSON File System
- **Authentication**: PBKDF2 + SHA256
- **Session**: 24h sliding expiration

### Frontend
- **View Engine**: Razor Pages
- **JavaScript**: Vanilla JS (ES6+)
- **CSS**: CSS3 Grid & Flexbox
- **Icons**: Font Awesome 6.4.0
- **Animations**: CSS Transitions & Keyframes

### Design Patterns
1. **MVC**: Model-View-Controller architecture
2. **Singleton**: Service lifetime management
3. **Repository**: Data access abstraction (IDataStore)
4. **Decorator**: Dynamic service additions (4 decorators)
5. **Strategy**: Flexible discount algorithms (5 strategies)
6. **Dependency Injection**: Built-in DI container

---

## 🎨 Design Patterns Chi Tiết

### 1. MVC Pattern
Tách biệt Model-View-Controller để dễ bảo trì và mở rộng.

```
Request → Controller → Service → DataStore → Response
                ↓
              View (Razor)
```

### 2. Repository Pattern
Tách biệt logic truy xuất dữ liệu.

```csharp
public interface IDataStore {
    T? Load<T>(string key);
    void Save<T>(string key, T data);
}
```

### 3. Decorator Pattern 🎯
Thêm dịch vụ bổ sung cho phòng linh hoạt.

**Ví dụ:**
```csharp
IServiceDecorator service = new BasicServiceDecorator(1000000);
service = new BreakfastDecorator(service);      // +100k
service = new SpaServiceDecorator(service);     // +500k
service = new AirportTransferDecorator(service); // +300k

decimal total = service.CalculatePrice(); // 1,900,000 VND
```

**Các Decorator:**
- `BreakfastDecorator` - Bữa sáng (+100,000 VND)
- `SpaServiceDecorator` - Spa & Massage (+500,000 VND)
- `AirportTransferDecorator` - Đưa đón sân bay (+300,000 VND)
- `LaundryServiceDecorator` - Giặt ủi (+50,000 VND)

### 4. Strategy Pattern 💰
Áp dụng các chiến lược giảm giá khác nhau.

**Ví dụ:**
```csharp
IDiscountStrategy strategy = new VIPDiscountStrategy();
decimal finalPrice = strategy.ApplyDiscount(1000000); // 900,000 VND
```

**Các Strategy:**
- `NoDiscountStrategy` - Không giảm giá (0%)
- `VIPDiscountStrategy` - Khách VIP (10%)
- `SeasonalDiscountStrategy` - Theo mùa (15%)
- `LoyaltyDiscountStrategy` - Khách thân thiết (5-20%)
- `EarlyBirdDiscountStrategy` - Đặt sớm (10-20%)

### 5. Dependency Injection
Quản lý dependencies thông qua DI container.

```csharp
builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
```

### 6. Singleton Pattern
Đảm bảo 1 instance duy nhất cho Services và DataStore.

---

## 📖 Hướng Dẫn Sử Dụng

### 1. Đăng Nhập
1. Truy cập http://localhost:5000
2. Nhập `admin` / `admin123`
3. Click "Đăng nhập"

### 2. Quản Lý Hồ Sơ
1. Click **👤 Hồ sơ** ở sidebar
2. Sửa thông tin
3. Click "📷 Đổi ảnh" để upload avatar
4. Click "💾 Lưu thay đổi"

### 3. Đổi Mật Khẩu
1. Click **🛠️ Cài đặt**
2. Nhập mật khẩu hiện tại và mật khẩu mới
3. Click "🔑 Đổi mật khẩu"

### 4. Quản Lý Phòng
1. Vào **Quản lý phòng**
2. Click "➕ Thêm phòng"
3. Điền thông tin và lưu

### 5. Đặt Phòng
1. Vào **Đặt phòng**
2. Click "➕ Tạo booking"
3. Chọn khách, phòng, ngày
4. Chọn dịch vụ (Decorator)
5. Áp dụng chiết khấu (Strategy)
6. Tạo booking

### 6. Thanh Toán
1. Vào **Thanh toán**
2. Lọc booking theo trạng thái
3. Click "👁️ Xem" để xem chi tiết
4. Click "💳 Xác nhận thanh toán"

### 7. Đăng Xuất
Click **🚪 Đăng xuất** ở sidebar

---

## 📡 API Endpoints

### Authentication
```
POST   /api/auth/login            # Đăng nhập
POST   /api/auth/register         # Đăng ký
POST   /api/auth/logout           # Đăng xuất
GET    /api/auth/me               # Thông tin user
PUT    /api/auth/profile          # Cập nhật hồ sơ
POST   /api/auth/change-password  # Đổi mật khẩu
GET    /api/auth/login-history    # Lịch sử login
```

### Rooms
```
GET    /api/rooms                 # Danh sách phòng
POST   /api/rooms                 # Tạo phòng
GET    /api/rooms/{id}            # Chi tiết phòng
PUT    /api/rooms/{id}            # Cập nhật phòng
DELETE /api/rooms/{id}            # Xóa phòng
```

### Bookings
```
GET    /api/bookings              # Danh sách booking
POST   /api/bookings              # Tạo booking
PUT    /api/bookings/{id}         # Cập nhật booking
DELETE /api/bookings/{id}         # Xóa booking
POST   /api/bookings/{id}/checkin # Check-in
POST   /api/bookings/{id}/checkout# Check-out
```

### Pricing
```
POST   /api/pricing/calculate     # Tính giá với decorators
POST   /api/pricing/discount      # Áp dụng discount strategy
GET    /api/pricing/strategies    # Danh sách strategies
GET    /api/pricing/services      # Danh sách decorators
```

### Customers
```
GET    /api/customers             # Danh sách khách
POST   /api/customers             # Tạo khách mới
PUT    /api/customers/{id}        # Cập nhật khách
DELETE /api/customers/{id}        # Xóa khách
```

### Services
```
GET    /api/services              # Danh sách dịch vụ
POST   /api/services              # Tạo dịch vụ
PUT    /api/services/{id}         # Cập nhật dịch vụ
DELETE /api/services/{id}         # Xóa dịch vụ
```

### Payment
```
GET    /api/payment/summary/{bookingId}  # Chi tiết thanh toán
```

**Chi tiết API**: Xem [CHI_TIET.md](CHI_TIET.md)

---

## 🔒 Bảo Mật

### Mã Hóa Mật Khẩu
- **Thuật toán**: PBKDF2 + SHA256
- **Salt**: 16 bytes random
- **Hash**: 32 bytes
- **Iterations**: 100,000
- **No plain text**: Không lưu password dạng text

### Session Management
- **Timeout**: 24 giờ
- **HttpOnly**: true
- **Sliding expiration**: true

### Validation
- **Email**: RFC 2822 compliant
- **Phone**: 10-11 digits
- **Password**: Min 6 chars

---

## 📚 Cấu Trúc Dự Án

```
QLKS/
├── Controllers/          # API & MVC Controllers
│   ├── AuthController.cs
│   ├── RoomsController.cs
│   ├── BookingsController.cs
│   ├── CustomersController.cs
│   ├── ServicesController.cs
│   ├── PaymentController.cs
│   ├── PricingController.cs
│   └── ...
├── Models/              # Data Models
│   ├── User.cs
│   ├── Room.cs
│   ├── Booking.cs
│   ├── Customer.cs
│   └── ...
├── Services/            # Business Logic
│   ├── AuthService.cs
│   ├── RoomService.cs
│   ├── BookingService.cs
│   ├── Decorators/      # 4 decorator files
│   │   ├── BasicServiceDecorator.cs
│   │   ├── BreakfastDecorator.cs
│   │   ├── SpaServiceDecorator.cs
│   │   └── ...
│   └── Strategies/      # 5 strategy files
│       ├── VIPDiscountStrategy.cs
│       ├── SeasonalDiscountStrategy.cs
│       └── ...
├── Utils/               # Utilities
│   ├── JsonDataStore.cs
│   ├── Logger.cs
│   ├── PasswordHasher.cs
│   └── Validator.cs
├── Views/               # Razor Views
│   ├── Home/
│   │   ├── Index.cshtml
│   │   ├── Dashboard.cshtml
│   │   ├── Rooms.cshtml
│   │   ├── Bookings.cshtml
│   │   └── ...
│   └── Shared/
│       └── _Layout.cshtml
├── wwwroot/             # Static Files
│   ├── css/
│   │   └── style.css    # 2000+ lines
│   └── js/
│       ├── app.js
│       ├── admin.js
│       ├── rooms.js
│       └── ...
├── Data/                # JSON Storage
│   ├── users.json
│   ├── rooms.json
│   ├── bookings.json
│   ├── customers.json
│   └── ...
├── Program.cs           # Entry Point
├── run.bat             # Windows auto-run script
├── run.sh              # Linux/Mac auto-run script
├── README.md           # File này
└── CHI_TIET.md         # Tài liệu chi tiết đầy đủ
```

**Total**: 60+ files, 10,000+ lines

---

## ❓ FAQ

**Q: Làm sao để thay đổi port?**  
A: Sửa `applicationUrl` trong `Properties/launchSettings.json`

**Q: Dữ liệu được lưu ở đâu?**  
A: Trong `bin/Debug/net8.0/Data/*.json`

**Q: Reset mật khẩu admin?**  
A: Xóa `users.json`, hệ thống tạo lại admin mặc định

**Q: Avatar lưu như thế nào?**  
A: Base64 trong localStorage (client-side)

**Q: Hỗ trợ SQL Server?**  
A: Hiện tại dùng JSON. Có thể thêm Entity Framework Core

**Q: Deploy production?**  
A: `dotnet publish -c Release`

---

## 📞 Liên Hệ

- **Email**: admin@hotelwangg.com
- **Hotline**: 1900 1234
- **URL**: http://localhost:5000

---

## 📄 License

MIT License - Tự do sử dụng cho mục đích cá nhân và thương mại.

---

<div align="center">

**Wangg's Hotel Management System**  
*Hệ thống quản lý khách sạn hiện đại, an toàn và dễ sử dụng*

Made with ❤️ using ASP.NET Core 8.0

**[⬆ Về đầu trang](#-wanggs-hotel---hệ-thống-quản-lý-khách-sạn)**

</div>
