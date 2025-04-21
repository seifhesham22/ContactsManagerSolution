using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactsManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecieveNewsLetter = table.Column<bool>(type: "bit", nullable: true),
                    TaxNumber = table.Column<string>(type: "varchar(8)", nullable: true, defaultValue: "ABC12345")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.Id);
                    table.CheckConstraint("CHK_TIN", "len([TaxNumber]) = 8");
                    table.ForeignKey(
                        name: "FK_persons_countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.InsertData(
                table: "countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("21f4c3b5-3ad7-4e56-9148-3b2b6484c8de"), "Canada" },
                    { new Guid("9c2aaf1b-6218-4ff7-a3c7-5e3d05aefdd0"), "Japan" },
                    { new Guid("b17e8c1a-54f3-4a4a-908f-1d938b2d8c47"), "United States" },
                    { new Guid("ebcf1d2e-2fcb-42c7-98e7-d5c49f8c6141"), "Australia" },
                    { new Guid("f6c5e2d8-99a4-4856-97f7-9b1eac378b53"), "Germany" }
                });

            migrationBuilder.InsertData(
                table: "persons",
                columns: new[] { "Id", "Address", "CountryId", "DateOfBirth", "Email", "Gender", "Name", "RecieveNewsLetter" },
                values: new object[,]
                {
                    { new Guid("32bcf50e-8e04-4c7a-8d2a-45e1d89cbb4d"), "456 Maple Ave, Toronto", new Guid("21f4c3b5-3ad7-4e56-9148-3b2b6484c8de"), new DateTime(1985, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), "jane.smith@example.com", "Female", "Jane Smith", false },
                    { new Guid("4a78d2bf-619e-4ea4-8e3f-4b6b5b97c2d3"), "45 Shibuya St, Tokyo", new Guid("9c2aaf1b-6218-4ff7-a3c7-5e3d05aefdd0"), new DateTime(1990, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "hiroshi.yamamoto@example.com", "Male", "Hiroshi Yamamoto", false },
                    { new Guid("5d19af83-7e8c-4cf2-9fb7-2a8f63e5d7a1"), "12 Oak St, Vancouver", new Guid("21f4c3b5-3ad7-4e56-9148-3b2b6484c8de"), new DateTime(1983, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), "sophia.williams@example.com", "Female", "Sophia Williams", true },
                    { new Guid("673d291a-46b2-4b79-bef5-973e1a75c8e4"), "67 Opera House St, Sydney", new Guid("ebcf1d2e-2fcb-42c7-98e7-d5c49f8c6141"), new DateTime(1995, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), "emily.white@example.com", "Female", "Emily White", true },
                    { new Guid("8d4e9d58-2f7a-4b4e-a637-07c6f652c7f9"), "789 Sunset Blvd, LA", new Guid("b17e8c1a-54f3-4a4a-908f-1d938b2d8c47"), new DateTime(1992, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "carlos.rivera@example.com", "Male", "Carlos Rivera", false },
                    { new Guid("9f12b3c4-5d87-4129-a6d8-38d3a3c9a52a"), "789 Beethoven Str, Berlin", new Guid("f6c5e2d8-99a4-4856-97f7-9b1eac378b53"), new DateTime(1993, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), "michael.brown@example.com", "Male", "Michael Brown", true },
                    { new Guid("a1d6bc59-3eaf-4b5d-9a2b-12f4345d1b67"), "123 Main St, NY", new Guid("b17e8c1a-54f3-4a4a-908f-1d938b2d8c47"), new DateTime(1990, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), "john.doe@example.com", "Male", "John Doe", true },
                    { new Guid("bc57a6de-4b39-4d3e-a7b9-5d58f9a37e4c"), "34 Mozart Ave, Munich", new Guid("f6c5e2d8-99a4-4856-97f7-9b1eac378b53"), new DateTime(1997, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "olivia.martens@example.com", "Female", "Olivia Martens", true },
                    { new Guid("ff36e1d3-7b4e-4f2b-91c5-ae9f32d672b5"), "23 Sakura St, Tokyo", new Guid("9c2aaf1b-6218-4ff7-a3c7-5e3d05aefdd0"), new DateTime(1988, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), "satoshi.tanaka@example.com", "Male", "Satoshi Tanaka", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_persons_CountryId",
                table: "persons",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "persons");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
