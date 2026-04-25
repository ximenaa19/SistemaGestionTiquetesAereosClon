// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Infrastructure\Repository\PersonEmailRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonEmails.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonEmails.Domain.Repositories;
using GestionAerolineas.src.Modules.PersonEmails.Domain.ValueObject;
using GestionAerolineas.src.Modules.PersonEmails.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.PersonEmails.Infrastructure.Repository;

public class PersonEmailRepository : IPersonEmailRepository
{
    private readonly AppDbContext _context;

    public PersonEmailRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PersonEmail>> GetAllAsync()
    {
        var entities = await _context.PersonEmails.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PersonEmail?> GetByIdAsync(PersonEmailId id)
    {
        var entity = await _context.PersonEmails
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PersonEmail?> GetByPersonAndUserAndDomainAsync(PersonEmailPersonId personId, PersonEmailUser user, PersonEmailDomainId emailDomainId)
    {
        var entity = await _context.PersonEmails
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.PersonId == personId.Value
                && e.EmailDomainId == emailDomainId.Value
                && e.User == user.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(PersonEmail email)
    {
        await _context.PersonEmails.AddAsync(MapToEntity(email));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PersonEmail email)
    {
        var existing = await _context.PersonEmails
            .FirstOrDefaultAsync(e => e.Id == email.Id.Value);

        if (existing is null)
            return;

        existing.PersonId = email.PersonId.Value;
        existing.User = email.User.Value;
        existing.EmailDomainId = email.EmailDomainId.Value;
        existing.IsPrimary = email.IsPrimary.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PersonEmail email)
    {
        var entity = await _context.PersonEmails.FindAsync(email.Id.Value);

        if (entity is null)
            return;

        _context.PersonEmails.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(PersonEmailId id)
    {
        return _context.PersonEmails.AnyAsync(e => e.Id == id.Value);
    }

    public Task<bool> ExistsByNormalizedUserAndDomainForPersonAsync(int personId, string normalizedUser, int emailDomainId, int? excludingId = null)
    {
        var query = _context.PersonEmails
            .AsNoTracking()
            .Where(e => e.PersonId == personId && e.EmailDomainId == emailDomainId && e.User != null);

        if (excludingId.HasValue)
            query = query.Where(e => e.Id != excludingId.Value);

        return query.AnyAsync(e => e.User!.Trim().ToUpper() == normalizedUser);
    }

    public Task<bool> ExistsPrimaryForPersonAsync(int personId, int? excludingId = null)
    {
        var query = _context.PersonEmails
            .AsNoTracking()
            .Where(e => e.PersonId == personId && e.IsPrimary);

        if (excludingId.HasValue)
            query = query.Where(e => e.Id != excludingId.Value);

        return query.AnyAsync();
    }

    private static PersonEmail MapToDomain(PersonEmailEntity entity)
    {
        try
        {
            return PersonEmail.Create(
                PersonEmailId.Create(entity.Id),
                PersonEmailPersonId.Create(entity.PersonId),
                PersonEmailUser.Create(entity.User ?? string.Empty),
                PersonEmailDomainId.Create(entity.EmailDomainId),
                PersonEmailIsPrimary.Create(entity.IsPrimary)
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"El registro personemails(id={entity.Id}) tiene datos invalidos. " +
                $"persona_id={entity.PersonId}, usuario_email='{entity.User}', dominio_email_id={entity.EmailDomainId}, es_principal={entity.IsPrimary}.",
                ex);
        }
    }

    private static PersonEmailEntity MapToEntity(PersonEmail email)
    {
        return new PersonEmailEntity
        {
            Id = email.Id.Value,
            PersonId = email.PersonId.Value,
            User = email.User.Value,
            EmailDomainId = email.EmailDomainId.Value,
            IsPrimary = email.IsPrimary.Value
        };
    }
}

