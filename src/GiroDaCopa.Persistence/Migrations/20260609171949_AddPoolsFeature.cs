using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiroDaCopa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPoolsFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    InviteCode = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    TournamentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pools_tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pools_users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pool_members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsOwner = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pool_members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pool_members_pools_PoolId",
                        column: x => x.PoolId,
                        principalTable: "pools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pool_members_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pool_predictions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    HomeGoals = table.Column<int>(type: "integer", nullable: false),
                    AwayGoals = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pool_predictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pool_predictions_matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pool_predictions_pools_PoolId",
                        column: x => x.PoolId,
                        principalTable: "pools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pool_predictions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pool_members_PoolId_UserId",
                table: "pool_members",
                columns: new[] { "PoolId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pool_members_UserId",
                table: "pool_members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_pool_predictions_MatchId",
                table: "pool_predictions",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_pool_predictions_PoolId_UserId_MatchId",
                table: "pool_predictions",
                columns: new[] { "PoolId", "UserId", "MatchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pool_predictions_UserId",
                table: "pool_predictions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_pools_CreatedByUserId",
                table: "pools",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_pools_InviteCode",
                table: "pools",
                column: "InviteCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pools_TournamentId",
                table: "pools",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pool_members");

            migrationBuilder.DropTable(
                name: "pool_predictions");

            migrationBuilder.DropTable(
                name: "pools");
        }
    }
}
