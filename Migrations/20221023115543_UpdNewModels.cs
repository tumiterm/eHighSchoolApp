using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.Migrations
{
    public partial class UpdNewModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResponse_Quiz_QuizId",
                table: "QuizResponse");

            migrationBuilder.DropTable(
                name: "QuizResponseViewModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuizResponse",
                table: "QuizResponse");

            migrationBuilder.RenameTable(
                name: "QuizResponse",
                newName: "Responses");

            migrationBuilder.RenameIndex(
                name: "IX_QuizResponse_QuizId",
                table: "Responses",
                newName: "IX_Responses_QuizId");

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "Responses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Responses",
                table: "Responses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Quiz_QuizId",
                table: "Responses",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Quiz_QuizId",
                table: "Responses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Responses",
                table: "Responses");

            migrationBuilder.RenameTable(
                name: "Responses",
                newName: "QuizResponse");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_QuizId",
                table: "QuizResponse",
                newName: "IX_QuizResponse_QuizId");

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "QuizResponse",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuizResponse",
                table: "QuizResponse",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QuizResponseViewModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizResponseViewModel", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResponse_Quiz_QuizId",
                table: "QuizResponse",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
