using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiroDaCopa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchEventVideoUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "match_events",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "match_events");
        }
    }
}
