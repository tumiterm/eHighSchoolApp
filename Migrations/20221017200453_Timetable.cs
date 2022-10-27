using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.Migrations
{
    public partial class Timetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "EventTimeTable");

            migrationBuilder.DropColumn(
                name: "IsFullDay",
                table: "EventTimeTable");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "EventTimeTable");

            migrationBuilder.RenameColumn(
                name: "ThemeColor",
                table: "EventTimeTable",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "EventTimeTable",
                newName: "Attachment");

            migrationBuilder.AlterColumn<int>(
                name: "Subject",
                table: "EventTimeTable",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "EventTimeTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "EventTimeTable");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "EventTimeTable",
                newName: "ThemeColor");

            migrationBuilder.RenameColumn(
                name: "Attachment",
                table: "EventTimeTable",
                newName: "Description");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "EventTimeTable",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "EventTimeTable",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsFullDay",
                table: "EventTimeTable",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "EventTimeTable",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
