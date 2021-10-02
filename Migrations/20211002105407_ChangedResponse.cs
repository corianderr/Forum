using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum.Migrations
{
    public partial class ChangedResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThemeId",
                table: "Responses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Responses_ThemeId",
                table: "Responses",
                column: "ThemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Themes_ThemeId",
                table: "Responses",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Themes_ThemeId",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_ThemeId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "ThemeId",
                table: "Responses");
        }
    }
}
