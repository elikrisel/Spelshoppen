using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spelshoppen.Migrations
{
    /// <inheritdoc />
    public partial class InitialAzureDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Spelshoppen",
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Spelshoppen",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Spelshoppen",
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Sverige" });

            migrationBuilder.InsertData(
                schema: "Spelshoppen",
                table: "PaymentMethods",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Faktura" });
        }
    }
}
