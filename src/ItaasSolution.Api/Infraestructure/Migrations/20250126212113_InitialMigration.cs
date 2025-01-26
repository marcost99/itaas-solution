using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItaasSolution.Api.Infraestructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HtttpMethod = table.Column<string>(type: "varchar(20)", nullable: true),
                    StatusCode = table.Column<int>(nullable: false),
                    UriPath = table.Column<string>(type: "varchar(50)", nullable: true),
                    TimeTaken = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ResponseSize = table.Column<int>(nullable: false),
                    CacheStatus = table.Column<string>(type: "varchar(20)", nullable: true),
                    DateCreation = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");
        }
    }
}
