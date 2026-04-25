// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Infrastructure\Repository\AddressRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Domain.Aggregate;
using GestionAerolineas.src.Modules.Addresses.Domain.Repositories;
using GestionAerolineas.src.Modules.Addresses.Domain.ValueObject;
using GestionAerolineas.src.Modules.Addresses.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.Addresses.Infrastructure.Repository;

public class AddressRepository : IAddressRepository
{
    private readonly AppDbContext _context;

    public AddressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Address>> GetAllAsync()
    {
        var entities = await _context.Addresses.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Address?> GetByIdAsync(AddressId id)
    {
        var entity = await _context.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(Address address)
    {
        await _context.Addresses.AddAsync(MapToEntity(address));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Address address)
    {
        var existing = await _context.Addresses
            .FirstOrDefaultAsync(e => e.Id == address.Id.Value);

        if (existing is null)
            return;

        existing.RoadTypeId = address.RoadTypeId.Value;
        existing.RoadName = address.RoadName.Value;
        existing.Number = address.Number.Value;
        existing.Complement = address.Complement.Value;
        existing.CityId = address.CityId.Value;
        existing.PostalCode = address.PostalCode.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Address address)
    {
        var entity = await _context.Addresses.FindAsync(address.Id.Value);

        if (entity is null)
            return;

        _context.Addresses.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(AddressId id)
    {
        return _context.Addresses.AnyAsync(e => e.Id == id.Value);
    }

    private static Address MapToDomain(AddressEntity entity)
    {
        try
        {
            return Address.Create(
                AddressId.Create(entity.Id),
                AddressRoadTypeId.Create(entity.RoadTypeId),
                AddressRoadName.Create(entity.RoadName ?? string.Empty),
                AddressNumber.Create(entity.Number),
                AddressComplement.Create(entity.Complement),
                AddressCityId.Create(entity.CityId),
                AddressPostalCode.Create(entity.PostalCode)
            );
        }
        catch (Exception ex)
        {
            throw new Exception($"El registro addresses(id={entity.Id}) tiene datos inválidos.", ex);
        }
    }

    private static AddressEntity MapToEntity(Address address)
    {
        return new AddressEntity
        {
            Id = address.Id.Value,
            RoadTypeId = address.RoadTypeId.Value,
            RoadName = address.RoadName.Value,
            Number = address.Number.Value,
            Complement = address.Complement.Value,
            CityId = address.CityId.Value,
            PostalCode = address.PostalCode.Value
        };
    }
}

