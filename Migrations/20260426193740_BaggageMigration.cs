using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAerolineas.Migrations
{
    /// <inheritdoc />
    public partial class BaggageMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "baggage_records",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_id = table.Column<int>(type: "int", nullable: false),
                    tiquete_id = table.Column<int>(type: "int", nullable: true),
                    reserva_pasajero_id = table.Column<int>(type: "int", nullable: true),
                    vuelo_id = table.Column<int>(type: "int", nullable: true),
                    pasajero_id = table.Column<int>(type: "int", nullable: true),
                    tipo_cabina_id = table.Column<int>(type: "int", nullable: false),
                    tipo_equipaje = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    peso_kg = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cantidad_permitida = table.Column<int>(type: "int", nullable: false),
                    peso_permitido_por_maleta_kg = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    peso_total_permitido_kg = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    exceso_cantidad = table.Column<int>(type: "int", nullable: false),
                    exceso_peso_kg = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    recargo_cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    recargo_peso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    recargo_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    fecha_registro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baggage_records", x => x.id);
                    table.ForeignKey(
                        name: "FK_baggage_records_CabinTypes_tipo_cabina_id",
                        column: x => x.tipo_cabina_id,
                        principalTable: "CabinTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_baggage_records_flights_vuelo_id",
                        column: x => x.vuelo_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_baggage_records_passengers_pasajero_id",
                        column: x => x.pasajero_id,
                        principalTable: "passengers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_baggage_records_reservationpassengers_reserva_pasajero_id",
                        column: x => x.reserva_pasajero_id,
                        principalTable: "reservationpassengers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_baggage_records_reservations_reserva_id",
                        column: x => x.reserva_id,
                        principalTable: "reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_baggage_records_tickets_tiquete_id",
                        column: x => x.tiquete_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_baggage_records_pasajero_id",
                table: "baggage_records",
                column: "pasajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_baggage_records_reserva_id",
                table: "baggage_records",
                column: "reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_baggage_records_reserva_pasajero_id",
                table: "baggage_records",
                column: "reserva_pasajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_baggage_records_tipo_cabina_id",
                table: "baggage_records",
                column: "tipo_cabina_id");

            migrationBuilder.CreateIndex(
                name: "IX_baggage_records_tiquete_id",
                table: "baggage_records",
                column: "tiquete_id");

            migrationBuilder.CreateIndex(
                name: "IX_baggage_records_vuelo_id",
                table: "baggage_records",
                column: "vuelo_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "baggage_records");
        }
    }
}
