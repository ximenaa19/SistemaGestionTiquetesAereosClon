# Implementacion: Reprogramacion de Reservas y Lista de Espera

## 1. Que se implemento

Se implemento un flujo completo y conservador para cliente autenticado con estos objetivos:

- Reprogramar una reserva propia sin pedir `customer_id` por consola.
- Mostrar solo reservas del cliente activo.
- Validar compatibilidad de vuelos y estado de la reserva.
- Si hay cupo, reprogramar y persistir cambios.
- Si no hay cupo, permitir entrada a lista de espera.
- Promover automaticamente lista de espera al liberar cupo (por cancelacion o reprogramacion).
- Registrar trazabilidad en historial de cambios.

Tambien se integro la opcion directa en menu cliente para iniciar esta logica (sin enviar al menu completo de reservas).

## 2. Como se implemento (paso a paso)

### Paso A: Dominio

Se agregaron dos agregados de dominio:

- `ReservationWaitlistEntry`: representa una solicitud pendiente/promovida en lista de espera.
- `ReservationRescheduleHistory`: representa eventos de trazabilidad (reprogramacion, waitlist, promocion).

Y dos contratos de repositorio para no romper separacion por capas:

- `IReservationWaitlistRepository`
- `IReservationRescheduleHistoryRepository`

### Paso B: Infraestructura (EF Core)

Se agregaron entidades y configuraciones EF para persistir en MySQL:

- `reservation_waitlist`
- `reservation_reschedule_history`

Tambien se crearon repositorios concretos para consultas y actualizaciones.

### Paso C: Casos de uso (Application)

Se crearon casos de uso especificos:

- `AddReservationToWaitlistUseCase`: agrega a espera evitando duplicados pendientes.
- `PromoteWaitlistForFlightUseCase`: promueve automaticamente al primer candidato cuando se libera un asiento.
- `RescheduleReservationUseCase`: valida y reprograma reserva del cliente activo, actualiza cupos y guarda historial.
- `CancelReservationForCustomerUseCase`: cancela reserva propia, libera cupos y dispara promocion automatica.

### Paso D: UI cliente

Se creo `CustomerRescheduleReservationFlow` con estilo de consola ya existente.

Este flujo:

- Toma cliente desde sesion (inyectado en constructor).
- Lista reservas confirmadas del cliente.
- Pide `reservation_id` y `reservation_flight_id`.
- Lista vuelos compatibles (misma ruta, futuros, no cancelados/completados).
- Si no hay vuelos, muestra exactamente: `No hay vuelos disponibles`.
- Si hay cupo: reprograma.
- Si no hay cupo: pregunta si desea entrar a lista de espera.
- Soporta cancelacion global con `000000`.

### Paso E: Integracion sin romper lo existente

- Se registro wiring en `Program.cs` para construir repositorios/casos de uso nuevos.
- Se agrego opcion directa en `CustomerSelfServiceMenu` para ejecutar el nuevo flujo.
- Se ajusto cancelacion de cliente para usar caso de uso seguro con promocion automatica.
- Se registraron nuevos `DbSet` en `AppDbContext`.

## 3. Por que se hizo asi

- Se mantuvo arquitectura actual (Domain/Application/Infrastructure/UI).
- Se hicieron cambios localizados para reducir riesgo de regresion.
- Se reutilizo logica existente de reservas/vuelos en vez de duplicar.
- Se priorizo consistencia transaccional basica con guardados puntuales y validaciones defensivas.

## 4. Archivos nuevos creados (codigo completo)

### src/Modules/Reservations/Domain/Aggregate/ReservationWaitlistEntry.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Domain
// Archivo: src\Modules\Reservations\Domain\Aggregate\ReservationWaitlistEntry.cs
// Responsabilidad: Representa una solicitud de lista de espera para mover una reserva a un vuelo sin cupo.
namespace GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;

public sealed class ReservationWaitlistEntry
{
    public int Id { get; private set; }
    public int ReservationId { get; private set; }
    public int ReservationFlightId { get; private set; }
    public int RequestedFlightId { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public int QueueOrder { get; private set; }
    public string Status { get; private set; }
    public string? Reason { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    private ReservationWaitlistEntry(
        int id,
        int reservationId,
        int reservationFlightId,
        int requestedFlightId,
        DateTime requestedAt,
        int queueOrder,
        string status,
        string? reason,
        DateTime? processedAt)
    {
        Id = id;
        ReservationId = reservationId;
        ReservationFlightId = reservationFlightId;
        RequestedFlightId = requestedFlightId;
        RequestedAt = requestedAt;
        QueueOrder = queueOrder;
        Status = status;
        Reason = reason;
        ProcessedAt = processedAt;
    }

    public static ReservationWaitlistEntry CreateNew(
        int reservationId,
        int reservationFlightId,
        int requestedFlightId,
        int queueOrder,
        string? reason)
    {
        if (reservationId <= 0) throw new ArgumentException("reservation_id invalido");
        if (reservationFlightId <= 0) throw new ArgumentException("reservation_flight_id invalido");
        if (requestedFlightId <= 0) throw new ArgumentException("requested_flight_id invalido");
        if (queueOrder <= 0) throw new ArgumentException("queue_order invalido");

        return new ReservationWaitlistEntry(
            id: 0,
            reservationId: reservationId,
            reservationFlightId: reservationFlightId,
            requestedFlightId: requestedFlightId,
            requestedAt: DateTime.Now,
            queueOrder: queueOrder,
            status: "PENDIENTE",
            reason: reason,
            processedAt: null);
    }

    public static ReservationWaitlistEntry Rehydrate(
        int id,
        int reservationId,
        int reservationFlightId,
        int requestedFlightId,
        DateTime requestedAt,
        int queueOrder,
        string status,
        string? reason,
        DateTime? processedAt)
    {
        return new ReservationWaitlistEntry(
            id,
            reservationId,
            reservationFlightId,
            requestedFlightId,
            requestedAt,
            queueOrder,
            status,
            reason,
            processedAt);
    }
}



```

### src/Modules/Reservations/Domain/Aggregate/ReservationRescheduleHistory.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Domain
// Archivo: src\Modules\Reservations\Domain\Aggregate\ReservationRescheduleHistory.cs
// Responsabilidad: Traza cambios de reprogramacion/lista de espera asociados a reservas.
namespace GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;

public sealed class ReservationRescheduleHistory
{
    public int Id { get; private set; }
    public int ReservationId { get; private set; }
    public int OldFlightId { get; private set; }
    public int NewFlightId { get; private set; }
    public DateTime ChangedAt { get; private set; }
    public string? Reason { get; private set; }
    public string ActionStatus { get; private set; }

    private ReservationRescheduleHistory(
        int id,
        int reservationId,
        int oldFlightId,
        int newFlightId,
        DateTime changedAt,
        string? reason,
        string actionStatus)
    {
        Id = id;
        ReservationId = reservationId;
        OldFlightId = oldFlightId;
        NewFlightId = newFlightId;
        ChangedAt = changedAt;
        Reason = reason;
        ActionStatus = actionStatus;
    }

    public static ReservationRescheduleHistory CreateNew(
        int reservationId,
        int oldFlightId,
        int newFlightId,
        string? reason,
        string actionStatus)
    {
        if (reservationId <= 0) throw new ArgumentException("reservation_id invalido");
        if (oldFlightId <= 0) throw new ArgumentException("old_flight_id invalido");
        if (newFlightId <= 0) throw new ArgumentException("new_flight_id invalido");
        if (string.IsNullOrWhiteSpace(actionStatus)) throw new ArgumentException("action_status es obligatorio");

        return new ReservationRescheduleHistory(
            id: 0,
            reservationId: reservationId,
            oldFlightId: oldFlightId,
            newFlightId: newFlightId,
            changedAt: DateTime.Now,
            reason: string.IsNullOrWhiteSpace(reason) ? null : reason.Trim(),
            actionStatus: actionStatus.Trim().ToUpperInvariant());
    }

    public static ReservationRescheduleHistory Rehydrate(
        int id,
        int reservationId,
        int oldFlightId,
        int newFlightId,
        DateTime changedAt,
        string? reason,
        string actionStatus)
    {
        return new ReservationRescheduleHistory(
            id,
            reservationId,
            oldFlightId,
            newFlightId,
            changedAt,
            reason,
            actionStatus);
    }
}



```

### src/Modules/Reservations/Domain/Repositories/IReservationWaitlistRepository.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Domain
// Archivo: src\Modules\Reservations\Domain\Repositories\IReservationWaitlistRepository.cs
// Responsabilidad: Contrato para persistir y consultar lista de espera de reprogramaciones.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Reservations.Domain.Repositories;

public interface IReservationWaitlistRepository
{
    Task AddAsync(ReservationWaitlistEntry entry);
    Task UpdateAsync(ReservationWaitlistEntry entry);
    Task<ReservationWaitlistEntry?> GetByIdAsync(int id);
    Task<ReservationWaitlistEntry?> GetFirstPendingByRequestedFlightIdAsync(int requestedFlightId);
    Task<IEnumerable<ReservationWaitlistEntry>> GetPendingByRequestedFlightIdAsync(int requestedFlightId);
    Task<bool> ExistsPendingByReservationFlightAndRequestedFlightAsync(int reservationFlightId, int requestedFlightId);
    Task<int> GetNextQueueOrderForRequestedFlightAsync(int requestedFlightId);
}



```

### src/Modules/Reservations/Domain/Repositories/IReservationRescheduleHistoryRepository.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Domain
// Archivo: src\Modules\Reservations\Domain\Repositories\IReservationRescheduleHistoryRepository.cs
// Responsabilidad: Contrato para registrar trazabilidad de reprogramaciones y promociones.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Reservations.Domain.Repositories;

public interface IReservationRescheduleHistoryRepository
{
    Task AddAsync(ReservationRescheduleHistory entry);
    Task<IEnumerable<ReservationRescheduleHistory>> GetByReservationIdAsync(int reservationId);
}



```

### src/Modules/Reservations/Infrastructure/Entity/ReservationWaitlistEntity.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Entity\ReservationWaitlistEntity.cs
// Responsabilidad: Modelo EF para solicitudes de lista de espera de reprogramacion.
namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;

public sealed class ReservationWaitlistEntity
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public int ReservationFlightId { get; set; }
    public int RequestedFlightId { get; set; }
    public DateTime RequestedAt { get; set; }
    public int QueueOrder { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public DateTime? ProcessedAt { get; set; }
}



```

### src/Modules/Reservations/Infrastructure/Entity/ReservationWaitlistEntityConfiguration.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Entity\ReservationWaitlistEntityConfiguration.cs
// Responsabilidad: Configuracion EF Core de tabla reservation_waitlist.
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;

public sealed class ReservationWaitlistEntityConfiguration : IEntityTypeConfiguration<ReservationWaitlistEntity>
{
    public void Configure(EntityTypeBuilder<ReservationWaitlistEntity> builder)
    {
        builder.ToTable("reservation_waitlist");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.ReservationId).HasColumnName("reservation_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.ReservationFlightId).HasColumnName("reservation_flight_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.RequestedFlightId).HasColumnName("requested_flight_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.RequestedAt).HasColumnName("requested_at").HasColumnType("datetime").IsRequired();
        builder.Property(x => x.QueueOrder).HasColumnName("queue_order").HasColumnType("int").IsRequired();
        builder.Property(x => x.Status).HasColumnName("status").HasColumnType("varchar(30)").HasMaxLength(30).IsRequired();
        builder.Property(x => x.Reason).HasColumnName("reason").HasColumnType("varchar(255)").HasMaxLength(255);
        builder.Property(x => x.ProcessedAt).HasColumnName("processed_at").HasColumnType("datetime");

        builder.HasIndex(x => new { x.RequestedFlightId, x.Status, x.QueueOrder }).HasDatabaseName("ix_waitlist_requested_status_order");
        builder.HasIndex(x => new { x.ReservationFlightId, x.RequestedFlightId, x.Status }).HasDatabaseName("ix_waitlist_unique_pending");

        builder.HasOne<ReservationEntity>().WithMany().HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<ReservationFlightEntity>().WithMany().HasForeignKey(x => x.ReservationFlightId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<FlightEntity>().WithMany().HasForeignKey(x => x.RequestedFlightId).OnDelete(DeleteBehavior.Restrict);
    }
}



```

### src/Modules/Reservations/Infrastructure/Entity/ReservationRescheduleHistoryEntity.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Entity\ReservationRescheduleHistoryEntity.cs
// Responsabilidad: Modelo EF para trazabilidad de reprogramaciones/promociones.
namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;

public sealed class ReservationRescheduleHistoryEntity
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public int OldFlightId { get; set; }
    public int NewFlightId { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Reason { get; set; }
    public string ActionStatus { get; set; } = string.Empty;
}



```

### src/Modules/Reservations/Infrastructure/Entity/ReservationRescheduleHistoryEntityConfiguration.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Entity\ReservationRescheduleHistoryEntityConfiguration.cs
// Responsabilidad: Configuracion EF Core de tabla reservation_reschedule_history.
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;

public sealed class ReservationRescheduleHistoryEntityConfiguration : IEntityTypeConfiguration<ReservationRescheduleHistoryEntity>
{
    public void Configure(EntityTypeBuilder<ReservationRescheduleHistoryEntity> builder)
    {
        builder.ToTable("reservation_reschedule_history");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int").ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.ReservationId).HasColumnName("reservation_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.OldFlightId).HasColumnName("old_flight_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.NewFlightId).HasColumnName("new_flight_id").HasColumnType("int").IsRequired();
        builder.Property(x => x.ChangedAt).HasColumnName("changed_at").HasColumnType("datetime").IsRequired();
        builder.Property(x => x.Reason).HasColumnName("reason").HasColumnType("varchar(255)").HasMaxLength(255);
        builder.Property(x => x.ActionStatus).HasColumnName("action_status").HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();

        builder.HasIndex(x => new { x.ReservationId, x.ChangedAt }).HasDatabaseName("ix_reschedule_history_reservation_date");

        builder.HasOne<ReservationEntity>().WithMany().HasForeignKey(x => x.ReservationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<FlightEntity>().WithMany().HasForeignKey(x => x.OldFlightId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<FlightEntity>().WithMany().HasForeignKey(x => x.NewFlightId).OnDelete(DeleteBehavior.Restrict);
    }
}



```

### src/Modules/Reservations/Infrastructure/Repository/ReservationWaitlistRepository.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Repository\ReservationWaitlistRepository.cs
// Responsabilidad: Acceso a datos de lista de espera de reprogramacion.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;

public sealed class ReservationWaitlistRepository : IReservationWaitlistRepository
{
    private readonly AppDbContext _context;

    public ReservationWaitlistRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ReservationWaitlistEntry entry)
    {
        await _context.Set<ReservationWaitlistEntity>().AddAsync(MapToEntity(entry));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ReservationWaitlistEntry entry)
    {
        var entity = await _context.Set<ReservationWaitlistEntity>().FirstOrDefaultAsync(x => x.Id == entry.Id);
        if (entity is null)
            return;

        entity.ReservationId = entry.ReservationId;
        entity.ReservationFlightId = entry.ReservationFlightId;
        entity.RequestedFlightId = entry.RequestedFlightId;
        entity.RequestedAt = entry.RequestedAt;
        entity.QueueOrder = entry.QueueOrder;
        entity.Status = entry.Status;
        entity.Reason = entry.Reason;
        entity.ProcessedAt = entry.ProcessedAt;

        await _context.SaveChangesAsync();
    }

    public async Task<ReservationWaitlistEntry?> GetByIdAsync(int id)
    {
        var entity = await _context.Set<ReservationWaitlistEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<ReservationWaitlistEntry?> GetFirstPendingByRequestedFlightIdAsync(int requestedFlightId)
    {
        var entity = await _context.Set<ReservationWaitlistEntity>()
            .AsNoTracking()
            .Where(x => x.RequestedFlightId == requestedFlightId && x.Status == "PENDIENTE")
            .OrderBy(x => x.QueueOrder)
            .ThenBy(x => x.RequestedAt)
            .ThenBy(x => x.Id)
            .FirstOrDefaultAsync();

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<ReservationWaitlistEntry>> GetPendingByRequestedFlightIdAsync(int requestedFlightId)
    {
        var entities = await _context.Set<ReservationWaitlistEntity>()
            .AsNoTracking()
            .Where(x => x.RequestedFlightId == requestedFlightId && x.Status == "PENDIENTE")
            .OrderBy(x => x.QueueOrder)
            .ThenBy(x => x.RequestedAt)
            .ThenBy(x => x.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public Task<bool> ExistsPendingByReservationFlightAndRequestedFlightAsync(int reservationFlightId, int requestedFlightId)
    {
        return _context.Set<ReservationWaitlistEntity>()
            .AsNoTracking()
            .AnyAsync(x => x.ReservationFlightId == reservationFlightId
                           && x.RequestedFlightId == requestedFlightId
                           && x.Status == "PENDIENTE");
    }

    public async Task<int> GetNextQueueOrderForRequestedFlightAsync(int requestedFlightId)
    {
        var max = await _context.Set<ReservationWaitlistEntity>()
            .AsNoTracking()
            .Where(x => x.RequestedFlightId == requestedFlightId)
            .Select(x => (int?)x.QueueOrder)
            .MaxAsync();

        return (max ?? 0) + 1;
    }

    private static ReservationWaitlistEntry MapToDomain(ReservationWaitlistEntity entity)
    {
        return ReservationWaitlistEntry.Rehydrate(
            entity.Id,
            entity.ReservationId,
            entity.ReservationFlightId,
            entity.RequestedFlightId,
            entity.RequestedAt,
            entity.QueueOrder,
            entity.Status,
            entity.Reason,
            entity.ProcessedAt);
    }

    private static ReservationWaitlistEntity MapToEntity(ReservationWaitlistEntry entry)
    {
        return new ReservationWaitlistEntity
        {
            Id = entry.Id,
            ReservationId = entry.ReservationId,
            ReservationFlightId = entry.ReservationFlightId,
            RequestedFlightId = entry.RequestedFlightId,
            RequestedAt = entry.RequestedAt,
            QueueOrder = entry.QueueOrder,
            Status = entry.Status,
            Reason = entry.Reason,
            ProcessedAt = entry.ProcessedAt
        };
    }
}



```

### src/Modules/Reservations/Infrastructure/Repository/ReservationRescheduleHistoryRepository.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Infrastructure
// Archivo: src\Modules\Reservations\Infrastructure\Repository\ReservationRescheduleHistoryRepository.cs
// Responsabilidad: Persistencia de historial de reprogramaciones/promociones.
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;

public sealed class ReservationRescheduleHistoryRepository : IReservationRescheduleHistoryRepository
{
    private readonly AppDbContext _context;

    public ReservationRescheduleHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ReservationRescheduleHistory entry)
    {
        await _context.Set<ReservationRescheduleHistoryEntity>().AddAsync(MapToEntity(entry));
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ReservationRescheduleHistory>> GetByReservationIdAsync(int reservationId)
    {
        var list = await _context.Set<ReservationRescheduleHistoryEntity>()
            .AsNoTracking()
            .Where(x => x.ReservationId == reservationId)
            .OrderByDescending(x => x.ChangedAt)
            .ThenByDescending(x => x.Id)
            .ToListAsync();

        return list.Select(MapToDomain).ToList();
    }

    private static ReservationRescheduleHistory MapToDomain(ReservationRescheduleHistoryEntity entity)
    {
        return ReservationRescheduleHistory.Rehydrate(
            entity.Id,
            entity.ReservationId,
            entity.OldFlightId,
            entity.NewFlightId,
            entity.ChangedAt,
            entity.Reason,
            entity.ActionStatus);
    }

    private static ReservationRescheduleHistoryEntity MapToEntity(ReservationRescheduleHistory entry)
    {
        return new ReservationRescheduleHistoryEntity
        {
            Id = entry.Id,
            ReservationId = entry.ReservationId,
            OldFlightId = entry.OldFlightId,
            NewFlightId = entry.NewFlightId,
            ChangedAt = entry.ChangedAt,
            Reason = entry.Reason,
            ActionStatus = entry.ActionStatus
        };
    }
}



```

### src/Modules/Reservations/Application/UseCases/AddReservationToWaitlistUseCase.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Application
// Archivo: src\Modules\Reservations\Application\UseCases\AddReservationToWaitlistUseCase.cs
// Responsabilidad: Registra una solicitud en lista de espera para reprogramacion.
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public sealed class AddReservationToWaitlistUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;
    private readonly IReservationWaitlistRepository _waitlistRepository;
    private readonly IReservationRescheduleHistoryRepository _historyRepository;

    public AddReservationToWaitlistUseCase(
        IReservationRepository reservationRepository,
        IReservationFlightRepository reservationFlightRepository,
        IReservationWaitlistRepository waitlistRepository,
        IReservationRescheduleHistoryRepository historyRepository)
    {
        _reservationRepository = reservationRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _waitlistRepository = waitlistRepository;
        _historyRepository = historyRepository;
    }

    public async Task AddAsync(int reservationId, int reservationFlightId, int requestedFlightId, string? reason)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId));
        if (reservation is null)
            throw new Exception("La reserva no existe.");

        var reservationFlight = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(reservationFlightId));
        if (reservationFlight is null || reservationFlight.ReservationId.Value != reservationId)
            throw new Exception("El tramo de reserva no existe o no pertenece a la reserva.");

        if (reservationFlight.FlightId.Value == requestedFlightId)
            throw new Exception("El nuevo vuelo debe ser diferente al vuelo actual.");

        var existsPending = await _waitlistRepository
            .ExistsPendingByReservationFlightAndRequestedFlightAsync(reservationFlightId, requestedFlightId);
        if (existsPending)
            throw new Exception("La reserva ya esta en lista de espera para ese vuelo.");

        var queueOrder = await _waitlistRepository.GetNextQueueOrderForRequestedFlightAsync(requestedFlightId);
        var entry = ReservationWaitlistEntry.CreateNew(
            reservationId,
            reservationFlightId,
            requestedFlightId,
            queueOrder,
            reason);

        await _waitlistRepository.AddAsync(entry);

        var history = ReservationRescheduleHistory.CreateNew(
            reservationId,
            reservationFlight.FlightId.Value,
            requestedFlightId,
            reason,
            "WAITLIST");
        await _historyRepository.AddAsync(history);
    }
}



```

### src/Modules/Reservations/Application/UseCases/PromoteWaitlistForFlightUseCase.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Application
// Archivo: src\Modules\Reservations\Application\UseCases\PromoteWaitlistForFlightUseCase.cs
// Responsabilidad: Promueve automaticamente al primer candidato en espera cuando se libera cupo.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public sealed class PromoteWaitlistForFlightUseCase
{
    private readonly IReservationWaitlistRepository _waitlistRepository;
    private readonly IReservationRescheduleHistoryRepository _historyRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;
    private readonly IReservationPassengerRepository _reservationPassengerRepository;
    private readonly FlightRepository _flightRepository;
    private readonly UpdateReservationFlightUseCase _updateReservationFlight;

    public PromoteWaitlistForFlightUseCase(
        IReservationWaitlistRepository waitlistRepository,
        IReservationRescheduleHistoryRepository historyRepository,
        IReservationFlightRepository reservationFlightRepository,
        IReservationPassengerRepository reservationPassengerRepository,
        FlightRepository flightRepository,
        UpdateReservationFlightUseCase updateReservationFlight)
    {
        _waitlistRepository = waitlistRepository;
        _historyRepository = historyRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _flightRepository = flightRepository;
        _updateReservationFlight = updateReservationFlight;
    }

    public async Task<bool> ExecuteAsync(int releasedFlightId, string trigger)
    {
        var pending = await _waitlistRepository.GetFirstPendingByRequestedFlightIdAsync(releasedFlightId);
        if (pending is null)
            return false;

        var requestedFlight = await _flightRepository.GetByIdAsync(FlightId.Create(releasedFlightId));
        if (requestedFlight is null)
            return false;

        var reservationFlight = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(pending.ReservationFlightId));
        if (reservationFlight is null)
            return false;

        var passengers = (await _reservationPassengerRepository.GetByReservationFlightIdAsync(
            ReservationPassengerReservationFlightId.Create(reservationFlight.Id.Value))).ToList();
        var seatsNeeded = passengers.Count;
        if (seatsNeeded <= 0)
            seatsNeeded = 1;

        if (requestedFlight.AvailableSeats.Value < seatsNeeded)
            return false;

        var oldFlight = await _flightRepository.GetByIdAsync(FlightId.Create(reservationFlight.FlightId.Value));
        if (oldFlight is null)
            return false;

        await _updateReservationFlight.ExecuteAsync(
            reservationFlight.Id.Value,
            reservationFlight.ReservationId.Value,
            releasedFlightId,
            reservationFlight.PartialAmount.Value);

        if (oldFlight.Id.Value != releasedFlightId)
            await UpdateFlightSeatsAsync(oldFlight, +seatsNeeded);

        var freshRequested = await _flightRepository.GetByIdAsync(FlightId.Create(releasedFlightId));
        if (freshRequested is not null)
            await UpdateFlightSeatsAsync(freshRequested, -seatsNeeded);

        var completed = ReservationWaitlistEntry.Rehydrate(
            pending.Id,
            pending.ReservationId,
            pending.ReservationFlightId,
            pending.RequestedFlightId,
            pending.RequestedAt,
            pending.QueueOrder,
            "PROMOVIDA",
            pending.Reason,
            DateTime.Now);
        await _waitlistRepository.UpdateAsync(completed);

        var history = ReservationRescheduleHistory.CreateNew(
            pending.ReservationId,
            oldFlight.Id.Value,
            releasedFlightId,
            $"Promocion automatica por cupo liberado ({trigger})",
            "PROMOTED");
        await _historyRepository.AddAsync(history);

        return true;
    }

    private async Task UpdateFlightSeatsAsync(Flight flight, int delta)
    {
        var newValue = flight.AvailableSeats.Value + delta;
        if (newValue < 0) newValue = 0;
        if (newValue > flight.TotalCapacity.Value) newValue = flight.TotalCapacity.Value;

        var updated = Flight.Create(
            flight.Id,
            flight.Code,
            flight.AirlineId,
            flight.RouteId,
            flight.AircraftId,
            flight.DepartureDateTime,
            flight.EstimatedArrivalDateTime,
            flight.TotalCapacity,
            FlightAvailableSeats.Create(newValue),
            flight.StateId,
            flight.RescheduledAt);

        await _flightRepository.UpdateAsync(updated);
    }
}



```

### src/Modules/Reservations/Application/UseCases/RescheduleReservationUseCase.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Application
// Archivo: src\Modules\Reservations\Application\UseCases\RescheduleReservationUseCase.cs
// Responsabilidad: Reprograma una reserva confirmada a otro vuelo compatible con cupo.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public sealed class RescheduleReservationUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;
    private readonly IReservationPassengerRepository _reservationPassengerRepository;
    private readonly FlightRepository _flightRepository;
    private readonly ReservationStatusRepository _reservationStatusRepository;
    private readonly UpdateReservationStatusUseCase _updateReservationStatus;
    private readonly UpdateReservationFlightUseCase _updateReservationFlight;
    private readonly IReservationRescheduleHistoryRepository _historyRepository;
    private readonly PromoteWaitlistForFlightUseCase _promoteWaitlist;

    public RescheduleReservationUseCase(
        IReservationRepository reservationRepository,
        IReservationFlightRepository reservationFlightRepository,
        IReservationPassengerRepository reservationPassengerRepository,
        FlightRepository flightRepository,
        ReservationStatusRepository reservationStatusRepository,
        UpdateReservationStatusUseCase updateReservationStatus,
        UpdateReservationFlightUseCase updateReservationFlight,
        IReservationRescheduleHistoryRepository historyRepository,
        PromoteWaitlistForFlightUseCase promoteWaitlist)
    {
        _reservationRepository = reservationRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _flightRepository = flightRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _updateReservationStatus = updateReservationStatus;
        _updateReservationFlight = updateReservationFlight;
        _historyRepository = historyRepository;
        _promoteWaitlist = promoteWaitlist;
    }

    public async Task ExecuteAsync(int customerId, int reservationId, int reservationFlightId, int newFlightId, string? reason)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId));
        if (reservation is null)
            throw new Exception("La reserva no existe.");

        if (reservation.CustomerId.Value != customerId)
            throw new Exception("La reserva no pertenece al cliente activo.");

        var status = await _reservationStatusRepository.GetByIdAsync(
            GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject.ReservationStatusId.Create(reservation.StatusId.Value));
        var statusName = (status?.Name.Value ?? string.Empty).Trim().ToUpperInvariant();
        if (!statusName.Contains("CONFIRM"))
            throw new Exception("Solo se pueden reprogramar reservas confirmadas.");

        var rf = await _reservationFlightRepository.GetByIdAsync(ReservationFlightId.Create(reservationFlightId));
        if (rf is null || rf.ReservationId.Value != reservationId)
            throw new Exception("El vuelo asociado a la reserva no es valido.");

        if (rf.FlightId.Value == newFlightId)
            throw new Exception("El nuevo vuelo no puede ser el mismo vuelo actual.");

        var oldFlight = await _flightRepository.GetByIdAsync(FlightId.Create(rf.FlightId.Value));
        var newFlight = await _flightRepository.GetByIdAsync(FlightId.Create(newFlightId));
        if (oldFlight is null || newFlight is null)
            throw new Exception("No se encontraron los vuelos de la reprogramacion.");

        if (newFlight.RouteId.Value != oldFlight.RouteId.Value)
            throw new Exception("El nuevo vuelo no es compatible con la misma ruta.");
        if (newFlight.DepartureDateTime.Value <= DateTime.Now)
            throw new Exception("La fecha del nuevo vuelo ya no es valida.");

        var passengers = (await _reservationPassengerRepository.GetByReservationFlightIdAsync(
            ReservationPassengerReservationFlightId.Create(rf.Id.Value))).ToList();
        var seatsNeeded = passengers.Count;
        if (seatsNeeded <= 0)
            seatsNeeded = 1;

        if (newFlight.AvailableSeats.Value < seatsNeeded)
            throw new Exception("El vuelo seleccionado no tiene cupo disponible.");

        await _updateReservationFlight.ExecuteAsync(
            rf.Id.Value,
            rf.ReservationId.Value,
            newFlightId,
            rf.PartialAmount.Value);

        await UpdateFlightSeatsAsync(oldFlight, +seatsNeeded);
        await _promoteWaitlist.ExecuteAsync(oldFlight.Id.Value, "REPROGRAMACION_RESERVA");
        var refreshedNewFlight = await _flightRepository.GetByIdAsync(FlightId.Create(newFlightId));
        if (refreshedNewFlight is not null)
            await UpdateFlightSeatsAsync(refreshedNewFlight, -seatsNeeded);

        var reprogrammedStatus = await FindStatusIdByNameContainsAsync("REPROGRAM");
        if (reprogrammedStatus.HasValue && reprogrammedStatus.Value != reservation.StatusId.Value)
        {
            try
            {
                await _updateReservationStatus.ExecuteAsync(reservationId, reprogrammedStatus.Value);
            }
            catch
            {
                // Si la transiciÃ³n no existe en catÃ¡logo, mantenemos el estado actual para no romper flujo.
            }
        }

        var history = ReservationRescheduleHistory.CreateNew(
            reservationId,
            oldFlight.Id.Value,
            newFlightId,
            reason,
            "RESCHEDULED");
        await _historyRepository.AddAsync(history);
    }

    private async Task<int?> FindStatusIdByNameContainsAsync(string token)
    {
        var statuses = await _reservationStatusRepository.GetAllAsync();
        var found = statuses.FirstOrDefault(x => x.Name.Value.Trim().ToUpperInvariant().Contains(token));
        return found?.Id.Value;
    }

    private async Task UpdateFlightSeatsAsync(Flight flight, int delta)
    {
        var newValue = flight.AvailableSeats.Value + delta;
        if (newValue < 0) newValue = 0;
        if (newValue > flight.TotalCapacity.Value) newValue = flight.TotalCapacity.Value;

        var updated = Flight.Create(
            flight.Id,
            flight.Code,
            flight.AirlineId,
            flight.RouteId,
            flight.AircraftId,
            flight.DepartureDateTime,
            flight.EstimatedArrivalDateTime,
            flight.TotalCapacity,
            FlightAvailableSeats.Create(newValue),
            flight.StateId,
            flight.RescheduledAt);

        await _flightRepository.UpdateAsync(updated);
    }
}


```

### src/Modules/Reservations/Application/UseCases/CancelReservationForCustomerUseCase.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: Application
// Archivo: src\Modules\Reservations\Application\UseCases\CancelReservationForCustomerUseCase.cs
// Responsabilidad: Cancela una reserva del cliente activo, libera cupos y promueve lista de espera.
using GestionAerolineas.src.Modules.Flights.Domain.Aggregate;
using GestionAerolineas.src.Modules.Flights.Domain.ValueObject;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationFlights.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.ReservationPassengers.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public sealed class CancelReservationForCustomerUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IReservationFlightRepository _reservationFlightRepository;
    private readonly IReservationPassengerRepository _reservationPassengerRepository;
    private readonly FlightRepository _flightRepository;
    private readonly ReservationStatusRepository _reservationStatusRepository;
    private readonly UpdateReservationStatusUseCase _updateReservationStatus;
    private readonly PromoteWaitlistForFlightUseCase _promoteWaitlist;

    public CancelReservationForCustomerUseCase(
        IReservationRepository reservationRepository,
        IReservationFlightRepository reservationFlightRepository,
        IReservationPassengerRepository reservationPassengerRepository,
        FlightRepository flightRepository,
        ReservationStatusRepository reservationStatusRepository,
        UpdateReservationStatusUseCase updateReservationStatus,
        PromoteWaitlistForFlightUseCase promoteWaitlist)
    {
        _reservationRepository = reservationRepository;
        _reservationFlightRepository = reservationFlightRepository;
        _reservationPassengerRepository = reservationPassengerRepository;
        _flightRepository = flightRepository;
        _reservationStatusRepository = reservationStatusRepository;
        _updateReservationStatus = updateReservationStatus;
        _promoteWaitlist = promoteWaitlist;
    }

    public async Task CancelAsync(int customerId, int reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(ReservationId.Create(reservationId));
        if (reservation is null)
            throw new Exception("La reserva no existe.");

        if (reservation.CustomerId.Value != customerId)
            throw new Exception("La reserva no pertenece al cliente activo.");

        var cancelStatus = (await _reservationStatusRepository.GetAllAsync())
            .FirstOrDefault(x => x.Name.Value.Trim().ToUpperInvariant().Contains("CANCEL"));
        if (cancelStatus is null)
            throw new Exception("No existe estado Cancelada en catalogos.");

        var reservationFlights = (await _reservationFlightRepository.GetByReservationIdAsync(
            ReservationFlightReservationId.Create(reservationId))).ToList();

        foreach (var rf in reservationFlights)
        {
            var passengers = (await _reservationPassengerRepository.GetByReservationFlightIdAsync(
                ReservationPassengerReservationFlightId.Create(rf.Id.Value))).ToList();
            var seatsToReturn = passengers.Count;
            if (seatsToReturn <= 0)
                seatsToReturn = 1;

            var flight = await _flightRepository.GetByIdAsync(FlightId.Create(rf.FlightId.Value));
            if (flight is not null)
            {
                await UpdateFlightSeatsAsync(flight, +seatsToReturn);
                await _promoteWaitlist.ExecuteAsync(flight.Id.Value, "CANCELACION_RESERVA");
            }
        }

        await _updateReservationStatus.ExecuteAsync(reservationId, cancelStatus.Id.Value);
    }

    private async Task UpdateFlightSeatsAsync(Flight flight, int delta)
    {
        var newValue = flight.AvailableSeats.Value + delta;
        if (newValue < 0) newValue = 0;
        if (newValue > flight.TotalCapacity.Value) newValue = flight.TotalCapacity.Value;

        var updated = Flight.Create(
            flight.Id,
            flight.Code,
            flight.AirlineId,
            flight.RouteId,
            flight.AircraftId,
            flight.DepartureDateTime,
            flight.EstimatedArrivalDateTime,
            flight.TotalCapacity,
            FlightAvailableSeats.Create(newValue),
            flight.StateId,
            flight.RescheduledAt);

        await _flightRepository.UpdateAsync(updated);
    }
}



```

### src/Modules/Reservations/UI/CustomerCreateReservationWizard.cs

`csharp
// [DocHeader]
// Modulo: General
// Capa: General
// Archivo: src\Modules\Reservations\UI\CustomerCreateReservationWizard.cs
// Responsabilidad: Flujo guiado para que un cliente autenticado cree reservas sin pasar por el modulo completo de reservas.
// Flujo: Se ejecuta desde MENU CLIENTE -> opcion "Crear reserva (wizard simple)".
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.DocumentTypes.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationPassengers.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Routes.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Reservations.UI;

public sealed class CustomerCreateReservationWizard
{
    private readonly int _customerId;
    private readonly string _username;
    private readonly CreateReservationUseCase _createReservation;
    private readonly CreateReservationFlightUseCase _createReservationFlight;
    private readonly CreateReservationPassengerUseCase _createReservationPassenger;
    private readonly GetAllReservationStatusesUseCase _getAllReservationStatuses;
    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllRoutesUseCase _getAllRoutes;
    private readonly GetAllAirportsUseCase _getAllAirports;
    private readonly GetAllDocumentTypesUseCase _getAllDocumentTypes;
    private readonly CreatePersonUseCase _createPerson;
    private readonly GetPersonByDocumentUseCase _getPersonByDocument;
    private readonly CreatePassengerUseCase _createPassenger;
    private readonly GetPassengerByPersonIdUseCase _getPassengerByPersonId;
    private readonly GetAllPassengerTypesUseCase _getAllPassengerTypes;

    public CustomerCreateReservationWizard(
        int customerId,
        string username,
        CreateReservationUseCase createReservation,
        CreateReservationFlightUseCase createReservationFlight,
        CreateReservationPassengerUseCase createReservationPassenger,
        GetAllReservationStatusesUseCase getAllReservationStatuses,
        GetAllFlightsUseCase getAllFlights,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllRoutesUseCase getAllRoutes,
        GetAllAirportsUseCase getAllAirports,
        GetAllDocumentTypesUseCase getAllDocumentTypes,
        CreatePersonUseCase createPerson,
        GetPersonByDocumentUseCase getPersonByDocument,
        CreatePassengerUseCase createPassenger,
        GetPassengerByPersonIdUseCase getPassengerByPersonId,
        GetAllPassengerTypesUseCase getAllPassengerTypes)
    {
        _customerId = customerId;
        _username = username;
        _createReservation = createReservation;
        _createReservationFlight = createReservationFlight;
        _createReservationPassenger = createReservationPassenger;
        _getAllReservationStatuses = getAllReservationStatuses;
        _getAllFlights = getAllFlights;
        _getAllAirlines = getAllAirlines;
        _getAllRoutes = getAllRoutes;
        _getAllAirports = getAllAirports;
        _getAllDocumentTypes = getAllDocumentTypes;
        _createPerson = createPerson;
        _getPersonByDocument = getPersonByDocument;
        _createPassenger = createPassenger;
        _getPassengerByPersonId = getPassengerByPersonId;
        _getAllPassengerTypes = getAllPassengerTypes;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            var draft = await CollectDraftAsync();
            if (draft is null)
            {
                Console.WriteLine("\nOperacion cancelada por el usuario.");
                Pause();
                return;
            }

            var summary = await BuildSummaryAsync(draft);
            var choice = AdminFlowConsole.ReadConfirmChoice(summary);
            if (choice == 2)
                continue;

            if (choice == 3)
            {
                Console.WriteLine("\nOperacion cancelada por el usuario.");
                Pause();
                return;
            }

            try
            {
                var result = await PersistAsync(draft);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nReserva creada correctamente. Id={result.ReservationId}, Codigo={result.ReservationCode}");
                Console.ResetColor();
                Console.WriteLine($"Vuelos asociados: {result.FlightsCount}");
                Console.WriteLine($"Pasajeros asociados: {result.PassengersCount}");
            }
            catch (Exception ex)
            {
                AdminFlowConsole.PrintError($"No se pudo completar la operacion: {ex.GetBaseException().Message}");
            }

            Pause();
            return;
        }
    }

    private async Task<WizardDraft?> CollectDraftAsync()
    {
        var flightIds = await SelectFlightsAsync();
        if (flightIds is null)
            return null;

        var passengers = await CollectPassengersAsync();
        if (passengers is null)
            return null;

        return new WizardDraft(flightIds, passengers);
    }

    private async Task<List<int>?> SelectFlightsAsync()
    {
        var flights = (await _getAllFlights.ExecuteAsync())
            .Where(f => f.AvailableSeats.Value > 0)
            .OrderBy(f => f.DepartureDateTime.Value)
            .ToList();

        if (flights.Count == 0)
            throw new Exception("No hay vuelos disponibles con asientos.");

        var flightMap = await GetFlightDisplayMapAsync();
        var selected = new List<int>();

        while (true)
        {
            PrintContext();
            AdminFlowConsole.PrintMenuBox(
                "VUELOS DISPONIBLES",
                flights.Select(f => $"[{f.Id.Value}] {GetDisplay(flightMap, f.Id.Value)}").ToList());

            if (selected.Count > 0)
                AdminFlowConsole.PrintMenuBox("VUELOS SELECCIONADOS", selected.Select(x => $"[{x}]").ToList());

            var raw = AdminFlowConsole.ReadRaw("Ingrese ID de vuelo (000000 cancela)");
            if (raw == AdminFlowConsole.CancelToken)
                return null;

            if (!int.TryParse(raw, out var flightId))
            {
                AdminFlowConsole.PrintError("El valor ingresado no es valido. Intente nuevamente.");
                continue;
            }

            var exists = flights.Any(x => x.Id.Value == flightId);
            if (!exists)
            {
                AdminFlowConsole.PrintError("El vuelo no existe.");
                continue;
            }

            if (selected.Contains(flightId))
            {
                AdminFlowConsole.PrintError("Ese vuelo ya fue agregado.");
                continue;
            }

            selected.Add(flightId);
            var addMore = AdminFlowConsole.ReadYesNo("Desea agregar otro vuelo? (s/n)");
            if (addMore is null)
                return null;
            if (!addMore.Value)
                break;
        }

        return selected;
    }

    private async Task<List<PassengerDraft>?> CollectPassengersAsync()
    {
        var list = new List<PassengerDraft>();
        var addPassenger = AdminFlowConsole.ReadYesNo("Desea agregar un nuevo pasajero? (s/n)");
        if (addPassenger is null)
            return null;

        while (addPassenger.Value)
        {
            var draft = await ReadPassengerDraftAsync();
            if (draft is null)
                return null;

            list.Add(draft);

            addPassenger = AdminFlowConsole.ReadYesNo("Desea agregar otro pasajero? (s/n)");
            if (addPassenger is null)
                return null;
        }

        return list;
    }

    private async Task<PassengerDraft?> ReadPassengerDraftAsync()
    {
        var documentTypes = (await _getAllDocumentTypes.ExecuteAsync())
            .OrderBy(x => x.Id.Value)
            .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.Code.Value})"))
            .ToList();

        var documentType = AdminFlowConsole.SelectById(
            "TIPOS DE DOCUMENTO",
            "Seleccione tipo_documento_id",
            documentTypes);
        if (documentType is null)
            return null;

        var documentNumber = AdminFlowConsole.ReadRequiredText("Numero de documento");
        if (documentNumber is null)
            return null;

        var firstNames = AdminFlowConsole.ReadRequiredText("Nombres");
        if (firstNames is null)
            return null;

        var lastNames = AdminFlowConsole.ReadRequiredText("Apellidos");
        if (lastNames is null)
            return null;

        return new PassengerDraft(
            documentType.Value.id,
            documentType.Value.name,
            documentNumber,
            firstNames,
            lastNames);
    }

    private async Task<List<string>> BuildSummaryAsync(WizardDraft draft)
    {
        var flightMap = await GetFlightDisplayMapAsync();
        var lines = new List<string>
        {
            $"Cliente principal: {_username} (customer_id={_customerId})",
            $"Cantidad de vuelos: {draft.FlightIds.Count}"
        };

        lines.AddRange(draft.FlightIds.Select((id, i) => $"Vuelo {i + 1}: {GetDisplay(flightMap, id)}"));

        if (draft.Passengers.Count == 0)
        {
            lines.Add("Pasajeros agregados: 0");
        }
        else
        {
            lines.Add($"Pasajeros agregados: {draft.Passengers.Count}");
            lines.AddRange(draft.Passengers.Select((p, i) =>
                $"Pax {i + 1}: {p.FirstNames} {p.LastNames} - {p.DocumentTypeName} {p.DocumentNumber}"));
        }

        return lines;
    }

    private async Task<PersistResult> PersistAsync(WizardDraft draft)
    {
        var statusId = await ResolvePendingStatusIdAsync();
        var expiresAt = DateTime.Now.AddMinutes(15);

        var reservation = await _createReservation.ExecuteAsync(_customerId, statusId, expiresAt);

        var reservationFlights = new List<int>();
        foreach (var flightId in draft.FlightIds)
        {
            // En autoservicio cliente no pedimos valor parcial, se registra en 0 y puede ajustarse luego por proceso comercial.
            var reservationFlight = await _createReservationFlight.ExecuteAsync(reservation.Id.Value, flightId, 0m);
            reservationFlights.Add(reservationFlight.Id.Value);
        }

        var passengerIds = await ResolvePassengerIdsAsync(draft.Passengers);
        foreach (var reservationFlightId in reservationFlights)
        {
            foreach (var passengerId in passengerIds)
                await _createReservationPassenger.ExecuteAsync(reservationFlightId, passengerId);
        }

        return new PersistResult(
            reservation.Id.Value,
            reservation.Code?.Value ?? "NULL",
            reservationFlights.Count,
            passengerIds.Count);
    }

    private async Task<List<int>> ResolvePassengerIdsAsync(IReadOnlyList<PassengerDraft> drafts)
    {
        var result = new List<int>();
        if (drafts.Count == 0)
            return result;

        var defaultPassengerTypeId = await ResolveDefaultPassengerTypeIdAsync();

        foreach (var draft in drafts)
        {
            var person = await _getPersonByDocument.ExecuteAsync(draft.DocumentTypeId, draft.DocumentNumber);
            if (person is null)
            {
                await _createPerson.ExecuteAsync(
                    draft.DocumentTypeId,
                    draft.DocumentNumber,
                    draft.FirstNames,
                    draft.LastNames,
                    null,
                    null,
                    null);

                person = await _getPersonByDocument.ExecuteAsync(draft.DocumentTypeId, draft.DocumentNumber);
                if (person is null)
                    throw new Exception("No se pudo recuperar la persona creada para un pasajero.");
            }

            var passenger = await _getPassengerByPersonId.ExecuteAsync(person.Id.Value);
            if (passenger is null)
            {
                await _createPassenger.ExecuteAsync(person.Id.Value, defaultPassengerTypeId);
                passenger = await _getPassengerByPersonId.ExecuteAsync(person.Id.Value);
                if (passenger is null)
                    throw new Exception("No se pudo recuperar el pasajero creado.");
            }

            if (!result.Contains(passenger.Id.Value))
                result.Add(passenger.Id.Value);
        }

        return result;
    }

    private async Task<int> ResolvePendingStatusIdAsync()
    {
        var statuses = (await _getAllReservationStatuses.ExecuteAsync()).ToList();
        var pending = statuses.FirstOrDefault(s => s.Name.Value.Trim().ToUpperInvariant().Contains("PEND"));
        if (pending is null)
            throw new Exception("No se encontro estado Pendiente en reservationstatuses.");

        return pending.Id.Value;
    }

    private async Task<int> ResolveDefaultPassengerTypeIdAsync()
    {
        var types = (await _getAllPassengerTypes.ExecuteAsync()).ToList();
        if (types.Count == 0)
            throw new Exception("No hay passengertypes registrados.");

        var adult = types.FirstOrDefault(x =>
            x.Name.Value.Trim().ToUpperInvariant().Contains("ADUL"));

        return adult?.Id.Value ?? types.OrderBy(x => x.Id.Value).First().Id.Value;
    }

    private async Task<Dictionary<int, string>> GetFlightDisplayMapAsync()
    {
        var flights = await _getAllFlights.ExecuteAsync();
        var airlines = await _getAllAirlines.ExecuteAsync();
        var routes = await _getAllRoutes.ExecuteAsync();
        var airports = await _getAllAirports.ExecuteAsync();

        var airlineMap = airlines.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
        var airportMap = airports.ToDictionary(a => a.Id.Value, a => $"{a.Name.Value} ({a.IataCode.Value})");
        var routeMap = routes.ToDictionary(
            r => r.Id.Value,
            r =>
            {
                var origin = GetDisplay(airportMap, r.OriginAirportId.Value);
                var destination = GetDisplay(airportMap, r.DestinationAirportId.Value);
                return $"{origin} -> {destination}";
            });

        return flights.ToDictionary(
            f => f.Id.Value,
            f =>
            {
                var airline = GetDisplay(airlineMap, f.AirlineId.Value);
                var route = GetDisplay(routeMap, f.RouteId.Value);
                return $"{f.Code.Value} - {airline} - {route} - dep={f.DepartureDateTime.Value:yyyy-MM-dd HH:mm}";
            });
    }

    private void PrintContext()
    {
        Console.Clear();
        AdminFlowConsole.PrintHeader("CREAR RESERVA (WIZARD)");
        Console.WriteLine($"Cliente: {_username} (customer_id={_customerId})");
        Console.WriteLine($"Cancela en cualquier campo con: {AdminFlowConsole.CancelToken}");
    }

    private static void Pause()
    {
        Console.WriteLine("\nPresiona una tecla para continuar...");
        Console.ReadKey();
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }

    private sealed record PassengerDraft(
        int DocumentTypeId,
        string DocumentTypeName,
        string DocumentNumber,
        string FirstNames,
        string LastNames);

    private sealed record WizardDraft(
        IReadOnlyList<int> FlightIds,
        IReadOnlyList<PassengerDraft> Passengers);

    private sealed record PersistResult(
        int ReservationId,
        string ReservationCode,
        int FlightsCount,
        int PassengersCount);
}


```

### src/Modules/Reservations/UI/CustomerRescheduleReservationFlow.cs

`csharp
// [DocHeader]
// Modulo: Reservations
// Capa: UI
// Archivo: src\Modules\Reservations\UI\CustomerRescheduleReservationFlow.cs
// Responsabilidad: Flujo de consola para reprogramar reservas propias de cliente y gestionar lista de espera.
using GestionAerolineas.src.Modules.FlightStates.Application.UseCases;
using GestionAerolineas.src.Modules.Flights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Reservations.UI;

public sealed class CustomerRescheduleReservationFlow
{
    private readonly int _customerId;
    private readonly string _username;
    private readonly GetReservationsByCustomerIdUseCase _getReservationsByCustomerId;
    private readonly GetReservationDetailsByIdUseCase _getReservationDetailsById;
    private readonly GetAllReservationStatusesUseCase _getAllReservationStatuses;
    private readonly GetAllFlightsUseCase _getAllFlights;
    private readonly GetFlightByIdUseCase _getFlightById;
    private readonly GetAllFlightStatesUseCase _getAllFlightStates;
    private readonly RescheduleReservationUseCase _rescheduleReservation;
    private readonly AddReservationToWaitlistUseCase _addReservationToWaitlist;

    public CustomerRescheduleReservationFlow(
        int customerId,
        string username,
        GetReservationsByCustomerIdUseCase getReservationsByCustomerId,
        GetReservationDetailsByIdUseCase getReservationDetailsById,
        GetAllReservationStatusesUseCase getAllReservationStatuses,
        GetAllFlightsUseCase getAllFlights,
        GetFlightByIdUseCase getFlightById,
        GetAllFlightStatesUseCase getAllFlightStates,
        RescheduleReservationUseCase rescheduleReservation,
        AddReservationToWaitlistUseCase addReservationToWaitlist)
    {
        _customerId = customerId;
        _username = username;
        _getReservationsByCustomerId = getReservationsByCustomerId;
        _getReservationDetailsById = getReservationDetailsById;
        _getAllReservationStatuses = getAllReservationStatuses;
        _getAllFlights = getAllFlights;
        _getFlightById = getFlightById;
        _getAllFlightStates = getAllFlightStates;
        _rescheduleReservation = rescheduleReservation;
        _addReservationToWaitlist = addReservationToWaitlist;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            PrintHeader();

            var confirmed = await GetConfirmedReservationsAsync();
            if (confirmed.Count == 0)
            {
                Console.WriteLine("No tienes reservas confirmadas para reprogramar.");
                Pause();
                return;
            }

            AdminFlowConsole.PrintMenuBox(
                "RESERVAS CONFIRMADAS",
                confirmed.Select(x => $"[{x.Id}] Reserva {x.Code} - Estado={x.StatusName}").ToList());

            var reservationId = ReadIntRequired("Ingrese reservation_id");
            if (!reservationId.HasValue)
                return;

            var selected = confirmed.FirstOrDefault(x => x.Id == reservationId.Value);
            if (selected is null)
            {
                AdminFlowConsole.PrintError("La reserva no existe o no pertenece al cliente activo.");
                continue;
            }

            var details = await _getReservationDetailsById.ExecuteAsync(selected.Id);
            if (details is null || details.ReservationFlights.Count == 0)
            {
                AdminFlowConsole.PrintError("La reserva no tiene vuelos para reprogramar.");
                continue;
            }

            var reservationFlightId = await SelectReservationFlightIdAsync(details);
            if (!reservationFlightId.HasValue)
                return;

            var currentRf = details.ReservationFlights.First(x => x.Id.Value == reservationFlightId.Value);
            var currentFlight = await _getFlightById.ExecuteAsync(currentRf.FlightId.Value);
            if (currentFlight is null)
            {
                AdminFlowConsole.PrintError("No se encontro el vuelo actual de la reserva.");
                continue;
            }

            var compatibles = await GetCompatibleFlightsAsync(currentFlight.Id.Value, currentFlight.RouteId.Value);
            if (compatibles.Count == 0)
            {
                Console.WriteLine("No hay vuelos disponibles");
                Pause();
                return;
            }

            AdminFlowConsole.PrintMenuBox(
                "VUELOS COMPATIBLES",
                compatibles.Select(f => $"[{f.Id}] {f.Display} | cupo={f.AvailableSeats}").ToList());

            var newFlightId = ReadIntRequired("Ingrese nuevo vuelo_id");
            if (!newFlightId.HasValue)
                return;

            var newFlight = compatibles.FirstOrDefault(x => x.Id == newFlightId.Value);
            if (newFlight is null)
            {
                AdminFlowConsole.PrintError("El vuelo no existe en la lista compatible.");
                continue;
            }

            if (newFlight.Id == currentFlight.Id.Value)
            {
                AdminFlowConsole.PrintError("El nuevo vuelo debe ser diferente al vuelo actual.");
                continue;
            }

            var summaryLines = new List<string>
            {
                $"Cliente: {_username} (customer_id={_customerId})",
                $"Reserva: {selected.Id} - {selected.Code}",
                $"Vuelo actual: {currentFlight.Code.Value} [{currentFlight.Id.Value}]",
                $"Nuevo vuelo: {newFlight.Display}",
                $"Motivo: Reprogramacion solicitada por cliente"
            };
            var decision = AdminFlowConsole.ReadConfirmChoice(summaryLines);
            if (decision == 2)
                continue;
            if (decision == 3)
                return;

            try
            {
                if (newFlight.AvailableSeats > 0)
                {
                    await _rescheduleReservation.ExecuteAsync(
                        _customerId,
                        selected.Id,
                        currentRf.Id.Value,
                        newFlight.Id,
                        "Reprogramacion manual por cliente");
                    Console.WriteLine("Reserva reprogramada correctamente.");
                }
                else
                {
                    var joinWaitlist = AdminFlowConsole.ReadYesNo("El vuelo no tiene cupo. Desea entrar a lista de espera? (s/n)");
                    if (joinWaitlist is null)
                        return;

                    if (joinWaitlist.Value)
                    {
                        await _addReservationToWaitlist.AddAsync(
                            selected.Id,
                            currentRf.Id.Value,
                            newFlight.Id,
                            "Sin cupo al reprogramar");
                        Console.WriteLine("La reserva fue agregada a la lista de espera.");
                    }
                    else
                    {
                        Console.WriteLine("No se realizaron cambios.");
                    }
                }
            }
            catch (Exception ex)
            {
                AdminFlowConsole.PrintError(ex.GetBaseException().Message);
            }

            Pause();
            return;
        }
    }

    private async Task<int?> SelectReservationFlightIdAsync(
        GestionAerolineas.src.Modules.Reservations.Domain.Aggregate.ReservationDetails details)
    {
        if (details.ReservationFlights.Count == 1)
            return details.ReservationFlights[0].Id.Value;

        var lines = details.ReservationFlights
            .Select(x => $"[{x.Id.Value}] reservation_flight_id - flight_id={x.FlightId.Value}")
            .ToList();
        AdminFlowConsole.PrintMenuBox("VUELOS ACTUALES DE LA RESERVA", lines);

        return ReadIntRequired("Ingrese reservation_flight_id a reprogramar");
    }

    private async Task<List<ReservationView>> GetConfirmedReservationsAsync()
    {
        var statusMap = (await _getAllReservationStatuses.ExecuteAsync())
            .ToDictionary(x => x.Id.Value, x => x.Name.Value);

        var all = (await _getReservationsByCustomerId.ExecuteAsync(_customerId)).ToList();
        var list = new List<ReservationView>();
        foreach (var item in all)
        {
            var name = statusMap.TryGetValue(item.StatusId.Value, out var n) ? n : $"#{item.StatusId.Value}";
            if (!name.Trim().ToUpperInvariant().Contains("CONFIRM"))
                continue;

            list.Add(new ReservationView(
                item.Id.Value,
                item.Code?.Value ?? "NULL",
                name));
        }

        return list;
    }

    private async Task<List<FlightView>> GetCompatibleFlightsAsync(int currentFlightId, int routeId)
    {
        var flights = (await _getAllFlights.ExecuteAsync())
            .Where(x => x.Id.Value != currentFlightId
                        && x.RouteId.Value == routeId
                        && x.DepartureDateTime.Value > DateTime.Now)
            .OrderBy(x => x.DepartureDateTime.Value)
            .ToList();

        var states = (await _getAllFlightStates.ExecuteAsync())
            .ToDictionary(x => x.Id.Value, x => x.Name.Value.Trim().ToUpperInvariant());

        return flights
            .Where(f =>
            {
                var state = states.TryGetValue(f.StateId.Value, out var name) ? name : string.Empty;
                return state != "CANCELADO" && state != "COMPLETADO";
            })
            .Select(f => new FlightView(
                f.Id.Value,
                $"{f.Code.Value} | route_id={f.RouteId.Value} | {f.DepartureDateTime.Value:yyyy-MM-dd HH:mm}",
                f.AvailableSeats.Value))
            .ToList();
    }

    private int? ReadIntRequired(string label)
    {
        while (true)
        {
            var raw = AdminFlowConsole.ReadRaw(label);
            if (raw == AdminFlowConsole.CancelToken)
                return null;

            if (int.TryParse(raw, out var value))
                return value;

            AdminFlowConsole.PrintError("El valor ingresado no es valido. Intente nuevamente.");
        }
    }

    private void PrintHeader()
    {
        Console.Clear();
        AdminFlowConsole.PrintHeader("REPROGRAMAR RESERVA");
        Console.WriteLine($"Cliente: {_username} (customer_id={_customerId})");
        Console.WriteLine($"Cancela en cualquier campo con: {AdminFlowConsole.CancelToken}");
    }

    private static void Pause()
    {
        Console.WriteLine("\nPresiona una tecla para continuar...");
        Console.ReadKey();
    }

    private sealed record ReservationView(int Id, string Code, string StatusName);
    private sealed record FlightView(int Id, string Display, int AvailableSeats);
}



```

## 5. Archivos existentes modificados (resumen)

- `Program.cs`
  - Registro e inyeccion de repositorios y casos de uso nuevos.
  - Integracion de flujo directo de reprogramacion en menu cliente.

- `src/shared/Ui/RoleMenus/CustomerSelfServiceMenu.cs`
  - Nueva opcion de menu para reprogramar reserva directamente.
  - Cancelacion de reserva cliente con liberacion de cupo y promocion de espera.

- `src/shared/Context/AppDbContext.cs`
  - Registro de `DbSet` para waitlist e historial.

## 6. Validacion tecnica ejecutada

Se valido compilacion:

```powershell
dotnet msbuild .\GestionAerolineas.csproj /t:Compile /p:Configuration=Debug
```

Resultado: compilacion correcta.

## 7. Migracion necesaria (pendiente)

Como se agregaron tablas nuevas, debes crear y aplicar migracion:

```powershell
dotnet ef migrations add add_reschedule_waitlist_history
dotnet ef database update
```

## 8. Guia corta para defender la implementacion

Si te preguntan que se hizo y por que:

1. Se agrego reprogramacion para cliente autenticado sin pedir `customer_id`.
2. Se validan propiedad de reserva, estado confirmado y compatibilidad de vuelo.
3. Si no hay cupo, se deriva a lista de espera.
4. Al liberar cupo, se promueve automaticamente el primero en espera.
5. Todo deja trazabilidad en historial.
6. Se mantuvo arquitectura actual y se evitaron refactors grandes.
