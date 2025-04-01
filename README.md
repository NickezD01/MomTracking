# MomTracking - Nền tảng Theo dõi Sức khỏe Mẹ và Bé

## Tổng quan

MomTracking là nền tảng theo dõi sức khỏe toàn diện dành cho mẹ và bé, được phát triển trên nền tảng .NET 8.0. Ứng dụng giúp người dùng theo dõi các chỉ số sức khỏe của trẻ, so sánh với tiêu chuẩn WHO, cảnh báo sớm các vấn đề tiềm ẩn, quản lý lịch hẹn y tế, và tham gia cộng đồng chia sẻ. Hệ thống cung cấp các gói dịch vụ khác nhau với nhiều tính năng đa dạng qua mô hình thuê bao.

## Tính năng chính

### Theo dõi và Phân tích Sức khỏe Trẻ em
- **Ghi nhận chỉ số sức khỏe**: Cân nặng, chiều cao, chu vi đầu, nhịp tim, BPD, AC, FL và các thông số khác
- **Phân tích tự động**: So sánh dữ liệu với tiêu chuẩn WHO theo tuần thai
- **Hệ thống cảnh báo thông minh**: Phát hiện các dấu hiệu bất thường như suy dinh dưỡng hoặc béo phì
- **Theo dõi tiến trình**: Biểu đồ trực quan hóa sự phát triển theo thời gian

### Hệ thống Quản lý Lịch hẹn
- **Lịch hẹn y tế**: Tạo và quản lý các cuộc hẹn khám thai, tiêm chủng, khám định kỳ
- **Nhắc nhở tự động**: Hệ thống gửi thông báo trước các cuộc hẹn qua email
- **Tùy chỉnh lịch trình**: Phù hợp với nhu cầu cá nhân và lịch khám của bác sĩ

### Cộng đồng và Chia sẻ Kinh nghiệm
- **Diễn đàn thảo luận**: Đăng bài, bình luận, chia sẻ kinh nghiệm chăm sóc con
- **Hỗ trợ hình ảnh**: Đăng tải hình ảnh kèm theo bài viết
- **Phân trang thông minh**: Dễ dàng tìm kiếm và theo dõi các cuộc thảo luận

### Mô hình Thuê bao Linh hoạt
- **Ba gói thuê bao**:
    - **Đồng**: Các tính năng cơ bản
    - **Bạc**: Tính năng mở rộng
    - **Vàng**: Đầy đủ tính năng cao cấp
- **Thanh toán an toàn**: Tích hợp cổng thanh toán VNPay
- **Quản lý thuê bao**: Theo dõi, nâng cấp hoặc hủy gói dịch vụ

### Quản lý Người dùng và Bảo mật
- **Xác thực hai lớp**: Đăng ký tài khoản với xác minh email
- **Phân quyền**: Người dùng (Customer) và Quản trị viên (Manager)
- **Bảo mật JWT**: Xác thực và phân quyền dựa trên token
- **Quản lý hồ sơ**: Cập nhật thông tin cá nhân và hình ảnh

### Báo cáo và Xuất dữ liệu
- **Xuất báo cáo PDF**: Tạo báo cáo về dữ liệu sức khỏe của trẻ
- **Tùy chỉnh báo cáo**: Lựa chọn thông tin và khoảng thời gian hiển thị
- **Chia sẻ dữ liệu**: Dễ dàng chia sẻ với bác sĩ và chuyên gia y tế

## Kiến trúc và Công nghệ

### Kiến trúc Clean Architecture
MomTracking áp dụng nguyên tắc Clean Architecture với 4 lớp rõ ràng:

#### 1. Domain Layer
- Chứa các entity core: UserAccount, Children, HealthMetric, Schedule, Post, Comment, Subscription...
- Định nghĩa business rules và enumerations (Role, SubscriptionStatus, PaymentStatus...)
- Không phụ thuộc vào bất kỳ layer nào khác

#### 2. Application Layer
- Triển khai business logic thông qua các service: AuthService, HealthMetricService, SubscriptionService...
- Định nghĩa các interface: IUnitOfWork, IHeathMetricService, ISubscriptionService...
- Xử lý DTO (Data Transfer Objects): Requests/Responses
- Tích hợp validation với FluentValidation
- Cấu hình mapping với AutoMapper

#### 3. Infrastructure Layer
- Thực hiện các interface từ Application layer
- Quản lý truy cập dữ liệu với Repository pattern và Unit of Work
- Cấu hình Entity Framework Core và DbContext
- Triển khai các dịch vụ ngoài: EmailService, VnPayService, FirebaseStorageService...

#### 4. API Layer
- Cung cấp RESTful API endpoints qua controllers
- Xử lý authentication, authorization và validation
- Middleware tùy chỉnh: ValidationMiddleware, ExceptionMiddleware, RequirePaidStatusAttribute
- Tích hợp Swagger/OpenAPI cho tài liệu API
- Cấu hình background jobs với Hangfire

### Công nghệ chính
- **Backend Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core với SQL Server
- **Authentication**: JWT (JSON Web Tokens)
- **Validation**: FluentValidation
- **Object Mapping**: AutoMapper
- **Background Processing**: Hangfire
- **API Documentation**: Swagger/OpenAPI
- **Payment Gateway**: VNPay
- **File Storage**: Firebase Storage
- **Email Service**: SMTP Email
- **Containerization**: Docker & Docker Compose

## Cài đặt và Triển khai

### Yêu cầu hệ thống
- .NET 8.0 SDK
- SQL Server 2019 trở lên
- Visual Studio 2022 hoặc VS Code
- Docker (tùy chọn cho containerization)

### Cài đặt thông thường

1. Clone repository
```bash
git clone https://github.com/yourusername/MomTracking.git
cd MomTracking
```

2. Cấu hình chuỗi kết nối
   Mở file `API/appsettings.json` và cập nhật chuỗi kết nối SQL Server:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=MomTrackingDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

3. Cấu hình JWT và các dịch vụ khác
```json
"SecretToken": {
  "Value": "your-secure-secret-token-at-least-32-characters"
},
"EmailConfiguration": {
  "From": "your-email@example.com",
  "SmtpServer": "smtp.example.com",
  "Port": 587,
  "Username": "your-username",
  "Password": "your-password"
}
```

4. Áp dụng migrations để khởi tạo cơ sở dữ liệu
```bash
dotnet ef database update --project Infrastructure --startup-project API
```

5. Chạy ứng dụng
```bash
dotnet run --project API
```

6. Truy cập Swagger UI tại `https://localhost:5001/swagger`

### Triển khai với Docker

1. Đảm bảo Docker và Docker Compose đã được cài đặt

2. Cập nhật các biến môi trường trong `docker-compose.yml` nếu cần thiết

3. Chạy Docker Compose
```bash
docker-compose up -d
```

4. Truy cập ứng dụng tại `http://localhost:8080`

## API Endpoints

### Authentication
- `POST /api/Auth/register` - Đăng ký tài khoản mới
- `POST /api/Auth/login` - Đăng nhập và nhận JWT token
- `POST /api/Auth/Verification` - Xác minh email

### Quản lý Người dùng
- `GET /api/UserAccount/profile` - Lấy thông tin profile
- `PUT /api/UserAccount/updateprofile` - Cập nhật thông tin cá nhân
- `PUT /api/UserAccount/changepassword` - Thay đổi mật khẩu

### Quản lý Trẻ em
- `POST /api/Children/AddNewChildren` - Thêm thông tin trẻ mới
- `GET /api/Children/GetAllChildren` - Lấy danh sách tất cả trẻ
- `PUT /api/Children/UpdateChildrenData/{id}` - Cập nhật thông tin trẻ
- `DELETE /api/Children/DeleteChildrenDetail/{id}` - Xóa thông tin trẻ

### Theo dõi Sức khỏe
- `POST /api/HealthMetric/AddNewHealthMetric` - Thêm chỉ số sức khỏe mới
- `GET /api/HealthMetric/GetAllHealthMetric` - Lấy tất cả chỉ số sức khỏe
- `PUT /api/HealthMetric/UpdateHealthMetric/{id}` - Cập nhật chỉ số sức khỏe
- `DELETE /api/HealthMetric/DeleteHealthMetric/{id}` - Xóa chỉ số sức khỏe
- `GET /api/HealthMetric/CompareHealthMetricData/{id}` - So sánh với tiêu chuẩn WHO

### Tiêu chuẩn WHO
- `GET /api/WHOStandard` - Lấy tất cả tiêu chuẩn WHO
- `POST /api/WHOStandard` - Thêm tiêu chuẩn mới (chỉ Manager)
- `PUT /api/WHOStandard/{id}` - Cập nhật tiêu chuẩn (chỉ Manager)
- `DELETE /api/WHOStandard/{id}` - Xóa tiêu chuẩn (chỉ Manager)

### Lịch hẹn
- `POST /api/Schedule` - Tạo lịch hẹn mới
- `GET /api/Schedule` - Lấy tất cả lịch hẹn
- `PUT /api/Schedule/{id}` - Cập nhật lịch hẹn
- `DELETE /api/Schedule/{id}` - Xóa lịch hẹn

### Quản lý Thuê bao
- `GET /api/SubscriptionPlan` - Lấy tất cả gói thuê bao
- `POST /api/SubscriptionPlan` - Tạo gói mới (chỉ Manager)
- `PUT /api/SubscriptionPlan/{planId}` - Cập nhật gói (chỉ Manager)
- `DELETE /api/SubscriptionPlan/{planId}` - Xóa gói (chỉ Manager)
- `POST /api/Subscription` - Đăng ký gói thuê bao
- `GET /api/Subscription/my-subscriptions` - Lấy thuê bao của tôi
- `POST /api/Subscription/{subscriptionId}/cancel` - Hủy thuê bao

### Thanh toán
- `POST /api/Payment` - Tạo URL thanh toán VNPay
- `GET /api/Payment/callback` - Callback từ cổng thanh toán

### Bài đăng Cộng đồng
- `GET /api/Post` - Lấy tất cả bài đăng (có phân trang)
- `GET /api/Post/{postId}` - Lấy chi tiết bài đăng
- `POST /api/Post` - Tạo bài đăng mới
- `PUT /api/Post/{postId}` - Cập nhật bài đăng
- `DELETE /api/Post/{postId}` - Xóa bài đăng

### Bình luận
- `POST /api/Comment` - Thêm bình luận mới
- `PUT /api/Comment/{commentId}` - Cập nhật bình luận
- `DELETE /api/Comment/{commentId}` - Xóa bình luận

### Xuất dữ liệu
- `GET /api/PdfExport/health/{childId}` - Xuất báo cáo sức khỏe dạng PDF

### Dashboard (chỉ Manager)
- `GET /api/SubscriptionPlan/NumberOfSubscribers` - Thống kê số người đăng ký theo gói
- `GET /api/SubscriptionPlan/CalculateTotalRevenue` - Tính tổng doanh thu theo gói
- `GET /api/SubscriptionPlan/TotalPrice` - Tính tổng doanh thu

## Background Jobs

Ứng dụng sử dụng Hangfire để quản lý các tác vụ nền:

1. **Nhắc nhở lịch hẹn hàng ngày**
    - Chạy lúc 00:00 mỗi ngày
    - Gửi email nhắc nhở cho các cuộc hẹn trong ngày

2. **Kiểm tra thuê bao hết hạn**
    - Chạy lúc 01:00 mỗi ngày
    - Cập nhật trạng thái các thuê bao đã hết hạn

Bạn có thể truy cập Hangfire Dashboard tại `/hangfire` để theo dõi và quản lý các job.

## Bảo mật

### Xác thực và Phân quyền
- **JWT Authentication**: Sử dụng JSON Web Tokens với thời hạn có thể cấu hình
- **Role-based Authorization**: Phân quyền theo vai trò (Customer, Manager)
- **Xác minh Email**: Gửi mã xác nhận qua email khi đăng ký
- **Mã hóa mật khẩu**: Sử dụng HMACSHA512 cho mã hóa mật khẩu

### Bảo vệ API
- **RequirePaidStatusAttribute**: Middleware tùy chỉnh để kiểm tra người dùng đã thanh toán thuê bao
- **ValidationMiddleware**: Xử lý và chuẩn hóa lỗi validation
- **ExceptionMiddleware**: Xử lý exception an toàn, không để lộ thông tin nhạy cảm

### Xử lý dữ liệu
- **Input Validation**: Sử dụng FluentValidation để kiểm tra dữ liệu đầu vào
- **API Response chuẩn hóa**: Tất cả response đều có cấu trúc nhất quán
- **Logging**: Ghi log chi tiết cho các lỗi và hoạt động hệ thống

## Tính năng đang phát triển

- **Mobile App**: Ứng dụng di động cho iOS và Android
- **Trí tuệ nhân tạo**: Phân tích dữ liệu sức khỏe bằng AI để dự đoán vấn đề
- **Tích hợp y tế**: Kết nối với hệ thống của các bệnh viện và phòng khám
- **Chat trực tiếp**: Tính năng chat với chuyên gia y tế
- **Bảng tin cá nhân hóa**: Nội dung được đề xuất dựa trên tuổi và tình trạng của trẻ
- **Hỗ trợ đa ngôn ngữ**: Mở rộng hỗ trợ nhiều ngôn ngữ

## Đóng góp

Chúng tôi rất hoan nghênh đóng góp từ cộng đồng! Nếu bạn muốn tham gia phát triển MomTracking:

1. Fork repository
2. Tạo nhánh mới (`git checkout -b feature/amazing-feature`)
3. Commit thay đổi (`git commit -m 'Thêm tính năng XYZ'`)
4. Push lên nhánh của bạn (`git push origin feature/amazing-feature`)
5. Tạo Pull Request

Vui lòng tuân thủ coding standards và viết unit tests cho các tính năng mới.

## Xử lý sự cố

### Các vấn đề thường gặp

1. **Lỗi kết nối cơ sở dữ liệu**
    - Kiểm tra chuỗi kết nối trong appsettings.json
    - Đảm bảo SQL Server đang chạy và có thể truy cập
    - Kiểm tra firewall không chặn kết nối

2. **Lỗi xác thực JWT**
    - Đảm bảo SecretToken đủ mạnh và nhất quán
    - Kiểm tra claim types và values

3. **Lỗi khi chạy migrations**
    - Xóa thư mục Migrations và tạo lại migration đầu tiên
    - Chạy `dotnet ef database update` từ thư mục gốc

4. **Lỗi Hangfire**
    - Đảm bảo kết nối đến cơ sở dữ liệu Hangfire
    - Kiểm tra logs để xem chi tiết về lỗi job

### Logs và Debugging

- Logs được lưu tại `API/logs/`
- Có thể cấu hình mức độ log trong appsettings.json
- Sử dụng Swagger UI để test API trực tiếp

## Liên hệ và Hỗ trợ

- **Website**: https://www.momtracking.com
- **Email hỗ trợ**: support@momtracking.com
- **GitHub Issues**: https://github.com/momtracking/issues
- **Tài liệu API**: https://api.momtracking.com/docs

## License

Dự án MomTracking được phân phối dưới giấy phép MIT. Xem file `LICENSE` để biết thêm chi tiết.

---

© 2025 MomTracking. Bảo lưu mọi quyền.