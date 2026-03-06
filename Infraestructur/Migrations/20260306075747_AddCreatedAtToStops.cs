using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructur.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToStops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "stops",
                type: "timestamp",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "stops");
        }
    }
}
