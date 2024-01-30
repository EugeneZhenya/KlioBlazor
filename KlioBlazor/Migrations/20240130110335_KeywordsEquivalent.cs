using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KlioBlazor.Migrations
{
    /// <inheritdoc />
    public partial class KeywordsEquivalent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Equivalent",
                table: "Keywords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Equivalent",
                table: "Keywords");
        }
    }
}
