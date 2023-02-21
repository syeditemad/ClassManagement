using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClassManagement.Migrations
{
    public partial class FirstInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "checkList",
                columns: table => new
                {
                    Checkid = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    identityUserId = table.Column<string>(nullable: true),
                    CourseId = table.Column<int>(nullable: false),
                    CourseListCourseId = table.Column<int>(nullable: true),
                    TotalAmount = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    CreatOn = table.Column<DateTime>(nullable: false),
                    IsConfirm = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkList", x => x.Checkid);
                    table.ForeignKey(
                        name: "FK_checkList_CourseLists_CourseListCourseId",
                        column: x => x.CourseListCourseId,
                        principalTable: "CourseLists",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkList_AspNetUsers_identityUserId",
                        column: x => x.identityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_checkList_CourseListCourseId",
                table: "checkList",
                column: "CourseListCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_checkList_identityUserId",
                table: "checkList",
                column: "identityUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "checkList");
        }
    }
}
