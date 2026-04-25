// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\Infrastructure\Repository\PhoneCodeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.Repositories;
using GestionAerolineas.src.Modules.PhoneCodes.Domain.ValueObject;
using GestionAerolineas.src.Modules.PhoneCodes.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.PhoneCodes.Infrastructure.Repository;

public class PhoneCodeRepository : IPhoneCodeRepository
{
    private readonly AppDbContext _context;

    public PhoneCodeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PhoneCode>> GetAllAsync()
    {
        var entities = await _context.PhoneCodes.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PhoneCode?> GetByIdAsync(PhoneCodeId id)
    {
        var entity = await _context.PhoneCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PhoneCode?> GetByCountryCodeAsync(PhoneCountryCode countryCode)
    {
        var entity = await _context.PhoneCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.CountryCode == countryCode.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PhoneCode?> GetByCountryNameAsync(CountryName countryName)
    {
        var entity = await _context.PhoneCodes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.CountryName == countryName.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(PhoneCode phoneCode)
    {
        await _context.PhoneCodes.AddAsync(MapToEntity(phoneCode));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PhoneCode phoneCode)
    {
        var existing = await _context.PhoneCodes
            .FirstOrDefaultAsync(e => e.Id == phoneCode.Id.Value);

        if (existing is null)
            return;

        existing.CountryCode = phoneCode.CountryCode.Value;
        existing.CountryName = phoneCode.CountryName.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PhoneCode phoneCode)
    {
        var entity = await _context.PhoneCodes.FindAsync(phoneCode.Id.Value);

        if (entity is null)
            return;

        _context.PhoneCodes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(PhoneCodeId id)
    {
        return await _context.PhoneCodes.AnyAsync(e => e.Id == id.Value);
    }

    private static PhoneCode MapToDomain(PhoneCodeEntity entity)
    {
        return PhoneCode.Create(
            PhoneCodeId.Create(entity.Id),
            PhoneCountryCode.Create(entity.CountryCode ?? string.Empty),
            CountryName.Create(entity.CountryName ?? string.Empty)
        );
    }

    private static PhoneCodeEntity MapToEntity(PhoneCode phoneCode)
    {
        return new PhoneCodeEntity
        {
            Id = phoneCode.Id.Value,
            CountryCode = phoneCode.CountryCode.Value,
            CountryName = phoneCode.CountryName.Value
        };
    }
}
