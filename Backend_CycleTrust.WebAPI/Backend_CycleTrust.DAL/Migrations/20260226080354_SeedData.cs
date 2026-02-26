using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend_CycleTrust.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "brands",
                columns: new[] { "brand_id", "brand_name" },
                values: new object[,]
                {
                    { 1, "Giant" },
                    { 2, "Trek" },
                    { 3, "Specialized" },
                    { 4, "Cannondale" },
                    { 5, "Merida" }
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "category_id", "category_name" },
                values: new object[,]
                {
                    { 1, "Road Bike" },
                    { 2, "Mountain Bike" },
                    { 3, "City Bike" },
                    { 4, "Folding Bike" },
                    { 5, "Electric Bike" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "address", "created_at", "email", "full_name", "password", "phone", "role_id", "status" },
                values: new object[,]
                {
                    { 1, "Ho Chi Minh City", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@cycletrust.com", "Admin CycleTrust", "admin123", "0901000001", 1, "ACTIVE" },
                    { 2, "Ha Noi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "buyer@cycletrust.com", "Nguyen Van Buyer", "buyer123", "0901000002", 2, "ACTIVE" },
                    { 3, "Da Nang", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seller@cycletrust.com", "Tran Thi Seller", "seller123", "0901000003", 3, "ACTIVE" },
                    { 4, "Ho Chi Minh City", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "inspector@cycletrust.com", "Le Van Inspector", "inspector123", "0901000004", 4, "ACTIVE" }
                });

            migrationBuilder.InsertData(
                table: "bikes",
                columns: new[] { "bike_id", "bike_condition", "brand_id", "category_id", "created_at", "description", "frame_size", "price", "seller_id", "status", "title" },
                values: new object[,]
                {
                    { 1, "USED_LIKE_NEW", 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Xe đạp đường trường Giant Contend AR 1, khung nhôm nhẹ, groupset Shimano 105.", "M", 25000000m, 3, "APPROVED", "Giant Contend AR 1 2025" },
                    { 2, "USED_GOOD", 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Xe đạp leo núi Trek Marlin 7, phuộc RockShox, 27.5 inch.", "L", 18000000m, 3, "APPROVED", "Trek Marlin 7 2024" },
                    { 3, "NEW", 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Xe đạp thành phố cao cấp, khung carbon, phù hợp đi làm và tập luyện.", "M", 35000000m, 3, "PENDING", "Specialized Sirrus X 4.0" },
                    { 4, "USED_FAIR", 4, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Xe đạp fitness nhẹ, phù hợp di chuyển hàng ngày trong thành phố.", "S", 12000000m, 3, "APPROVED", "Cannondale Quick 5" }
                });

            migrationBuilder.InsertData(
                table: "bike_images",
                columns: new[] { "image_id", "bike_id", "image_url" },
                values: new object[,]
                {
                    { 1, 1, "https://via.placeholder.com/600x400?text=Giant+Contend+AR+1" },
                    { 2, 1, "https://via.placeholder.com/600x400?text=Giant+Contend+Side" },
                    { 3, 2, "https://via.placeholder.com/600x400?text=Trek+Marlin+7" },
                    { 4, 3, "https://via.placeholder.com/600x400?text=Specialized+Sirrus" },
                    { 5, 4, "https://via.placeholder.com/600x400?text=Cannondale+Quick+5" }
                });

            migrationBuilder.InsertData(
                table: "inspection_reports",
                columns: new[] { "report_id", "bike_id", "brake_condition", "drivetrain_condition", "frame_condition", "inspected_at", "inspection_status", "inspector_id", "overall_comment", "report_file" },
                values: new object[,]
                {
                    { 1, 1, "Phanh đĩa hoạt động tốt.", "Hệ thống truyền động Shimano 105 mượt mà.", "Khung nhôm còn mới, không trầy xước.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "APPROVED", 4, "Xe trong tình trạng rất tốt, đạt tiêu chuẩn.", null },
                    { 2, 2, "Phanh cần điều chỉnh lại.", "Xích hơi giãn, cần thay sớm.", "Khung nhôm có vài vết trầy nhỏ.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "APPROVED", 4, "Xe ổn, cần bảo dưỡng nhẹ.", null }
                });

            migrationBuilder.InsertData(
                table: "messages",
                columns: new[] { "message_id", "bike_id", "content", "receiver_id", "sender_id", "sent_at" },
                values: new object[,]
                {
                    { 1, 2, "Chào bạn, xe Trek Marlin 7 còn không ạ?", 3, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 2, "Chào bạn, xe vẫn còn nhé. Bạn muốn xem trực tiếp không?", 2, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "order_id", "bike_id", "buyer_id", "created_at", "deposit_amount", "seller_id", "status", "total_amount" },
                values: new object[,]
                {
                    { 1, 1, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5000000m, 3, "COMPLETED", 25000000m },
                    { 2, 4, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2000000m, 3, "PENDING", 12000000m }
                });

            migrationBuilder.InsertData(
                table: "reports",
                columns: new[] { "report_id", "bike_id", "created_at", "reason", "reporter_id", "status" },
                values: new object[] { 1, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Giá niêm yết không hợp lý so với tình trạng xe.", 2, "PENDING" });

            migrationBuilder.InsertData(
                table: "wishlist",
                columns: new[] { "wishlist_id", "bike_id", "buyer_id", "created_at" },
                values: new object[] { 1, 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "reviews",
                columns: new[] { "review_id", "buyer_id", "comment", "created_at", "order_id", "rating", "seller_id" },
                values: new object[] { 1, 2, "Xe rất đẹp, đúng mô tả. Seller giao hàng nhanh!", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 5, 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "bike_images",
                keyColumn: "image_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "bike_images",
                keyColumn: "image_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "bike_images",
                keyColumn: "image_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "bike_images",
                keyColumn: "image_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "bike_images",
                keyColumn: "image_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "brands",
                keyColumn: "brand_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "category_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "category_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "inspection_reports",
                keyColumn: "report_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "inspection_reports",
                keyColumn: "report_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "messages",
                keyColumn: "message_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "messages",
                keyColumn: "message_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "order_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "reports",
                keyColumn: "report_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "reviews",
                keyColumn: "review_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "wishlist",
                keyColumn: "wishlist_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "bikes",
                keyColumn: "bike_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "bikes",
                keyColumn: "bike_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "bikes",
                keyColumn: "bike_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "order_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "bikes",
                keyColumn: "bike_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "brands",
                keyColumn: "brand_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "brands",
                keyColumn: "brand_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "brands",
                keyColumn: "brand_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "category_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "category_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "brands",
                keyColumn: "brand_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "categories",
                keyColumn: "category_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3);
        }
    }
}
