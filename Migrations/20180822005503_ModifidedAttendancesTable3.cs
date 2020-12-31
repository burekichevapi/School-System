using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProjectSPC.Migrations
{
    public partial class ModifidedAttendancesTable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_Attendances_AttendanceId",
                table: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_AttendanceId",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "AttendanceId",
                table: "Instructors");

            migrationBuilder.AddColumn<Guid>(
                name: "InstructorId",
                table: "Attendances",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_InstructorId",
                table: "Attendances",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Instructors_InstructorId",
                table: "Attendances",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Instructors_InstructorId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_InstructorId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Attendances");

            migrationBuilder.AddColumn<Guid>(
                name: "AttendanceId",
                table: "Instructors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_AttendanceId",
                table: "Instructors",
                column: "AttendanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_Attendances_AttendanceId",
                table: "Instructors",
                column: "AttendanceId",
                principalTable: "Attendances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
