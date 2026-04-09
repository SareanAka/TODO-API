using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TODO_API.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreatedAt_SetUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "TodoItems",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TodoItems",
                newName: "createdAt");
        }
    }
}
