using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_CycleTrust.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAnonymous : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_anonymous",
                table: "bikes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "bikes",
                keyColumn: "bike_id",
                keyValue: 1,
                column: "is_anonymous",
                value: false);

            migrationBuilder.UpdateData(
                table: "bikes",
                keyColumn: "bike_id",
                keyValue: 2,
                column: "is_anonymous",
                value: false);

            migrationBuilder.UpdateData(
                table: "bikes",
                keyColumn: "bike_id",
                keyValue: 3,
                column: "is_anonymous",
                value: false);

            migrationBuilder.UpdateData(
                table: "bikes",
                keyColumn: "bike_id",
                keyValue: 4,
                column: "is_anonymous",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_anonymous",
                table: "bikes");
        }
    }
}
