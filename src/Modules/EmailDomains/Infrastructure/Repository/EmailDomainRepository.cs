// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Infrastructure\Repository\EmailDomainRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Domain.Aggregate;
using GestionAerolineas.src.Modules.EmailDomains.Domain.Repositories;
using GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;
using GestionAerolineas.src.Modules.EmailDomains.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.EmailDomains.Infrastructure.Repository;

public class EmailDomainRepository : IEmailDomainRepository
{
    private readonly AppDbContext _context;

    public EmailDomainRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EmailDomain>> GetAllAsync()
    {
        var entities = await _context.EmailDomains.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<EmailDomain?> GetByIdAsync(EmailDomainId id)
    {
        var entity = await _context.EmailDomains
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<EmailDomain?> GetByDomainAsync(EmailDomainValue domain)
    {
        var entity = await _context.EmailDomains
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Domain == domain.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(EmailDomain emailDomain)
    {
        await _context.EmailDomains.AddAsync(MapToEntity(emailDomain));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmailDomain emailDomain)
    {
        var existing = await _context.EmailDomains
            .FirstOrDefaultAsync(e => e.Id == emailDomain.Id.Value);

        if (existing is null)
            return;

        existing.Domain = emailDomain.Domain.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(EmailDomain emailDomain)
    {
        var entity = await _context.EmailDomains.FindAsync(emailDomain.Id.Value);

        if (entity is null)
            return;

        _context.EmailDomains.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(EmailDomainId id)
    {
        return await _context.EmailDomains.AnyAsync(e => e.Id == id.Value);
    }

    private static EmailDomain MapToDomain(EmailDomainEntity entity)
    {
        return EmailDomain.Create(
            EmailDomainId.Create(entity.Id),
            EmailDomainValue.Create(entity.Domain ?? string.Empty)
        );
    }

    private static EmailDomainEntity MapToEntity(EmailDomain emailDomain)
    {
        return new EmailDomainEntity
        {
            Id = emailDomain.Id.Value,
            Domain = emailDomain.Domain.Value
        };
    }
}

