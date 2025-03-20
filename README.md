# MomTracking - Nền tảng Theo dõi Sức khỏe Mẹ và Bé

## Tổng quan

MomTracking là nền tảng theo dõi sức khỏe toàn diện dành cho mẹ và bé. Ứng dụng cho phép người dùng theo dõi các chỉ số sức khỏe của trẻ, so sánh dữ liệu tăng trưởng với tiêu chuẩn WHO, quản lý lịch hẹn thông qua hệ thống đặt lịch, và truy cập các tính năng cao cấp thông qua mô hình thuê bao.

## Tính năng chính

- **Theo dõi Sức khỏe Trẻ em**: Giám sát và ghi lại các chỉ số sức khỏe quan trọng bao gồm cân nặng, chiều cao, chu vi đầu và các thông số tăng trưởng khác
- **Phân tích Tăng trưởng**: So sánh dữ liệu sức khỏe của trẻ với tiêu chuẩn WHO để xác định các vấn đề sức khỏe tiềm ẩn
- **Lịch hẹn Khám bệnh**: Tạo và quản lý các cuộc hẹn chăm sóc sức khỏe với thông báo nhắc nhở
- **Diễn đàn Cộng đồng**: Chức năng đăng bài và bình luận để hỗ trợ cộng đồng và chia sẻ thông tin
- **Quản lý Gói thuê bao**: Các gói thuê bao theo cấp bậc (Đồng, Bạc, Vàng) với các tính năng và thời hạn khác nhau
- **Xuất PDF**: Tạo báo cáo về dữ liệu sức khỏe của trẻ để chia sẻ với nhà cung cấp dịch vụ chăm sóc sức khỏe
- **Quản lý Người dùng**: Đăng ký, xác thực và quản lý hồ sơ với xác minh email
- **Xử lý Thanh toán**: Tích hợp với VNPay cho thanh toán thuê bao

## Công nghệ sử dụng

### Backend
- **Framework**: ASP.NET Core 8.0
- **Kiến trúc**: Mô hình Clean Architecture với các lớp Domain, Application, Infrastructure và API
- **Cơ sở dữ liệu**: SQL Server với Entity Framework Core
- **Xác thực**: JWT (JSON Web Tokens)
- **Xác thực dữ liệu**: FluentValidation
- **Mapping đối tượng**: AutoMapper
- **Xử lý nền**: Hangfire cho các tác vụ theo lịch
- **Tài liệu API**: Swagger/OpenAPI

### Hạ tầng
- **Docker**: Hỗ trợ container hóa với docker-compose
- **Dịch vụ Email**: Dịch vụ email SMTP cho thông báo và xác minh

## Kiến trúc

Ứng dụng tuân theo nguyên tắc Clean Architecture với bốn lớp riêng biệt:

### Lớp Domain
Chứa các quy tắc nghiệp vụ doanh nghiệp và các thực thể đại diện cho các khái niệm kinh doanh cốt lõi:
- Tài khoản người dùng và hồ sơ trẻ em
- Chỉ số sức khỏe và tiêu chuẩn WHO
- Thuê bao và xử lý thanh toán
- Bài đăng và bình luận cộng đồng
- Lịch hẹn và thông báo

### Lớp Application
Chứa các quy tắc nghiệp vụ ứng dụng và các trường hợp sử dụng:
- Giao diện dịch vụ và triển khai
- DTO Yêu cầu/Phản hồi
- Quy tắc xác thực
- Cấu hình mapping
- Xử lý ngoại lệ tùy chỉnh

### Lớp Infrastructure
Chứa framework và chi tiết triển khai:
- Context và cấu hình cơ sở dữ liệu
- Triển khai repository
- Triển khai mô hình Unit of Work
- Triển khai dịch vụ email
- Tích hợp dịch vụ bên ngoài

### Lớp API
Chứa lớp trình bày và các điểm đầu vào:
- Controllers REST API
- Các thành phần middleware
- Cấu hình xác thực
- Xử lý ngoại lệ
- Tài liệu API

## Bắt đầu

### Yêu cầu hệ thống
- .NET 8.0 SDK
- SQL Server
- Docker (tùy chọn)

### Cài đặt

1. Clone repository
```bash
git clone <đường-dẫn-repository>
cd MomTracking
```

2. Cập nhật chuỗi kết nối trong `appsettings.json` để trỏ đến instance SQL Server của bạn

3. Áp dụng migration cơ sở dữ liệu
```bash
dotnet ef database update
```

4. Chạy ứng dụng
```bash
dotnet run --project API
```

5. Truy cập tài liệu Swagger tại `https://localhost:5001/swagger`

### Triển khai bằng Docker

Ứng dụng có thể được triển khai bằng Docker:

```bash
docker-compose up -d
```

## Các điểm cuối API

API cung cấp các điểm cuối chính sau:

- **Xác thực**: `/api/Auth` - Đăng ký, đăng nhập và xác minh email
- **Quản lý Người dùng**: `/api/UserAccount` - Quản lý hồ sơ người dùng
- **Trẻ em**: `/api/Children` - Quản lý hồ sơ trẻ
- **Chỉ số Sức khỏe**: `/api/HealthMetric` - Theo dõi và phân tích dữ liệu sức khỏe
- **Tiêu chuẩn WHO**: `/api/WHOStandard` - Truy cập tiêu chuẩn tăng trưởng
- **Lịch hẹn**: `/api/Schedule` - Quản lý cuộc hẹn
- **Cộng đồng**: `/api/Post` và `/api/Comment` - Chức năng diễn đàn
- **Thuê bao**: `/api/Subscription` và `/api/SubscriptionPlan` - Quản lý thuê bao
- **Thanh toán**: `/api/Payment` - Xử lý thanh toán
- **Xuất PDF**: `/api/PdfExport` - Tạo báo cáo

## Công việc nền

Ứng dụng sử dụng Hangfire để chạy các tác vụ theo lịch sau:

- Nhắc nhở cuộc hẹn hàng ngày (chạy lúc 00:00)
- Kiểm tra hết hạn thuê bao (chạy lúc 01:00)

## Bảo mật

- Xác thực JWT với phân quyền dựa trên vai trò
- Mã hóa mật khẩu với HMACSHA512
- Xác minh email cho tài khoản mới
- Kiểm soát truy cập dựa trên thuê bao cho các tính năng cao cấp

## Cấu trúc dự án

```
MomTracking/
├── API/                 # Lớp API với controllers và middleware
├── Application/         # Lớp Application với services và DTOs
├── Domain/              # Lớp Domain với entities và business rules
├── Infrastructure/      # Lớp Infrastructure với data access và external services
├── docker-compose.yml   # Cấu hình Docker
└── MomTracking.sln      # File solution
```

## Tính năng tương lai

- Tích hợp ứng dụng di động
- Phân tích nâng cao cho dữ liệu sức khỏe
- Tích hợp với hệ thống nhà cung cấp dịch vụ y tế
- Hỗ trợ đa ngôn ngữ
- Mở rộng tính năng cộng đồng

## Đóng góp

Chúng tôi hoan nghênh mọi đóng góp cho dự án MomTracking! Nếu bạn muốn đóng góp:

1. Fork repository
2. Tạo nhánh mới (`git checkout -b feature/amazing-feature`)
3. Commit thay đổi (`git commit -m 'Thêm tính năng tuyệt vời'`)
4. Push lên nhánh (`git push origin feature/amazing-feature`)
5. Tạo Pull Request

## Xử lý sự cố

Nếu bạn gặp vấn đề khi cài đặt hoặc chạy ứng dụng:

- Kiểm tra xem bạn đã cài đặt đúng phiên bản .NET 8.0 SDK
- Đảm bảo SQL Server đang chạy và chuỗi kết nối chính xác
- Kiểm tra logs ở thư mục `API/logs` để xem chi tiết lỗi

## Giấy phép

Dự án này được cấp phép theo giấy phép MIT. Xem file `LICENSE` để biết thêm thông tin.

## Liên hệ

Nếu bạn có bất kỳ câu hỏi hoặc phản hồi nào, vui lòng liên hệ:

- **Email**: support@momtracking.com
- **Website**: https://www.momtracking.com
- **GitHub**: https://github.com/momtracking

---

© 2025 MomTracking. Bảo lưu mọi quyền.
