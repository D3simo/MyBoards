using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoards.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkItems_WorkItemStates_WorkItemStateId",
                table: "WorkItems");

            migrationBuilder.DropIndex(
                name: "IX_WorkItems_WorkItemStateId",
                table: "WorkItems");

            migrationBuilder.DropColumn(
                name: "WorkItemStateId",
                table: "WorkItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkItemStateId",
                table: "WorkItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkItems_WorkItemStateId",
                table: "WorkItems",
                column: "WorkItemStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkItems_WorkItemStates_WorkItemStateId",
                table: "WorkItems",
                column: "WorkItemStateId",
                principalTable: "WorkItemStates",
                principalColumn: "Id");
        }
    }
}
