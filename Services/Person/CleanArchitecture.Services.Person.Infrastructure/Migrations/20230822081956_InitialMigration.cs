using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Services.Person.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "prsn");

            migrationBuilder.CreateSequence(
                name: "eventlogseq",
                schema: "prsn",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "personseq",
                schema: "prsn",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "EventTypes",
                schema: "prsn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                schema: "prsn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventLogs",
                schema: "prsn",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    EventLogGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    EventTypeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventLogs_EventTypes_EventTypeId",
                        column: x => x.EventTypeId,
                        principalSchema: "prsn",
                        principalTable: "EventTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "People",
                schema: "prsn",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PersonGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AddedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Genders_GenderId",
                        column: x => x.GenderId,
                        principalSchema: "prsn",
                        principalTable: "Genders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "prsn",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WardNo = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_People_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "prsn",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PersonId",
                schema: "prsn",
                table: "Addresses",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_EventLogGUID",
                schema: "prsn",
                table: "EventLogs",
                column: "EventLogGUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_EventTypeId",
                schema: "prsn",
                table: "EventLogs",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_People_GenderId",
                schema: "prsn",
                table: "People",
                column: "GenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "prsn");

            migrationBuilder.DropTable(
                name: "EventLogs",
                schema: "prsn");

            migrationBuilder.DropTable(
                name: "People",
                schema: "prsn");

            migrationBuilder.DropTable(
                name: "EventTypes",
                schema: "prsn");

            migrationBuilder.DropTable(
                name: "Genders",
                schema: "prsn");

            migrationBuilder.DropSequence(
                name: "eventlogseq",
                schema: "prsn");

            migrationBuilder.DropSequence(
                name: "personseq",
                schema: "prsn");
        }
    }
}
