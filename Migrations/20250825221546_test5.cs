using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyBoards.Migrations
{
    /// <inheritdoc />
    public partial class test5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkItemStates",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 4, "On Hold" },
                    { 5, "Rejected" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkItemStates",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WorkItemStates",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
