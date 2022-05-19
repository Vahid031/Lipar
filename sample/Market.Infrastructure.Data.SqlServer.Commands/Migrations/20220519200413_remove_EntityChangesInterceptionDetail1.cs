using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Market.Infrastructure.Data.SqlServer.Commands.Migrations
{
    public partial class remove_EntityChangesInterceptionDetail1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityChangesInterceptionDetail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityChangesInterceptionDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityChangesInterceptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityChangesInterceptionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityChangesInterceptionDetail__EntityChangesInterceptions_EntityChangesInterceptionId",
                        column: x => x.EntityChangesInterceptionId,
                        principalTable: "_EntityChangesInterceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangesInterceptionDetail_EntityChangesInterceptionId",
                table: "EntityChangesInterceptionDetail",
                column: "EntityChangesInterceptionId");
        }
    }
}
