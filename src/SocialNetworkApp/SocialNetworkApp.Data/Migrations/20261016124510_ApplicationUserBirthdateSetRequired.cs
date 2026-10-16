using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserBirthdateSetRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "Birthdate",
                table: "AspNetUsers",
                type: "DATE",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "DATE",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "Birthdate",
                table: "AspNetUsers",
                type: "DATE",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "DATE");
        }
    }
}
