using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Library.Entities.Migrations
{
    public partial class AddLibraryDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Isbn = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Title = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    PagesNum = table.Column<int>(nullable: false),
                    PubYear = table.Column<int>(nullable: false),
                    PubName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Isbn);
                });

            migrationBuilder.CreateTable(
                name: "BookCat",
                columns: table => new
                {
                    Isbn = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCat", x => new { x.Isbn, x.CategoryName });
                });

            migrationBuilder.CreateTable(
                name: "Borrowing",
                columns: table => new
                {
                    ReaderNr = table.Column<int>(nullable: false),
                    Isbn = table.Column<int>(nullable: false),
                    CopyNumber = table.Column<int>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowing", x => new { x.ReaderNr, x.Isbn, x.CopyNumber });
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryName = table.Column<string>(nullable: false),
                    ParentCatCategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryName);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCatCategoryName",
                        column: x => x.ParentCatCategoryName,
                        principalTable: "Category",
                        principalColumn: "CategoryName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Copy",
                columns: table => new
                {
                    Isbn = table.Column<int>(nullable: false),
                    CopyNumber = table.Column<int>(nullable: false),
                    ShelfPosition = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Copy", x => new { x.Isbn, x.CopyNumber });
                });

            migrationBuilder.CreateTable(
                name: "Publisher",
                columns: table => new
                {
                    PubName = table.Column<string>(nullable: false),
                    PubAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publisher", x => x.PubName);
                });

            migrationBuilder.CreateTable(
                name: "Reader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LastName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reader", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCatCategoryName",
                table: "Category",
                column: "ParentCatCategoryName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "BookCat");

            migrationBuilder.DropTable(
                name: "Borrowing");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Copy");

            migrationBuilder.DropTable(
                name: "Publisher");

            migrationBuilder.DropTable(
                name: "Reader");
        }
    }
}
