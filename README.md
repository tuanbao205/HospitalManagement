# 🏥 Hospital Management System

Hệ thống quản lý bệnh viện xây dựng bằng **WPF .NET Framework** + **Entity Framework 6** + **SQL Server**.

## 📋 Tính năng

### Admin
- Dashboard thống kê tổng quan
- Quản lý bệnh nhân (thêm/sửa/xóa/tìm kiếm)
- Quản lý bác sĩ (thêm/sửa/xóa/tìm kiếm)
- Đặt lịch khám + kiểm tra trùng lịch tự động
- Xem hồ sơ bệnh án
- Quản lý tài khoản người dùng

### Doctor
- Xem lịch khám của mình
- Tạo + xem + in hồ sơ bệnh án
- Đổi mật khẩu

## 🛠️ Công nghệ

| Thành phần | Công nghệ |
|---|---|
| UI | WPF .NET Framework 4.7.2 |
| Pattern | MVVM + Repository + Service |
| ORM | Entity Framework 6 (Code First) |
| Database | SQL Server Express |
| UI Theme | MaterialDesignThemes 4.9.0 |

## 🚀 Cài đặt và chạy

### Yêu cầu
- Visual Studio 2022+
- SQL Server Express
- .NET Framework 4.7.2

### Các bước
1. Clone repo về máy
```bash
   git clone https://github.com/tuanbao205/HospitalManagement.git
```

2. Mở file `HospitalManagement.slnx` bằng Visual Studio

3. Sửa connection string trong `App.config` nếu cần
```xml
   <add name="HospitalDB"
        connectionString="Server=localhost\SQLEXPRESS;Database=HospitalDB;Trusted_Connection=True;"
        providerName="System.Data.SqlClient"/>
```

4. Chạy Migration để tạo database
```bash
   Update-Database
```

5. Build và chạy (F5)
