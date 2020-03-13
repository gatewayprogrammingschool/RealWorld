using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RealWorldWebAPI.Migrations
{
    public partial class Articles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArticleUID",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArticleUID = table.Column<Guid>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(nullable: true),
                    Favorited = table.Column<bool>(nullable: false),
                    FavoritesCount = table.Column<int>(nullable: false),
                    AuthorUserUID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleUID);
                    table.ForeignKey(
                        name: "FK_Articles_Users_AuthorUserUID",
                        column: x => x.AuthorUserUID,
                        principalTable: "Users",
                        principalColumn: "UserUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagUID = table.Column<Guid>(nullable: false),
                    ArticleUID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagUID);
                    table.ForeignKey(
                        name: "FK_Tag_Articles_ArticleUID",
                        column: x => x.ArticleUID,
                        principalTable: "Articles",
                        principalColumn: "ArticleUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleUID",
                table: "Comments",
                column: "ArticleUID");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorUserUID",
                table: "Articles",
                column: "AuthorUserUID");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ArticleUID",
                table: "Tag",
                column: "ArticleUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_ArticleUID",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ArticleUID",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ArticleUID",
                table: "Comments");
        }
    }
}
