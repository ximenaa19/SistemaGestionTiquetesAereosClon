// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardIssuers\Infrastructure\Repository\CardIssuerRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Domain.Aggregate;
using GestionAerolineas.src.Modules.CardIssuers.Domain.Repositories;
using GestionAerolineas.src.Modules.CardIssuers.Domain.ValueObject;
using GestionAerolineas.src.Modules.CardIssuers.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.CardIssuers.Infrastructure.Repository;

public class CardIssuerRepository : ICardIssuerRepository
{
    private readonly AppDbContext _context;

    public CardIssuerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CardIssuer>> GetAllAsync()
    {
        var entities = await _context.CardIssuers.AsNoTracking().ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<CardIssuer?> GetByIdAsync(CardIssuerId id)
    {
        var entity = await _context.CardIssuers
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<CardIssuer?> GetByNameAsync(CardIssuerName name)
    {
        var entity = await _context.CardIssuers
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(CardIssuer cardIssuer)
    {
        await _context.CardIssuers.AddAsync(MapToEntity(cardIssuer));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CardIssuer cardIssuer)
    {
        var existing = await _context.CardIssuers
            .FirstOrDefaultAsync(e => e.Id == cardIssuer.Id.Value);

        if (existing is null)
            return;

        existing.Name = cardIssuer.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CardIssuer cardIssuer)
    {
        var entity = await _context.CardIssuers.FindAsync(cardIssuer.Id.Value);

        if (entity is null)
            return;

        _context.CardIssuers.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(CardIssuerId id)
    {
        return await _context.CardIssuers.AnyAsync(e => e.Id == id.Value);
    }

    private static CardIssuer MapToDomain(CardIssuerEntity entity)
    {
        return CardIssuer.Create(
            CardIssuerId.Create(entity.Id),
            CardIssuerName.Create(entity.Name ?? string.Empty)
        );
    }

    private static CardIssuerEntity MapToEntity(CardIssuer cardIssuer)
    {
        return new CardIssuerEntity
        {
            Id = cardIssuer.Id.Value,
            Name = cardIssuer.Name.Value
        };
    }
}
