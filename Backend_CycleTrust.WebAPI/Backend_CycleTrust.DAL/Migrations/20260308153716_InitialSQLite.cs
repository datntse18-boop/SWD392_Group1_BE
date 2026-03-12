using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend_CycleTrust.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialSQLite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    brand_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    brand_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.brand_id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    category_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    role_name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    full_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    role_id = table.Column<int>(type: "INTEGER", nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bikes",
                columns: table => new
                {
                    bike_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    seller_id = table.Column<int>(type: "INTEGER", nullable: false),
                    title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    price = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    brand_id = table.Column<int>(type: "INTEGER", nullable: true),
                    category_id = table.Column<int>(type: "INTEGER", nullable: true),
                    frame_size = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    bike_condition = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bikes", x => x.bike_id);
                    table.ForeignKey(
                        name: "FK_bikes_brands_brand_id",
                        column: x => x.brand_id,
                        principalTable: "brands",
                        principalColumn: "brand_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_bikes_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_bikes_users_seller_id",
                        column: x => x.seller_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bike_images",
                columns: table => new
                {
                    image_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    bike_id = table.Column<int>(type: "INTEGER", nullable: false),
                    image_url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bike_images", x => x.image_id);
                    table.ForeignKey(
                        name: "FK_bike_images_bikes_bike_id",
                        column: x => x.bike_id,
                        principalTable: "bikes",
                        principalColumn: "bike_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inspection_reports",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    bike_id = table.Column<int>(type: "INTEGER", nullable: false),
                    inspector_id = table.Column<int>(type: "INTEGER", nullable: false),
                    frame_condition = table.Column<string>(type: "TEXT", nullable: true),
                    brake_condition = table.Column<string>(type: "TEXT", nullable: true),
                    drivetrain_condition = table.Column<string>(type: "TEXT", nullable: true),
                    overall_comment = table.Column<string>(type: "TEXT", nullable: true),
                    report_file = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    inspection_status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    inspected_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inspection_reports", x => x.report_id);
                    table.ForeignKey(
                        name: "FK_inspection_reports_bikes_bike_id",
                        column: x => x.bike_id,
                        principalTable: "bikes",
                        principalColumn: "bike_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inspection_reports_users_inspector_id",
                        column: x => x.inspector_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    message_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sender_id = table.Column<int>(type: "INTEGER", nullable: false),
                    receiver_id = table.Column<int>(type: "INTEGER", nullable: false),
                    bike_id = table.Column<int>(type: "INTEGER", nullable: true),
                    content = table.Column<string>(type: "TEXT", nullable: false),
                    sent_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.message_id);
                    table.ForeignKey(
                        name: "FK_messages_bikes_bike_id",
                        column: x => x.bike_id,
                        principalTable: "bikes",
                        principalColumn: "bike_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_messages_users_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_messages_users_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    bike_id = table.Column<int>(type: "INTEGER", nullable: false),
                    buyer_id = table.Column<int>(type: "INTEGER", nullable: false),
                    seller_id = table.Column<int>(type: "INTEGER", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    deposit_amount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_orders_bikes_bike_id",
                        column: x => x.bike_id,
                        principalTable: "bikes",
                        principalColumn: "bike_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_users_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_users_seller_id",
                        column: x => x.seller_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    reporter_id = table.Column<int>(type: "INTEGER", nullable: false),
                    bike_id = table.Column<int>(type: "INTEGER", nullable: true),
                    reason = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reports", x => x.report_id);
                    table.ForeignKey(
                        name: "FK_reports_bikes_bike_id",
                        column: x => x.bike_id,
                        principalTable: "bikes",
                        principalColumn: "bike_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_reports_users_reporter_id",
                        column: x => x.reporter_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "wishlist",
                columns: table => new
                {
                    wishlist_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    buyer_id = table.Column<int>(type: "INTEGER", nullable: false),
                    bike_id = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wishlist", x => x.wishlist_id);
                    table.ForeignKey(
                        name: "FK_wishlist_bikes_bike_id",
                        column: x => x.bike_id,
                        principalTable: "bikes",
                        principalColumn: "bike_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_wishlist_users_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    review_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    order_id = table.Column<int>(type: "INTEGER", nullable: false),
                    buyer_id = table.Column<int>(type: "INTEGER", nullable: false),
                    seller_id = table.Column<int>(type: "INTEGER", nullable: false),
                    rating = table.Column<int>(type: "INTEGER", nullable: true),
                    comment = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_reviews_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reviews_users_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reviews_users_seller_id",
                        column: x => x.seller_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                table: "roles",
                columns: new[] { "role_id", "role_name" },
                values: new object[,]
                {
                    { 1, "ADMIN" },
                    { 2, "BUYER" },
                    { 3, "SELLER" },
                    { 4, "INSPECTOR" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "user_id", "address", "created_at", "email", "full_name", "password", "phone", "role_id", "status" },
                values: new object[,]
                {
                    { 1, "Ho Chi Minh City", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@cycletrust.com", "Admin CycleTrust", "$2a$11$VhQo3xikBLmFMVGM3U0kYOHGlhwtAPrwjjZV4WJmkjmoqm8SuC4Pu", "0901000001", 1, "ACTIVE" },
                    { 2, "Ha Noi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "buyer@cycletrust.com", "Nguyen Van Buyer", "$2a$11$Gtt/lWpnPzDa/VVmR/fYm.rl9lzFFlpuhxY7ExHZd1huXudhQMHqm", "0901000002", 2, "ACTIVE" },
                    { 3, "Da Nang", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seller@cycletrust.com", "Tran Thi Seller", "$2a$11$tBkwBXLD4x16.MkVgIvUjuuWGUQNgUhyulVRNRc8Ps2.Rb4A6Qdsm", "0901000003", 3, "ACTIVE" },
                    { 4, "Ho Chi Minh City", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "inspector@cycletrust.com", "Le Van Inspector", "$2a$11$QPwNKuIisbgosdAq.f.xdeEncp0Wpq4mGP2VQCkmcSyEZVg8GO/ba", "0901000004", 4, "ACTIVE" }
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

            migrationBuilder.CreateIndex(
                name: "IX_bike_images_bike_id",
                table: "bike_images",
                column: "bike_id");

            migrationBuilder.CreateIndex(
                name: "IX_bikes_brand_id",
                table: "bikes",
                column: "brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_bikes_category_id",
                table: "bikes",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_bikes_seller_id",
                table: "bikes",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_brands_brand_name",
                table: "brands",
                column: "brand_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_category_name",
                table: "categories",
                column: "category_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inspection_reports_bike_id",
                table: "inspection_reports",
                column: "bike_id");

            migrationBuilder.CreateIndex(
                name: "IX_inspection_reports_inspector_id",
                table: "inspection_reports",
                column: "inspector_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_bike_id",
                table: "messages",
                column: "bike_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_receiver_id",
                table: "messages",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_sender_id",
                table: "messages",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_bike_id",
                table: "orders",
                column: "bike_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_buyer_id",
                table: "orders",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_seller_id",
                table: "orders",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_reports_bike_id",
                table: "reports",
                column: "bike_id");

            migrationBuilder.CreateIndex(
                name: "IX_reports_reporter_id",
                table: "reports",
                column: "reporter_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_buyer_id",
                table: "reviews",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_order_id",
                table: "reviews",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_seller_id",
                table: "reviews",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_role_name",
                table: "roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_wishlist_bike_id",
                table: "wishlist",
                column: "bike_id");

            migrationBuilder.CreateIndex(
                name: "IX_wishlist_buyer_id_bike_id",
                table: "wishlist",
                columns: new[] { "buyer_id", "bike_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bike_images");

            migrationBuilder.DropTable(
                name: "inspection_reports");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "reports");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "wishlist");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "bikes");

            migrationBuilder.DropTable(
                name: "brands");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
