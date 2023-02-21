using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClassManagement.Migrations
{
    public partial class SecondInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductId1 = table.Column<Guid>(nullable: true),
                    CourseName = table.Column<string>(maxLength: 100, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    UpiId = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: false),
                    ContactNo = table.Column<long>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_payment_addCart_ProductId1",
                        column: x => x.ProductId1,
                        principalTable: "addCart",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payment_ProductId1",
                table: "payment",
                column: "ProductId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payment");
        }
    }
}
