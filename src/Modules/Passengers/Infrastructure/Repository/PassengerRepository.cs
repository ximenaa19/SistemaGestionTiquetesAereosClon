// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Infrastructure\Repository\PassengerRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Passengers.Domain.Repositories;
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Passengers.Infrastructure.Repository;

public class PassengerRepository : IPassengerRepository
{
    private readonly AppDbContext _context;

    public PassengerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Passenger>> GetAllAsync()
    {
        var entities = await _context.Passengers.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Passenger?> GetByIdAsync(PassengerId id)
    {
        var entity = await _context.Passengers
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Passenger?> GetByPersonIdAsync(PassengerPersonId personId)
    {
        var entity = await _context.Passengers
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.PersonId == personId.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Passenger?> GetByPersonNameAsync(PassengerPersonName personName)
    {
        var normalizedName = PassengerPersonName.Normalize(personName.Value);

        var entity = await (
            from passenger in _context.Passengers.AsNoTracking()
            join person in _context.People.AsNoTracking() on passenger.PersonId equals person.Id
            where ((person.FirstNames ?? string.Empty).Trim() + " " + (person.LastNames ?? string.Empty).Trim()).Trim().ToUpper() == normalizedName
            select passenger
        ).FirstOrDefaultAsync();

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Passenger passenger)
    {
        await _context.Passengers.AddAsync(MapToEntity(passenger));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Passenger passenger)
    {
        var existing = await _context.Passengers
            .FirstOrDefaultAsync(e => e.Id == passenger.Id.Value);

        if (existing is null)
            return;

        existing.PersonId = passenger.PersonId.Value;
        existing.PassengerTypeId = passenger.PassengerTypeId.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Passenger passenger)
    {
        var entity = await _context.Passengers.FindAsync(passenger.Id.Value);

        if (entity is null)
            return;

        _context.Passengers.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(PassengerId id)
    {
        return _context.Passengers.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByPersonIdAsync(PassengerPersonId personId, PassengerId? excludingId = null)
    {
        var query = _context.Passengers
            .AsNoTracking()
            .Where(p => p.PersonId == personId.Value);

        if (excludingId != null)
            query = query.Where(p => p.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static Passenger MapToDomain(PassengerEntity entity)
    {
        try
        {
            return Passenger.Create(
                PassengerId.Create(entity.Id),
                PassengerPersonId.Create(entity.PersonId),
                PassengerTypeId.Create(entity.PassengerTypeId)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro passengers(id={entity.Id}) tiene datos invalidos. " +
                $"persona_id={entity.PersonId}, tipo_pasajero_id={entity.PassengerTypeId}.",
                ex);
        }
    }

    private static PassengerEntity MapToEntity(Passenger passenger)
    {
        return new PassengerEntity
        {
            Id = passenger.Id.Value,
            PersonId = passenger.PersonId.Value,
            PassengerTypeId = passenger.PassengerTypeId.Value
        };
    }
}
