using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunctionAppBurgarKollen.Migrations
{
    public partial class ratinguser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "UserRatings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserRatings",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "UserRatings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserRatings");
        }
    }
}
