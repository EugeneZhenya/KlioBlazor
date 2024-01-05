using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KlioBlazor.Migrations
{
    /// <inheritdoc />
    public partial class HasPicturePersonAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasPicture",
                table: "People",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasPicture",
                table: "People");
        }
    }
}
