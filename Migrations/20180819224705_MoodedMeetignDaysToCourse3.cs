using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProjectSPC.Migrations
{
    public partial class MoodedMeetignDaysToCourse3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseDay");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Days",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Days_CourseId",
                table: "Days",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Days_Courses_CourseId",
                table: "Days",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Days_Courses_CourseId",
                table: "Days");

            migrationBuilder.DropIndex(
                name: "IX_Days_CourseId",
                table: "Days");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Days");

            migrationBuilder.CreateTable(
                name: "CourseDay",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<Guid>(nullable: true),
                    DayId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseDay_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseDay_Days_DayId",
                        column: x => x.DayId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseDay_CourseId",
                table: "CourseDay",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDay_DayId",
                table: "CourseDay",
                column: "DayId");
        }
    }
}
