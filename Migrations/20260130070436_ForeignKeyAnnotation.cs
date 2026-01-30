using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spelshoppen.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyAnnotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodsId",
                schema: "Spelshoppen",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentMethodsId",
                schema: "Spelshoppen",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethodsId",
                schema: "Spelshoppen",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentId",
                schema: "Spelshoppen",
                table: "Orders",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentId",
                schema: "Spelshoppen",
                table: "Orders",
                column: "PaymentId",
                principalSchema: "Spelshoppen",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentId",
                schema: "Spelshoppen",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentId",
                schema: "Spelshoppen",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodsId",
                schema: "Spelshoppen",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentMethodsId",
                schema: "Spelshoppen",
                table: "Orders",
                column: "PaymentMethodsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodsId",
                schema: "Spelshoppen",
                table: "Orders",
                column: "PaymentMethodsId",
                principalSchema: "Spelshoppen",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
