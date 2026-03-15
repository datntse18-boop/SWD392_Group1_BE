using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_CycleTrust.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingSellerUpgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "pending_seller_upgrade",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 1,
                column: "pending_seller_upgrade",
                value: false);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 2,
                column: "pending_seller_upgrade",
                value: false);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 3,
                column: "pending_seller_upgrade",
                value: false);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "user_id",
                keyValue: 4,
                column: "pending_seller_upgrade",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pending_seller_upgrade",
                table: "users");
        }
    }
}
