using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.Migrations
{
    public partial class AddAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Attachments",
                table: "Attachments",
                newName: "AttachmentType");

            migrationBuilder.AddColumn<string>(
                name: "UserAttachment",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAttachment",
                table: "Attachments");

            migrationBuilder.RenameColumn(
                name: "AttachmentType",
                table: "Attachments",
                newName: "Attachments");
        }
    }
}
