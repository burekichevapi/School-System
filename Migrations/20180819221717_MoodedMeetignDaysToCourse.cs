using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProjectSPC.Migrations
{
    public partial class MoodedMeetignDaysToCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
