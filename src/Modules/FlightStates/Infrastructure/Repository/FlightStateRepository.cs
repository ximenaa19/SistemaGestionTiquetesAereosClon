// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStates\Infrastructure\Repository\FlightStateRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Domain.Aggregate;
using GestionAerolineas.src.Modules.FlightStates.Domain.Repositories;
using GestionAerolineas.src.Modules.FlightStates.Domain.ValueObject;
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.FlightStates.Infrastructure.Repository;

public class FlightStateRepository : IFlightStateRepository
{
    private readonly AppDbContext _context;

    public FlightStateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FlightState>> GetAllAsync()
    {
        var entities = await _context.FlightStates
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<FlightState?> GetByIdAsync(FlightStateId id)
    {
        var entity = await _context.FlightStates
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<FlightState?> GetByNameAsync(FlightStateName name)
    {
        var entity = await _context.FlightStates
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(FlightState flightState)
    {
        await _context.FlightStates.AddAsync(MapToEntity(flightState));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(FlightState flightState)
    {
        var existing = await _context.FlightStates
            .FirstOrDefaultAsync(e => e.Id == flightState.Id.Value);

        if (existing is null)
            return;

        existing.Name = flightState.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(FlightState flightState)
    {
        var entity = await _context.FlightStates.FindAsync(flightState.Id.Value);

        if (entity is null)
            return;

        _context.FlightStates.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(FlightStateId id)
    {
        return _context.FlightStates.AnyAsync(e => e.Id == id.Value);
    }

    private static FlightState MapToDomain(FlightStateEntity entity)
    {
        return FlightState.Create(
            FlightStateId.Create(entity.Id),
            FlightStateName.Create(entity.Name ?? string.Empty)
        );
    }

    private static FlightStateEntity MapToEntity(FlightState flightState)
    {
        return new FlightStateEntity
        {
            Id = flightState.Id.Value,
            Name = flightState.Name.Value
        };
    }
}
