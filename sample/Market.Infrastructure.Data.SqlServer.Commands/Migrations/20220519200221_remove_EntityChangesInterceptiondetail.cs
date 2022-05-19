using Microsoft.EntityFrameworkCore.Migrations;

namespace Market.Infrastructure.Data.SqlServer.Commands.Migrations
{
    public partial class remove_EntityChangesInterceptiondetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityChangesInterceptionDetail__EntityChangesInterceptions_EntityChangesInterceptorId",
                table: "EntityChangesInterceptionDetail");

            migrationBuilder.RenameColumn(
                name: "EntityChangesInterceptorId",
                table: "EntityChangesInterceptionDetail",
                newName: "EntityChangesInterceptionId");

            migrationBuilder.RenameIndex(
                name: "IX_EntityChangesInterceptionDetail_EntityChangesInterceptorId",
                table: "EntityChangesInterceptionDetail",
                newName: "IX_EntityChangesInterceptionDetail_EntityChangesInterceptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityChangesInterceptionDetail__EntityChangesInterceptions_EntityChangesInterceptionId",
                table: "EntityChangesInterceptionDetail",
                column: "EntityChangesInterceptionId",
                principalTable: "_EntityChangesInterceptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityChangesInterceptionDetail__EntityChangesInterceptions_EntityChangesInterceptionId",
                table: "EntityChangesInterceptionDetail");

            migrationBuilder.RenameColumn(
                name: "EntityChangesInterceptionId",
                table: "EntityChangesInterceptionDetail",
                newName: "EntityChangesInterceptorId");

            migrationBuilder.RenameIndex(
                name: "IX_EntityChangesInterceptionDetail_EntityChangesInterceptionId",
                table: "EntityChangesInterceptionDetail",
                newName: "IX_EntityChangesInterceptionDetail_EntityChangesInterceptorId");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityChangesInterceptionDetail__EntityChangesInterceptions_EntityChangesInterceptorId",
                table: "EntityChangesInterceptionDetail",
                column: "EntityChangesInterceptorId",
                principalTable: "_EntityChangesInterceptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
