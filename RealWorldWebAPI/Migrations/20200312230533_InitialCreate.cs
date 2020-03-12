using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RealWorldWebAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileUID = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Following = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileUID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserUID = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserUID);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    AuthorUserUID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Users_AuthorUserUID",
                        column: x => x.AuthorUserUID,
                        principalTable: "Users",
                        principalColumn: "UserUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorUserUID",
                table: "Comments",
                column: "AuthorUserUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
