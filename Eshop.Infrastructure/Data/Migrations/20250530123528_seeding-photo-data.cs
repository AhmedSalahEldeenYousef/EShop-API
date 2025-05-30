using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedingphotodata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Photos",
                columns: new[] { "Id", "ImageName", "ProductId" },
                values: new object[] { 1, "laptop photo", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
