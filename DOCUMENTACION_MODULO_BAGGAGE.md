# Documentacion detallada del modulo Baggage: equipaje y recargos

Proyecto: `GestionAerolineas`  
Modulo: `src/Modules/Baggage`  
Tecnologias: C#/.NET, Entity Framework Core, MySQL, consola, arquitectura hexagonal y organizacion por modulos/vertical slices.

---

## 1. Resumen rapido para sustentacion

El modulo `Baggage` permite registrar equipaje asociado a una reserva o a un tiquete, calcular automaticamente si hay recargos por exceso de cantidad o peso, guardar el registro en la tabla `baggage_records` y actualizar el valor total de la reserva cuando se genera un recargo.

Tambien permite consultar:

- Equipaje registrado por cliente.
- Equipaje registrado por vuelo.
- Registros que tuvieron recargos.
- Equipaje y recargos del cliente autenticado, sin pedirle manualmente su `customer_id`.

La idea central es esta:

1. El usuario elige registrar equipaje por reserva o por tiquete.
2. El sistema busca el contexto relacionado: reserva, tiquete, vuelo, pasajero y total actual.
3. El usuario selecciona cabina, tipo de equipaje, cantidad y peso por maleta.
4. El sistema calcula el peso total.
5. El sistema aplica reglas de negocio segun cabina y tipo de equipaje.
6. El sistema muestra una previsualizacion del recargo.
7. Si el usuario confirma, se guarda el equipaje.
8. Si hay recargo, se suma al total de la reserva.

---

## 2. Como esta organizado el modulo

Ruta principal:

```text
src/Modules/Baggage
```

Estructura:

```text
Baggage
|-- Domain
|   |-- Aggregate
|   |   `-- BaggageRecord.cs
|   |-- ValueObject
|   |   |-- BaggageId.cs
|   |   |-- BaggageReservationId.cs
|   |   |-- BaggageCabinTypeId.cs
|   |   |-- BaggageType.cs
|   |   |-- BaggageQuantity.cs
|   |   |-- BaggageWeightKg.cs
|   |   |-- BaggageSurchargeAmount.cs
|   |   `-- BaggageRegisteredAt.cs
|   |-- Models
|   |   |-- BaggagePolicy.cs
|   |   |-- BaggageSurchargeResult.cs
|   |   |-- RegisterBaggageResult.cs
|   |   |-- BaggageRegistrationContext.cs
|   |   |-- BaggageRecordView.cs
|   |   `-- CabinTypeOption.cs
|   `-- Repositories
|       `-- IBaggageRecordRepository.cs
|-- Application
|   |-- Interfaces
|   |   `-- IBaggageValidator.cs
|   |-- Services
|   |   |-- BaggageValidator.cs
|   |   `-- BaggageSurchargeCalculator.cs
|   `-- UseCases
|       |-- RegisterBaggageUseCase.cs
|       |-- PreviewBaggageSurchargeUseCase.cs
|       |-- GetBaggageByCustomerUseCase.cs
|       |-- GetBaggageByFlightUseCase.cs
|       |-- GetBaggageSurchargesUseCase.cs
|       |-- GetCabinTypesForBaggageUseCase.cs
|       `-- GetBaggageRegistrationContextUseCase.cs
|-- Infrastructure
|   |-- Entity
|   |   |-- BaggageRecordEntity.cs
|   |   `-- BaggageRecordEntityConfiguration.cs
|   `-- Repository
|       `-- BaggageRecordRepository.cs
|-- UI
|   |-- BaggageMenu.cs
|   |-- CustomerBaggageMenu.cs
|   `-- ConsoleMenu.cs
`-- BaggageModule.cs
```

Ademas, el modulo se conecta con:

- `src/shared/Context/AppDbContext.cs`
- `Program.cs`
- `Migrations/20260426193740_BaggageMigration.cs`
- `Migrations/AppDbContextModelSnapshot.cs`

---

## 3. Arquitectura usada

El proyecto sigue una arquitectura por capas dentro de cada modulo.

### 3.1 Domain

La capa `Domain` contiene las reglas y conceptos propios del negocio. Aqui estan:

- La entidad principal `BaggageRecord`.
- Los value objects que validan datos importantes.
- Los modelos de resultado o consulta.
- El contrato del repositorio.

Esta capa no deberia depender de Entity Framework ni de consola. Su objetivo es representar el negocio.

### 3.2 Application

La capa `Application` contiene lo que el sistema puede hacer. Aqui estan:

- Casos de uso: registrar equipaje, consultar equipaje, previsualizar recargos.
- Servicios de aplicacion: validar entrada y calcular recargos.
- Interfaces que abstraen validaciones.

Esta capa coordina el flujo, pero no sabe detalles concretos de MySQL ni de menus de consola.

### 3.3 Infrastructure

La capa `Infrastructure` conecta el modulo con tecnologias externas:

- Entity Framework Core.
- Base de datos MySQL.
- Entidad plana `BaggageRecordEntity`.
- Configuracion de tabla, columnas, indices y relaciones.
- Repositorio real que ejecuta consultas LINQ contra `AppDbContext`.

### 3.4 UI

La capa `UI` contiene menus de consola:

- Menu para Admin/Staff.
- Menu para Cliente autenticado.

Esta capa lee datos por consola, llama casos de uso y muestra resultados.

### 3.5 BaggageModule

`BaggageModule.cs` es la clase de composicion. Su trabajo es armar todas las dependencias:

- Repositorio.
- Validador.
- Calculadora.
- Casos de uso.
- Menu correspondiente.

---

## 4. Orden recomendado de creacion del modulo

Este es el orden logico para explicar como se construyo el modulo:

1. Leer requisitos del examen.
2. Identificar entidades relacionadas existentes: reservas, tiquetes, pasajeros, vuelos, cabinas y clientes.
3. Crear el agregado de dominio `BaggageRecord`.
4. Crear value objects para evitar datos invalidos.
5. Crear modelos auxiliares de dominio.
6. Crear contrato del repositorio `IBaggageRecordRepository`.
7. Crear interfaz de validacion `IBaggageValidator`.
8. Crear servicio de validacion `BaggageValidator`.
9. Crear calculadora de recargos `BaggageSurchargeCalculator`.
10. Crear casos de uso de registro, previsualizacion y consultas.
11. Crear entidad EF `BaggageRecordEntity`.
12. Crear configuracion EF `BaggageRecordEntityConfiguration`.
13. Crear repositorio EF `BaggageRecordRepository`.
14. Registrar `DbSet<BaggageRecordEntity>` en `AppDbContext`.
15. Crear migracion `BaggageMigration`.
16. Crear menus de consola.
17. Crear `BaggageModule.cs`.
18. Integrar el modulo en `Program.cs`.
19. Compilar y probar flujos por Admin/Staff/Cliente.

---

## 5. Domain: capa de dominio

La capa de dominio representa el negocio puro. En este modulo, el negocio es: "un registro de equipaje pertenece a una reserva, puede estar asociado a un tiquete, pasajero y vuelo, tiene una cabina y tipo de equipaje, y puede generar recargos".

---

## 5.1 Domain/Aggregate/BaggageRecord.cs

Archivo:

```text
src/Modules/Baggage/Domain/Aggregate/BaggageRecord.cs
```

### Para que fue creado

`BaggageRecord` es la entidad principal del modulo. Representa un registro de equipaje guardado o por guardar. Es el objeto de dominio que concentra:

- Reserva relacionada.
- Tiquete relacionado, si aplica.
- Pasajero y vuelo relacionados, si se pudieron resolver.
- Cabina seleccionada.
- Tipo de equipaje.
- Cantidad de maletas.
- Peso total.
- Politica aplicada.
- Excesos.
- Recargos.
- Fecha de registro.

### Por que es un aggregate

En arquitectura de dominio, un agregado es una entidad importante que protege la consistencia de sus datos. Aqui se protege que:

- El id de reserva sea valido.
- El tipo de equipaje sea valido.
- La cantidad sea positiva.
- El peso sea positivo.
- Los recargos no sean negativos.
- La fecha exista.

Eso se logra usando value objects.

### Codigo explicado

```csharp
using GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;
```

Importa los value objects del modulo. La clase no trabaja directamente con todos los primitivos; por ejemplo, no guarda solo `int Quantity`, sino `BaggageQuantity Quantity`.

```csharp
public class BaggageRecord
{
    public BaggageId Id { get; private set; }
    public BaggageReservationId ReservationId { get; private set; }
    public int? TicketId { get; private set; }
    public int? ReservationPassengerId { get; private set; }
    public int? FlightId { get; private set; }
    public int? PassengerId { get; private set; }
```

Estas propiedades identifican a que esta asociado el registro:

- `Id`: identificador del registro de equipaje.
- `ReservationId`: obligatorio, porque todo equipaje pertenece a una reserva.
- `TicketId`: opcional, porque se puede registrar por reserva sin tiquete especifico.
- `ReservationPassengerId`: opcional, relacion entre reserva y pasajero.
- `FlightId`: opcional, vuelo asociado.
- `PassengerId`: opcional, pasajero asociado.

```csharp
    public BaggageCabinTypeId CabinTypeId { get; private set; }
    public BaggageType BaggageType { get; private set; }
    public BaggageQuantity Quantity { get; private set; }
    public BaggageWeightKg WeightKg { get; private set; }
    public string? Description { get; private set; }
```

Estas propiedades describen el equipaje:

- `CabinTypeId`: cabina o clase seleccionada.
- `BaggageType`: `MANO` o `BODEGA`.
- `Quantity`: cantidad de maletas.
- `WeightKg`: peso total en kilogramos.
- `Description`: observacion opcional.

```csharp
    public int AllowedQuantity { get; private set; }
    public decimal AllowedWeightPerBagKg { get; private set; }
    public decimal AllowedTotalWeightKg { get; private set; }
```

Aqui se guarda la politica aplicada en el momento del registro:

- Cuantas maletas se permitian.
- Cuanto peso se permitia por maleta.
- Cuanto peso total se permitia.

Esto es importante porque, si luego cambian las reglas, el registro historico conserva la politica usada en ese momento.

```csharp
    public int ExcessQuantity { get; private set; }
    public decimal ExcessWeightKg { get; private set; }
    public BaggageSurchargeAmount QuantitySurcharge { get; private set; }
    public BaggageSurchargeAmount WeightSurcharge { get; private set; }
    public BaggageSurchargeAmount TotalSurcharge { get; private set; }
```

Estas propiedades guardan el resultado del calculo:

- `ExcessQuantity`: cuantas maletas excedieron la cantidad permitida.
- `ExcessWeightKg`: cuantos kilos excedieron la politica.
- `QuantitySurcharge`: cobro por maletas adicionales.
- `WeightSurcharge`: cobro por exceso de peso.
- `TotalSurcharge`: suma de los dos recargos.

```csharp
    public BaggageRegisteredAt RegisteredAt { get; private set; }
```

Guarda la fecha del registro.

```csharp
    private BaggageRecord(...)
```

El constructor es privado. Esto obliga a crear registros usando metodos de fabrica como `Create` o `CreateNew`. Asi se evita crear objetos incompletos desde cualquier parte del sistema.

```csharp
    public static BaggageRecord Create(...)
```

Este metodo crea un registro a partir de valores simples. Se usa tanto para crear objetos nuevos como para reconstruir datos que vienen desde la base de datos.

Detalles importantes:

```csharp
id <= 0 ? BaggageId.CreateEmpty() : BaggageId.Create(id)
```

Cuando el registro es nuevo, todavia no tiene id de base de datos. Por eso se permite `0` mediante `CreateEmpty()`. Cuando ya existe, el id debe ser mayor que cero.

```csharp
BaggageType.Create(baggageType)
BaggageQuantity.Create(quantity)
BaggageWeightKg.Create(weightKg)
```

Cada dato importante se valida al crear el objeto.

```csharp
string.IsNullOrWhiteSpace(description) ? null : description.Trim()
```

La descripcion es opcional. Si viene vacia, se guarda como `null`. Si viene con espacios, se limpia.

```csharp
decimal.Round(allowedWeightPerBagKg, 2)
decimal.Round(allowedTotalWeightKg, 2)
```

Los pesos y valores monetarios se redondean a dos decimales.

```csharp
Math.Max(0, excessQuantity)
Math.Max(0, excessWeightKg)
```

Los excesos nunca pueden ser negativos. Si no hay exceso, se guarda `0`.

```csharp
    public static BaggageRecord CreateNew(...)
```

Este metodo es para registros nuevos. Internamente llama a `Create` con:

- `id = 0`
- `registeredAt = DateTime.Now`

Es decir, se usa cuando el usuario acaba de registrar equipaje.

---

## 5.2 Domain/ValueObject

Los value objects son objetos pequenos que envuelven valores simples y los validan. Sirven para no llenar el sistema con `int`, `decimal` y `string` sin control.

Ejemplo: en vez de aceptar cualquier `int` como cantidad, se crea `BaggageQuantity`, que no permite valores menores o iguales a cero.

---

## 5.2.1 BaggageId.cs

Archivo:

```text
src/Modules/Baggage/Domain/ValueObject/BaggageId.cs
```

Representa el id del registro de equipaje.

Reglas:

- Si es un registro ya existente, el id debe ser mayor que cero.
- Si es un registro nuevo antes de guardarse, se permite id `0`.

Codigo clave:

```csharp
public static BaggageId Create(int value)
{
    if (value <= 0)
        throw new ArgumentException("El id del equipaje debe ser mayor que cero.");

    return new BaggageId(value);
}
```

Valida ids reales.

```csharp
public static BaggageId CreateEmpty()
{
    return new BaggageId(0);
}
```

Permite crear un objeto nuevo que aun no ha sido insertado en la base de datos.

---

## 5.2.2 BaggageReservationId.cs

Representa el id de la reserva.

Regla:

- La reserva es obligatoria.
- Por eso el valor debe ser mayor que cero.

```csharp
if (value <= 0)
    throw new ArgumentException("La reserva es obligatoria.");
```

Sin reserva no puede existir registro de equipaje.

---

## 5.2.3 BaggageCabinTypeId.cs

Representa la clase/cabina seleccionada.

Regla:

- Debe ser mayor que cero.

Este value object valida que el dato tenga forma correcta. Luego `BaggageValidator` valida que realmente exista en la base de datos.

---

## 5.2.4 BaggageType.cs

Representa el tipo de equipaje.

Tipos validos:

- `MANO`
- `BODEGA`

Acepta varias entradas del usuario:

```csharp
"1" or "MANO" or "EQUIPAJE DE MANO" or "CABINA" => new BaggageType("MANO")
"2" or "BODEGA" or "EQUIPAJE EN BODEGA" or "HOLD" => new BaggageType("BODEGA")
```

Esto permite que desde consola el usuario pueda escribir `1`, `2`, `MANO` o `BODEGA`.

Si el tipo no coincide:

```csharp
throw new ArgumentException("Tipo de equipaje invalido. Usa MANO o BODEGA.")
```

Importante para sustentacion: este archivo evita que se guarden valores como `maleta`, `carry`, `x`, etc. Todo se normaliza a `MANO` o `BODEGA`.

---

## 5.2.5 BaggageQuantity.cs

Representa la cantidad de maletas.

Regla:

- Debe ser mayor que cero.

No se permite registrar cero maletas ni cantidades negativas.

---

## 5.2.6 BaggageWeightKg.cs

Representa el peso total del equipaje en kilogramos.

Reglas:

- Debe ser mayor que cero.
- Se redondea a dos decimales.

```csharp
return new BaggageWeightKg(decimal.Round(value, 2));
```

En la UI se pide peso por maleta, pero al caso de uso se le entrega el peso total.

---

## 5.2.7 BaggageSurchargeAmount.cs

Representa un valor monetario de recargo.

Reglas:

- No puede ser negativo.
- Se redondea a dos decimales.

Se usa para:

- Recargo por cantidad.
- Recargo por peso.
- Recargo total.

---

## 5.2.8 BaggageRegisteredAt.cs

Representa la fecha de registro.

Reglas:

- No puede ser una fecha por defecto.

Tiene dos formas:

```csharp
Create(DateTime value)
```

Para crear desde una fecha recibida.

```csharp
CreateNow()
```

Para crear con la fecha actual.

En `BaggageRecord.CreateNew` se usa `DateTime.Now`.

---

## 5.3 Domain/Models

Estos archivos no son tablas. Son modelos de apoyo para mover datos dentro del modulo.

---

## 5.3.1 BaggagePolicy.cs

Representa la politica de equipaje aplicada.

Campos:

```csharp
public sealed record BaggagePolicy(
    string CabinFamily,
    string BaggageType,
    int AllowedQuantity,
    decimal AllowedWeightPerBagKg,
    decimal AllowedTotalWeightKg,
    decimal ExtraBagFee,
    decimal ExcessKgFee);
```

Significado:

- `CabinFamily`: familia de cabina normalizada: `PRIMERA`, `EJECUTIVA` o `ECONOMICA`.
- `BaggageType`: `MANO` o `BODEGA`.
- `AllowedQuantity`: cantidad permitida.
- `AllowedWeightPerBagKg`: peso permitido por maleta.
- `AllowedTotalWeightKg`: peso total permitido.
- `ExtraBagFee`: precio por maleta adicional.
- `ExcessKgFee`: precio por kilo excedido.

---

## 5.3.2 BaggageSurchargeResult.cs

Representa el resultado del calculo.

Campos:

- Politica aplicada.
- Exceso por cantidad.
- Exceso por peso.
- Recargo por cantidad.
- Recargo por peso.
- Recargo total.

Se usa para mostrar previsualizacion y para guardar el registro.

---

## 5.3.3 RegisterBaggageResult.cs

Representa el resultado final despues de registrar.

Contiene:

- `Record`: el registro creado.
- `Context`: datos de reserva/tiquete/vuelo/pasajero.
- `Surcharge`: calculo aplicado.
- `PreviousReservationTotal`: total anterior de la reserva.
- `NewReservationTotal`: total nuevo despues del recargo.

Este modelo permite que la UI muestre al usuario:

```text
Total anterior reserva
Recargo aplicado
Nuevo total reserva
```

---

## 5.3.4 BaggageRegistrationContext.cs

Representa los datos que el sistema muestra antes de registrar equipaje.

Incluye:

- Id y codigo de reserva.
- Id y codigo de tiquete.
- Id de reserva-pasajero.
- Id y codigo de vuelo.
- Id y nombre del pasajero.
- Total actual de la reserva.

Es clave porque permite confirmar que el usuario esta registrando equipaje sobre la reserva o tiquete correcto.

---

## 5.3.5 BaggageRecordView.cs

Es un modelo de consulta para mostrar registros de equipaje.

No se usa para guardar, sino para listar:

- Equipaje por cliente.
- Equipaje por vuelo.
- Recargos aplicados.

Incluye datos combinados de varias tablas:

- `baggage_records`
- `reservations`
- `tickets`
- `flights`
- `passengers`
- `people`
- `CabinTypes`

---

## 5.3.6 CabinTypeOption.cs

Modelo simple para listar cabinas disponibles:

```csharp
public sealed record CabinTypeOption(int Id, string Name);
```

La UI lo usa para imprimir:

```text
1. Economy
2. Business
3. First
```

---

## 5.4 Domain/Repositories/IBaggageRecordRepository.cs

Archivo:

```text
src/Modules/Baggage/Domain/Repositories/IBaggageRecordRepository.cs
```

### Para que fue creado

Define lo que el dominio/aplicacion necesita de persistencia, sin decir como se implementa.

Esto es arquitectura hexagonal: la aplicacion depende de una interfaz, no de EF Core directamente.

Metodos:

```csharp
Task AddAsync(BaggageRecord record);
```

Guarda un registro de equipaje.

```csharp
Task<BaggageRegistrationContext?> GetRegistrationContextByTicketIdAsync(int ticketId);
Task<BaggageRegistrationContext?> GetRegistrationContextByReservationIdAsync(int reservationId);
```

Buscan datos de contexto para registrar por tiquete o reserva.

```csharp
Task<IReadOnlyList<CabinTypeOption>> GetCabinTypesAsync();
Task<bool> CabinTypeExistsAsync(int cabinTypeId);
```

Listan y validan cabinas.

```csharp
Task AddSurchargeToReservationAsync(int reservationId, decimal surcharge);
```

Suma el recargo al total de la reserva.

```csharp
Task<IReadOnlyList<BaggageRecordView>> GetByCustomerIdAsync(int customerId);
Task<IReadOnlyList<BaggageRecordView>> GetByFlightIdAsync(int flightId);
Task<IReadOnlyList<BaggageRecordView>> GetWithSurchargesAsync();
```

Consultas para los menus.

---

## 6. Application: capa de aplicacion

Esta capa contiene los casos de uso y servicios que coordinan el modulo.

---

## 6.1 Application/Interfaces/IBaggageValidator.cs

Define que validaciones debe tener el modulo:

```csharp
Task ValidateCabinTypeExistsAsync(int cabinTypeId);
void ValidateRegistrationInput(string baggageType, int quantity, decimal totalWeightKg);
```

Se separa en una interfaz para que los casos de uso dependan de una abstraccion.

---

## 6.2 Application/Services/BaggageValidator.cs

Implementa validaciones generales.

### ValidateCabinTypeExistsAsync

Primero valida forma:

```csharp
if (cabinTypeId <= 0)
    throw new ArgumentException("La clase/cabina es obligatoria.");
```

Luego valida existencia real en base de datos:

```csharp
var exists = await _repository.CabinTypeExistsAsync(cabinTypeId);
if (!exists)
    throw new ArgumentException("La clase/cabina seleccionada no existe.");
```

### ValidateRegistrationInput

Usa value objects:

```csharp
BaggageType.Create(baggageType);
BaggageQuantity.Create(quantity);
BaggageWeightKg.Create(totalWeightKg);
```

Si algun dato es invalido, el value object lanza excepcion.

---

## 6.3 Application/Services/BaggageSurchargeCalculator.cs

Este archivo contiene la logica de negocio mas importante: reglas y calculo de recargos.

### Tipos de equipaje

```csharp
public const string CarryOnType = "MANO";
public const string CheckedType = "BODEGA";
```

Se definen constantes para no escribir strings sueltos por todo el codigo.

### Metodo Calculate

Firma:

```csharp
public BaggageSurchargeResult Calculate(
    string cabinTypeName,
    string baggageType,
    int quantity,
    decimal totalWeightKg)
```

Recibe:

- Nombre de cabina.
- Tipo de equipaje.
- Cantidad de maletas.
- Peso total.

Validaciones:

```csharp
if (quantity <= 0)
    throw new ArgumentException("La cantidad de maletas debe ser mayor que cero.");

if (totalWeightKg <= 0)
    throw new ArgumentException("El peso debe ser mayor que cero.");
```

Normalizacion:

```csharp
var normalizedType = BaggageType.Create(baggageType).Value;
```

Convierte entradas como `1`, `MANO`, `cabina` a `MANO`.

Resolucion de politica:

```csharp
var policy = ResolvePolicy(cabinTypeName, normalizedType);
```

Escoge la politica segun cabina y tipo de equipaje.

Calculo de exceso por cantidad:

```csharp
var excessQuantity = Math.Max(0, quantity - policy.AllowedQuantity);
```

Ejemplo: si se permiten 1 y el usuario registra 3, exceso es `2`.

Calculo de peso por maleta:

```csharp
var weightPerBag = decimal.Round(totalWeightKg / quantity, 2);
```

El caso de uso recibe peso total. Para validar exceso por maleta, divide el total entre la cantidad.

Calculo de exceso por peso total:

```csharp
var excessByTotalWeight = Math.Max(0, totalWeightKg - policy.AllowedTotalWeightKg);
```

Ejemplo: si se permiten 23 kg en total y se registran 30 kg, exceso total es 7 kg.

Calculo de exceso por peso individual:

```csharp
var excessByBagWeight = Math.Max(0, weightPerBag - policy.AllowedWeightPerBagKg) * quantity;
```

Ejemplo: si cada maleta permite 23 kg y el promedio por maleta es 25 kg, hay 2 kg de exceso por maleta. Si son 2 maletas, exceso por esta regla es 4 kg.

Eleccion del exceso real:

```csharp
var excessWeight = decimal.Round(Math.Max(excessByTotalWeight, excessByBagWeight), 2);
```

Se toma el mayor entre:

- Exceso por peso total.
- Exceso por peso por maleta.

Esto evita cobrar dos veces por el mismo exceso de peso.

Calculo monetario:

```csharp
var quantitySurcharge = decimal.Round(excessQuantity * policy.ExtraBagFee, 2);
var weightSurcharge = decimal.Round(excessWeight * policy.ExcessKgFee, 2);
```

Recargo total:

```csharp
quantitySurcharge + weightSurcharge
```

Resultado:

```csharp
return new BaggageSurchargeResult(
    policy,
    excessQuantity,
    excessWeight,
    quantitySurcharge,
    weightSurcharge,
    quantitySurcharge + weightSurcharge);
```

### Reglas de negocio por cabina

El metodo `ResolvePolicy` contiene las reglas:

```csharp
("PRIMERA", "MANO")   => cantidad 2, 12 kg por maleta, 24 kg total, 90000 por maleta extra, 18000 por kg
("PRIMERA", "BODEGA") => cantidad 3, 32 kg por maleta, 96 kg total, 130000 por maleta extra, 22000 por kg
("EJECUTIVA", "MANO") => cantidad 1, 12 kg por maleta, 12 kg total, 80000 por maleta extra, 16000 por kg
("EJECUTIVA", "BODEGA") => cantidad 2, 32 kg por maleta, 64 kg total, 120000 por maleta extra, 20000 por kg
("ECONOMICA", "MANO") => cantidad 1, 10 kg por maleta, 10 kg total, 70000 por maleta extra, 15000 por kg
("ECONOMICA", "BODEGA") => cantidad 1, 23 kg por maleta, 23 kg total, 100000 por maleta extra, 18000 por kg
```

### Como detecta la familia de cabina

```csharp
if (name.Contains("FIRST") || name.Contains("PRIMER"))
    return "PRIMERA";

if (name.Contains("BUSINESS") || name.Contains("EJECUT") || name.Contains("PREMIUM"))
    return "EJECUTIVA";

return "ECONOMICA";
```

Esto permite que funcione tanto con nombres en ingles como en espanol:

- `First Class` -> `PRIMERA`
- `Primera Clase` -> `PRIMERA`
- `Business` -> `EJECUTIVA`
- `Ejecutiva` -> `EJECUTIVA`
- Cualquier otra -> `ECONOMICA`

---

## 6.4 Application/UseCases/RegisterBaggageUseCase.cs

Es el caso de uso principal.

### Responsabilidad

Registrar equipaje y actualizar la reserva si hay recargo.

Tiene dos entradas:

```csharp
ExecuteByTicketAsync(...)
ExecuteByReservationAsync(...)
```

### Registro por tiquete

Flujo:

1. Valida que `ticketId` sea mayor que cero.
2. Valida que exista la cabina.
3. Valida tipo, cantidad y peso.
4. Obtiene el contexto por tiquete.
5. Si no encuentra tiquete, lanza error.
6. Llama al metodo privado `RegisterAsync`.

Codigo clave:

```csharp
var context = await _repository.GetRegistrationContextByTicketIdAsync(ticketId);
if (context is null)
    throw new InvalidOperationException("No se encontro el tiquete o no esta relacionado con una reserva.");
```

### Registro por reserva

Hace lo mismo, pero buscando por `reservationId`.

### Metodo privado RegisterAsync

Este metodo evita duplicar logica entre registro por reserva y registro por tiquete.

Pasos:

1. Carga cabinas disponibles.
2. Busca la cabina seleccionada.
3. Normaliza el tipo de equipaje.
4. Calcula el recargo.
5. Guarda el total anterior.
6. Crea `BaggageRecord`.
7. Guarda en repositorio.
8. Si hay recargo, actualiza total de reserva.
9. Devuelve `RegisterBaggageResult`.

Codigo explicado:

```csharp
var cabinTypes = await _repository.GetCabinTypesAsync();
var cabin = cabinTypes.FirstOrDefault(c => c.Id == cabinTypeId);
```

Busca el nombre de la cabina porque el calculador necesita el nombre para resolver la familia.

```csharp
var normalizedType = BaggageSurchargeCalculator.NormalizeBaggageType(baggageType);
```

Convierte `1` o `mano` en `MANO`, y `2` o `bodega` en `BODEGA`.

```csharp
var surcharge = _calculator.Calculate(cabin.Name, normalizedType, quantity, totalWeightKg);
```

Aplica reglas de negocio.

```csharp
var previousTotal = context.CurrentReservationTotal;
```

Guarda el total antes de modificarlo.

```csharp
var record = BaggageRecord.CreateNew(...);
```

Crea el agregado de dominio con datos normalizados y recargos calculados.

```csharp
await _repository.AddAsync(record);
```

Persiste el registro.

```csharp
if (surcharge.TotalSurcharge > 0)
    await _repository.AddSurchargeToReservationAsync(context.ReservationId, surcharge.TotalSurcharge);
```

Solo modifica la reserva si realmente hay recargo.

```csharp
return new RegisterBaggageResult(...);
```

Devuelve informacion para imprimir confirmacion en consola.

---

## 6.5 PreviewBaggageSurchargeUseCase.cs

Sirve para calcular el recargo antes de guardar.

Esto permite mostrar:

- Politica aplicada.
- Exceso por cantidad.
- Exceso por peso.
- Recargo por cantidad.
- Recargo por peso.
- Recargo total.

Luego la UI pregunta:

```text
Confirmar registro y actualizar total de la reserva? (s/n):
```

Importante: este caso de uso no guarda nada.

---

## 6.6 GetBaggageRegistrationContextUseCase.cs

Obtiene los datos previos al registro.

Tiene dos metodos:

- `ExecuteByTicketAsync(int ticketId)`
- `ExecuteByReservationAsync(int reservationId)`

Sirve para mostrar:

```text
Reserva
Tiquete
Vuelo
Pasajero
Valor actual reserva
```

Asi el usuario confirma visualmente que esta registrando equipaje en el lugar correcto.

---

## 6.7 Casos de uso de consulta

### GetBaggageByCustomerUseCase.cs

Consulta equipaje por cliente.

Valida:

```csharp
if (customerId <= 0)
    throw new ArgumentException("El cliente es obligatorio.");
```

Luego delega al repositorio:

```csharp
return _repository.GetByCustomerIdAsync(customerId);
```

### GetBaggageByFlightUseCase.cs

Consulta equipaje por vuelo.

Valida que el id del vuelo sea mayor que cero y llama al repositorio.

### GetBaggageSurchargesUseCase.cs

Consulta registros con recargo:

```csharp
return _repository.GetWithSurchargesAsync();
```

### GetCabinTypesForBaggageUseCase.cs

Lista cabinas para que el usuario pueda seleccionar una politica de equipaje.

---

## 7. Infrastructure: Entity Framework Core y MySQL

Esta capa conecta el modulo con la base de datos.

---

## 7.1 Infrastructure/Entity/BaggageRecordEntity.cs

Esta clase representa la tabla `baggage_records`.

Es una entidad plana para EF Core. A diferencia de `BaggageRecord`, aqui se usan tipos simples:

```csharp
public int Id { get; set; }
public int ReservationId { get; set; }
public int? TicketId { get; set; }
public int? ReservationPassengerId { get; set; }
public int? FlightId { get; set; }
public int? PassengerId { get; set; }
public int CabinTypeId { get; set; }
public string? BaggageType { get; set; }
public int Quantity { get; set; }
public decimal WeightKg { get; set; }
...
```

### Por que existe si ya existe BaggageRecord

Porque `BaggageRecord` es del dominio y usa value objects. EF Core trabaja mas facil con entidades planas. Por eso el repositorio convierte:

```text
BaggageRecord -> BaggageRecordEntity
```

---

## 7.2 Infrastructure/Entity/BaggageRecordEntityConfiguration.cs

Configura como se mapea `BaggageRecordEntity` a MySQL.

### Tabla

```csharp
builder.ToTable("baggage_records");
```

### Llave primaria

```csharp
builder.HasKey(x => x.Id);
```

### Columnas

Ejemplos:

```csharp
builder.Property(x => x.ReservationId)
    .HasColumnName("reserva_id")
    .HasColumnType("int")
    .IsRequired();
```

Mapea `ReservationId` de C# a la columna `reserva_id`.

```csharp
builder.Property(x => x.WeightKg)
    .HasColumnName("peso_kg")
    .HasColumnType("decimal(10,2)")
    .IsRequired();
```

El peso se guarda como decimal con dos decimales.

```csharp
builder.Property(x => x.TotalSurcharge)
    .HasColumnName("recargo_total")
    .HasColumnType("decimal(18,2)")
    .IsRequired();
```

Los valores monetarios usan `decimal(18,2)`.

### Indices

```csharp
builder.HasIndex(x => x.ReservationId);
builder.HasIndex(x => x.TicketId);
builder.HasIndex(x => x.FlightId);
builder.HasIndex(x => x.PassengerId);
```

Permiten consultar mas rapido por reserva, tiquete, vuelo y pasajero.

### Relaciones

```csharp
builder.HasOne<ReservationEntity>()
    .WithMany()
    .HasForeignKey(x => x.ReservationId)
    .OnDelete(DeleteBehavior.Restrict);
```

Cada equipaje pertenece a una reserva. No se permite borrar una reserva si tiene equipaje relacionado.

```csharp
builder.HasOne<TicketEntity>()
    .WithMany()
    .HasForeignKey(x => x.TicketId)
    .OnDelete(DeleteBehavior.SetNull);
```

Si se borra el tiquete, el campo queda en `null`. El registro de equipaje se conserva como historico.

Aplica igual para:

- ReservationPassenger
- Flight
- Passenger

Cabina usa `Restrict`:

```csharp
builder.HasOne<CabinTypeEntity>()
    .WithMany()
    .HasForeignKey(x => x.CabinTypeId)
    .OnDelete(DeleteBehavior.Restrict);
```

No se debe borrar una cabina usada por registros de equipaje.

---

## 7.3 Infrastructure/Repository/BaggageRecordRepository.cs

Es la implementacion real de `IBaggageRecordRepository`.

Usa:

```csharp
private readonly AppDbContext _context;
```

para consultar y modificar MySQL mediante EF Core.

### AddAsync

```csharp
await _context.Set<BaggageRecordEntity>().AddAsync(MapToEntity(record));
await _context.SaveChangesAsync();
```

Convierte el agregado de dominio en entidad EF y lo guarda.

### GetRegistrationContextByTicketIdAsync

Hace joins entre:

- `tickets`
- `reservationpassengers`
- `reservationflights`
- `reservations`
- `flights`
- `passengers`
- `people`

Objetivo: a partir del id del tiquete, encontrar toda la informacion relacionada.

Flujo logico:

```text
ticket
-> reservationPassenger
-> reservationFlight
-> reservation
-> flight
-> passenger
-> person
```

Devuelve un `BaggageRegistrationContext`.

### GetRegistrationContextByReservationIdAsync

Busca por reserva.

Usa joins opcionales (`DefaultIfEmpty`) porque una reserva podria no tener toda la informacion asociada en ese momento.

Devuelve el primer contexto encontrado ordenando por vuelo y pasajero.

### GetCabinTypesAsync

Consulta `CabinTypes`:

```csharp
return await _context.Set<CabinTypeEntity>()
    .AsNoTracking()
    .OrderBy(e => e.Id)
    .Select(e => new CabinTypeOption(e.Id, e.Name ?? string.Empty))
    .ToListAsync();
```

`AsNoTracking()` se usa porque solo se consulta, no se modifica.

### CabinTypeExistsAsync

Valida existencia:

```csharp
return _context.Set<CabinTypeEntity>().AnyAsync(e => e.Id == cabinTypeId);
```

### AddSurchargeToReservationAsync

Busca la reserva y suma el recargo:

```csharp
reservation.TotalAmount = decimal.Round(reservation.TotalAmount + surcharge, 2);
reservation.UpdatedAt = DateTime.Now;
await _context.SaveChangesAsync();
```

Si el recargo es cero o negativo, retorna sin hacer nada.

### Consultas por cliente, vuelo y recargo

Los metodos:

```csharp
GetByCustomerIdAsync
GetByFlightIdAsync
GetWithSurchargesAsync
```

usan un metodo comun:

```csharp
BuildViewQuery(...)
```

Este metodo arma una consulta con joins para construir `BaggageRecordView`.

### Correccion importante de EF Core

Se corrigio el error donde EF Core intentaba traducir a SQL cosas como `BuildPersonName` en lugares donde no podia.

La solucion aplicada fue:

1. Construir una consulta base con columnas reales.
2. Aplicar filtros:
   - por cliente
   - por vuelo
   - solo recargos
3. Aplicar ordenamientos.
4. Al final proyectar al modelo `BaggageRecordView`.

Esto mejora la traduccion a SQL y evita errores de LINQ.

### MapToEntity

Convierte de dominio a EF:

```csharp
private static BaggageRecordEntity MapToEntity(BaggageRecord record)
{
    return new BaggageRecordEntity
    {
        Id = record.Id.Value,
        ReservationId = record.ReservationId.Value,
        TicketId = record.TicketId,
        ...
    };
}
```

Como el dominio usa value objects, aqui se extrae `.Value`.

---

## 8. UI: menus de consola

La capa UI es la que interactua con el usuario.

---

## 8.1 UI/BaggageMenu.cs

Menu general para Admin/Staff.

Opciones:

```text
1. Registrar equipaje por tiquete
2. Registrar equipaje por reserva
3. Consultar equipaje por cliente
4. Consultar equipaje por vuelo
5. Ver recargos aplicados
0. Volver
```

### StartAsync

Tiene un ciclo `while (true)` para mantener el menu activo hasta que el usuario elija `0`.

Cada opcion llama un metodo privado.

### RegisterByTicketAsync

Flujo:

1. Pide id del tiquete.
2. Obtiene contexto.
3. Imprime contexto.
4. Lee cabina, tipo, cantidad, peso y descripcion.
5. Previsualiza calculo.
6. Pide confirmacion.
7. Registra equipaje.
8. Imprime resultado.

### RegisterByReservationAsync

Igual al anterior, pero pide id de reserva.

### ReadBaggageInputAsync

Lee los datos:

```text
Id de clase/cabina
Tipo de equipaje
Cantidad de maletas
Peso por maleta
Descripcion
```

Luego calcula:

```csharp
var totalWeightKg = decimal.Round(quantity * weightPerBagKg, 2);
```

Esto cumple el requisito de calculo automatico del peso total.

### PrintCalculation

Muestra el detalle antes de confirmar:

- Clase aplicada.
- Tipo de equipaje.
- Cantidad permitida.
- Cantidad registrada.
- Peso permitido por maleta.
- Peso registrado por maleta.
- Peso total permitido.
- Peso registrado.
- Exceso de cantidad.
- Exceso de peso.
- Recargo por cantidad.
- Recargo por peso.
- Recargo total.

### PrintRegistrationResult

Muestra:

- Reserva.
- Tiquete.
- Vuelo.
- Pasajero.
- Total anterior.
- Recargo aplicado.
- Nuevo total.

---

## 8.2 UI/CustomerBaggageMenu.cs

Menu especial para cliente autenticado.

Opciones:

```text
1. Ver mi equipaje registrado
2. Registrar equipaje por una de mis reservas
3. Registrar equipaje por uno de mis tiquetes
4. Ver mis recargos aplicados
0. Volver
```

### Diferencia principal con BaggageMenu

El cliente no escribe `customer_id`.

El menu recibe:

```csharp
private readonly int _customerId;
```

Ese valor viene del usuario autenticado en `Program.cs`.

### Seguridad funcional

Antes de registrar por reserva:

```csharp
if (!reservations.Any(x => x.Id.Value == reservationId))
    throw new InvalidOperationException("Esa reserva no pertenece a tu cuenta.");
```

Antes de registrar por tiquete:

```csharp
if (!tickets.Any(x => x.Id.Value == ticketId))
    throw new InvalidOperationException("Ese tiquete no pertenece a tu cuenta.");
```

Esto evita que un cliente registre equipaje en reservas o tiquetes de otro cliente.

### GetOwnReservationsAsync

Obtiene reservas del cliente autenticado:

```csharp
return (await _getReservationsByCustomerId.ExecuteAsync(_customerId))
    .OrderByDescending(x => x.ReservedAt.Value)
    .ToList();
```

### GetOwnTicketsAsync

Primero obtiene reservas del cliente. Luego, por cada reserva, busca sus tiquetes por codigo de reserva.

Esto reutiliza modulos existentes:

- Reservations
- Tickets

---

## 8.3 UI/ConsoleMenu.cs

Este archivo existe, pero actualmente esta vacio:

```csharp
public class ConsoleMenu
{
}
```

No participa en el flujo actual.

---

## 9. BaggageModule.cs

Archivo:

```text
src/Modules/Baggage/BaggageModule.cs
```

### Para que fue creado

Es la clase que arma el modulo. Evita que `Program.cs` tenga que construir manualmente todos los casos de uso.

### Build

```csharp
public static BaggageMenu Build(AppDbContext context)
```

Crea el menu general para Admin/Staff.

Construye:

- Repositorio.
- Servicios comunes.
- Consulta por vuelo.
- Consulta de recargos.
- Menu `BaggageMenu`.

### BuildCustomer

```csharp
public static CustomerBaggageMenu BuildCustomer(
    AppDbContext context,
    int customerId,
    GetReservationsByCustomerIdUseCase getReservationsByCustomerId,
    GetTicketsByReservationCodeUseCase getTicketsByReservationCode)
```

Crea el menu para cliente autenticado.

Ademas del modulo Baggage, recibe casos de uso de Reservations y Tickets para poder listar solo lo que pertenece al cliente.

### BuildServices

Metodo privado que centraliza servicios compartidos:

```csharp
IBaggageRecordRepository repository = new BaggageRecordRepository(context);
var calculator = new BaggageSurchargeCalculator();
IBaggageValidator validator = new BaggageValidator(repository);
```

Luego crea:

- `RegisterBaggageUseCase`
- `PreviewBaggageSurchargeUseCase`
- `GetBaggageByCustomerUseCase`
- `GetCabinTypesForBaggageUseCase`
- `GetBaggageRegistrationContextUseCase`

---

## 10. AppDbContext.cs

Archivo:

```text
src/shared/Context/AppDbContext.cs
```

### Cambio realizado

Se agrego:

```csharp
using GestionAerolineas.src.Modules.Baggage.Infrastructure.Entity;
```

y:

```csharp
public DbSet<BaggageRecordEntity> BaggageRecords { get; set; }
```

### Para que sirve

`DbSet<BaggageRecordEntity>` le dice a EF Core que existe una entidad que debe ser reconocida como tabla.

Ademas, el proyecto usa:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
```

Esto hace que EF Core encuentre automaticamente `BaggageRecordEntityConfiguration`.

---

## 11. Program.cs

Archivo:

```text
Program.cs
```

### Cambios realizados

Se importo el modulo:

```csharp
using GestionAerolineas.src.Modules.Baggage;
```

Se construyo el menu general:

```csharp
var baggageMenu = BaggageModule.Build(context);
```

Se agrego al menu de Admin:

```csharp
new("Equipaje y recargos", () => baggageMenu.StartAsync())
```

Se agrego al menu de Staff:

```csharp
new("Equipaje y recargos", () => baggageMenu.StartAsync())
```

Se construyo el menu de cliente autenticado:

```csharp
var customerBaggageMenu = BaggageModule.BuildCustomer(
    context,
    customer.Id.Value,
    getReservationsByCustomerId,
    getTicketsByReservationCode);
```

Se agrego al menu del cliente:

```csharp
new("Mi equipaje y recargos", () => customerBaggageMenu.StartAsync())
```

### Importante para explicar

Admin y Staff usan el menu general, donde pueden consultar por cliente o por vuelo.

Cliente usa un menu especial, donde el sistema ya sabe cual es su `customer_id`.

---

## 12. Migracion BaggageMigration

Archivo:

```text
Migrations/20260426193740_BaggageMigration.cs
```

### Que hace

Crea la tabla:

```text
baggage_records
```

Columnas principales:

- `id`
- `reserva_id`
- `tiquete_id`
- `reserva_pasajero_id`
- `vuelo_id`
- `pasajero_id`
- `tipo_cabina_id`
- `tipo_equipaje`
- `cantidad`
- `peso_kg`
- `descripcion`
- `cantidad_permitida`
- `peso_permitido_por_maleta_kg`
- `peso_total_permitido_kg`
- `exceso_cantidad`
- `exceso_peso_kg`
- `recargo_cantidad`
- `recargo_peso`
- `recargo_total`
- `fecha_registro`

### Llaves foraneas

Relaciona con:

- `CabinTypes`
- `flights`
- `passengers`
- `reservationpassengers`
- `reservations`
- `tickets`

### Metodo Up

Se ejecuta al aplicar la migracion. Crea tabla, columnas, relaciones e indices.

### Metodo Down

Se ejecuta si se revierte la migracion:

```csharp
migrationBuilder.DropTable(
    name: "baggage_records");
```

---

## 13. Relaciones con otros modulos

El modulo Baggage se relaciona con varios modulos porque el equipaje no existe aislado.

### Reservations

Relacion principal. Todo equipaje pertenece a una reserva.

Uso:

- Obtener contexto por reserva.
- Sumar recargos al total de la reserva.
- Consultar equipaje por cliente mediante `Reservation.CustomerId`.

### Tickets

Permite registrar equipaje directamente por tiquete.

Uso:

- Obtener reserva, pasajero y vuelo a partir del tiquete.
- Mostrar codigo del tiquete en consultas.

### ReservationPassengers

Relaciona reserva/vuelo con pasajero.

Uso:

- Saber a que pasajero corresponde un tiquete o registro.

### ReservationFlights

Relaciona reserva con vuelo.

Uso:

- Llegar desde reserva hasta vuelo.

### Flights

Permite consultar equipaje por vuelo.

### Passengers y People

Se usan para mostrar el nombre del pasajero.

### CabinTypes

Determina la cabina seleccionada y permite resolver reglas de equipaje.

### Customers

En consultas por cliente, se filtra por `Reservation.CustomerId`.

---

## 14. Reglas de negocio del modulo

### 14.1 Tipos de equipaje

Solo se aceptan:

- Equipaje de mano: `MANO`
- Equipaje en bodega: `BODEGA`

Entradas aceptadas:

- `1`, `MANO`, `EQUIPAJE DE MANO`, `CABINA`
- `2`, `BODEGA`, `EQUIPAJE EN BODEGA`, `HOLD`

### 14.2 Cantidad

Debe ser mayor que cero.

### 14.3 Peso

Debe ser mayor que cero.

La UI pide peso por maleta, pero el caso de uso recibe peso total:

```text
peso total = cantidad * peso por maleta
```

### 14.4 Cabina

La cabina debe:

1. Tener id mayor que cero.
2. Existir en la tabla `CabinTypes`.

### 14.5 Politicas por cabina

| Cabina | Tipo | Cantidad permitida | Peso por maleta | Peso total | Recargo por maleta extra | Recargo por kg |
|---|---|---:|---:|---:|---:|---:|
| Primera | Mano | 2 | 12 kg | 24 kg | 90000 | 18000 |
| Primera | Bodega | 3 | 32 kg | 96 kg | 130000 | 22000 |
| Ejecutiva | Mano | 1 | 12 kg | 12 kg | 80000 | 16000 |
| Ejecutiva | Bodega | 2 | 32 kg | 64 kg | 120000 | 20000 |
| Economica | Mano | 1 | 10 kg | 10 kg | 70000 | 15000 |
| Economica | Bodega | 1 | 23 kg | 23 kg | 100000 | 18000 |

### 14.6 Exceso por cantidad

Formula:

```text
exceso_cantidad = max(0, cantidad_registrada - cantidad_permitida)
```

### 14.7 Exceso por peso total

Formula:

```text
exceso_peso_total = max(0, peso_total_registrado - peso_total_permitido)
```

### 14.8 Exceso por peso por maleta

Formula:

```text
peso_promedio_por_maleta = peso_total_registrado / cantidad
exceso_por_maleta = max(0, peso_promedio_por_maleta - peso_permitido_por_maleta) * cantidad
```

### 14.9 Exceso de peso final

Formula:

```text
exceso_peso = max(exceso_peso_total, exceso_por_maleta)
```

Esto evita duplicar cobros por peso.

### 14.10 Recargo por cantidad

Formula:

```text
recargo_cantidad = exceso_cantidad * tarifa_maleta_extra
```

### 14.11 Recargo por peso

Formula:

```text
recargo_peso = exceso_peso * tarifa_kg_extra
```

### 14.12 Recargo total

Formula:

```text
recargo_total = recargo_cantidad + recargo_peso
```

### 14.13 Actualizacion de reserva

Si:

```text
recargo_total > 0
```

entonces:

```text
reservation.TotalAmount = reservation.TotalAmount + recargo_total
```

---

## 15. Flujo completo de registro por tiquete

1. Usuario entra a `Equipaje y recargos`.
2. Selecciona `Registrar equipaje por tiquete`.
3. Digita `ticket_id`.
4. `BaggageMenu` llama `GetBaggageRegistrationContextUseCase.ExecuteByTicketAsync`.
5. El caso de uso llama `BaggageRecordRepository.GetRegistrationContextByTicketIdAsync`.
6. El repositorio hace joins para encontrar reserva, vuelo, pasajero y total.
7. Se imprime el contexto.
8. Usuario selecciona cabina.
9. Usuario selecciona tipo de equipaje.
10. Usuario escribe cantidad de maletas.
11. Usuario escribe peso por maleta.
12. La UI calcula peso total.
13. La UI llama `PreviewBaggageSurchargeUseCase`.
14. Se valida cabina, tipo, cantidad y peso.
15. `BaggageSurchargeCalculator` calcula recargos.
16. La UI muestra detalle.
17. Usuario confirma.
18. La UI llama `RegisterBaggageUseCase.ExecuteByTicketAsync`.
19. El caso de uso valida todo otra vez.
20. Obtiene contexto otra vez.
21. Calcula recargo.
22. Crea `BaggageRecord`.
23. Repositorio guarda en `baggage_records`.
24. Si hay recargo, repositorio suma a `reservations.total_amount`.
25. La UI muestra total anterior, recargo y nuevo total.

---

## 16. Flujo completo de registro por reserva

Es casi igual al flujo por tiquete, pero empieza desde `reservation_id`.

Diferencia:

- Puede no tener un tiquete especifico.
- El contexto puede traer el primer vuelo/pasajero asociado si existe.
- El registro queda asociado obligatoriamente a la reserva y opcionalmente a tiquete/pasajero/vuelo.

---

## 17. Flujo del cliente autenticado

El cliente entra a:

```text
Mi equipaje y recargos
```

El sistema ya tiene su `customerId`.

### Ver mi equipaje

1. Llama `GetBaggageByCustomerUseCase`.
2. Filtra por el cliente autenticado.
3. Imprime registros.

### Registrar por una de mis reservas

1. Obtiene reservas del cliente.
2. Las muestra.
3. Cliente ingresa `reservation_id`.
4. Se valida que esa reserva este en la lista del cliente.
5. Si no pertenece, se bloquea.
6. Si pertenece, sigue el flujo normal de registro.

### Registrar por uno de mis tiquetes

1. Obtiene reservas del cliente.
2. Busca tiquetes de esas reservas.
3. Muestra tiquetes.
4. Cliente ingresa `ticket_id`.
5. Se valida que el tiquete pertenezca a su cuenta.
6. Si pertenece, sigue el flujo normal.

---

## 18. Guia para correr el modulo

### 18.1 Compilar

Desde la raiz del proyecto:

```powershell
dotnet build --no-restore
```

Resultado esperado:

```text
Compilacion correcta.
0 Advertencia(s)
0 Errores
```

En este proyecto ya se verifico que compila correctamente.

### 18.2 Configurar base de datos

El archivo:

```text
appsettings.json
```

contiene la cadena:

```json
{
  "ConnectionStrings": {
    "MySqlDB": "server=localhost;port=3306;database=prueba;user=root;password=09052008;SslMode=None;"
  }
}
```

Si se corre en otro computador, hay que cambiar:

- `server`
- `port`
- `database`
- `user`
- `password`

Ejemplo:

```json
{
  "ConnectionStrings": {
    "MySqlDB": "server=localhost;port=3306;database=aerolineas;user=root;password=TU_PASSWORD;SslMode=None;"
  }
}
```

### 18.3 Aplicar migraciones

Si la base de datos esta nueva:

```powershell
dotnet ef database update
```

Esto crea las tablas, incluyendo `baggage_records`, si la migracion no ha sido aplicada.

Si `dotnet ef` no esta instalado:

```powershell
dotnet tool install --global dotnet-ef
```

Luego repetir:

```powershell
dotnet ef database update
```

### 18.4 Ejecutar

```powershell
dotnet run
```

El programa:

1. Crea el `AppDbContext`.
2. Prueba conexion a la base de datos.
3. Ejecuta seed de catalogos y datos maestros.
4. Carga menus.

---

## 19. Como demostrar el modulo en el examen

### Demo como Admin o Staff

1. Iniciar sesion como Admin o Staff.
2. Entrar a `Equipaje y recargos`.
3. Elegir registrar por tiquete o por reserva.
4. Ingresar un id existente.
5. Mostrar que el sistema imprime:
   - reserva
   - tiquete
   - vuelo
   - pasajero
   - valor actual
6. Seleccionar una cabina.
7. Seleccionar tipo de equipaje.
8. Ingresar cantidad y peso por maleta.
9. Mostrar que el sistema calcula el peso total y recargo.
10. Confirmar.
11. Mostrar que se guarda y actualiza el total de la reserva.
12. Entrar a `Ver recargos aplicados`.
13. Mostrar el registro nuevo.

### Demo como Cliente

1. Iniciar sesion como cliente.
2. Entrar a `Mi equipaje y recargos`.
3. Elegir `Ver mi equipaje registrado`.
4. Elegir registrar por una de mis reservas.
5. Mostrar que el sistema lista solo reservas propias.
6. Ingresar una reserva propia.
7. Registrar equipaje.
8. Mostrar calculo y confirmacion.
9. Explicar que si intenta usar una reserva ajena, el sistema lo bloquea.

---

## 20. Explicacion corta para decir en sustentacion

"El modulo Baggage gestiona el equipaje y los recargos asociados a reservas y tiquetes. Esta construido siguiendo la misma arquitectura del proyecto: Domain, Application, Infrastructure y UI. En Domain tengo el agregado `BaggageRecord`, value objects para validar datos como tipo, cantidad, peso y recargos, y modelos auxiliares para politicas y resultados. En Application estan los casos de uso: registrar equipaje, previsualizar recargos y consultar por cliente, vuelo o recargos. La logica de negocio principal esta en `BaggageSurchargeCalculator`, que aplica reglas segun cabina y tipo de equipaje. En Infrastructure uso Entity Framework Core para mapear la tabla `baggage_records` y hacer joins con reservas, tiquetes, vuelos, pasajeros y cabinas. En UI hay un menu para Admin/Staff y otro para cliente autenticado, donde el cliente solo puede usar sus propias reservas y tiquetes. Cuando se registra equipaje, el sistema calcula el recargo, guarda el registro y si hay recargo lo suma al total de la reserva."

---

## 21. Preguntas probables y respuestas

### Por que usaste value objects?

Para evitar guardar datos invalidos. Por ejemplo, `BaggageQuantity` no permite cantidad cero o negativa, `BaggageType` solo permite `MANO` o `BODEGA`, y `BaggageSurchargeAmount` no permite recargos negativos.

### Por que existe `BaggageRecordEntity` si ya existe `BaggageRecord`?

Porque `BaggageRecord` es el modelo de dominio y usa value objects. `BaggageRecordEntity` es la clase plana que EF Core usa para mapear la tabla MySQL.

### Donde esta la logica de recargos?

En:

```text
src/Modules/Baggage/Application/Services/BaggageSurchargeCalculator.cs
```

Alli se definen las politicas por cabina y tipo de equipaje.

### Como se actualiza el valor total de la reserva?

En:

```text
src/Modules/Baggage/Infrastructure/Repository/BaggageRecordRepository.cs
```

Metodo:

```csharp
AddSurchargeToReservationAsync
```

Suma el recargo a `reservation.TotalAmount`.

### Como se evita que el cliente registre equipaje de otro cliente?

En `CustomerBaggageMenu`. Antes de registrar, el sistema lista reservas o tiquetes del cliente autenticado y valida que el id ingresado pertenezca a esa lista.

### Que tabla guarda el equipaje?

```text
baggage_records
```

### Se guarda la politica aplicada?

Si. Se guardan:

- cantidad permitida
- peso permitido por maleta
- peso total permitido
- excesos
- recargos

Eso permite consultar historicamente que regla se uso al momento del registro.

---

## 22. Archivos clave para abrir durante la sustentacion

1. `src/Modules/Baggage/Application/Services/BaggageSurchargeCalculator.cs`
   - Mostrar reglas de negocio.

2. `src/Modules/Baggage/Application/UseCases/RegisterBaggageUseCase.cs`
   - Mostrar flujo principal de registro.

3. `src/Modules/Baggage/Infrastructure/Repository/BaggageRecordRepository.cs`
   - Mostrar persistencia, joins y actualizacion de reserva.

4. `src/Modules/Baggage/UI/BaggageMenu.cs`
   - Mostrar menu Admin/Staff.

5. `src/Modules/Baggage/UI/CustomerBaggageMenu.cs`
   - Mostrar validacion de cliente autenticado.

6. `src/Modules/Baggage/Infrastructure/Entity/BaggageRecordEntityConfiguration.cs`
   - Mostrar tabla, columnas y relaciones.

7. `Program.cs`
   - Mostrar integracion del modulo.

8. `src/shared/Context/AppDbContext.cs`
   - Mostrar `DbSet<BaggageRecordEntity>`.

---

## 23. Checklist de funcionamiento

- [x] Registra equipaje por reserva.
- [x] Registra equipaje por tiquete.
- [x] Valida tipo de equipaje.
- [x] Valida cantidad positiva.
- [x] Valida peso positivo.
- [x] Valida cabina existente.
- [x] Calcula peso total.
- [x] Calcula exceso por cantidad.
- [x] Calcula exceso por peso total.
- [x] Calcula exceso por peso por maleta.
- [x] Calcula recargo por cantidad.
- [x] Calcula recargo por peso.
- [x] Suma recargo total.
- [x] Pide confirmacion antes de guardar.
- [x] Persiste en `baggage_records`.
- [x] Actualiza total de reserva si hay recargo.
- [x] Consulta por cliente.
- [x] Consulta por vuelo.
- [x] Consulta recargos.
- [x] Menu Admin/Staff.
- [x] Menu Cliente autenticado.
- [x] Compila correctamente.

---

## 24. Nota sobre el modulo Checkins

Existe un modulo `Checkins` que tambien maneja datos relacionados con equipaje, como:

- Si el pasajero tiene equipaje en bodega.
- Peso de equipaje en check-in.

Pero `Baggage` cumple otro objetivo:

- Registrar equipaje por reserva o tiquete.
- Calcular recargos.
- Guardar politica aplicada.
- Actualizar total de la reserva.
- Consultar recargos.

Por eso ambos modulos pueden coexistir. `Checkins` esta mas asociado al proceso operativo de abordaje/check-in; `Baggage` esta asociado a gestion comercial de equipaje y cobros.

---

## 25. Comandos utiles

Compilar:

```powershell
dotnet build --no-restore
```

Ejecutar:

```powershell
dotnet run
```

Aplicar migraciones:

```powershell
dotnet ef database update
```

Crear una nueva migracion si se cambia el modelo:

```powershell
dotnet ef migrations add NombreDeLaMigracion
```

Ver migraciones:

```powershell
dotnet ef migrations list
```

---

## 26. Guia mental para entender el codigo

Si necesitas explicarlo paso a paso, sigue esta ruta:

1. Empieza por `BaggageMenu` o `CustomerBaggageMenu`, porque ahi se ve lo que el usuario hace.
2. Luego ve a `PreviewBaggageSurchargeUseCase`, porque muestra el calculo antes de guardar.
3. Luego ve a `BaggageSurchargeCalculator`, porque ahi estan las reglas.
4. Luego ve a `RegisterBaggageUseCase`, porque ahi ocurre el registro real.
5. Luego ve a `BaggageRecord`, porque es el objeto que se crea.
6. Luego ve a `BaggageRecordRepository`, porque ahi se guarda y se actualiza la reserva.
7. Luego ve a `BaggageRecordEntityConfiguration`, porque ahi esta la tabla.
8. Finalmente muestra `Program.cs` y `AppDbContext.cs`, porque ahi esta la integracion.

---

## 27. Ejemplo de calculo para explicar

Supongamos:

- Cabina: Economica.
- Tipo: Bodega.
- Cantidad: 2 maletas.
- Peso por maleta: 25 kg.
- Peso total: 50 kg.

Politica economica bodega:

- 1 maleta permitida.
- 23 kg por maleta.
- 23 kg total.
- 100000 por maleta extra.
- 18000 por kg excedido.

Calculos:

```text
exceso_cantidad = max(0, 2 - 1) = 1
recargo_cantidad = 1 * 100000 = 100000

peso_total = 2 * 25 = 50
exceso_peso_total = max(0, 50 - 23) = 27

peso_promedio_por_maleta = 50 / 2 = 25
exceso_por_maleta = max(0, 25 - 23) * 2 = 4

exceso_peso = max(27, 4) = 27
recargo_peso = 27 * 18000 = 486000

recargo_total = 100000 + 486000 = 586000
```

Si la reserva antes valia `1,000,000`, despues queda:

```text
1,000,000 + 586,000 = 1,586,000
```

---

## 28. Estado final verificado

Se ejecuto:

```powershell
dotnet build --no-restore
```

Resultado:

```text
Compilacion correcta.
0 Advertencia(s)
0 Errores
```

Por tanto, el modulo compila integrado con el resto del proyecto.

