using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnagramSolver.EF.CodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Words",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Words");
        }
    }
}
