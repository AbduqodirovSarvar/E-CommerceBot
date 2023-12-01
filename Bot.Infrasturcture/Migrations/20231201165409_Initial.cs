using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bot.Infrasturcture.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameEN = table.Column<string>(type: "text", nullable: false),
                    NameUZ = table.Column<string>(type: "text", nullable: false),
                    NameRU = table.Column<string>(type: "text", nullable: false),
                    DescriptionUZ = table.Column<string>(type: "text", nullable: false),
                    DescriptionRU = table.Column<string>(type: "text", nullable: false),
                    DescriptionEN = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameUZ = table.Column<string>(type: "text", nullable: false),
                    NameEN = table.Column<string>(type: "text", nullable: false),
                    NameRU = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<int>(type: "integer", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameUZ = table.Column<string>(type: "text", nullable: false),
                    NameEN = table.Column<string>(type: "text", nullable: false),
                    NameRU = table.Column<string>(type: "text", nullable: false),
                    DescriptionUZ = table.Column<string>(type: "text", nullable: true),
                    DescriptionEN = table.Column<string>(type: "text", nullable: true),
                    DescriptionRU = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    TypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_NameEN",
                table: "Products",
                column: "NameEN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_NameRU",
                table: "Products",
                column: "NameRU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_NameUZ",
                table: "Products",
                column: "NameUZ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_TypeId",
                table: "Products",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_NameEN",
                table: "ProductTypes",
                column: "NameEN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_NameRU",
                table: "ProductTypes",
                column: "NameRU",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_NameUZ",
                table: "ProductTypes",
                column: "NameUZ",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filials");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProductTypes");
        }
    }
}
