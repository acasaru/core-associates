using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Interview.Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    Identifier = table.Column<string>(maxLength: 64, nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.InvoiceId);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ApiKey = table.Column<string>(maxLength: 1024, nullable: false),
                    Role = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    NoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    InvoiceId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Note_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserInfo",
                columns: new[] { "UserId", "ApiKey", "Role" },
                values: new object[,]
                {
                    { 1, "admin123", "Admin" },
                    { 2, "admin345", "Admin" },
                    { 3, "user123", "User" },
                    { 4, "user345", "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Identifier",
                table: "Invoice",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Note_InvoiceId",
                table: "Note",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_ApiKey",
                table: "UserInfo",
                column: "ApiKey",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "UserInfo");

            migrationBuilder.DropTable(
                name: "Invoice");
        }
    }
}
