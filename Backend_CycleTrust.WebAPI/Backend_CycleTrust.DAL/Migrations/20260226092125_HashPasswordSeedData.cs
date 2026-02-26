using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend_CycleTrust.DAL.Migrations
{
    /// <inheritdoc />
    public partial class HashPasswordSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "password",
                value: "$2a$11$VhQo3xikBLmFMVGM3U0kYOHGlhwtAPrwjjZV4WJmkjmoqm8SuC4Pu");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                column: "password",
                value: "$2a$11$Gtt/lWpnPzDa/VVmR/fYm.rl9lzFFlpuhxY7ExHZd1huXudhQMHqm");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                column: "password",
                value: "$2a$11$tBkwBXLD4x16.MkVgIvUjuuWGUQNgUhyulVRNRc8Ps2.Rb4A6Qdsm");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 4,
                column: "password",
                value: "$2a$11$QPwNKuIisbgosdAq.f.xdeEncp0Wpq4mGP2VQCkmcSyEZVg8GO/ba");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "password",
                value: "admin123");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                column: "password",
                value: "buyer123");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                column: "password",
                value: "seller123");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 4,
                column: "password",
                value: "inspector123");
        }
    }
}
