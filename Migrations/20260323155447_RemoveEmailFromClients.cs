using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalonBooking.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailFromClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                schema: "public",
                table: "clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "public",
                table: "clients",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
