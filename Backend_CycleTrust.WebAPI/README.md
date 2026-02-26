# CycleTrust Backend – API Documentation

> **Base URL:** `https://localhost:{port}/api`
> **Database:** PostgreSQL (`CycleTrustDB`)
> **Framework:** ASP.NET Core 9 Web API
> **Content-Type:** `application/json`
> **CORS:** Cho phép tất cả origins, methods, headers
> **Swagger UI:** `/swagger` (chỉ ở môi trường Development)

---

## Mục lục

1. [Enums – Giá trị cố định](#enums--giá-trị-cố-định)
2. [Roles](#roles)
3. [Users](#users)
4. [Brands](#brands)
5. [Categories](#categories)
6. [Bikes](#bikes)
7. [BikeImages](#bikeimages)
8. [InspectionReports](#inspectionreports)
9. [Orders](#orders)
10. [Reviews](#reviews)
11. [Messages](#messages)
12. [Wishlists](#wishlists)
13. [Reports](#reports)
14. [Seed Data mặc định](#seed-data-mặc-định)

---

## Enums – Giá trị cố định

Tất cả enum được lưu và trả về dạng **chuỗi in hoa**.

| Enum | Các giá trị hợp lệ |
|---|---|
| `UserStatus` | `ACTIVE` `BANNED` |
| `BikeStatus` | `PENDING` `APPROVED` `REJECTED` `SOLD` |
| `BikeCondition` | `NEW` `USED_LIKE_NEW` `USED_GOOD` `USED_FAIR` |
| `OrderStatus` | `PENDING` `DEPOSITED` `COMPLETED` `CANCELLED` |
| `InspectionStatus` | `PENDING` `APPROVED` `REJECTED` |
| `ReportStatus` | `PENDING` `RESOLVED` `REJECTED` |

---

## Roles

**Base endpoint:** `/api/roles`

### `GET /api/roles`
Lấy danh sách tất cả vai trò.

**Response `200`:**
```json
[
  {
    "roleId": 1,
    "roleName": "ADMIN"
  }
]
```

---

### `GET /api/roles/{id}`
Lấy vai trò theo ID.

| Param | Vị trí | Kiểu | Bắt buộc |
|---|---|---|---|
| `id` | path | `int` | ✅ |

**Response `200`:**
```json
{
  "roleId": 1,
  "roleName": "ADMIN"
}
```
**Response `404`:** Không tìm thấy.

---

### `POST /api/roles`
Tạo vai trò mới.

**Request Body:**
```json
{
  "roleName": "string"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `roleName` | `string` | ✅ | Unique, max 50 ký tự |

**Response `201`:**
```json
{
  "roleId": 5,
  "roleName": "MODERATOR"
}
```

---

### `PUT /api/roles/{id}`
Cập nhật vai trò.

**Request Body:**
```json
{
  "roleName": "string"
}
```

| Trường | Kiểu | Bắt buộc |
|---|---|---|
| `roleName` | `string` | ✅ |

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

### `DELETE /api/roles/{id}`
Xóa vai trò.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

## Users

**Base endpoint:** `/api/users`

### `GET /api/users`
Lấy danh sách tất cả người dùng.

**Response `200`:**
```json
[
  {
    "userId": 1,
    "fullName": "Admin CycleTrust",
    "email": "admin@cycletrust.com",
    "phone": "0901000001",
    "address": "Ho Chi Minh City",
    "roleId": 1,
    "roleName": "ADMIN",
    "status": "ACTIVE",
    "createdAt": "2026-01-01T00:00:00Z"
  }
]
```

---

### `GET /api/users/{id}`
Lấy người dùng theo ID.

**Response `200`:** Cấu trúc như trên.
**Response `404`:** Không tìm thấy.

---

### `POST /api/users`
Tạo người dùng mới. `status` mặc định là `ACTIVE`.

**Request Body:**
```json
{
  "fullName": "string",
  "email": "string",
  "password": "string",
  "phone": "string",
  "address": "string",
  "roleId": 2
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `fullName` | `string` | ✅ | Max 100 ký tự |
| `email` | `string` | ✅ | Unique, max 100 ký tự |
| `password` | `string` | ✅ | Max 255 ký tự |
| `phone` | `string` | ❌ | Max 20 ký tự |
| `address` | `string` | ❌ | Max 255 ký tự |
| `roleId` | `int` | ✅ | 1=ADMIN, 2=BUYER, 3=SELLER, 4=INSPECTOR |

**Response `201`:** `UserResponseDto`

---

### `PUT /api/users/{id}`
Cập nhật thông tin người dùng.

**Request Body:**
```json
{
  "fullName": "string",
  "email": "string",
  "phone": "string",
  "address": "string",
  "roleId": 2,
  "status": "ACTIVE"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `fullName` | `string` | ✅ | |
| `email` | `string` | ✅ | |
| `phone` | `string` | ❌ | |
| `address` | `string` | ❌ | |
| `roleId` | `int` | ✅ | |
| `status` | `string` | ✅ | `ACTIVE` hoặc `BANNED` |

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

### `DELETE /api/users/{id}`
Xóa người dùng.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

## Brands

**Base endpoint:** `/api/brands`

### `GET /api/brands`
Lấy danh sách thương hiệu.

**Response `200`:**
```json
[
  { "brandId": 1, "brandName": "Giant" },
  { "brandId": 2, "brandName": "Trek" }
]
```

---

### `GET /api/brands/{id}`
Lấy thương hiệu theo ID.

**Response `200`:**
```json
{ "brandId": 1, "brandName": "Giant" }
```
**Response `404`:** Không tìm thấy.

---

### `POST /api/brands`
Tạo thương hiệu mới.

**Request Body:**
```json
{
  "brandName": "string"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `brandName` | `string` | ✅ | Unique, max 100 ký tự |

**Response `201`:**
```json
{ "brandId": 6, "brandName": "Bianchi" }
```

---

### `PUT /api/brands/{id}`
Cập nhật thương hiệu.

**Request Body:**
```json
{ "brandName": "string" }
```

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

### `DELETE /api/brands/{id}`
Xóa thương hiệu.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

## Categories

**Base endpoint:** `/api/categories`

### `GET /api/categories`
Lấy danh sách danh mục.

**Response `200`:**
```json
[
  { "categoryId": 1, "categoryName": "Road Bike" },
  { "categoryId": 2, "categoryName": "Mountain Bike" }
]
```

---

### `GET /api/categories/{id}`
Lấy danh mục theo ID.

**Response `200`:**
```json
{ "categoryId": 1, "categoryName": "Road Bike" }
```
**Response `404`:** Không tìm thấy.

---

### `POST /api/categories`
Tạo danh mục mới.

**Request Body:**
```json
{ "categoryName": "string" }
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `categoryName` | `string` | ✅ | Unique, max 100 ký tự |

**Response `201`:**
```json
{ "categoryId": 6, "categoryName": "BMX" }
```

---

### `PUT /api/categories/{id}`
Cập nhật danh mục.

**Request Body:**
```json
{ "categoryName": "string" }
```

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

### `DELETE /api/categories/{id}`
Xóa danh mục.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

## Bikes

**Base endpoint:** `/api/bikes`

### `GET /api/bikes`
Lấy danh sách tất cả xe đạp (bao gồm hình ảnh, thương hiệu, danh mục, người bán).

**Response `200`:**
```json
[
  {
    "bikeId": 1,
    "sellerId": 3,
    "sellerName": "Tran Thi Seller",
    "title": "Giant Contend AR 1 2025",
    "description": "Xe đạp đường trường Giant Contend AR 1...",
    "price": 25000000,
    "brandId": 1,
    "brandName": "Giant",
    "categoryId": 1,
    "categoryName": "Road Bike",
    "frameSize": "M",
    "bikeCondition": "USED_LIKE_NEW",
    "status": "APPROVED",
    "createdAt": "2026-01-01T00:00:00Z",
    "imageUrls": [
      "https://example.com/image1.jpg",
      "https://example.com/image2.jpg"
    ]
  }
]
```

---

### `GET /api/bikes/{id}`
Lấy xe đạp theo ID.

**Response `200`:** Cấu trúc như trên.
**Response `404`:** Không tìm thấy.

---

### `POST /api/bikes`
Tạo bài đăng xe mới. `status` mặc định là `PENDING`.

**Request Body:**
```json
{
  "sellerId": 3,
  "title": "string",
  "description": "string",
  "price": 25000000,
  "brandId": 1,
  "categoryId": 1,
  "frameSize": "M",
  "bikeCondition": "NEW"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `sellerId` | `int` | ✅ | ID của user có role SELLER |
| `title` | `string` | ✅ | Max 255 ký tự |
| `description` | `string` | ❌ | |
| `price` | `decimal` | ✅ | Đơn vị VND, tối đa 12 chữ số, 2 chữ số thập phân |
| `brandId` | `int` | ❌ | |
| `categoryId` | `int` | ❌ | |
| `frameSize` | `string` | ❌ | Max 50 ký tự, ví dụ: `S`, `M`, `L`, `XL` |
| `bikeCondition` | `string` | ❌ | `NEW` `USED_LIKE_NEW` `USED_GOOD` `USED_FAIR` |

**Response `201`:** `BikeResponseDto`

---

### `PUT /api/bikes/{id}`
Cập nhật thông tin xe. Admin dùng để duyệt/từ chối.

**Request Body:**
```json
{
  "title": "string",
  "description": "string",
  "price": 25000000,
  "brandId": 1,
  "categoryId": 1,
  "frameSize": "M",
  "bikeCondition": "USED_GOOD",
  "status": "APPROVED"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `title` | `string` | ✅ | |
| `description` | `string` | ❌ | |
| `price` | `decimal` | ✅ | |
| `brandId` | `int` | ❌ | |
| `categoryId` | `int` | ❌ | |
| `frameSize` | `string` | ❌ | |
| `bikeCondition` | `string` | ❌ | `NEW` `USED_LIKE_NEW` `USED_GOOD` `USED_FAIR` |
| `status` | `string` | ❌ | `PENDING` `APPROVED` `REJECTED` `SOLD` |

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

### `DELETE /api/bikes/{id}`
Xóa xe đạp. Cascade xóa toàn bộ `BikeImages` liên quan.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

## BikeImages

**Base endpoint:** `/api/bikeimages`

### `GET /api/bikeimages`
Lấy danh sách tất cả hình ảnh xe.

**Response `200`:**
```json
[
  {
    "imageId": 1,
    "bikeId": 1,
    "imageUrl": "https://example.com/image.jpg"
  }
]
```

---

### `GET /api/bikeimages/{id}`
Lấy hình ảnh theo ID.

**Response `200`:** Cấu trúc như trên.
**Response `404`:** Không tìm thấy.

---

### `POST /api/bikeimages`
Thêm hình ảnh cho xe.

**Request Body:**
```json
{
  "bikeId": 1,
  "imageUrl": "https://example.com/image.jpg"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `bikeId` | `int` | ✅ | |
| `imageUrl` | `string` | ✅ | Max 500 ký tự |

**Response `201`:** `BikeImageResponseDto`

---

### `DELETE /api/bikeimages/{id}`
Xóa hình ảnh theo ID.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

> ⚠️ BikeImages **không có** endpoint `PUT`.

---

## InspectionReports

**Base endpoint:** `/api/inspectionreports`

### `GET /api/inspectionreports`
Lấy danh sách tất cả báo cáo kiểm định.

**Response `200`:**
```json
[
  {
    "reportId": 1,
    "bikeId": 1,
    "bikeTitle": "Giant Contend AR 1 2025",
    "inspectorId": 4,
    "inspectorName": "Le Van Inspector",
    "frameCondition": "Khung nhôm còn mới, không trầy xước.",
    "brakeCondition": "Phanh đĩa hoạt động tốt.",
    "drivetrainCondition": "Hệ thống truyền động Shimano 105 mượt mà.",
    "overallComment": "Xe trong tình trạng rất tốt.",
    "reportFile": null,
    "inspectionStatus": "APPROVED",
    "inspectedAt": "2026-01-01T00:00:00Z"
  }
]
```

---

### `GET /api/inspectionreports/{id}`
Lấy báo cáo kiểm định theo ID.

**Response `200`:** Cấu trúc như trên.
**Response `404`:** Không tìm thấy.

---

### `POST /api/inspectionreports`
Tạo báo cáo kiểm định mới. `inspectionStatus` mặc định là `PENDING`.

**Request Body:**
```json
{
  "bikeId": 1,
  "inspectorId": 4,
  "frameCondition": "string",
  "brakeCondition": "string",
  "drivetrainCondition": "string",
  "overallComment": "string",
  "reportFile": "https://example.com/report.pdf"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `bikeId` | `int` | ✅ | |
| `inspectorId` | `int` | ✅ | ID của user có role INSPECTOR |
| `frameCondition` | `string` | ❌ | |
| `brakeCondition` | `string` | ❌ | |
| `drivetrainCondition` | `string` | ❌ | |
| `overallComment` | `string` | ❌ | |
| `reportFile` | `string` | ❌ | URL file báo cáo, max 500 ký tự |

**Response `201`:** `InspectionReportResponseDto`

---

### `PUT /api/inspectionreports/{id}`
Cập nhật báo cáo kiểm định. Inspector dùng để cập nhật nội dung và kết quả.

**Request Body:**
```json
{
  "frameCondition": "string",
  "brakeCondition": "string",
  "drivetrainCondition": "string",
  "overallComment": "string",
  "reportFile": "string",
  "inspectionStatus": "APPROVED"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `frameCondition` | `string` | ❌ | |
| `brakeCondition` | `string` | ❌ | |
| `drivetrainCondition` | `string` | ❌ | |
| `overallComment` | `string` | ❌ | |
| `reportFile` | `string` | ❌ | |
| `inspectionStatus` | `string` | ❌ | `PENDING` `APPROVED` `REJECTED` |

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

### `DELETE /api/inspectionreports/{id}`
Xóa báo cáo kiểm định.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

## Orders

**Base endpoint:** `/api/orders`

### `GET /api/orders`
Lấy danh sách tất cả đơn hàng.

**Response `200`:**
```json
[
  {
    "orderId": 1,
    "bikeId": 1,
    "bikeTitle": "Giant Contend AR 1 2025",
    "buyerId": 2,
    "buyerName": "Nguyen Van Buyer",
    "sellerId": 3,
    "sellerName": "Tran Thi Seller",
    "totalAmount": 25000000,
    "depositAmount": 5000000,
    "status": "COMPLETED",
    "createdAt": "2026-01-01T00:00:00Z"
  }
]
```

---

### `GET /api/orders/{id}`
Lấy đơn hàng theo ID.

**Response `200`:** Cấu trúc như trên.
**Response `404`:** Không tìm thấy.

---

### `POST /api/orders`
Tạo đơn hàng mới. `status` mặc định là `PENDING`.

**Request Body:**
```json
{
  "bikeId": 1,
  "buyerId": 2,
  "sellerId": 3,
  "totalAmount": 25000000,
  "depositAmount": 5000000
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `bikeId` | `int` | ✅ | |
| `buyerId` | `int` | ✅ | ID của user có role BUYER |
| `sellerId` | `int` | ✅ | ID của user có role SELLER |
| `totalAmount` | `decimal` | ✅ | Đơn vị VND |
| `depositAmount` | `decimal` | ❌ | Tiền đặt cọc |

**Response `201`:** `OrderResponseDto`

---

### `PUT /api/orders/{id}`
Cập nhật trạng thái đơn hàng.

**Request Body:**
```json
{
  "depositAmount": 5000000,
  "status": "COMPLETED"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `depositAmount` | `decimal` | ❌ | |
| `status` | `string` | ✅ | `PENDING` `DEPOSITED` `COMPLETED` `CANCELLED` |

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

### `DELETE /api/orders/{id}`
Xóa đơn hàng.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

## Reviews

**Base endpoint:** `/api/reviews`

### `GET /api/reviews`
Lấy danh sách tất cả đánh giá.

**Response `200`:**
```json
[
  {
    "reviewId": 1,
    "orderId": 1,
    "buyerId": 2,
    "buyerName": "Nguyen Van Buyer",
    "sellerId": 3,
    "sellerName": "Tran Thi Seller",
    "rating": 5,
    "comment": "Xe rất đẹp, đúng mô tả. Seller giao hàng nhanh!",
    "createdAt": "2026-01-01T00:00:00Z"
  }
]
```

---

### `GET /api/reviews/{id}`
Lấy đánh giá theo ID.

**Response `200`:** Cấu trúc như trên.
**Response `404`:** Không tìm thấy.

---

### `POST /api/reviews`
Tạo đánh giá mới.

**Request Body:**
```json
{
  "orderId": 1,
  "buyerId": 2,
  "sellerId": 3,
  "rating": 5,
  "comment": "string"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `orderId` | `int` | ✅ | |
| `buyerId` | `int` | ✅ | |
| `sellerId` | `int` | ✅ | |
| `rating` | `int` | ❌ | Gợi ý từ 1 đến 5 |
| `comment` | `string` | ❌ | |

**Response `201`:** `ReviewResponseDto`

---

### `PUT /api/reviews/{id}`
Cập nhật đánh giá.

**Request Body:**
```json
{
  "rating": 4,
  "comment": "string"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `rating` | `int` | ❌ | |
| `comment` | `string` | ❌ | |

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

### `DELETE /api/reviews/{id}`
Xóa đánh giá.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

## Messages

**Base endpoint:** `/api/messages`

### `GET /api/messages`
Lấy danh sách tất cả tin nhắn.

**Response `200`:**
```json
[
  {
    "messageId": 1,
    "senderId": 2,
    "senderName": "Nguyen Van Buyer",
    "receiverId": 3,
    "receiverName": "Tran Thi Seller",
    "bikeId": 2,
    "content": "Chào bạn, xe Trek Marlin 7 còn không ạ?",
    "sentAt": "2026-01-01T00:00:00Z"
  }
]
```

---

### `GET /api/messages/{id}`
Lấy tin nhắn theo ID.

**Response `200`:** Cấu trúc như trên.
**Response `404`:** Không tìm thấy.

---

### `POST /api/messages`
Gửi tin nhắn mới.

**Request Body:**
```json
{
  "senderId": 2,
  "receiverId": 3,
  "bikeId": 2,
  "content": "string"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `senderId` | `int` | ✅ | |
| `receiverId` | `int` | ✅ | |
| `bikeId` | `int` | ❌ | Liên kết tin nhắn với một xe cụ thể |
| `content` | `string` | ✅ | |

**Response `201`:** `MessageResponseDto`

---

### `DELETE /api/messages/{id}`
Xóa tin nhắn.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

> ⚠️ Messages **không có** endpoint `PUT` (không hỗ trợ chỉnh sửa tin nhắn).

---

## Wishlists

**Base endpoint:** `/api/wishlists`

### `GET /api/wishlists`
Lấy danh sách tất cả mục yêu thích.

**Response `200`:**
```json
[
  {
    "wishlistId": 1,
    "buyerId": 2,
    "buyerName": "Nguyen Van Buyer",
    "bikeId": 2,
    "bikeTitle": "Trek Marlin 7 2024",
    "createdAt": "2026-01-01T00:00:00Z"
  }
]
```

---

### `GET /api/wishlists/{id}`
Lấy mục yêu thích theo ID.

**Response `200`:** Cấu trúc như trên.
**Response `404`:** Không tìm thấy.

---

### `POST /api/wishlists`
Thêm xe vào danh sách yêu thích. Cặp `(buyerId, bikeId)` phải là duy nhất.

**Request Body:**
```json
{
  "buyerId": 2,
  "bikeId": 2
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `buyerId` | `int` | ✅ | |
| `bikeId` | `int` | ✅ | |

**Response `201`:** `WishlistResponseDto`

---

### `DELETE /api/wishlists/{id}`
Xóa khỏi danh sách yêu thích.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

> ⚠️ Wishlists **không có** endpoint `PUT`.

---

## Reports

**Base endpoint:** `/api/reports`

### `GET /api/reports`
Lấy danh sách tất cả báo cáo vi phạm.

**Response `200`:**
```json
[
  {
    "reportId": 1,
    "reporterId": 2,
    "reporterName": "Nguyen Van Buyer",
    "bikeId": 3,
    "bikeTitle": "Specialized Sirrus X 4.0",
    "reason": "Giá niêm yết không hợp lý so với tình trạng xe.",
    "status": "PENDING",
    "createdAt": "2026-01-01T00:00:00Z"
  }
]
```

---

### `GET /api/reports/{id}`
Lấy báo cáo vi phạm theo ID.

**Response `200`:** Cấu trúc như trên.
**Response `404`:** Không tìm thấy.

---

### `POST /api/reports`
Tạo báo cáo vi phạm mới. `status` mặc định là `PENDING`.

**Request Body:**
```json
{
  "reporterId": 2,
  "bikeId": 3,
  "reason": "string"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `reporterId` | `int` | ✅ | |
| `bikeId` | `int` | ❌ | Có thể null |
| `reason` | `string` | ✅ | |

**Response `201`:** `ReportResponseDto`

---

### `PUT /api/reports/{id}`
Xử lý báo cáo vi phạm. Dành cho ADMIN.

**Request Body:**
```json
{
  "status": "RESOLVED"
}
```

| Trường | Kiểu | Bắt buộc | Ghi chú |
|---|---|---|---|
| `status` | `string` | ✅ | `PENDING` `RESOLVED` `REJECTED` |

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

### `DELETE /api/reports/{id}`
Xóa báo cáo vi phạm.

**Response `204`:** Thành công.
**Response `404`:** Không tìm thấy.

---

## Seed Data mặc định

Dữ liệu được tự động seed khi chạy migration lần đầu.

### Roles

| roleId | roleName |
|---|---|
| 1 | ADMIN |
| 2 | BUYER |
| 3 | SELLER |
| 4 | INSPECTOR |

### Users

| userId | email | password | roleName |
|---|---|---|---|
| 1 | admin@cycletrust.com | admin123 | ADMIN |
| 2 | buyer@cycletrust.com | buyer123 | BUYER |
| 3 | seller@cycletrust.com | seller123 | SELLER |
| 4 | inspector@cycletrust.com | inspector123 | INSPECTOR |

### Brands

| brandId | brandName |
|---|---|
| 1 | Giant |
| 2 | Trek |
| 3 | Specialized |
| 4 | Cannondale |
| 5 | Merida |

### Categories

| categoryId | categoryName |
|---|---|
| 1 | Road Bike |
| 2 | Mountain Bike |
| 3 | City Bike |
| 4 | Folding Bike |
| 5 | Electric Bike |

### Bikes

| bikeId | title | status | bikeCondition | price |
|---|---|---|---|---|
| 1 | Giant Contend AR 1 2025 | APPROVED | USED_LIKE_NEW | 25,000,000 |
| 2 | Trek Marlin 7 2024 | APPROVED | USED_GOOD | 18,000,000 |
| 3 | Specialized Sirrus X 4.0 | PENDING | NEW | 35,000,000 |
| 4 | Cannondale Quick 5 | APPROVED | USED_FAIR | 12,000,000 |

### Các bản ghi khác

| Entity | Mô tả |
|---|---|
| BikeImages | 5 ảnh cho bike 1, 2, 3, 4 |
| InspectionReports | 2 báo cáo (bike 1 và 2, status APPROVED) |
| Orders | orderId=1 (COMPLETED), orderId=2 (PENDING) |
| Reviews | 1 đánh giá 5 sao cho orderId=1 |
| Messages | 2 tin nhắn giữa buyer và seller về bike 2 |
| Wishlists | buyer lưu bike 2 |
| Reports | 1 báo cáo vi phạm bike 3 (PENDING) |

---

## Ghi chú cho Frontend

| Vấn đề | Chi tiết |
|---|---|
| **DateTime format** | Tất cả trả về UTC ISO 8601: `"2026-01-01T00:00:00Z"` |
| **Decimal** | `price`, `totalAmount`, `depositAmount` kiểu số thập phân, đơn vị VND |
| **Trường \*Name** | `sellerName`, `buyerName`... là read-only trong response, dùng để hiển thị |
| **imageUrls** | Mảng string URL trong `BikeResponseDto`, quản lý qua `/api/bikeimages` |
| **Authentication** | Hiện tại chưa có JWT – tất cả endpoint đều public |
| **Swagger** | Truy cập tại `/swagger` khi chạy ở môi trường Development |
| **Messages** | Không có `PUT` – tin nhắn không thể chỉnh sửa |
| **Wishlists** | Không có `PUT` – chỉ thêm hoặc xóa |
| **BikeImages** | Không có `PUT` – chỉ thêm hoặc xóa |
| **Status mặc định** | Bike=`PENDING`, Order=`PENDING`, InspectionReport=`PENDING`, Report=`PENDING`, User=`ACTIVE` |
