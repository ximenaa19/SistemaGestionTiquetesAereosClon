// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\Infrastructure\Repository\PersonPhoneRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonPhones.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonPhones.Domain.Repositories;
using GestionAerolineas.src.Modules.PersonPhones.Domain.ValueObject;
using GestionAerolineas.src.Modules.PersonPhones.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.PersonPhones.Infrastructure.Repository;

public class PersonPhoneRepository : IPersonPhoneRepository
{
    private readonly AppDbContext _context;

    public PersonPhoneRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PersonPhone>> GetAllAsync()
    {
        var entities = await _context.PersonPhones.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PersonPhone?> GetByIdAsync(PersonPhoneId id)
    {
        var entity = await _context.PersonPhones
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PersonPhone?> GetByPersonAndCodeAndNumberAsync(PersonPhonePersonId personId, PersonPhoneCodeId phoneCodeId, PersonPhoneNumber phoneNumber)
    {
        var entity = await _context.PersonPhones
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.PersonId == personId.Value
                && e.PhoneCodeId == phoneCodeId.Value
                && e.PhoneNumber == phoneNumber.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(PersonPhone phone)
    {
        await _context.PersonPhones.AddAsync(MapToEntity(phone));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PersonPhone phone)
    {
        var existing = await _context.PersonPhones
            .FirstOrDefaultAsync(e => e.Id == phone.Id.Value);

        if (existing is null)
            return;

        existing.PersonId = phone.PersonId.Value;
        existing.PhoneCodeId = phone.PhoneCodeId.Value;
        existing.PhoneNumber = phone.PhoneNumber.Value;
        existing.IsPrimary = phone.IsPrimary.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PersonPhone phone)
    {
        var entity = await _context.PersonPhones.FindAsync(phone.Id.Value);

        if (entity is null)
            return;

        _context.PersonPhones.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(PersonPhoneId id)
    {
        return _context.PersonPhones.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedPhoneForPersonAsync(int personId, int phoneCodeId, string normalizedPhoneNumber, int? excludingId = null)
    {
        var query = _context.PersonPhones
            .AsNoTracking()
            .Where(p => p.PersonId == personId && p.PhoneCodeId == phoneCodeId && p.PhoneNumber != null);

        if (excludingId.HasValue)
            query = query.Where(p => p.Id != excludingId.Value);

        return query.AnyAsync(p => p.PhoneNumber!.Trim().ToUpper() == normalizedPhoneNumber);
    }

    public Task<bool> ExistsPrimaryForPersonAsync(int personId, int? excludingId = null)
    {
        var query = _context.PersonPhones
            .AsNoTracking()
            .Where(p => p.PersonId == personId && p.IsPrimary);

        if (excludingId.HasValue)
            query = query.Where(p => p.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static PersonPhone MapToDomain(PersonPhoneEntity entity)
    {
        try
        {
            return PersonPhone.Create(
                PersonPhoneId.Create(entity.Id),
                PersonPhonePersonId.Create(entity.PersonId),
                PersonPhoneCodeId.Create(entity.PhoneCodeId),
                PersonPhoneNumber.Create(entity.PhoneNumber ?? string.Empty),
                PersonPhoneIsPrimary.Create(entity.IsPrimary)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro personphones(id={entity.Id}) tiene datos invalidos. " +
                $"persona_id={entity.PersonId}, codigo_telefono_id={entity.PhoneCodeId}, numero_telefono='{entity.PhoneNumber}', es_principal={entity.IsPrimary}.",
                ex);
        }
    }

    private static PersonPhoneEntity MapToEntity(PersonPhone phone)
    {
        return new PersonPhoneEntity
        {
            Id = phone.Id.Value,
            PersonId = phone.PersonId.Value,
            PhoneCodeId = phone.PhoneCodeId.Value,
            PhoneNumber = phone.PhoneNumber.Value,
            IsPrimary = phone.IsPrimary.Value
        };
    }
}

