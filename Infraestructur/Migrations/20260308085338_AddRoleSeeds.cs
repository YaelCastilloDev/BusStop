using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestructur.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name", "normalized_name" },
                values: new object[,]
                {
                    { new Guid("0195758d-7b2a-7c9e-9f4a-1a2b3c4d5e6f"), "SuperAdmin", "SUPERADMIN" },
                    { new Guid("0195758d-7b2a-7c9e-9f4b-2b3c4d5e6f7a"), "Admin", "ADMIN" },
                    { new Guid("0195758d-7b2a-7c9e-9f4c-3c4d5e6f7a8b"), "Moderator", "MODERATOR" },
                    { new Guid("0195758d-7b2a-7c9e-9f4d-4d5e6f7a8b9c"), "BasicUser", "BASICUSER" }
                });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_refresh_tokens_users",
                table: "refresh_tokens");

            migrationBuilder.DropForeignKey(
                name: "fk_user_identities_users1",
                table: "user_identities");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("0195758d-7b2a-7c9e-9f4a-1a2b3c4d5e6f"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("0195758d-7b2a-7c9e-9f4b-2b3c4d5e6f7a"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("0195758d-7b2a-7c9e-9f4c-3c4d5e6f7a8b"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: new Guid("0195758d-7b2a-7c9e-9f4d-4d5e6f7a8b9c"));

            migrationBuilder.AddColumn<byte[]>(
                name: "TempId",
                table: "users",
                type: "varbinary(3072)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "TempId1",
                table: "users",
                type: "varbinary(3072)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_users_TempId",
                table: "users",
                column: "TempId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_users_TempId1",
                table: "users",
                column: "TempId1");

            migrationBuilder.AddForeignKey(
                name: "fk_refresh_tokens_users",
                table: "refresh_tokens",
                column: "UserId",
                principalTable: "users",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_identities_users1",
                table: "user_identities",
                column: "users_id",
                principalTable: "users",
                principalColumn: "TempId1",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
