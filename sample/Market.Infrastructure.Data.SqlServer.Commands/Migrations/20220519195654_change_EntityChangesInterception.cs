using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Market.Infrastructure.Data.SqlServer.Commands.Migrations
{
    public partial class change_EntityChangesInterception : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_EntityChangesInterceptorDetails");

            migrationBuilder.DropTable(
                name: "_EntityChangesInterceptors");

            migrationBuilder.CreateTable(
                name: "_EntityChangesInterceptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EntityChangesInterceptions", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "EntityChangesInterceptionDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityChangesInterceptorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityChangesInterceptionDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityChangesInterceptionDetail__EntityChangesInterceptions_EntityChangesInterceptorId",
                        column: x => x.EntityChangesInterceptorId,
                        principalTable: "_EntityChangesInterceptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX__EntityChangesInterceptions_Date",
                table: "_EntityChangesInterceptions",
                column: "Date",
                unique: true)
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangesInterceptionDetail_EntityChangesInterceptorId",
                table: "EntityChangesInterceptionDetail",
                column: "EntityChangesInterceptorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityChangesInterceptionDetail");

            migrationBuilder.DropTable(
                name: "_EntityChangesInterceptions");

            migrationBuilder.CreateTable(
                name: "_EntityChangesInterceptors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EntityChangesInterceptors", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "_EntityChangesInterceptorDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityChangesInterceptorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EntityChangesInterceptorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK__EntityChangesInterceptorDetails__EntityChangesInterceptors_EntityChangesInterceptorId",
                        column: x => x.EntityChangesInterceptorId,
                        principalTable: "_EntityChangesInterceptors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX__EntityChangesInterceptorDetails_EntityChangesInterceptorId",
                table: "_EntityChangesInterceptorDetails",
                column: "EntityChangesInterceptorId");

            migrationBuilder.CreateIndex(
                name: "IX__EntityChangesInterceptors_Date",
                table: "_EntityChangesInterceptors",
                column: "Date",
                unique: true)
                .Annotation("SqlServer:Clustered", true);
        }
    }
}
