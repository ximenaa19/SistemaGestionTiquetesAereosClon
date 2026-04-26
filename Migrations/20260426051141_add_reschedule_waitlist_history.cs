using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAerolineas.Migrations
{
    /// <inheritdoc />
    public partial class add_reschedule_waitlist_history : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reservation_reschedule_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reservation_id = table.Column<int>(type: "int", nullable: false),
                    old_flight_id = table.Column<int>(type: "int", nullable: false),
                    new_flight_id = table.Column<int>(type: "int", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    reason = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    action_status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation_reschedule_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservation_reschedule_history_flights_new_flight_id",
                        column: x => x.new_flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_reschedule_history_flights_old_flight_id",
                        column: x => x.old_flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_reschedule_history_reservations_reservation_id",
                        column: x => x.reservation_id,
                        principalTable: "reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reservation_waitlist",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reservation_id = table.Column<int>(type: "int", nullable: false),
                    reservation_flight_id = table.Column<int>(type: "int", nullable: false),
                    requested_flight_id = table.Column<int>(type: "int", nullable: false),
                    requested_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    queue_order = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reason = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    processed_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservation_waitlist", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservation_waitlist_flights_requested_flight_id",
                        column: x => x.requested_flight_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_waitlist_reservationflights_reservation_flight_id",
                        column: x => x.reservation_flight_id,
                        principalTable: "reservationflights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservation_waitlist_reservations_reservation_id",
                        column: x => x.reservation_id,
                        principalTable: "reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_reschedule_history_reservation_date",
                table: "reservation_reschedule_history",
                columns: new[] { "reservation_id", "changed_at" });

            migrationBuilder.CreateIndex(
                name: "IX_reservation_reschedule_history_new_flight_id",
                table: "reservation_reschedule_history",
                column: "new_flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_reschedule_history_old_flight_id",
                table: "reservation_reschedule_history",
                column: "old_flight_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_waitlist_reservation_id",
                table: "reservation_waitlist",
                column: "reservation_id");

            migrationBuilder.CreateIndex(
                name: "ix_waitlist_requested_status_order",
                table: "reservation_waitlist",
                columns: new[] { "requested_flight_id", "status", "queue_order" });

            migrationBuilder.CreateIndex(
                name: "ix_waitlist_unique_pending",
                table: "reservation_waitlist",
                columns: new[] { "reservation_flight_id", "requested_flight_id", "status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservation_reschedule_history");

            migrationBuilder.DropTable(
                name: "reservation_waitlist");
        }
    }
}
