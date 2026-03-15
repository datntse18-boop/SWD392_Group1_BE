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
                    { 1, "USED_LIKE_NEW", 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Road bike Giant Contend AR 1, light aluminum frame, Shimano 105 groupset.", "M", 25000000m, 3, "APPROVED", "Giant Contend AR 1 2025" },
                    { 2, "USED_GOOD", 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mountain bike Trek Marlin 7, RockShox fork, 27.5 inch.", "L", 18000000m, 3, "APPROVED", "Trek Marlin 7 2024" },
                    { 3, "NEW", 3, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Premium city bike, carbon frame, suitable for commuting and training.", "M", 35000000m, 3, "PENDING", "Specialized Sirrus X 4.0" },
                    { 4, "USED_FAIR", 4, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Light fitness bike, suitable for daily city travel.", "S", 12000000m, 3, "APPROVED", "Cannondale Quick 5" }
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
                    { 1, 1, "Disc brake works well.", "Shimano 105 drivetrain runs smoothly.", "Aluminum frame is still in good condition.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "APPROVED", 4, "Bike condition is very good and meets standards.", null },
                    { 2, 2, "Brake needs minor adjustment.", "Chain is slightly stretched and should be replaced soon.", "Frame has a few minor scratches.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "APPROVED", 4, "Bike is okay and needs light maintenance.", null }
                });

            migrationBuilder.InsertData(
                table: "messages",
                columns: new[] { "message_id", "bike_id", "content", "receiver_id", "sender_id", "sent_at" },
                values: new object[,]
                {
                    { 1, 2, "Hi, is the Trek Marlin 7 still available?", 3, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 2, "Hi, yes it is available. Would you like to view it in person?", 2, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
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
                values: new object[] { 1, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Listed price seems unreasonable for bike condition.", 2, "PENDING" });

            migrationBuilder.InsertData(
                table: "wishlist",
                columns: new[] { "wishlist_id", "bike_id", "buyer_id", "created_at" },
                values: new object[] { 1, 2, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "reviews",
                columns: new[] { "review_id", "buyer_id", "comment", "created_at", "order_id", "rating", "seller_id" },
                values: new object[] { 1, 2, "Bike is very nice and matches description. Fast delivery!", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 5, 3 });
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
