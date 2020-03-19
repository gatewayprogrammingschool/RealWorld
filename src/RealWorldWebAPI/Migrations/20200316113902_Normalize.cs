using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RealWorldWebAPI.Migrations
{
    public partial class Normalize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ArticleTag");
            migrationBuilder.DropTable("Favorites");
            migrationBuilder.DropTable("FavoriteAuthor");
            migrationBuilder.DropTable("Tags");
            migrationBuilder.DropTable("Comments");
            migrationBuilder.DropTable("Users");
            migrationBuilder.DropTable("Articles");

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagUid = table.Column<Guid>(nullable: false),
                    TagName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagUid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserUid = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserUid);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArticleUid = table.Column<Guid>(nullable: false),
                    Slug = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    AuthorUserUid = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleUid);
                    table.ForeignKey(
                        name: "FK_Articles_Users_AuthorUserUid",
                        column: x => x.AuthorUserUid,
                        principalTable: "Users",
                        principalColumn: "UserUid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteAuthor",
                columns: table => new
                {
                    FavoriteAuthorUid = table.Column<Guid>(nullable: false),
                    AuthorUserUid = table.Column<Guid>(nullable: false),
                    FanUserUid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteAuthor", x => x.FavoriteAuthorUid);
                    table.ForeignKey(
                        name: "FK_FavoriteAuthor_Users_AuthorUserUid",
                        column: x => x.AuthorUserUid,
                        principalTable: "Users",
                        principalColumn: "UserUid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteAuthor_Users_FanUserUid",
                        column: x => x.FanUserUid,
                        principalTable: "Users",
                        principalColumn: "UserUid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                columns: table => new
                {
                    ArticleTagUid = table.Column<Guid>(nullable: false),
                    ArticleUid = table.Column<Guid>(nullable: false),
                    TagUid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => x.ArticleTagUid);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Articles_ArticleUid",
                        column: x => x.ArticleUid,
                        principalTable: "Articles",
                        principalColumn: "ArticleUid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Tags_TagUid",
                        column: x => x.TagUid,
                        principalTable: "Tags",
                        principalColumn: "TagUid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    AuthorUserUid = table.Column<Guid>(nullable: false),
                    ArticleUid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleUid",
                        column: x => x.ArticleUid,
                        principalTable: "Articles",
                        principalColumn: "ArticleUid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Users_AuthorUserUid",
                        column: x => x.AuthorUserUid,
                        principalTable: "Users",
                        principalColumn: "UserUid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    ArticleFavoriteUid = table.Column<Guid>(nullable: false),
                    ArticleUid = table.Column<Guid>(nullable: false),
                    UserUid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.ArticleFavoriteUid);
                    table.ForeignKey(
                        name: "FK_Favorites_Articles_ArticleUid",
                        column: x => x.ArticleUid,
                        principalTable: "Articles",
                        principalColumn: "ArticleUid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Favorites_Users_UserUid",
                        column: x => x.UserUid,
                        principalTable: "Users",
                        principalColumn: "UserUid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorUserUid",
                table: "Articles",
                column: "AuthorUserUid");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_ArticleUid",
                table: "ArticleTag",
                column: "ArticleUid");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_TagUid",
                table: "ArticleTag",
                column: "TagUid");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleUid",
                table: "Comments",
                column: "ArticleUid");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorUserUid",
                table: "Comments",
                column: "AuthorUserUid");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteAuthor_AuthorUserUid",
                table: "FavoriteAuthor",
                column: "AuthorUserUid");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteAuthor_FanUserUid",
                table: "FavoriteAuthor",
                column: "FanUserUid");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_ArticleUid",
                table: "Favorites",
                column: "ArticleUid");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserUid",
                table: "Favorites",
                column: "UserUid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTag");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "FavoriteAuthor");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
