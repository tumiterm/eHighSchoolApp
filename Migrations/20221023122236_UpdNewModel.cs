using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.Migrations
{
    public partial class UpdNewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Quiz_QuizId",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_QuizId",
                table: "Responses");

            migrationBuilder.RenameColumn(
                name: "QuizId",
                table: "Responses",
                newName: "AssociativeKey");

            migrationBuilder.AddColumn<Guid>(
                name: "ResponseId",
                table: "Quiz",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_ResponseId",
                table: "Quiz",
                column: "ResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Responses_ResponseId",
                table: "Quiz",
                column: "ResponseId",
                principalTable: "Responses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Responses_ResponseId",
                table: "Quiz");

            migrationBuilder.DropIndex(
                name: "IX_Quiz_ResponseId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "ResponseId",
                table: "Quiz");

            migrationBuilder.RenameColumn(
                name: "AssociativeKey",
                table: "Responses",
                newName: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_QuizId",
                table: "Responses",
                column: "QuizId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Quiz_QuizId",
                table: "Responses",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "QuizId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
