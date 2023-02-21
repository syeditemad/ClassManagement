using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClassManagement.Migrations
{
    public partial class thirdInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orderConfirmations",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<string>(nullable: true),
                    ProductId1 = table.Column<Guid>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    IdentityUserId = table.Column<string>(nullable: true),
                    PaymentMode = table.Column<int>(nullable: false),
                    ContactNo = table.Column<long>(nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EditedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderConfirmations", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_orderConfirmations_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orderConfirmations_addCart_ProductId1",
                        column: x => x.ProductId1,
                        principalTable: "addCart",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orderConfirmations_IdentityUserId",
                table: "orderConfirmations",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_orderConfirmations_ProductId1",
                table: "orderConfirmations",
                column: "ProductId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orderConfirmations");
        }
    }
}
