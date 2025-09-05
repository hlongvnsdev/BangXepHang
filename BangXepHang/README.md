# BangXepHang API

API xếp hạng khách hàng theo tháng sử dụng .NET 8 và PostgreSQL.

## Cấu hình

### 1. Cài đặt PostgreSQL
- Cài đặt PostgreSQL trên máy của bạn
- Tạo database `BangXepHangDB`
- Cập nhật connection string trong `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=BangXepHangDB;Username=postgres;Password=your_password"
  }
}
```

### 2. Chạy ứng dụng
```bash
cd BangXepHang
dotnet restore
dotnet run
```

API sẽ chạy tại: `https://localhost:7000` (hoặc `http://localhost:5000`)

## API Endpoints

### Khách hàng (Customer)

#### GET /api/customer
Lấy danh sách tất cả khách hàng

#### GET /api/customer/{id}
Lấy thông tin khách hàng theo ID

#### POST /api/customer
Tạo khách hàng mới
```json
{
  "name": "Tên khách hàng",
  "avatar": "URL avatar (tùy chọn)"
}
```

#### PUT /api/customer/{id}
Cập nhật thông tin khách hàng

#### DELETE /api/customer/{id}
Xóa khách hàng

### Điểm số (GameScore)

#### GET /api/gamescore
Lấy danh sách điểm số
- `customerId` (tùy chọn): Lọc theo ID khách hàng
- `limit` (mặc định 50): Số lượng bản ghi trả về

#### GET /api/gamescore/{id}
Lấy điểm số theo ID

#### POST /api/gamescore
Thêm điểm số mới
```json
{
  "customerId": 1,
  "playTime": "2024-01-15T10:30:00Z",
  "score": 1500
}
```

#### PUT /api/gamescore/{id}
Cập nhật điểm số

#### DELETE /api/gamescore/{id}
Xóa điểm số

#### GET /api/gamescore/stats/{customerId}
Lấy thống kê điểm số của khách hàng

### Xếp hạng (Ranking)

#### GET /api/ranking/monthly
Lấy bảng xếp hạng theo tháng
- `year` (tùy chọn): Năm (mặc định năm hiện tại)
- `month` (tùy chọn): Tháng (mặc định tháng hiện tại)
- `limit` (mặc định 10): Số lượng người chơi trả về

Ví dụ: `/api/ranking/monthly?year=2024&month=1&limit=20`

#### GET /api/ranking/overall
Lấy bảng xếp hạng tổng thể (tất cả thời gian)
- `limit` (mặc định 10): Số lượng người chơi trả về

#### GET /api/ranking/available-months
Lấy danh sách các tháng có dữ liệu xếp hạng

#### GET /api/ranking/by-days
Lấy bảng xếp hạng theo số ngày truyền vào
- `days` (mặc định 30): Số ngày từ hiện tại trở về trước
- `limit` (mặc định 10): Số lượng người chơi trả về

Ví dụ: `/api/ranking/by-days?days=7&limit=20` - Xếp hạng 7 ngày gần đây

#### GET /api/ranking/by-date-range
Lấy bảng xếp hạng theo khoảng thời gian tùy chỉnh
- `startDate` (required): Ngày bắt đầu (format: yyyy-MM-dd)
- `endDate` (required): Ngày kết thúc (format: yyyy-MM-dd)
- `limit` (mặc định 10): Số lượng người chơi trả về

Ví dụ: `/api/ranking/by-date-range?startDate=2024-01-01&endDate=2024-01-31&limit=15`

## Cấu trúc Database

### Bảng Customers
- `Id`: Primary Key
- `Name`: Tên khách hàng (Required, Max 100 ký tự)
- `Avatar`: URL avatar (Optional, Max 500 ký tự)
- `CreatedAt`: Thời gian tạo

### Bảng GameScores
- `Id`: Primary Key
- `CustomerId`: Foreign Key đến bảng Customers
- `PlayTime`: Thời gian chơi (Required)
- `Score`: Điểm đạt được (Required)
- `CreatedAt`: Thời gian tạo

## Ví dụ sử dụng

### 1. Tạo khách hàng mới
```bash
curl -X POST "https://localhost:7000/api/customer" \
  -H "Content-Type: application/json" \
  -d '{"name": "Nguyễn Văn A", "avatar": "https://example.com/avatar.jpg"}'
```

### 2. Thêm điểm số
```bash
curl -X POST "https://localhost:7000/api/gamescore" \
  -H "Content-Type: application/json" \
  -d '{"customerId": 1, "playTime": "2024-01-15T10:30:00Z", "score": 1500}'
```

### 3. Lấy xếp hạng tháng 1/2024
```bash
curl "https://localhost:7000/api/ranking/monthly?year=2024&month=1&limit=10"
```

### 4. Lấy xếp hạng 7 ngày gần đây
```bash
curl "https://localhost:7000/api/ranking/by-days?days=7&limit=10"
```

### 5. Lấy xếp hạng theo khoảng thời gian
```bash
curl "https://localhost:7000/api/ranking/by-date-range?startDate=2024-01-01&endDate=2024-01-31&limit=15"
```

## Swagger UI

Khi chạy ứng dụng trong môi trường Development, bạn có thể truy cập Swagger UI tại:
- `https://localhost:7000/swagger` (HTTPS)
- `http://localhost:5000/swagger` (HTTP)

Swagger UI cung cấp giao diện tương tác để test các API endpoints.
