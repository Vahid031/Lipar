using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Market.Infrastructure.Data.SqlServer.Commands.Migrations
{
    public partial class addOutBoxEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutBoxEventItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccuredByUserId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    AccuredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AggregateName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AggregateTypeName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AggregateId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EventTypeName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EventPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutBoxEventItems", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutBoxEventItems_AccuredOn",
                table: "OutBoxEventItems",
                column: "AccuredOn",
                unique: true)
                .Annotation("SqlServer:Clustered", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutBoxEventItems");
        }
    }
}
