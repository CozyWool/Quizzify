﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Quizzify.Client.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    package_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    package_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE"),
                    difficulty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("packages_pkey", x => x.package_id);
                });

            migrationBuilder.CreateTable(
                name: "SecretQuestions",
                columns: table => new
                {
                    secret_q_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("secretquestions_pkey", x => x.secret_q_id);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    round_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    package_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    round_type = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("rounds_pkey", x => x.round_id);
                    table.ForeignKey(
                        name: "rounds_package_id_fk",
                        column: x => x.package_id,
                        principalTable: "Packages",
                        principalColumn: "package_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    login = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    selected_secret_question_id = table.Column<int>(type: "integer", nullable: true),
                    secret_answer_hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    twofa_auth_method = table.Column<string>(type: "text", nullable: true),
                    google_authorization = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "secret_question_fk",
                        column: x => x.selected_secret_question_id,
                        principalTable: "SecretQuestions",
                        principalColumn: "secret_q_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    round_id = table.Column<int>(type: "integer", nullable: false),
                    question_text = table.Column<string>(type: "text", nullable: true),
                    question_theme = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    question_image_url = table.Column<byte[]>(type: "bytea", nullable: true),
                    question_cost = table.Column<int>(type: "integer", nullable: false),
                    question_comment = table.Column<string>(type: "text", nullable: true),
                    answer_text = table.Column<string>(type: "text", nullable: true),
                    answer_image_url = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("questions_pkey", x => x.question_id);
                    table.ForeignKey(
                        name: "questions_round_id_fk",
                        column: x => x.round_id,
                        principalTable: "Rounds",
                        principalColumn: "round_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    player_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nickname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    user_profile_picture = table.Column<byte[]>(type: "bytea", nullable: true),
                    about = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("players_pkey", x => x.player_id);
                    table.ForeignKey(
                        name: "user_id_fk",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "players_user_id_key",
                table: "Players",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_round_id",
                table: "Questions",
                column: "round_id");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_package_id",
                table: "Rounds",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_selected_secret_question_id",
                table: "Users",
                column: "selected_secret_question_id");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_login_key",
                table: "Users",
                column: "login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "SecretQuestions");

            migrationBuilder.DropTable(
                name: "Packages");
        }
    }
}
