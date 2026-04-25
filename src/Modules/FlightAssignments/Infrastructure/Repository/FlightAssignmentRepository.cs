// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightAssignments\Infrastructure\Repository\FlightAssignmentRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightAssignments.Domain.ValueObject;
using GestionAerolineas.src.Modules.FlightAssignments.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.FlightAssignments.Infrastructure.Repository;

public class FlightAssignmentRepository : IFlightAssignmentRepository
{
    private readonly AppDbContext _context;

    public FlightAssignmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FlightAssignment>> GetAllAsync()
    {
        var entities = await _context.FlightAssignments
            .AsNoTracking()
            .OrderBy(e => e.FlightId)
            .ThenBy(e => e.StaffId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<FlightAssignment?> GetByIdAsync(FlightAssignmentId id)
    {
        var entity = await _context.FlightAssignments
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<FlightAssignment>> GetByFlightIdAsync(FlightAssignmentFlightId flightId)
    {
        var entities = await _context.FlightAssignments
            .AsNoTracking()
            .Where(e => e.FlightId == flightId.Value)
            .OrderBy(e => e.StaffId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<FlightAssignment>> GetByStaffIdAsync(FlightAssignmentStaffId staffId)
    {
        var entities = await _context.FlightAssignments
            .AsNoTracking()
            .Where(e => e.StaffId == staffId.Value)
            .OrderBy(e => e.FlightId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IEnumerable<FlightAssignment>> GetByFlightRoleIdAsync(FlightAssignmentFlightRoleId flightRoleId)
    {
        var entities = await _context.FlightAssignments
            .AsNoTracking()
            .Where(e => e.FlightRoleId == flightRoleId.Value)
            .OrderBy(e => e.FlightId)
            .ThenBy(e => e.StaffId)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<FlightAssignment?> GetByFlightAndStaffAsync(FlightAssignmentFlightId flightId, FlightAssignmentStaffId staffId)
    {
        var entity = await _context.FlightAssignments
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.FlightId == flightId.Value && e.StaffId == staffId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(FlightAssignment assignment)
    {
        await _context.FlightAssignments.AddAsync(MapToEntity(assignment));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(FlightAssignment assignment)
    {
        var existing = await _context.FlightAssignments
            .FirstOrDefaultAsync(e => e.Id == assignment.Id.Value);

        if (existing is null)
            return;

        existing.FlightId = assignment.FlightId.Value;
        existing.StaffId = assignment.StaffId.Value;
        existing.FlightRoleId = assignment.FlightRoleId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(FlightAssignment assignment)
    {
        var entity = await _context.FlightAssignments.FindAsync(assignment.Id.Value);
        if (entity is null)
            return;

        _context.FlightAssignments.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(FlightAssignmentId id)
    {
        return _context.FlightAssignments.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByFlightAndStaffAsync(int flightId, int staffId, int? excludingId = null)
    {
        var query = _context.FlightAssignments
            .AsNoTracking()
            .Where(a => a.FlightId == flightId && a.StaffId == staffId);

        if (excludingId.HasValue)
            query = query.Where(a => a.Id != excludingId.Value);

        return query.AnyAsync();
    }

    public Task<bool> ExistsStaffOverlapAsync(int staffId, DateTime departure, DateTime arrival, int? excludingId = null)
    {
        var query =
            from a in _context.FlightAssignments.AsNoTracking()
            join f in _context.Flights.AsNoTracking() on a.FlightId equals f.Id
            where a.StaffId == staffId
            where f.DepartureDateTime < arrival && f.EstimatedArrivalDateTime > departure
            select a;

        if (excludingId.HasValue)
            query = query.Where(x => x.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static FlightAssignment MapToDomain(FlightAssignmentEntity entity)
    {
        try
        {
            return FlightAssignment.Create(
                FlightAssignmentId.Create(entity.Id),
                FlightAssignmentFlightId.Create(entity.FlightId),
                FlightAssignmentStaffId.Create(entity.StaffId),
                FlightAssignmentFlightRoleId.Create(entity.FlightRoleId)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro flightassignments(id={entity.Id}) tiene datos invalidos. " +
                $"vuelo_id={entity.FlightId}, personal_id={entity.StaffId}, rol_vuelo_id={entity.FlightRoleId}.",
                ex);
        }
    }

    private static FlightAssignmentEntity MapToEntity(FlightAssignment assignment)
    {
        return new FlightAssignmentEntity
        {
            Id = assignment.Id.Value,
            FlightId = assignment.FlightId.Value,
            StaffId = assignment.StaffId.Value,
            FlightRoleId = assignment.FlightRoleId.Value
        };
    }
}

