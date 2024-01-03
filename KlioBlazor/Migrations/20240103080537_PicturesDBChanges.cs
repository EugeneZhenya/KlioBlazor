using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KlioBlazor.Migrations
{
    /// <inheritdoc />
    public partial class PicturesDBChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "People");

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "People",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "IndependenceDay",
                table: "Countries",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndependenceDay",
                table: "Countries");

            migrationBuilder.AlterColumn<string>(
                name: "Biography",
                table: "People",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "People",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
