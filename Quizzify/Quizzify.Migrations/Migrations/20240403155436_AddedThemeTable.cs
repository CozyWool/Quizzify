using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Quizzify.Client.Migrations
{
    /// <inheritdoc />
    public partial class AddedThemeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "questions_round_id_fk",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "question_theme",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "round_id",
                table: "Questions",
                newName: "theme_id");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_round_id",
                table: "Questions",
                newName: "IX_Questions_theme_id");

            migrationBuilder.CreateTable(
                name: "Themes",
                columns: table => new
                {
                    theme_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    round_id = table.Column<int>(type: "integer", nullable: false),
                    theme_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("themes_pkey", x => x.theme_id);
                    table.ForeignKey(
                        name: "themes_round_id_fk",
                        column: x => x.round_id,
                        principalTable: "Rounds",
                        principalColumn: "round_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Themes_round_id",
                table: "Themes",
                column: "round_id");

            migrationBuilder.AddForeignKey(
                name: "questions_theme_id_fk",
                table: "Questions",
                column: "theme_id",
                principalTable: "Themes",
                principalColumn: "theme_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "questions_theme_id_fk",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Themes");

            migrationBuilder.RenameColumn(
                name: "theme_id",
                table: "Questions",
                newName: "round_id");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_theme_id",
                table: "Questions",
                newName: "IX_Questions_round_id");

            migrationBuilder.AddColumn<string>(
                name: "question_theme",
                table: "Questions",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "questions_round_id_fk",
                table: "Questions",
                column: "round_id",
                principalTable: "Rounds",
                principalColumn: "round_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
