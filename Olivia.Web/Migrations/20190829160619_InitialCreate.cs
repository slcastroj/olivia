using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Olivia.Web.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sheeminc_olivia");

            migrationBuilder.CreateTable(
                name: "user",
                schema: "sheeminc_olivia",
                columns: table => new
                {
                    username = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    password = table.Column<string>(unicode: false, nullable: false),
                    email = table.Column<string>(unicode: false, nullable: false),
                    email_confirmed = table.Column<byte>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "post",
                schema: "sheeminc_olivia",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    content = table.Column<string>(unicode: false, nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    username = table.Column<string>(unicode: false, maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.id);
                    table.ForeignKey(
                        name: "fk_post_user",
                        column: x => x.username,
                        principalSchema: "sheeminc_olivia",
                        principalTable: "user",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "fk_post_user",
                schema: "sheeminc_olivia",
                table: "post",
                column: "username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "post",
                schema: "sheeminc_olivia");

            migrationBuilder.DropTable(
                name: "user",
                schema: "sheeminc_olivia");
        }
    }
}
