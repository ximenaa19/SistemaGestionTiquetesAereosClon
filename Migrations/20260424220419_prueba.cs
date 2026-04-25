using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionAerolineas.Migrations
{
    /// <inheritdoc />
    public partial class prueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CabinTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabinTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "card_issuers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_issuers", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cargos_personal",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargos_personal", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "continents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_continents", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "emaildomains",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    domain = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emaildomains", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estados_checkin",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estados_checkin", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estados_disponibilidad",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estados_disponibilidad", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estados_pago",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estados_pago", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estados_tiquete",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estados_tiquete", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estados_vuelo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estados_vuelo", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fligthroles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fligthroles", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment_method_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_method_types", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permisos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permisos", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "phonecodes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    codigo_pais = table.Column<string>(type: "varchar(5)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre_pais = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phonecodes", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reservationstatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservationstatus", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RoadTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles_sistema",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles_sistema", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "temporadas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    precio_factor = table.Column<decimal>(type: "decimal(5,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_temporadas", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipos_item_factura",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_item_factura", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipos_pasajero",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    edad_min = table.Column<int>(type: "int", nullable: true),
                    edad_max = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_pasajero", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipos_tarjeta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_tarjeta", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipos_ubicacion_asiento",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_ubicacion_asiento", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_iso = table.Column<string>(type: "varchar(3)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    continente_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.id);
                    table.ForeignKey(
                        name: "FK_countries_continents_continente_id",
                        column: x => x.continente_id,
                        principalTable: "continents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flightstatustransitions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    estado_origen_id = table.Column<int>(type: "int", nullable: false),
                    estado_destino_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flightstatustransitions", x => x.id);
                    table.ForeignKey(
                        name: "FK_flightstatustransitions_estados_vuelo_estado_destino_id",
                        column: x => x.estado_destino_id,
                        principalTable: "estados_vuelo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flightstatustransitions_estados_vuelo_estado_origen_id",
                        column: x => x.estado_origen_id,
                        principalTable: "estados_vuelo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "transiciones_estado_reserva",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    estado_origen_id = table.Column<int>(type: "int", nullable: false),
                    estado_destino_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transiciones_estado_reserva", x => x.id);
                    table.ForeignKey(
                        name: "FK_transiciones_estado_reserva_reservationstatus_estado_destino~",
                        column: x => x.estado_destino_id,
                        principalTable: "reservationstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transiciones_estado_reserva_reservationstatus_estado_origen_~",
                        column: x => x.estado_origen_id,
                        principalTable: "reservationstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rolepermissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    rol_id = table.Column<int>(type: "int", nullable: false),
                    permiso_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolepermissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_rolepermissions_permisos_permiso_id",
                        column: x => x.permiso_id,
                        principalTable: "permisos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_rolepermissions_roles_sistema_rol_id",
                        column: x => x.rol_id,
                        principalTable: "roles_sistema",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "paymentmethods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tipo_medio_pago_id = table.Column<int>(type: "int", nullable: false),
                    tipo_tarjeta_id = table.Column<int>(type: "int", nullable: true),
                    emisor_tarjeta_id = table.Column<int>(type: "int", nullable: true),
                    nombre_comercial = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentmethods", x => x.id);
                    table.ForeignKey(
                        name: "FK_paymentmethods_card_issuers_emisor_tarjeta_id",
                        column: x => x.emisor_tarjeta_id,
                        principalTable: "card_issuers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_paymentmethods_payment_method_types_tipo_medio_pago_id",
                        column: x => x.tipo_medio_pago_id,
                        principalTable: "payment_method_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_paymentmethods_tipos_tarjeta_tipo_tarjeta_id",
                        column: x => x.tipo_tarjeta_id,
                        principalTable: "tipos_tarjeta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aircraftmanufacturers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    country_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircraftmanufacturers", x => x.id);
                    table.ForeignKey(
                        name: "FK_aircraftmanufacturers_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "airlines",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_iata = table.Column<string>(type: "varchar(3)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pais_origen_id = table.Column<int>(type: "int", nullable: false),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    creado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    actualizado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_airlines", x => x.id);
                    table.ForeignKey(
                        name: "FK_airlines_countries_pais_origen_id",
                        column: x => x.pais_origen_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pais_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regions", x => x.id);
                    table.ForeignKey(
                        name: "FK_regions_countries_pais_id",
                        column: x => x.pais_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aircraftmodels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fabricante_id = table.Column<int>(type: "int", nullable: false),
                    nombre_modelo = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    capacidad_maxima = table.Column<int>(type: "int", nullable: false),
                    peso_max_despegue_kg = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    consumo_combustible_kg_h = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    velocidad_crucero_kmh = table.Column<int>(type: "int", nullable: true),
                    altitud_crucero_ft = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircraftmodels", x => x.id);
                    table.ForeignKey(
                        name: "FK_aircraftmodels_aircraftmanufacturers_fabricante_id",
                        column: x => x.fabricante_id,
                        principalTable: "aircraftmanufacturers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    region_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.id);
                    table.ForeignKey(
                        name: "FK_cities_regions_region_id",
                        column: x => x.region_id,
                        principalTable: "regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "aircraft",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    modelo_id = table.Column<int>(type: "int", nullable: false),
                    aerolinea_id = table.Column<int>(type: "int", nullable: false),
                    matricula = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_fabricacion = table.Column<DateTime>(type: "date", nullable: true),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aircraft", x => x.id);
                    table.ForeignKey(
                        name: "FK_aircraft_aircraftmodels_modelo_id",
                        column: x => x.modelo_id,
                        principalTable: "aircraftmodels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aircraft_airlines_aerolinea_id",
                        column: x => x.aerolinea_id,
                        principalTable: "airlines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tipo_via_id = table.Column<int>(type: "int", nullable: false),
                    nombre_via = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numero = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    complemento = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ciudad_id = table.Column<int>(type: "int", nullable: false),
                    codigo_postal = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_addresses_RoadTypes_tipo_via_id",
                        column: x => x.tipo_via_id,
                        principalTable: "RoadTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_addresses_cities_ciudad_id",
                        column: x => x.ciudad_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "airports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_iata = table.Column<string>(type: "varchar(3)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    codigo_icao = table.Column<string>(type: "varchar(4)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ciudad_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_airports", x => x.id);
                    table.ForeignKey(
                        name: "FK_airports_cities_ciudad_id",
                        column: x => x.ciudad_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cabinconfiguration",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    aeronave_id = table.Column<int>(type: "int", nullable: false),
                    tipo_cabina_id = table.Column<int>(type: "int", nullable: false),
                    fila_inicio = table.Column<int>(type: "int", nullable: false),
                    fila_fin = table.Column<int>(type: "int", nullable: false),
                    asientos_por_fila = table.Column<int>(type: "int", nullable: false),
                    letras_asientos = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cabinconfiguration", x => x.id);
                    table.ForeignKey(
                        name: "FK_cabinconfiguration_CabinTypes_tipo_cabina_id",
                        column: x => x.tipo_cabina_id,
                        principalTable: "CabinTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cabinconfiguration_aircraft_aeronave_id",
                        column: x => x.aeronave_id,
                        principalTable: "aircraft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tipo_documento_id = table.Column<int>(type: "int", nullable: false),
                    numero_documento = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombres = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    apellidos = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_nacimiento = table.Column<DateTime>(type: "date", nullable: true),
                    genero = table.Column<string>(type: "char(1)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion_id = table.Column<int>(type: "int", nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    actualizado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_people", x => x.id);
                    table.ForeignKey(
                        name: "FK_people_DocumentTypes_tipo_documento_id",
                        column: x => x.tipo_documento_id,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_people_addresses_direccion_id",
                        column: x => x.direccion_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "airportairline",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    aeropuerto_id = table.Column<int>(type: "int", nullable: false),
                    aerolinea_id = table.Column<int>(type: "int", nullable: false),
                    terminal = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_inicio = table.Column<DateTime>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "date", nullable: true),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_airportairline", x => x.id);
                    table.ForeignKey(
                        name: "FK_airportairline_airlines_aerolinea_id",
                        column: x => x.aerolinea_id,
                        principalTable: "airlines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_airportairline_airports_aeropuerto_id",
                        column: x => x.aeropuerto_id,
                        principalTable: "airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "routes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    aeropuerto_origen_id = table.Column<int>(type: "int", nullable: false),
                    aeropuerto_destino_id = table.Column<int>(type: "int", nullable: false),
                    distancia_km = table.Column<int>(type: "int", nullable: true),
                    duracion_estimada_min = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routes", x => x.id);
                    table.ForeignKey(
                        name: "FK_routes_airports_aeropuerto_destino_id",
                        column: x => x.aeropuerto_destino_id,
                        principalTable: "airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_routes_airports_aeropuerto_origen_id",
                        column: x => x.aeropuerto_origen_id,
                        principalTable: "airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    creado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.id);
                    table.ForeignKey(
                        name: "FK_clients_people_persona_id",
                        column: x => x.persona_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "passengers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    tipo_pasajero_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passengers", x => x.id);
                    table.ForeignKey(
                        name: "FK_passengers_people_persona_id",
                        column: x => x.persona_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_passengers_tipos_pasajero_tipo_pasajero_id",
                        column: x => x.tipo_pasajero_id,
                        principalTable: "tipos_pasajero",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "personemails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    usuario_email = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dominio_email_id = table.Column<int>(type: "int", nullable: false),
                    es_principal = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personemails", x => x.id);
                    table.ForeignKey(
                        name: "FK_personemails_emaildomains_dominio_email_id",
                        column: x => x.dominio_email_id,
                        principalTable: "emaildomains",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personemails_people_persona_id",
                        column: x => x.persona_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "personphones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    codigo_telefono_id = table.Column<int>(type: "int", nullable: false),
                    numero_telefono = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    es_principal = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personphones", x => x.id);
                    table.ForeignKey(
                        name: "FK_personphones_people_persona_id",
                        column: x => x.persona_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_personphones_phonecodes_codigo_telefono_id",
                        column: x => x.codigo_telefono_id,
                        principalTable: "phonecodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<int>(type: "int", nullable: false),
                    cargo_id = table.Column<int>(type: "int", nullable: false),
                    aerolinea_id = table.Column<int>(type: "int", nullable: true),
                    aeropuerto_id = table.Column<int>(type: "int", nullable: true),
                    fecha_ingreso = table.Column<DateTime>(type: "date", nullable: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    creado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    actualizado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staff", x => x.id);
                    table.ForeignKey(
                        name: "FK_staff_airlines_aerolinea_id",
                        column: x => x.aerolinea_id,
                        principalTable: "airlines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_airports_aeropuerto_id",
                        column: x => x.aeropuerto_id,
                        principalTable: "airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_cargos_personal_cargo_id",
                        column: x => x.cargo_id,
                        principalTable: "cargos_personal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staff_people_persona_id",
                        column: x => x.persona_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    persona_id = table.Column<int>(type: "int", nullable: true),
                    rol_id = table.Column<int>(type: "int", nullable: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    ultimo_acceso = table.Column<DateTime>(type: "datetime", nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    actualizado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_people_persona_id",
                        column: x => x.persona_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_roles_sistema_rol_id",
                        column: x => x.rol_id,
                        principalTable: "roles_sistema",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fares",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ruta_id = table.Column<int>(type: "int", nullable: false),
                    tipo_cabina_id = table.Column<int>(type: "int", nullable: false),
                    tipo_pasajero_id = table.Column<int>(type: "int", nullable: false),
                    temporada_id = table.Column<int>(type: "int", nullable: false),
                    precio_base = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    vigencia_desde = table.Column<DateTime>(type: "date", nullable: true),
                    vigencia_hasta = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fares", x => x.id);
                    table.ForeignKey(
                        name: "FK_fares_CabinTypes_tipo_cabina_id",
                        column: x => x.tipo_cabina_id,
                        principalTable: "CabinTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fares_routes_ruta_id",
                        column: x => x.ruta_id,
                        principalTable: "routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fares_temporadas_temporada_id",
                        column: x => x.temporada_id,
                        principalTable: "temporadas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fares_tipos_pasajero_tipo_pasajero_id",
                        column: x => x.tipo_pasajero_id,
                        principalTable: "tipos_pasajero",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flights",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    codigo_vuelo = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    aerolinea_id = table.Column<int>(type: "int", nullable: false),
                    ruta_id = table.Column<int>(type: "int", nullable: false),
                    aeronave_id = table.Column<int>(type: "int", nullable: false),
                    fecha_salida = table.Column<DateTime>(type: "datetime", nullable: false),
                    fecha_llegada_estimada = table.Column<DateTime>(type: "datetime", nullable: false),
                    capacidad_total = table.Column<int>(type: "int", nullable: false),
                    asientos_disponibles = table.Column<int>(type: "int", nullable: false),
                    estado_vuelo_id = table.Column<int>(type: "int", nullable: false),
                    reprogramado_en = table.Column<DateTime>(type: "datetime", nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    actualizado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flights", x => x.id);
                    table.ForeignKey(
                        name: "FK_flights_aircraft_aeronave_id",
                        column: x => x.aeronave_id,
                        principalTable: "aircraft",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flights_airlines_aerolinea_id",
                        column: x => x.aerolinea_id,
                        principalTable: "airlines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flights_estados_vuelo_estado_vuelo_id",
                        column: x => x.estado_vuelo_id,
                        principalTable: "estados_vuelo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flights_routes_ruta_id",
                        column: x => x.ruta_id,
                        principalTable: "routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "routestops",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ruta_id = table.Column<int>(type: "int", nullable: false),
                    aeropuerto_escala_id = table.Column<int>(type: "int", nullable: false),
                    orden = table.Column<int>(type: "int", nullable: false),
                    duracion_escala_min = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routestops", x => x.id);
                    table.ForeignKey(
                        name: "FK_routestops_airports_aeropuerto_escala_id",
                        column: x => x.aeropuerto_escala_id,
                        principalTable: "airports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_routestops_routes_ruta_id",
                        column: x => x.ruta_id,
                        principalTable: "routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    codigo_reserva = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    fecha_reserva = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    estado_reserva_id = table.Column<int>(type: "int", nullable: false),
                    valor_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    vence_en = table.Column<DateTime>(type: "datetime", nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    actualizado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservations_clients_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservations_reservationstatus_estado_reserva_id",
                        column: x => x.estado_reserva_id,
                        principalTable: "reservationstatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "staffavailability",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    personal_id = table.Column<int>(type: "int", nullable: false),
                    estado_disponibilidad_id = table.Column<int>(type: "int", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "datetime", nullable: false),
                    observacion = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_staffavailability", x => x.id);
                    table.ForeignKey(
                        name: "FK_staffavailability_estados_disponibilidad_estado_disponibilid~",
                        column: x => x.estado_disponibilidad_id,
                        principalTable: "estados_disponibilidad",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_staffavailability_staff_personal_id",
                        column: x => x.personal_id,
                        principalTable: "staff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    iniciada_en = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    cerrada_en = table.Column<DateTime>(type: "datetime", nullable: true),
                    ip_origen = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_sessions_users_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flightassignments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    vuelo_id = table.Column<int>(type: "int", nullable: false),
                    personal_id = table.Column<int>(type: "int", nullable: false),
                    rol_vuelo_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flightassignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_flightassignments_flights_vuelo_id",
                        column: x => x.vuelo_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flightassignments_fligthroles_rol_vuelo_id",
                        column: x => x.rol_vuelo_id,
                        principalTable: "fligthroles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flightassignments_staff_personal_id",
                        column: x => x.personal_id,
                        principalTable: "staff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flightseats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    vuelo_id = table.Column<int>(type: "int", nullable: false),
                    codigo_asiento = table.Column<string>(type: "varchar(5)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_cabina_id = table.Column<int>(type: "int", nullable: false),
                    tipo_ubicacion_id = table.Column<int>(type: "int", nullable: false),
                    esta_ocupado = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flightseats", x => x.id);
                    table.ForeignKey(
                        name: "FK_flightseats_CabinTypes_tipo_cabina_id",
                        column: x => x.tipo_cabina_id,
                        principalTable: "CabinTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flightseats_flights_vuelo_id",
                        column: x => x.vuelo_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_flightseats_tipos_ubicacion_asiento_tipo_ubicacion_id",
                        column: x => x.tipo_ubicacion_id,
                        principalTable: "tipos_ubicacion_asiento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_id = table.Column<int>(type: "int", nullable: false),
                    numero_factura = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_emision = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    impuestos = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    creado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.id);
                    table.ForeignKey(
                        name: "FK_invoices_reservations_reserva_id",
                        column: x => x.reserva_id,
                        principalTable: "reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_id = table.Column<int>(type: "int", nullable: false),
                    monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "datetime", nullable: false),
                    estado_pago_id = table.Column<int>(type: "int", nullable: false),
                    metodo_pago_id = table.Column<int>(type: "int", nullable: false),
                    creado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    actualizado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_estados_pago_estado_pago_id",
                        column: x => x.estado_pago_id,
                        principalTable: "estados_pago",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payments_paymentmethods_metodo_pago_id",
                        column: x => x.metodo_pago_id,
                        principalTable: "paymentmethods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payments_reservations_reserva_id",
                        column: x => x.reserva_id,
                        principalTable: "reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reservationflights",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_id = table.Column<int>(type: "int", nullable: false),
                    vuelo_id = table.Column<int>(type: "int", nullable: false),
                    valor_parcial = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservationflights", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservationflights_flights_vuelo_id",
                        column: x => x.vuelo_id,
                        principalTable: "flights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservationflights_reservations_reserva_id",
                        column: x => x.reserva_id,
                        principalTable: "reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reservationpassengers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_vuelo_id = table.Column<int>(type: "int", nullable: false),
                    pasajero_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservationpassengers", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservationpassengers_passengers_pasajero_id",
                        column: x => x.pasajero_id,
                        principalTable: "passengers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservationpassengers_reservationflights_reserva_vuelo_id",
                        column: x => x.reserva_vuelo_id,
                        principalTable: "reservationflights",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "invoiceitems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    factura_id = table.Column<int>(type: "int", nullable: false),
                    tipo_item_id = table.Column<int>(type: "int", nullable: false),
                    descripcion = table.Column<string>(type: "varchar(200)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cantidad = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    precio_unitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    reserva_pasajero_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoiceitems", x => x.id);
                    table.ForeignKey(
                        name: "FK_invoiceitems_invoices_factura_id",
                        column: x => x.factura_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invoiceitems_reservationpassengers_reserva_pasajero_id",
                        column: x => x.reserva_pasajero_id,
                        principalTable: "reservationpassengers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_invoiceitems_tipos_item_factura_tipo_item_id",
                        column: x => x.tipo_item_id,
                        principalTable: "tipos_item_factura",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reserva_pasajero_id = table.Column<int>(type: "int", nullable: false),
                    codigo_tiquete = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_emision = table.Column<DateTime>(type: "datetime", nullable: false),
                    estado_tiquete_id = table.Column<int>(type: "int", nullable: false),
                    creado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    actualizado_en = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tickets", x => x.id);
                    table.ForeignKey(
                        name: "FK_tickets_estados_tiquete_estado_tiquete_id",
                        column: x => x.estado_tiquete_id,
                        principalTable: "estados_tiquete",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tickets_reservationpassengers_reserva_pasajero_id",
                        column: x => x.reserva_pasajero_id,
                        principalTable: "reservationpassengers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "checkins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tiquete_id = table.Column<int>(type: "int", nullable: false),
                    personal_id = table.Column<int>(type: "int", nullable: false),
                    asiento_vuelo_id = table.Column<int>(type: "int", nullable: false),
                    fecha_checkin = table.Column<DateTime>(type: "datetime", nullable: false),
                    estado_checkin_id = table.Column<int>(type: "int", nullable: false),
                    numero_tarjeta_embarque = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    equipaje_bodega = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    peso_equipaje_kg = table.Column<decimal>(type: "decimal(5,2)", nullable: true, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checkins", x => x.id);
                    table.ForeignKey(
                        name: "FK_checkins_estados_checkin_estado_checkin_id",
                        column: x => x.estado_checkin_id,
                        principalTable: "estados_checkin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkins_flightseats_asiento_vuelo_id",
                        column: x => x.asiento_vuelo_id,
                        principalTable: "flightseats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkins_staff_personal_id",
                        column: x => x.personal_id,
                        principalTable: "staff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_checkins_tickets_tiquete_id",
                        column: x => x.tiquete_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_ciudad_id",
                table: "addresses",
                column: "ciudad_id");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_tipo_via_id",
                table: "addresses",
                column: "tipo_via_id");

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_aerolinea_id",
                table: "aircraft",
                column: "aerolinea_id");

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_matricula",
                table: "aircraft",
                column: "matricula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aircraft_modelo_id",
                table: "aircraft",
                column: "modelo_id");

            migrationBuilder.CreateIndex(
                name: "IX_aircraftmanufacturers_country_id",
                table: "aircraftmanufacturers",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_aircraftmanufacturers_nombre",
                table: "aircraftmanufacturers",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aircraftmodels_fabricante_id",
                table: "aircraftmodels",
                column: "fabricante_id");

            migrationBuilder.CreateIndex(
                name: "IX_airlines_pais_origen_id",
                table: "airlines",
                column: "pais_origen_id");

            migrationBuilder.CreateIndex(
                name: "IX_airportairline_aerolinea_id",
                table: "airportairline",
                column: "aerolinea_id");

            migrationBuilder.CreateIndex(
                name: "IX_airportairline_aeropuerto_id_aerolinea_id",
                table: "airportairline",
                columns: new[] { "aeropuerto_id", "aerolinea_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_airports_ciudad_id",
                table: "airports",
                column: "ciudad_id");

            migrationBuilder.CreateIndex(
                name: "IX_airports_codigo_iata",
                table: "airports",
                column: "codigo_iata",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_airports_codigo_icao",
                table: "airports",
                column: "codigo_icao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cabinconfiguration_aeronave_id_tipo_cabina_id",
                table: "cabinconfiguration",
                columns: new[] { "aeronave_id", "tipo_cabina_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cabinconfiguration_tipo_cabina_id",
                table: "cabinconfiguration",
                column: "tipo_cabina_id");

            migrationBuilder.CreateIndex(
                name: "IX_card_issuers_nombre",
                table: "card_issuers",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cargos_personal_nombre",
                table: "cargos_personal",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_checkins_asiento_vuelo_id",
                table: "checkins",
                column: "asiento_vuelo_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_checkins_estado_checkin_id",
                table: "checkins",
                column: "estado_checkin_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkins_numero_tarjeta_embarque",
                table: "checkins",
                column: "numero_tarjeta_embarque",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_checkins_personal_id",
                table: "checkins",
                column: "personal_id");

            migrationBuilder.CreateIndex(
                name: "IX_checkins_tiquete_id",
                table: "checkins",
                column: "tiquete_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cities_region_id",
                table: "cities",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "IX_clients_persona_id",
                table: "clients",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_continents_Name",
                table: "continents",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_codigo_iso",
                table: "countries",
                column: "codigo_iso",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_continente_id",
                table: "countries",
                column: "continente_id");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_Code",
                table: "DocumentTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTypes_Name",
                table: "DocumentTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_emaildomains_domain",
                table: "emaildomains",
                column: "domain",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_estados_checkin_nombre",
                table: "estados_checkin",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_estados_disponibilidad_nombre",
                table: "estados_disponibilidad",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_estados_pago_nombre",
                table: "estados_pago",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_estados_tiquete_nombre",
                table: "estados_tiquete",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_estados_vuelo_nombre",
                table: "estados_vuelo",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fares_ruta_id_tipo_cabina_id_tipo_pasajero_id_temporada_id",
                table: "fares",
                columns: new[] { "ruta_id", "tipo_cabina_id", "tipo_pasajero_id", "temporada_id" });

            migrationBuilder.CreateIndex(
                name: "IX_fares_temporada_id",
                table: "fares",
                column: "temporada_id");

            migrationBuilder.CreateIndex(
                name: "IX_fares_tipo_cabina_id",
                table: "fares",
                column: "tipo_cabina_id");

            migrationBuilder.CreateIndex(
                name: "IX_fares_tipo_pasajero_id",
                table: "fares",
                column: "tipo_pasajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_flightassignments_personal_id",
                table: "flightassignments",
                column: "personal_id");

            migrationBuilder.CreateIndex(
                name: "IX_flightassignments_rol_vuelo_id",
                table: "flightassignments",
                column: "rol_vuelo_id");

            migrationBuilder.CreateIndex(
                name: "IX_flightassignments_vuelo_id_personal_id",
                table: "flightassignments",
                columns: new[] { "vuelo_id", "personal_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flights_aerolinea_id",
                table: "flights",
                column: "aerolinea_id");

            migrationBuilder.CreateIndex(
                name: "IX_flights_aeronave_id",
                table: "flights",
                column: "aeronave_id");

            migrationBuilder.CreateIndex(
                name: "IX_flights_codigo_vuelo",
                table: "flights",
                column: "codigo_vuelo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flights_estado_vuelo_id",
                table: "flights",
                column: "estado_vuelo_id");

            migrationBuilder.CreateIndex(
                name: "IX_flights_ruta_id",
                table: "flights",
                column: "ruta_id");

            migrationBuilder.CreateIndex(
                name: "IX_flightseats_tipo_cabina_id",
                table: "flightseats",
                column: "tipo_cabina_id");

            migrationBuilder.CreateIndex(
                name: "IX_flightseats_tipo_ubicacion_id",
                table: "flightseats",
                column: "tipo_ubicacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_flightseats_vuelo_id_codigo_asiento",
                table: "flightseats",
                columns: new[] { "vuelo_id", "codigo_asiento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flightstatustransitions_estado_destino_id",
                table: "flightstatustransitions",
                column: "estado_destino_id");

            migrationBuilder.CreateIndex(
                name: "IX_flightstatustransitions_estado_origen_id_estado_destino_id",
                table: "flightstatustransitions",
                columns: new[] { "estado_origen_id", "estado_destino_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fligthroles_nombre",
                table: "fligthroles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invoiceitems_factura_id",
                table: "invoiceitems",
                column: "factura_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoiceitems_reserva_pasajero_id",
                table: "invoiceitems",
                column: "reserva_pasajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoiceitems_tipo_item_id",
                table: "invoiceitems",
                column: "tipo_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_numero_factura",
                table: "invoices",
                column: "numero_factura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_invoices_reserva_id",
                table: "invoices",
                column: "reserva_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_passengers_persona_id",
                table: "passengers",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_passengers_tipo_pasajero_id",
                table: "passengers",
                column: "tipo_pasajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_method_types_nombre",
                table: "payment_method_types",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paymentmethods_emisor_tarjeta_id",
                table: "paymentmethods",
                column: "emisor_tarjeta_id");

            migrationBuilder.CreateIndex(
                name: "IX_paymentmethods_nombre_comercial",
                table: "paymentmethods",
                column: "nombre_comercial",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paymentmethods_tipo_medio_pago_id",
                table: "paymentmethods",
                column: "tipo_medio_pago_id");

            migrationBuilder.CreateIndex(
                name: "IX_paymentmethods_tipo_tarjeta_id",
                table: "paymentmethods",
                column: "tipo_tarjeta_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_estado_pago_id",
                table: "payments",
                column: "estado_pago_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_metodo_pago_id",
                table: "payments",
                column: "metodo_pago_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_reserva_id",
                table: "payments",
                column: "reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_people_direccion_id",
                table: "people",
                column: "direccion_id");

            migrationBuilder.CreateIndex(
                name: "IX_people_tipo_documento_id_numero_documento",
                table: "people",
                columns: new[] { "tipo_documento_id", "numero_documento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permisos_nombre",
                table: "permisos",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_personemails_dominio_email_id",
                table: "personemails",
                column: "dominio_email_id");

            migrationBuilder.CreateIndex(
                name: "IX_personemails_persona_id",
                table: "personemails",
                column: "persona_id");

            migrationBuilder.CreateIndex(
                name: "IX_personphones_codigo_telefono_id",
                table: "personphones",
                column: "codigo_telefono_id");

            migrationBuilder.CreateIndex(
                name: "IX_personphones_persona_id",
                table: "personphones",
                column: "persona_id");

            migrationBuilder.CreateIndex(
                name: "IX_phonecodes_codigo_pais",
                table: "phonecodes",
                column: "codigo_pais",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_regions_pais_id",
                table: "regions",
                column: "pais_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservationflights_reserva_id_vuelo_id",
                table: "reservationflights",
                columns: new[] { "reserva_id", "vuelo_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservationflights_vuelo_id",
                table: "reservationflights",
                column: "vuelo_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservationpassengers_pasajero_id",
                table: "reservationpassengers",
                column: "pasajero_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservationpassengers_reserva_vuelo_id_pasajero_id",
                table: "reservationpassengers",
                columns: new[] { "reserva_vuelo_id", "pasajero_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservations_cliente_id",
                table: "reservations",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_codigo_reserva",
                table: "reservations",
                column: "codigo_reserva",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservations_estado_reserva_id",
                table: "reservations",
                column: "estado_reserva_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservationstatus_nombre",
                table: "reservationstatus",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rolepermissions_permiso_id",
                table: "rolepermissions",
                column: "permiso_id");

            migrationBuilder.CreateIndex(
                name: "IX_rolepermissions_rol_id_permiso_id",
                table: "rolepermissions",
                columns: new[] { "rol_id", "permiso_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_sistema_nombre",
                table: "roles_sistema",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_routes_aeropuerto_destino_id",
                table: "routes",
                column: "aeropuerto_destino_id");

            migrationBuilder.CreateIndex(
                name: "IX_routes_aeropuerto_origen_id_aeropuerto_destino_id",
                table: "routes",
                columns: new[] { "aeropuerto_origen_id", "aeropuerto_destino_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_routestops_aeropuerto_escala_id",
                table: "routestops",
                column: "aeropuerto_escala_id");

            migrationBuilder.CreateIndex(
                name: "IX_routestops_ruta_id_orden",
                table: "routestops",
                columns: new[] { "ruta_id", "orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sessions_usuario_id",
                table: "sessions",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_aerolinea_id",
                table: "staff",
                column: "aerolinea_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_aeropuerto_id",
                table: "staff",
                column: "aeropuerto_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_cargo_id",
                table: "staff",
                column: "cargo_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_persona_id",
                table: "staff",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staffavailability_estado_disponibilidad_id",
                table: "staffavailability",
                column: "estado_disponibilidad_id");

            migrationBuilder.CreateIndex(
                name: "IX_staffavailability_personal_id",
                table: "staffavailability",
                column: "personal_id");

            migrationBuilder.CreateIndex(
                name: "IX_temporadas_nombre",
                table: "temporadas",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tickets_codigo_tiquete",
                table: "tickets",
                column: "codigo_tiquete",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tickets_estado_tiquete_id",
                table: "tickets",
                column: "estado_tiquete_id");

            migrationBuilder.CreateIndex(
                name: "IX_tickets_reserva_pasajero_id",
                table: "tickets",
                column: "reserva_pasajero_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_item_factura_nombre",
                table: "tipos_item_factura",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_pasajero_nombre",
                table: "tipos_pasajero",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_tarjeta_nombre",
                table: "tipos_tarjeta",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tipos_ubicacion_asiento_nombre",
                table: "tipos_ubicacion_asiento",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transiciones_estado_reserva_estado_destino_id",
                table: "transiciones_estado_reserva",
                column: "estado_destino_id");

            migrationBuilder.CreateIndex(
                name: "IX_transiciones_estado_reserva_estado_origen_id_estado_destino_~",
                table: "transiciones_estado_reserva",
                columns: new[] { "estado_origen_id", "estado_destino_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_persona_id",
                table: "users",
                column: "persona_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_rol_id",
                table: "users",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "airportairline");

            migrationBuilder.DropTable(
                name: "cabinconfiguration");

            migrationBuilder.DropTable(
                name: "checkins");

            migrationBuilder.DropTable(
                name: "fares");

            migrationBuilder.DropTable(
                name: "flightassignments");

            migrationBuilder.DropTable(
                name: "flightstatustransitions");

            migrationBuilder.DropTable(
                name: "invoiceitems");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "personemails");

            migrationBuilder.DropTable(
                name: "personphones");

            migrationBuilder.DropTable(
                name: "rolepermissions");

            migrationBuilder.DropTable(
                name: "routestops");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "staffavailability");

            migrationBuilder.DropTable(
                name: "transiciones_estado_reserva");

            migrationBuilder.DropTable(
                name: "estados_checkin");

            migrationBuilder.DropTable(
                name: "flightseats");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "temporadas");

            migrationBuilder.DropTable(
                name: "fligthroles");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "tipos_item_factura");

            migrationBuilder.DropTable(
                name: "estados_pago");

            migrationBuilder.DropTable(
                name: "paymentmethods");

            migrationBuilder.DropTable(
                name: "emaildomains");

            migrationBuilder.DropTable(
                name: "phonecodes");

            migrationBuilder.DropTable(
                name: "permisos");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "estados_disponibilidad");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "CabinTypes");

            migrationBuilder.DropTable(
                name: "tipos_ubicacion_asiento");

            migrationBuilder.DropTable(
                name: "estados_tiquete");

            migrationBuilder.DropTable(
                name: "reservationpassengers");

            migrationBuilder.DropTable(
                name: "card_issuers");

            migrationBuilder.DropTable(
                name: "payment_method_types");

            migrationBuilder.DropTable(
                name: "tipos_tarjeta");

            migrationBuilder.DropTable(
                name: "roles_sistema");

            migrationBuilder.DropTable(
                name: "cargos_personal");

            migrationBuilder.DropTable(
                name: "passengers");

            migrationBuilder.DropTable(
                name: "reservationflights");

            migrationBuilder.DropTable(
                name: "tipos_pasajero");

            migrationBuilder.DropTable(
                name: "flights");

            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "aircraft");

            migrationBuilder.DropTable(
                name: "estados_vuelo");

            migrationBuilder.DropTable(
                name: "routes");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "reservationstatus");

            migrationBuilder.DropTable(
                name: "aircraftmodels");

            migrationBuilder.DropTable(
                name: "airlines");

            migrationBuilder.DropTable(
                name: "airports");

            migrationBuilder.DropTable(
                name: "people");

            migrationBuilder.DropTable(
                name: "aircraftmanufacturers");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "RoadTypes");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "regions");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "continents");
        }
    }
}
