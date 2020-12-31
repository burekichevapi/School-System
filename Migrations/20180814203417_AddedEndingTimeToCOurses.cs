using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProjectSPC.Migrations
{
    public partial class AddedEndingTimeToCOurses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndingTime",
                table: "Courses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndingTime",
                table: "Courses");
        }
    }
}
