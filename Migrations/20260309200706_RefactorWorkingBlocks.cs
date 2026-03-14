using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonBooking.Api.Migrations
{
    /// <inheritdoc />
    public partial class RefactorWorkingBlocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "block_number",
                schema: "public",
                table: "appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "block_number",
                schema: "public",
                table: "appointments");
        }
    }
}
