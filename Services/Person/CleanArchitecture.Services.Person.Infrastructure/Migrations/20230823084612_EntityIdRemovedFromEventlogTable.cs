using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Services.Person.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EntityIdRemovedFromEventlogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityId",
                schema: "prsn",
                table: "EventLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                schema: "prsn",
                table: "EventLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
