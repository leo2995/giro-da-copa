using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiroDaCopa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlignFrontendContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "teams",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExternalCode",
                table: "matches",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "matches",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "groups",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<int>(
                name: "Drawn",
                table: "group_teams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoalsAgainst",
                table: "group_teams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GoalsFor",
                table: "group_teams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Lost",
                table: "group_teams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Played",
                table: "group_teams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Won",
                table: "group_teams",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "FifaCode",
                table: "countries",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);

            migrationBuilder.AddColumn<string>(
                name: "FlagEmoji",
                table: "countries",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "broadcast_channels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    LogoColor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UrlPlaceholder = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_broadcast_channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "match_broadcasts",
                columns: table => new
                {
                    MatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    BroadcastChannelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_match_broadcasts", x => new { x.MatchId, x.BroadcastChannelId });
                    table.ForeignKey(
                        name: "FK_match_broadcasts_broadcast_channels_BroadcastChannelId",
                        column: x => x.BroadcastChannelId,
                        principalTable: "broadcast_channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_match_broadcasts_matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_teams_Code",
                table: "teams",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_matches_ExternalCode",
                table: "matches",
                column: "ExternalCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_matches_GroupId",
                table: "matches",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_broadcast_channels_Code",
                table: "broadcast_channels",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_match_broadcasts_BroadcastChannelId",
                table: "match_broadcasts",
                column: "BroadcastChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_matches_groups_GroupId",
                table: "matches",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_matches_groups_GroupId",
                table: "matches");

            migrationBuilder.DropTable(
                name: "match_broadcasts");

            migrationBuilder.DropTable(
                name: "broadcast_channels");

            migrationBuilder.DropIndex(
                name: "IX_teams_Code",
                table: "teams");

            migrationBuilder.DropIndex(
                name: "IX_matches_ExternalCode",
                table: "matches");

            migrationBuilder.DropIndex(
                name: "IX_matches_GroupId",
                table: "matches");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "teams");

            migrationBuilder.DropColumn(
                name: "ExternalCode",
                table: "matches");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "matches");

            migrationBuilder.DropColumn(
                name: "Drawn",
                table: "group_teams");

            migrationBuilder.DropColumn(
                name: "GoalsAgainst",
                table: "group_teams");

            migrationBuilder.DropColumn(
                name: "GoalsFor",
                table: "group_teams");

            migrationBuilder.DropColumn(
                name: "Lost",
                table: "group_teams");

            migrationBuilder.DropColumn(
                name: "Played",
                table: "group_teams");

            migrationBuilder.DropColumn(
                name: "Won",
                table: "group_teams");

            migrationBuilder.DropColumn(
                name: "FlagEmoji",
                table: "countries");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "groups",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "FifaCode",
                table: "countries",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5);
        }
    }
}
