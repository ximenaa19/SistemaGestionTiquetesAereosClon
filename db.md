-- ============================================================
--  SISTEMA DE GESTIÓN DE TIQUETES AÉREOS
--  DDL completo — 61 tablas
--  Motor : MySQL 8.0+  |  utf8mb4_unicode_ci
--  Verificado contra especificación del proyecto
--  (Must Have / Should Have / Could Have)
-- ============================================================

CREATE DATABASE IF NOT EXISTS sistema_tiquetes
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE sistema_tiquetes;

-- ============================================================
-- 1. GEOGRAFÍA
--    Jerarquía: Continente → País → Región/Estado/Departamento → Ciudad
-- ============================================================

CREATE TABLE continents (YA ESTA HECHA
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL UNIQUE   -- América, Europa, Asia, África, Oceanía, Antártida
);

CREATE TABLE countries (YA ESTA HECHA
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    nombre          VARCHAR(100) NOT NULL,
    codigo_iso      VARCHAR(3)   NOT NULL UNIQUE,
    continente_id   INT          NOT NULL,
    FOREIGN KEY (continente_id) REFERENCES continents(id)
);

-- Cubre estados, departamentos, provincias y regions según el país
CREATE TABLE regions (YA ESTA HECHA
    id          INT          AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(100) NOT NULL,
    tipo        VARCHAR(30)  NOT NULL,   -- Estado, Departamento, Provincia, Región, Comunidad...
    pais_id     INT          NOT NULL,
    FOREIGN KEY (pais_id) REFERENCES countries(id)
);

CREATE TABLE cities (Faltan partes
    id          INT          AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(100) NOT NULL,
    region_id   INT          NOT NULL,
    FOREIGN KEY (region_id) REFERENCES regions(id)
);

-- ============================================================
-- 2. DIRECCIÓN NORMALIZADA
-- ============================================================

CREATE TABLE roadtypes (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL            -- Calle, Carrera, Avenida, Diagonal...
);

CREATE TABLE addresses (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    tipo_via_id     INT          NOT NULL,
    nombre_via      VARCHAR(100) NOT NULL,
    numero          VARCHAR(20)  NULL,
    complemento     VARCHAR(100) NULL,      -- Apto 301, Torre B
    ciudad_id       INT          NOT NULL,
    codigo_postal   VARCHAR(20)  NULL,
    FOREIGN KEY (tipo_via_id) REFERENCES roadtypes(id),
    FOREIGN KEY (ciudad_id)   REFERENCES cities(id)
);

-- ============================================================
-- 3. PERSONAS  (base común para clients, passengers y staff)
-- ============================================================

CREATE TABLE documenttypes (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL,
    codigo  VARCHAR(10) NOT NULL UNIQUE     -- PASAPORTE, CC, TI, DNI...
);

CREATE TABLE people (
    id                  INT          AUTO_INCREMENT PRIMARY KEY,
    tipo_documento_id   INT          NOT NULL,
    numero_documento    VARCHAR(30)  NOT NULL,
    nombres             VARCHAR(100) NOT NULL,
    apellidos           VARCHAR(100) NOT NULL,
    fecha_nacimiento    DATE         NULL,
    genero              CHAR(1)      NULL,  -- M / F / N
    direccion_id        INT          NULL,
    creado_en           DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    actualizado_en      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                            ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE (tipo_documento_id, numero_documento),
    FOREIGN KEY (tipo_documento_id) REFERENCES documenttypes(id),
    FOREIGN KEY (direccion_id)      REFERENCES addresses(id)
);

-- ============================================================
-- 4. EMAILS Y TELÉFONOS DE PERSONAS
--    Aplica a cualquier persona: clients, passengers y staff.
--    Se relaciona directamente con people, no con clients.
-- ============================================================

CREATE TABLE emaildomains (
    id      INT          AUTO_INCREMENT PRIMARY KEY,
    dominio VARCHAR(100) NOT NULL UNIQUE    -- gmail.com, outlook.com...
);

CREATE TABLE phonecodes (
    id              INT         AUTO_INCREMENT PRIMARY KEY,
    codigo_pais     VARCHAR(5)  NOT NULL UNIQUE,   -- +57, +34...
    nombre_pais     VARCHAR(100) NOT NULL
);

-- Emails de cualquier persona (cliente, pasajero, empleado)
CREATE TABLE personemails (
    id                  INT          AUTO_INCREMENT PRIMARY KEY,
    persona_id          INT          NOT NULL,
    usuario_email       VARCHAR(100) NOT NULL,      -- parte antes de @
    dominio_email_id    INT          NOT NULL,
    es_principal        TINYINT(1)   NOT NULL DEFAULT 0,
    FOREIGN KEY (persona_id)        REFERENCES people(id),
    FOREIGN KEY (dominio_email_id)  REFERENCES emaildomains(id)
);

-- Teléfonos de cualquier persona (cliente, pasajero, empleado)
CREATE TABLE personphones (
    id                  INT          AUTO_INCREMENT PRIMARY KEY,
    persona_id          INT          NOT NULL,
    codigo_telefono_id  INT          NOT NULL,
    numero_telefono     VARCHAR(20)  NOT NULL,
    es_principal        TINYINT(1)   NOT NULL DEFAULT 0,
    FOREIGN KEY (persona_id)         REFERENCES people(id),
    FOREIGN KEY (codigo_telefono_id) REFERENCES phonecodes(id)
);

-- ============================================================
-- 5. CLIENTES  (compradores — extiende people)
-- ============================================================

CREATE TABLE clients (
    id          INT      AUTO_INCREMENT PRIMARY KEY,
    persona_id  INT      NOT NULL UNIQUE,
    creado_en   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (persona_id) REFERENCES people(id)
);

-- ============================================================
-- 6. AEROLÍNEAS Y AEROPUERTOS
--    Los airports llevan código IATA (3 letras) e ICAO (4 letras).
--    La relación con aerolíneas se gestiona en airportairline
--    mediante FK a aerolinea_id (no se repite el código IATA ahí).
-- ============================================================

CREATE TABLE airlines (
    id                  INT          AUTO_INCREMENT PRIMARY KEY,
    nombre              VARCHAR(150) NOT NULL,
    codigo_iata         VARCHAR(3)   NOT NULL UNIQUE,
    pais_origen_id      INT          NOT NULL,
    activa              TINYINT(1)   NOT NULL DEFAULT 1,
    creado_en           DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    actualizado_en      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                             ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (pais_origen_id) REFERENCES countries(id)
);

CREATE TABLE airports (
    id          INT          AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(150) NOT NULL,
    codigo_iata VARCHAR(3)   NOT NULL UNIQUE,   -- código propio del aeropuerto
    codigo_icao VARCHAR(4)   NULL     UNIQUE,   -- código ICAO cuando aplique
    ciudad_id   INT          NOT NULL,
    FOREIGN KEY (ciudad_id) REFERENCES cities(id)
);

-- ============================================================
-- 7. AEROLÍNEAS QUE OPERAN EN CADA AEROPUERTO
--    La relación usa aerolinea_id (FK), no el código IATA.
--    Un aeropuerto puede tener varias aerolíneas y cada aerolínea
--    ya lleva su código en su propia tabla.
-- ============================================================

CREATE TABLE airportairline (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    aeropuerto_id   INT          NOT NULL,
    aerolinea_id    INT          NOT NULL,       -- FK, no se repite codigo_iata
    terminal        VARCHAR(20)  NULL,
    fecha_inicio    DATE         NOT NULL,
    fecha_fin       DATE         NULL,           -- NULL = sigue operando
    activa          TINYINT(1)   NOT NULL DEFAULT 1,
    UNIQUE (aeropuerto_id, aerolinea_id),
    FOREIGN KEY (aeropuerto_id) REFERENCES airports(id),
    FOREIGN KEY (aerolinea_id)  REFERENCES airlines(id)
);

-- ============================================================
-- 8. PERSONAL
--    Cubre tanto staff de vuelo (pilotos, auxiliares)
--    como staff administrativo (agentes de check-in, etc.).
--    cargo_id apunta a staffroles que solo tiene nombre
--    (sin código redundante).
-- ============================================================

CREATE TABLE staffroles (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(100) NOT NULL UNIQUE   -- Piloto, Copiloto, Agente Check-In, Administrativo...
);

CREATE TABLE staff (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    persona_id      INT          NOT NULL UNIQUE,
    cargo_id        INT          NOT NULL,
    aerolinea_id    INT          NULL,   -- staff de aerolínea
    aeropuerto_id   INT          NULL,   -- staff de aeropuerto
    fecha_ingreso   DATE         NOT NULL,
    activo          TINYINT(1)   NOT NULL DEFAULT 1,
    creado_en       DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    actualizado_en  DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                         ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (persona_id)    REFERENCES people(id),
    FOREIGN KEY (cargo_id)      REFERENCES staffroles(id),
    FOREIGN KEY (aerolinea_id)  REFERENCES airlines(id),
    FOREIGN KEY (aeropuerto_id) REFERENCES airports(id)
);

-- ============================================================
-- 9. DISPONIBILIDAD DEL PERSONAL
--    Bloques de tiempo por empleado.
--    Se consulta antes de crear una asignacion_vuelo.
--    El campo se llama 'nombre' (no 'codigo') porque es descriptivo.
-- ============================================================

CREATE TABLE availabilitystatuses (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL UNIQUE   -- Disponible, Asignado, Vacaciones, Licencia, Baja
);

CREATE TABLE staffavailability (
    id                          INT          AUTO_INCREMENT PRIMARY KEY,
    personal_id                 INT          NOT NULL,
    estado_disponibilidad_id    INT          NOT NULL,
    fecha_inicio                DATETIME     NOT NULL,
    fecha_fin                   DATETIME     NOT NULL,
    observacion                 VARCHAR(255) NULL,
    FOREIGN KEY (personal_id)              REFERENCES staff(id),
    FOREIGN KEY (estado_disponibilidad_id) REFERENCES availabilitystatuses(id),
    CONSTRAINT chk_fechas_disponibilidad CHECK (fecha_fin > fecha_inicio)
);

-- ============================================================
-- 10. AERONAVES Y CONFIGURACIÓN DE CABINA
--     Los datos operacionales del avión (combustible, peso máximo,
--     velocidad de crucero) pertenecen al modelo, no a la ruta.
-- ============================================================

CREATE TABLE aircraftmanufacturers (
    id      INT          AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(100) NOT NULL,
    pais    VARCHAR(100) NOT NULL
);

CREATE TABLE aircraftmodels (
    id                          INT            AUTO_INCREMENT PRIMARY KEY,
    fabricante_id               INT            NOT NULL,
    nombre_modelo               VARCHAR(100)   NOT NULL,   -- Boeing 737-800, Airbus A320...
    capacidad_maxima            INT            NOT NULL,
    peso_max_despegue_kg        DECIMAL(10,2)  NULL,       -- MTOW en kg
    consumo_combustible_kg_h    DECIMAL(8,2)   NULL,       -- consumo por hora de vuelo
    velocidad_crucero_kmh       INT            NULL,       -- velocidad típica de crucero
    altitud_crucero_ft          INT            NULL,       -- altitud típica de crucero
    FOREIGN KEY (fabricante_id) REFERENCES aircraftmanufacturers(id)
);

CREATE TABLE aircraft (
    id                  INT          AUTO_INCREMENT PRIMARY KEY,
    modelo_id           INT          NOT NULL,
    aerolinea_id        INT          NOT NULL,
    matricula           VARCHAR(20)  NOT NULL UNIQUE,
    fecha_fabricacion   DATE         NULL,
    activa              TINYINT(1)   NOT NULL DEFAULT 1,
    FOREIGN KEY (modelo_id)    REFERENCES aircraftmodels(id),
    FOREIGN KEY (aerolinea_id) REFERENCES airlines(id)
);

-- Clases de cabina: Económica, Business, Primera, VIP
CREATE TABLE cabintypes (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL UNIQUE
);

-- Distribución física de cabinas dentro de una aeronave concreta
CREATE TABLE cabinconfiguration (
    id                  INT          AUTO_INCREMENT PRIMARY KEY,
    aeronave_id         INT          NOT NULL,
    tipo_cabina_id      INT          NOT NULL,
    fila_inicio         INT          NOT NULL,
    fila_fin            INT          NOT NULL,
    asientos_por_fila   INT          NOT NULL,
    letras_asientos     VARCHAR(10)  NOT NULL,  -- "ABCDEF", "ABC"
    UNIQUE (aeronave_id, tipo_cabina_id),
    FOREIGN KEY (aeronave_id)    REFERENCES aircraft(id),
    FOREIGN KEY (tipo_cabina_id) REFERENCES cabintypes(id)
);

-- ============================================================
-- 11. RUTAS Y ESCALAS
--     La ruta define origen → destino y distancia.
--     Los datos operacionales (combustible, peso) van en el modelo
--     de aeronave.  El precio varía por escalas via tabla fares.
-- ============================================================

CREATE TABLE routes (
    id                      INT AUTO_INCREMENT PRIMARY KEY,
    aeropuerto_origen_id    INT NOT NULL,
    aeropuerto_destino_id   INT NOT NULL,
    distancia_km            INT NULL,
    duracion_estimada_min   INT NULL,
    UNIQUE (aeropuerto_origen_id, aeropuerto_destino_id),
    FOREIGN KEY (aeropuerto_origen_id)  REFERENCES airports(id),
    FOREIGN KEY (aeropuerto_destino_id) REFERENCES airports(id)
);

CREATE TABLE routestops (
    id                      INT AUTO_INCREMENT PRIMARY KEY,
    ruta_id                 INT NOT NULL,
    aeropuerto_escala_id    INT NOT NULL,
    orden                   INT NOT NULL,
    duracion_escala_min     INT NOT NULL DEFAULT 0,
    UNIQUE (ruta_id, orden),
    FOREIGN KEY (ruta_id)              REFERENCES routes(id),
    FOREIGN KEY (aeropuerto_escala_id) REFERENCES airports(id)
);

-- ============================================================
-- 12. TEMPORADAS
--     Tabla propia para seasons (alta, baja, media, festivos).
--     Reemplaza el campo VARCHAR en fares.
-- ============================================================

CREATE TABLE seasons (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    nombre          VARCHAR(50)  NOT NULL UNIQUE,   -- Alta, Baja, Media, Navidad, Semana Santa...
    descripcion     VARCHAR(150) NULL,
    precio_factor   DECIMAL(5,4) NOT NULL DEFAULT 1.0000  -- multiplicador: 1.20 = +20%
);

-- ============================================================
-- 13. TIPOS DE PASAJERO Y TARIFAS
--     Precio base = ruta + tipo cabina + tipo pasajero + temporada.
-- ============================================================

CREATE TABLE passengertypes (
    id          INT         AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(50) NOT NULL UNIQUE,  -- Bebé, Niño, Joven, Adulto, Adulto mayor
    edad_min    INT         NULL,
    edad_max    INT         NULL
);

CREATE TABLE fares (
    id                  INT            AUTO_INCREMENT PRIMARY KEY,
    ruta_id             INT            NOT NULL,
    tipo_cabina_id      INT            NOT NULL,
    tipo_pasajero_id    INT            NOT NULL,
    temporada_id        INT            NOT NULL,    -- FK a tabla seasons
    precio_base         DECIMAL(18,2)  NOT NULL,
    vigencia_desde      DATE           NULL,
    vigencia_hasta      DATE           NULL,
    FOREIGN KEY (ruta_id)          REFERENCES routes(id),
    FOREIGN KEY (tipo_cabina_id)   REFERENCES cabintypes(id),
    FOREIGN KEY (tipo_pasajero_id) REFERENCES passengertypes(id),
    FOREIGN KEY (temporada_id)     REFERENCES seasons(id),
    CONSTRAINT chk_precio_base CHECK (precio_base >= 0)
);

-- ============================================================
-- 14. VUELOS
--     'nombre' en estados (no 'codigo').
--     reprogramado_en: Could Have (reprogramación de flights).
-- ============================================================

CREATE TABLE flightstates (
    id          INT         AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(50) NOT NULL UNIQUE   -- Programado, Abordando, En vuelo, Cancelado, Completado, Reprogramado
);

-- Transiciones de estado válidas (Should Have).
-- Se valida en la capa Domain de la aplicación.
CREATE TABLE flightstatustransitions (
    id                  INT AUTO_INCREMENT PRIMARY KEY,
    estado_origen_id    INT NOT NULL,
    estado_destino_id   INT NOT NULL,
    UNIQUE (estado_origen_id, estado_destino_id),
    FOREIGN KEY (estado_origen_id)  REFERENCES flightstates(id),
    FOREIGN KEY (estado_destino_id) REFERENCES flightstates(id)
);

CREATE TABLE flights (
    id                      INT          AUTO_INCREMENT PRIMARY KEY,
    codigo_vuelo            VARCHAR(10)  NOT NULL UNIQUE,
    aerolinea_id            INT          NOT NULL,
    ruta_id                 INT          NOT NULL,
    aeronave_id             INT          NOT NULL,
    fecha_salida            DATETIME     NOT NULL,
    fecha_llegada_estimada  DATETIME     NOT NULL,
    capacidad_total         INT          NOT NULL,
    asientos_disponibles    INT          NOT NULL,
    estado_vuelo_id         INT          NOT NULL,
    reprogramado_en         DATETIME     NULL,
    creado_en               DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    actualizado_en          DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                                  ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (aerolinea_id)    REFERENCES airlines(id),
    FOREIGN KEY (ruta_id)         REFERENCES routes(id),
    FOREIGN KEY (aeronave_id)     REFERENCES aircraft(id),
    FOREIGN KEY (estado_vuelo_id) REFERENCES flightstates(id),
    CONSTRAINT chk_capacidad_total      CHECK (capacidad_total > 0),
    CONSTRAINT chk_asientos_disponibles CHECK (asientos_disponibles >= 0)
);

-- ============================================================
-- 15. ROLES EN VUELO Y ASIGNACIÓN DE TRIPULACIÓN
--     flightroles: solo nombre (sin código redundante).
--     Un empleado puede ser administrativo en tierra o tripulación
--     en vuelo; el rol lo diferencia.
-- ============================================================

CREATE TABLE flightroles (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(100) NOT NULL UNIQUE   -- Comandante, Copiloto, Jefe de Cabina, Auxiliar de Vuelo...
);

-- Un empleado aparece como máximo una vez por vuelo
CREATE TABLE flightassignments (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    vuelo_id        INT NOT NULL,
    personal_id     INT NOT NULL,
    rol_vuelo_id    INT NOT NULL,
    UNIQUE (vuelo_id, personal_id),
    FOREIGN KEY (vuelo_id)      REFERENCES flights(id),
    FOREIGN KEY (personal_id)   REFERENCES staff(id),
    FOREIGN KEY (rol_vuelo_id)  REFERENCES flightroles(id)
);

-- ============================================================
-- 16. ASIENTOS POR VUELO
--     Ventana / Pasillo / Centro + clase de cabina.
--     esta_ocupado se actualiza al asignar asiento en el check-in.
-- ============================================================

CREATE TABLE seatlocationtypes (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL UNIQUE   -- Ventana, Pasillo, Centro
);

CREATE TABLE flightseats (
    id                      INT          AUTO_INCREMENT PRIMARY KEY,
    vuelo_id                INT          NOT NULL,
    codigo_asiento          VARCHAR(5)   NOT NULL,  -- 12A, 14B...
    tipo_cabina_id          INT          NOT NULL,
    tipo_ubicacion_id       INT          NOT NULL,
    esta_ocupado            TINYINT(1)   NOT NULL DEFAULT 0,
    UNIQUE (vuelo_id, codigo_asiento),
    FOREIGN KEY (vuelo_id)          REFERENCES flights(id),
    FOREIGN KEY (tipo_cabina_id)    REFERENCES cabintypes(id),
    FOREIGN KEY (tipo_ubicacion_id) REFERENCES seatlocationtypes(id)
);

-- ============================================================
-- 17. PASAJEROS  (viajeros — extiende people)
-- ============================================================

CREATE TABLE passengers (
    id                  INT AUTO_INCREMENT PRIMARY KEY,
    persona_id          INT NOT NULL UNIQUE,
    tipo_pasajero_id    INT NOT NULL,
    FOREIGN KEY (persona_id)        REFERENCES people(id),
    FOREIGN KEY (tipo_pasajero_id)  REFERENCES passengertypes(id)
);

-- ============================================================
-- 18. RESERVAS
--     Al reservar se asigna cliente + vuelo(s) + pasajero(s).
--     El ASIENTO NO se asigna aquí — se asigna en el check-in.
--     vence_en: Could Have (política de vencimiento de pendientes).
--
--     REGLA DE EMISIÓN DE TIQUETES (Must Have):
--     Los tickets se emiten ÚNICAMENTE cuando:
--       reserva.estado = CONFIRMADA  Y  pago.estado = PAGADO
-- ============================================================

CREATE TABLE reservationstatuses (
    id          INT         AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(50) NOT NULL UNIQUE   -- Pendiente, Confirmada, Cancelada, Vencida
);

-- Transiciones de estado explícitas (Should Have)
CREATE TABLE reservationstatustransitions (
    id                  INT AUTO_INCREMENT PRIMARY KEY,
    estado_origen_id    INT NOT NULL,
    estado_destino_id   INT NOT NULL,
    UNIQUE (estado_origen_id, estado_destino_id),
    FOREIGN KEY (estado_origen_id)  REFERENCES reservationstatuses(id),
    FOREIGN KEY (estado_destino_id) REFERENCES reservationstatuses(id)
);

CREATE TABLE reservations (
    id                  INT            AUTO_INCREMENT PRIMARY KEY,
    codigo_reserva      VARCHAR(30)    NOT NULL UNIQUE,   -- PNR / localizador
    cliente_id          INT            NOT NULL,
    fecha_reserva       DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    estado_reserva_id   INT            NOT NULL,
    valor_total         DECIMAL(18,2)  NOT NULL,
    vence_en            DATETIME       NULL,
    creado_en           DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    actualizado_en      DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP
                                                ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (cliente_id)        REFERENCES clients(id),
    FOREIGN KEY (estado_reserva_id) REFERENCES reservationstatuses(id),
    CONSTRAINT chk_valor_total CHECK (valor_total >= 0)
);

-- Puente: una reserva puede incluir N flights (viaje con conexiones)
CREATE TABLE reservationflights (
    id              INT            AUTO_INCREMENT PRIMARY KEY,
    reserva_id      INT            NOT NULL,
    vuelo_id        INT            NOT NULL,
    valor_parcial   DECIMAL(18,2)  NOT NULL DEFAULT 0,
    UNIQUE (reserva_id, vuelo_id),
    FOREIGN KEY (reserva_id) REFERENCES reservations(id),
    FOREIGN KEY (vuelo_id)   REFERENCES flights(id),
    CONSTRAINT chk_valor_parcial CHECK (valor_parcial >= 0)
);

-- Por cada vuelo en la reserva se registra cada pasajero.
-- El asiento NO se asigna aquí — se asigna en el check-in.
CREATE TABLE reservationpassengers (
    id                  INT AUTO_INCREMENT PRIMARY KEY,
    reserva_vuelo_id    INT NOT NULL,
    pasajero_id         INT NOT NULL,
    UNIQUE (reserva_vuelo_id, pasajero_id),
    FOREIGN KEY (reserva_vuelo_id)  REFERENCES reservationflights(id),
    FOREIGN KEY (pasajero_id)       REFERENCES passengers(id)
);

-- ============================================================
-- 19. TIQUETES
--     Se emiten SOLO desde una reserva confirmada con pago aprobado.
--     Un tiquete por pasajero por vuelo.
-- ============================================================

CREATE TABLE ticketstatuses (
    id          INT         AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(50) NOT NULL UNIQUE   -- Emitido, Anulado, Usado
);

CREATE TABLE tickets (
    id                      INT          AUTO_INCREMENT PRIMARY KEY,
    reserva_pasajero_id     INT          NOT NULL UNIQUE,
    codigo_tiquete          VARCHAR(30)  NOT NULL UNIQUE,
    fecha_emision           DATETIME     NOT NULL,
    estado_tiquete_id       INT          NOT NULL,
    creado_en               DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    actualizado_en          DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                                 ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (reserva_pasajero_id) REFERENCES reservationpassengers(id),
    FOREIGN KEY (estado_tiquete_id)   REFERENCES ticketstatuses(id)
);

-- ============================================================
-- 20. CHECK-IN
--     Es aquí donde se asigna el asiento al pasajero.
--     El agente que atiende es staff del aeropuerto.
-- ============================================================

CREATE TABLE checkinstatuses (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL UNIQUE   -- Pendiente, Realizado, No presentado
);

CREATE TABLE checkins (
    id                          INT            AUTO_INCREMENT PRIMARY KEY,
    tiquete_id                  INT            NOT NULL UNIQUE,
    personal_id                 INT            NOT NULL,   -- agente que atiende
    asiento_vuelo_id            INT            NOT NULL UNIQUE, -- asiento asignado aquí
    fecha_checkin               DATETIME       NOT NULL,
    estado_checkin_id           INT            NOT NULL,
    numero_tarjeta_embarque     VARCHAR(20)    NOT NULL UNIQUE,
    equipaje_bodega             TINYINT(1)     NOT NULL DEFAULT 0,
    peso_equipaje_kg            DECIMAL(5,2)   NULL DEFAULT 0,
    FOREIGN KEY (tiquete_id)         REFERENCES tickets(id),
    FOREIGN KEY (personal_id)        REFERENCES staff(id),
    FOREIGN KEY (asiento_vuelo_id)   REFERENCES flightseats(id),
    FOREIGN KEY (estado_checkin_id)  REFERENCES checkinstatuses(id),
    CONSTRAINT chk_peso_equipaje CHECK (peso_equipaje_kg >= 0)
);

-- ============================================================
-- 21. FACTURAS E ÍTEMS DE FACTURA
--     La factura se genera a partir de una reserva.
--     Los ítems permiten detallar el valor base del tiquete
--     más extras: equipaje adicional, upgrade de cabina,
--     selección de asiento, comidas especiales, etc.
-- ============================================================

CREATE TABLE invoiceitemtypes (
    id      INT          AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(100) NOT NULL UNIQUE   -- Tiquete base, Equipaje adicional, Upgrade cabina...
);

CREATE TABLE invoices (
    id              INT            AUTO_INCREMENT PRIMARY KEY,
    reserva_id      INT            NOT NULL UNIQUE,
    numero_factura  VARCHAR(30)    NOT NULL UNIQUE,
    fecha_emision   DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    subtotal        DECIMAL(18,2)  NOT NULL DEFAULT 0,
    impuestos       DECIMAL(18,2)  NOT NULL DEFAULT 0,
    total           DECIMAL(18,2)  NOT NULL DEFAULT 0,
    creado_en       DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (reserva_id) REFERENCES reservations(id),
    CONSTRAINT chk_subtotal  CHECK (subtotal  >= 0),
    CONSTRAINT chk_impuestos CHECK (impuestos >= 0),
    CONSTRAINT chk_total_fac CHECK (total     >= 0)
);

CREATE TABLE invoiceitems (
    id                      INT            AUTO_INCREMENT PRIMARY KEY,
    factura_id              INT            NOT NULL,
    tipo_item_id            INT            NOT NULL,
    descripcion             VARCHAR(200)   NOT NULL,
    cantidad                INT            NOT NULL DEFAULT 1,
    precio_unitario         DECIMAL(18,2)  NOT NULL,
    subtotal                DECIMAL(18,2)  NOT NULL,   -- cantidad * precio_unitario
    reserva_pasajero_id     INT            NULL,        -- ítem ligado a pasajero específico
    FOREIGN KEY (factura_id)           REFERENCES invoices(id),
    FOREIGN KEY (tipo_item_id)         REFERENCES invoiceitemtypes(id),
    FOREIGN KEY (reserva_pasajero_id)  REFERENCES reservationpassengers(id),
    CONSTRAINT chk_item_cantidad  CHECK (cantidad        >= 1),
    CONSTRAINT chk_item_precio    CHECK (precio_unitario >= 0),
    CONSTRAINT chk_item_subtotal  CHECK (subtotal        >= 0)
);

-- ============================================================
-- 22. PAGOS
--     Estados simulados: Pendiente, Pagado, Rechazado, Reembolsado.
--     La emisión de tickets requiere estado_pago = Pagado.
-- ============================================================

CREATE TABLE paymentstates (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL UNIQUE   -- Pendiente, Pagado, Rechazado, Reembolsado
);

CREATE TABLE paymentmethodtypes (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL UNIQUE   -- Efectivo, Tarjeta, Transferencia, PSE
);

CREATE TABLE cardtypes (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL UNIQUE   -- Crédito, Débito, Prepago
);

CREATE TABLE cardissuers (
    id      INT         AUTO_INCREMENT PRIMARY KEY,
    nombre  VARCHAR(50) NOT NULL UNIQUE   -- Visa, Mastercard, Amex, Diners
);

CREATE TABLE paymentmethods (
    id                      INT          AUTO_INCREMENT PRIMARY KEY,
    tipo_medio_pago_id      INT          NOT NULL,
    tipo_tarjeta_id         INT          NULL,
    emisor_tarjeta_id       INT          NULL,
    nombre_comercial        VARCHAR(50)  NOT NULL UNIQUE,
    FOREIGN KEY (tipo_medio_pago_id) REFERENCES paymentmethodtypes(id),
    FOREIGN KEY (tipo_tarjeta_id)    REFERENCES cardtypes(id),
    FOREIGN KEY (emisor_tarjeta_id)  REFERENCES cardissuers(id)
);

CREATE TABLE payments (
    id                  INT            AUTO_INCREMENT PRIMARY KEY,
    reserva_id          INT            NOT NULL,
    monto               DECIMAL(18,2)  NOT NULL,
    fecha_pago          DATETIME       NOT NULL,
    estado_pago_id      INT            NOT NULL,
    metodo_pago_id      INT            NOT NULL,
    creado_en           DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    actualizado_en      DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP
                                                ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (reserva_id)       REFERENCES reservations(id),
    FOREIGN KEY (estado_pago_id)   REFERENCES paymentstates(id),
    FOREIGN KEY (metodo_pago_id)   REFERENCES paymentmethods(id),
    CONSTRAINT chk_monto_pago CHECK (monto >= 0)
);

-- ============================================================
-- FIN DEL SCRIPT — 56 tablas
-- ============================================================

-- ============================================================
-- 23. AUTENTICACIÓN Y AUTORIZACIÓN
--     Módulo de login para consola.
--     Un usuario puede estar vinculado a una persona (cliente
--     o staff) o ser un usuario técnico sin persona asociada.
--     Roles: Admin, Agente, Cliente — definen qué menús ve cada uno.
--     Las sessions registran cada inicio de sesión para trazabilidad.
-- ============================================================

-- Roles del sistema (Admin, Agente, Cliente)
CREATE TABLE systemroles (
    id          INT         AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(50) NOT NULL UNIQUE,      -- Admin, Agente, Cliente
    descripcion VARCHAR(150) NULL
);

-- Permisos granulares por módulo
CREATE TABLE permissions (
    id          INT          AUTO_INCREMENT PRIMARY KEY,
    nombre      VARCHAR(100) NOT NULL UNIQUE,     -- CREAR_VUELO, CANCELAR_RESERVA...
    descripcion VARCHAR(200) NULL
);

-- Qué permissions tiene cada rol
CREATE TABLE rolepermissions (
    id          INT AUTO_INCREMENT PRIMARY KEY,
    rol_id      INT NOT NULL,
    permiso_id  INT NOT NULL,
    UNIQUE (rol_id, permiso_id),
    FOREIGN KEY (rol_id)     REFERENCES systemroles(id),
    FOREIGN KEY (permiso_id) REFERENCES permissions(id)
);

-- Cuenta de acceso al sistema.
-- persona_id puede ser NULL para users técnicos / superadmin.
CREATE TABLE users (
    id              INT          AUTO_INCREMENT PRIMARY KEY,
    username        VARCHAR(50)  NOT NULL UNIQUE,
    password_hash   VARCHAR(255) NOT NULL,         -- bcrypt / SHA-256
    persona_id      INT          NULL UNIQUE,      -- vinculado a cliente o staff
    rol_id          INT          NOT NULL,
    activo          TINYINT(1)   NOT NULL DEFAULT 1,
    ultimo_acceso   DATETIME     NULL,
    creado_en       DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    actualizado_en  DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP
                                         ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (persona_id) REFERENCES people(id),
    FOREIGN KEY (rol_id)     REFERENCES systemroles(id)
);

-- Registro de cada inicio / cierre de sesión (trazabilidad)
CREATE TABLE sessions (
    id          INT         AUTO_INCREMENT PRIMARY KEY,
    usuario_id  INT         NOT NULL,
    iniciada_en DATETIME    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    cerrada_en  DATETIME    NULL,                  -- NULL = sesión activa
    ip_origen   VARCHAR(45) NULL,                  -- IPv4 o IPv6
    activa      TINYINT(1)  NOT NULL DEFAULT 1,
    FOREIGN KEY (usuario_id) REFERENCES users(id)
);

-- ============================================================
-- FIN DEL SCRIPT — 61 tablas
-- ============================================================
