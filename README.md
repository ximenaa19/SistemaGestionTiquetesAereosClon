# Sistema de Gestión de Tiquetes Aéreos (Consola)

Aplicación de consola en `.NET` para gestionar operación aérea, comercial y administrativa de una aerolínea: personas/usuarios, aerolíneas, aeropuertos, rutas, vuelos, reservas, pagos, tiquetes, check-ins, catálogos y reportes con LINQ.

## Tabla de contenido
- [Descripción](#descripción)
- [Funcionalidades principales](#funcionalidades-principales)
- [Arquitectura](#arquitectura)
- [Tecnologías](#tecnologías)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Requisitos previos](#requisitos-previos)
- [Instalación y configuración](#instalación-y-configuración)
- [Migraciones y base de datos](#migraciones-y-base-de-datos)
- [Ejecución](#ejecución)
- [Manual de usuario (consola)](#manual-de-usuario-consola)
- [Guia exhaustiva del menu cliente](#guia-exhaustiva-del-menu-cliente)
- [Reportes LINQ](#reportes-linq)
- [Guía técnica para desarrollo](#guía-técnica-para-desarrollo)
- [Troubleshooting](#troubleshooting)

## Descripción

Este sistema implementa un flujo de negocio académico de aerolínea, con separación por módulos y capas para mantener un diseño limpio y extensible.  
Permite operar datos maestros (catálogos), entidades transaccionales y menús por rol (`Admin`, `Agente/Staff`, `Cliente`), todo desde consola.

## Funcionalidades principales

### Autenticación y acceso por rol
- Registro/Login de usuario.
- Enrutamiento de menú según rol del usuario autenticado.
- Flujo diferenciado para `Admin`, `Staff/Agente` y `Cliente`.

### Gestión administrativa (Admin)
- CRUD principal sobre:
  - Personas
  - Aerolíneas
  - Aeropuertos
  - Rutas
  - Aeronaves
- Menús secundarios para operación aérea, comercial, seguridad, catálogos y sistema.
- Ejecución de seeders (maestros + catálogos) desde menú.

### Gestión operacional/comercial
- Vuelos, asignaciones, asientos, tarifas, reservas, pasajeros de reserva.
- Pagos, tiquetes, facturas e ítems de factura.
- Check-ins y estados de proceso.

### Catálogos y datos maestros
- Catálogos idempotentes (roles, permisos, estados, tipos).
- Maestros geográficos (continentes, países, regiones, ciudades, aeropuertos).
- Métodos de pago y catálogos de soporte.

### Reportería con LINQ
- Vuelos con mayor ocupación.
- Vuelos con asientos disponibles.
- Clientes con más reservas.
- Destinos más solicitados.
- Reservas por estado.
- Ingresos estimados por aerolínea.
- Tiquetes emitidos por rango de fechas.

## Arquitectura

El proyecto sigue una organización modular con enfoque de capas (estilo hexagonal/clean por módulo):

- `Domain`: entidades/agregados, value objects y contratos de repositorio.
- `Application`: casos de uso, validadores y servicios de aplicación.
- `Infrastructure`: persistencia EF Core (entities/configuration/repositories).
- `UI`: menús y flujos de consola.

Cada módulo en `src/Modules/*` encapsula su responsabilidad y se integra desde `Program.cs`.

## Tecnologías

- `.NET 10` (`net10.0`)
- `C#`
- `Entity Framework Core 9`
- `Pomelo.EntityFrameworkCore.MySql` (MySQL)
- `BCrypt.Net-Next` (hash de contraseñas)
- `Microsoft.Extensions.Configuration` (JSON + variables de entorno)

## Estructura del proyecto

```text
.
├─ Program.cs
├─ appsettings.json
├─ GestionAerolineas.csproj
├─ Migrations/
└─ src/
   ├─ Modules/
   │  ├─ Auth/
   │  ├─ Users/
   │  ├─ People/
   │  ├─ Airlines/
   │  ├─ Airports/
   │  ├─ Routes/
   │  ├─ Aircraft/
   │  ├─ Flights/
   │  ├─ Reservations/
   │  ├─ Payments/
   │  ├─ Tickets/
   │  ├─ Reports/
   │  └─ ... (catálogos y módulos de soporte)
   └─ shared/
      ├─ Context/
      ├─ Helpers/
      ├─ Seed/
      └─ Ui/
```

## Requisitos previos

- SDK `.NET 10` instalado.
- MySQL 8+ disponible.
- Herramienta EF CLI:

```bash
dotnet tool install --global dotnet-ef
```

> Si ya está instalada:
>
> ```bash
> dotnet tool update --global dotnet-ef
> ```

## Instalación y configuración

1. Clonar repositorio:

```bash
git clone <URL_DEL_REPOSITORIO>
cd SistemaGestionTiquetesAereos
```

2. Configurar conexión a BD:
   - Opción A: editar `appsettings.json` (`ConnectionStrings:MySqlDB`).
   - Opción B (recomendada): variable de entorno `MYSQL_CONNECTION` (tiene prioridad).

Ejemplo:

```bash
MYSQL_CONNECTION="server=localhost;port=3306;database=airlinesdb;user=root;password=1234;"
```

3. Restaurar paquetes:

```bash
dotnet restore
```

## Migraciones y base de datos

Aplicar migraciones existentes:

```bash
dotnet ef database update --context AppDbContext --project .\GestionAerolineas.csproj --startup-project .\GestionAerolineas.csproj
```

Crear una migración nueva:

```bash
dotnet ef migrations add NombreMigracion --context AppDbContext --project .\GestionAerolineas.csproj --startup-project .\GestionAerolineas.csproj
```

## Ejecución

Compilar:

```bash
dotnet build .\GestionAerolineas.csproj
```

Ejecutar:

```bash
dotnet run --project .\GestionAerolineas.csproj
```

## Manual de usuario (consola)

### 1) Inicio de sesión
- Al iniciar, se muestra el menú de autenticación (`Register user`, `Login`, `Salir`).
- Inicia sesión con usuario y contraseña.
- El sistema enruta al menú según el rol.

### 2) Flujo recomendado para datos consistentes
- Para cuentas operativas completas, crear usuarios desde el flujo de `Crear persona` (Admin), porque enlaza persona/cliente/staff según corresponda.
- El registro rápido de `Register user` puede crear usuario sin vínculo de persona, lo cual limita algunos menús dependientes de esa relación.

### 3) Menú Admin
- Menú principal:
  - Persona
  - Aerolínea
  - Aeropuerto
  - Ruta
  - Aeronave
  - Reportes (LINQ)
  - Menú secundario
- Cada opción principal contiene su CRUD.

### 4) Menú Cliente
- Acceso a consultas y operaciones propias:
  - Vuelos disponibles
  - Mis reservas / detalle
  - Mis tiquetes
  - Mis pagos
  - Check-in
  - Perfil básico (correo/teléfono)

### 5) Seeders desde el sistema
- En `Admin -> Sistema` puedes ejecutar seed de maestros y catálogos.

## Guia del menu cliente

Esta guia describe TODO el flujo que puede seguir un usuario con rol `Cliente`.

### 0) Requisito obligatorio antes de entrar
- El usuario debe existir en `users`.
- Ese usuario debe tener `persona_id` valido.
- Esa persona debe tener registro en `customers`.
- Si falta alguno, veras: `No se encontro persona asociada...` o `...no tiene registro en customers`.

### 1) Como entrar al menu cliente
1. Ejecuta el proyecto:
   - `dotnet run --project .\GestionAerolineas.csproj`
2. En menu inicial elige `Login`.
3. Ingresa `username` y `contrasenia`.
4. Si el rol es `Cliente/Customer`, el sistema abre `MENU CLIENTE`.

### 2) Opciones del MENU CLIENTE (una por una)

#### 2.1 Ver vuelos disponibles
- Que hace: abre el modulo de vuelos para consulta.
- Cuando usarlo: para revisar oferta antes de reservar.
- Resultado esperado: listado de vuelos existentes segun datos cargados.

#### 2.2 Crear reserva (wizard simple)
- Que hace: te redirige al modulo de reservas.
- Importante: el sistema te muestra tu `customer_id`; usa ese valor cuando el formulario lo pida.
- Flujo recomendado:
  1. Crear reserva base.
  2. Asociar vuelo(s) a la reserva (`reservation_flights`).
  3. Asociar pasajero(s) (`reservation_passengers`).
- Si te pide busqueda de pasajero:
  - `Buscar pasajero` es texto (nombre/apellido), no id.
  - `Ingrese pasajero_id` si es el id numerico real de `passengers.id`.
  - Dejar vacio en `pasajero_id` finaliza la carga de pasajeros.

#### 2.3 Mis reservas
- Que hace: lista solo reservas del cliente logueado.
- Muestra: `id`, `PNR`, estado, total y fecha.
- Si no hay reservas: muestra mensaje de lista vacia.

#### 2.4 Ver detalle de reserva
- Que hace: muestra detalle de una reserva puntual.
- Entrada: `reservation_id`.
- Validacion clave: solo permite ver reservas que sean tuyas.
- Muestra: datos de reserva, cantidad de vuelos asociados, cantidad de pasajeros asociados.

#### 2.5 Mis tiquetes
- Que hace: lista tiquetes asociados a tus reservas.
- Base de busqueda: PNR/codigo de reserva propio.
- Muestra: `ticket_id`, codigo de tiquete, estado, fecha de emision, PNR.

#### 2.6 Mis pagos
- Que hace: lista pagos asociados a tus reservas.
- Muestra: `payment_id`, monto, estado de pago, metodo de pago, fecha y PNR.

#### 2.7 Hacer check-in
- Que hace: abre modulo de check-in.
- Si hay `passenger_id` vinculado, el menu muestra ese contexto para facilitar el flujo.
- Si no hay `passenger_id`, aun puedes entrar al modulo general y operar manualmente.

#### 2.8 Cancelar reserva
- Que hace: intenta cambiar estado de una reserva propia a `Cancelada`.
- Flujo:
  1. Lista tus reservas.
  2. Pide `reservation_id`.
  3. Valida que la reserva sea tuya.
  4. Busca estado `Cancelada` en catalogos y ejecuta transicion.
- Posibles bloqueos:
  - No existe estado `Cancelada`.
  - La transicion de estados no esta permitida por reglas.
  - Reserva no pertenece al cliente.

#### 2.9 Actualizar mi perfil basico
- Que hace: abre submenu de perfil para mantener datos de contacto.
- Submenu esperado:
  - Gestionar correos (tabla `personemails`)
  - Gestionar telefonos (tabla `personphones`)
- Importante: usa siempre tu `person_id` cuando el flujo lo solicite.

##### 2.9.1 Dentro de "Gestionar correos"
- Flujo comun del modulo:
  1. Crear correo: pide `person_id`, usuario de correo y dominio.
  2. Listar correos: muestra los registros existentes.
  3. Buscar/filtrar (si aplica): por id o por persona.
  4. Actualizar: seleccionas id y cambias datos.
  5. Eliminar: seleccionas id y confirmas.
- Recomendacion: mantener solo un correo principal activo por persona, si el flujo te permite marcar principal.

##### 2.9.2 Dentro de "Gestionar telefonos"
- Flujo comun del modulo:
  1. Crear telefono: pide `person_id`, codigo de pais y numero.
  2. Listar telefonos por persona.
  3. Actualizar telefono existente por id.
  4. Eliminar telefono por id.
- Recomendacion: validar que el numero quede sin espacios y con formato consistente.

#### 2.10 Menu secundario
- Que hace: abre accesos secundarios del rol cliente (modulos completos expuestos para pruebas/uso extendido).
- Nota: aqui veras modulos adicionales; la ruta principal recomendada sigue siendo el menu cliente base.

##### 2.10.1 Vuelos
- Permite consultar/listar vuelos del sistema.
- En algunos flujos puedes filtrar por estado, origen, destino o fecha (segun datos disponibles del modulo).

##### 2.10.2 Reservas (modulo completo)
- CRUD completo de reservas.
- Flujo tecnico recomendado:
  1. Crear reserva base (`reservations`).
  2. Relacionar vuelo(s) en `reservation_flights`.
  3. Relacionar pasajero(s) en `reservation_passengers`.
  4. Confirmar estado y total.
- Si el modulo pide `customer_id`, usa el que muestra tu contexto de cliente.

##### 2.10.3 Reservas por vuelo
- Gestiona relaciones `reservation_flights`.
- Lo que pasa dentro:
  - Crear relacion reserva-vuelo (`reservation_id`, `flight_id`, `valor_parcial`).
  - Consultar/listar por reserva o por vuelo.
  - Actualizar o eliminar relacion existente.

##### 2.10.4 Pasajeros por reserva
- Gestiona relaciones `reservation_passengers`.
- Lo que pasa dentro:
  - Asociar pasajero a `reservation_flight_id`.
  - Listar pasajeros de una reserva o de un vuelo de reserva.
  - Actualizar estado/datos de relacion (si el modulo lo expone).
  - Eliminar asociacion.
- Importante: `Buscar pasajero` usa texto; `pasajero_id` usa id numerico real.

##### 2.10.5 Tiquetes (modulo completo)
- CRUD de tiquetes asociados a reservas/pasajeros.
- Acciones comunes:
  - Crear tiquete (con `reserva_pasajero_id`, estado y fecha de emision).
  - Consultar/listar tiquetes.
  - Actualizar estado de tiquete.
  - Eliminar/anular segun reglas del modulo.

##### 2.10.6 Pagos (modulo completo)
- CRUD de pagos.
- Acciones comunes:
  - Crear pago (`reserva_id` o PNR, metodo, estado, monto, fecha).
  - Listar pagos.
  - Actualizar estado/metodo/monto.
  - Eliminar registro (si permitido).

##### 2.10.7 Check-ins (modulo completo)
- Gestion de check-ins por pasajero/reserva.
- Acciones comunes:
  - Crear check-in.
  - Consultar check-ins.
  - Cambiar estado de check-in.
  - Eliminar check-in (si permitido).

##### 2.10.8 Clientes (modulo completo)
- Modulo administrativo de clientes.
- Desde perfil cliente normalmente se usa para consulta; evita editar registros de terceros.
- Si haces pruebas, usa solo tu `customer_id` para no mezclar datos.

### 2.11 Flujo completo recomendado (cliente, de inicio a fin)
1. Login como cliente.
2. `Ver vuelos disponibles` y anotar `flight_id`.
3. `Crear reserva` con tu `customer_id`.
4. Entrar a `Reservas por vuelo` y asociar el vuelo a la reserva.
5. Entrar a `Pasajeros por reserva` y asociar pasajero(s) al `reservation_flight_id`.
6. Verificar en `Mis reservas` y `Detalle de reserva`.
7. Registrar pago y verificar en `Mis pagos`.
8. Revisar/emitir tiquete y verificar en `Mis tiquetes`.
9. Hacer check-in cuando aplique.
10. Si corresponde, cancelar reserva y validar cambio de estado.

### 3) Atajos y navegacion
- En menus con flechas:
  - `↑` y `↓` para mover seleccion.
  - `Enter` para ejecutar opcion.
  - `Esc` para volver/salir.
- En formularios:
  - Sigue mensajes de validacion.
  - Cuando una validacion falla, normalmente se repite solo ese campo.

### 4) Mini checklist de prueba funcional del cliente
1. Login cliente exitoso.
2. Ver vuelos disponibles.
3. Crear una reserva completa (reserva + vuelo + pasajero).
4. Verla en `Mis reservas`.
5. Consultar `Detalle de reserva`.
6. Revisar `Mis tiquetes` y `Mis pagos`.
7. Probar `Cancelar reserva`.
8. Actualizar correo/telefono en `Perfil basico`.

## Reportes LINQ

Los reportes se encuentran en el módulo `Reports` y se consumen desde `Admin -> Reportes (LINQ)`.  
Implementan consultas con:
- filtros (`Where`)
- ordenamientos (`OrderBy`)
- agrupaciones (`GroupBy`)
- conteos/sumatorias (`Count`, `Sum`)
- proyecciones (`Select`)

## Guía técnica para desarrollo

### Convenciones del proyecto
- Crear funcionalidades nuevas por módulo en `src/Modules/<Modulo>`.
- Mantener separación por capas: `Domain`, `Application`, `Infrastructure`, `UI`.
- Reutilizar casos de uso y validadores existentes antes de agregar lógica nueva.

### Agregar una funcionalidad nueva (resumen)
1. Definir/ajustar entidad de dominio y contratos.
2. Implementar caso(s) de uso en `Application`.
3. Implementar repositorio EF en `Infrastructure`.
4. Exponer flujo en `UI`.
5. Registrar/enlazar en `Program.cs` o módulo correspondiente.
6. Si aplica, agregar migración y actualizar seeders.

## Troubleshooting

### Error: `No se encontro persona asociada al usuario cliente`
El usuario autenticó, pero `users.persona_id` está `NULL` o no existe relación en `customers`.  
Solución:
- Crear el usuario desde `Crear persona`, o
- Enlazar manualmente `users.persona_id` y garantizar registro en `customers`.

### Error al migrar: `Table 'users' already exists`
Suele ocurrir por desalineación entre esquema real e historial de migraciones.  
Revisar:
- Estado de tabla `__EFMigrationsHistory`
- Orden/migraciones aplicadas
- Consistencia entre BD actual y proyecto.

### Seed duplicado o clave única
Los seeders están diseñados para ser idempotentes por nombre/código normalizado.  
Si hubo datos de prueba inconsistentes previos, limpia datos conflictivos antes de re-ejecutar.

---

Si quieres, en el siguiente paso te preparo también una versión de este README con capturas/diagramas (flujo de login + menús por rol + mapa de módulos) para entrega académica.

## Guía de entrega académica

Esta sección está pensada para que puedas sustentar el proyecto en clase de forma ordenada y con evidencia verificable.

### 1) Checklist de evidencias (rúbrica)

Usa esta lista como validación final antes de presentar:

- [ ] El sistema compila sin errores (`dotnet build`).
- [ ] La conexión a MySQL funciona y arranca la app (`dotnet run`).
- [ ] Se evidencia arquitectura modular por capas en `src/Modules`.
- [ ] Se muestran CRUD principales (Persona, Aerolínea, Aeropuerto, Ruta, Aeronave).
- [ ] Se ejecuta seed de catálogos/maestros desde menú de sistema.
- [ ] Se demuestra login por rol y cambio de menú por rol.
- [ ] Se ejecutan reportes LINQ desde menú Admin.
- [ ] Se muestran consultas reales en BD (Workbench o SQL CLI).

### 2) Script de demo (10–15 min)

#### Bloque A — Arranque técnico (2 min)
1. Mostrar `appsettings.json` o variable `MYSQL_CONNECTION`.
2. Ejecutar:

```bash
dotnet build .\GestionAerolineas.csproj
dotnet run --project .\GestionAerolineas.csproj
```

3. Enseñar mensaje de conexión exitosa.

#### Bloque B — Seguridad y acceso por rol (2 min)
1. Hacer login con usuario `Admin`.
2. Mostrar que aparece `ADMIN - MENU PRINCIPAL`.
3. (Opcional) Login con `Cliente` para mostrar menú distinto.

#### Bloque C — CRUD principal (4–5 min)
1. Entrar a `Persona` y crear una persona.
2. Entrar a `Aerolínea` y crear una aerolínea.
3. Entrar a `Aeropuerto` y crear aeropuerto.
4. Entrar a `Ruta` y crear ruta.
5. Entrar a `Aeronave` y crear aeronave.
6. Mostrar una actualización y una eliminación controlada.

#### Bloque D — LINQ reportes (3–4 min)
1. Ir a `Reportes (LINQ)`.
2. Ejecutar al menos 3 reportes:
   - Vuelos con mayor ocupación
   - Clientes con más reservas
   - Tiquetes por rango de fechas
3. Mostrar resultados por consola y confirmar con SQL en Workbench.

### 3) Script SQL de verificación rápida

> Ajusta nombres si tu esquema usa columnas en español (por ejemplo `fecha_emision` en vez de `issued_at`).

```sql
-- Usuarios y relación a persona
SELECT id, username, persona_id, rol_id
FROM users
ORDER BY id DESC
LIMIT 20;

-- Personas creadas recientemente
SELECT id, nombres, apellidos, tipo_documento_id, numero_documento
FROM people
ORDER BY id DESC
LIMIT 20;

-- Rutas y aeropuertos
SELECT r.id, r.codigo, r.aeropuerto_origen_id, r.aeropuerto_destino_id
FROM routes r
ORDER BY r.id DESC
LIMIT 20;

-- Tiquetes por rango
SELECT COUNT(*) AS tickets_en_rango
FROM tickets
WHERE fecha_emision >= '2020-01-01'
  AND fecha_emision <= '2026-12-12 23:59:59';
```

### 4) Evidencias recomendadas para entregar

- Captura de `dotnet build` exitoso.
- Captura de login y menú por rol.
- Captura de cada CRUD principal (create/update/delete).
- Captura de 2–3 reportes LINQ ejecutados.
- Captura de verificación en Workbench.
- (Opcional) video corto 3–5 min de flujo completo.

### 5) Riesgos frecuentes y respuesta en sustentación

- **“Login ok pero no entra a menú cliente”**  
  Respuesta: validar vínculo `users.persona_id` y existencia en `customers`.

- **“No salen datos en reportes”**  
  Respuesta: revisar seed/datos transaccionales y rango de fechas consultado.

- **“Falla una migración por tabla existente”**  
  Respuesta: validar consistencia entre esquema actual y `__EFMigrationsHistory`.

### 6) Guion corto para explicar arquitectura

“El proyecto está organizado por módulos de negocio. Cada módulo separa dominio, aplicación, infraestructura y UI de consola.  
Los casos de uso viven en `Application`, la persistencia EF en `Infrastructure`, y el `Program.cs` orquesta menús y composición del sistema.  
Esto permite escalar funcionalidades sin mezclar lógica de negocio con entrada/salida de consola.”
