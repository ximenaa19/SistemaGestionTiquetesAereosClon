// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardTypes\Infrastructure\Repository\CardTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.CardTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.CardTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.CardTypes.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.Modules.CardTypes.Infrastructure.Repository;

public class CardTypeRepository : ICardTypeRepository
{
    private readonly AppDbContext _context;

    public CardTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CardType>> GetAllAsync()
    {
        var entities = await _context.CardTypes
            .AsNoTracking()
            .ToListAsync();

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<CardType?> GetByIdAsync(CardTypeId id)
    {
        var entity = await _context.CardTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<CardType?> GetByNameAsync(CardTypeName name)
    {
        var entity = await _context.CardTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Name == name.Value);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task AddAsync(CardType cardType)
    {
        await _context.CardTypes.AddAsync(MapToEntity(cardType));
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CardType cardType)
    {
        var existing = await _context.CardTypes
            .FirstOrDefaultAsync(e => e.Id == cardType.Id.Value);

        if (existing is null)
            return;

        existing.Name = cardType.Name.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CardType cardType)
    {
        var entity = await _context.CardTypes.FindAsync(cardType.Id.Value);

        if (entity is null)
            return;

        _context.CardTypes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(CardTypeId id)
    {
        return _context.CardTypes.AnyAsync(e => e.Id == id.Value);
    }

    private static CardType MapToDomain(CardTypeEntity entity)
    {
        return CardType.Create(
            CardTypeId.Create(entity.Id),
            CardTypeName.Create(entity.Name ?? string.Empty)
        );
    }

    private static CardTypeEntity MapToEntity(CardType cardType)
    {
        return new CardTypeEntity
        {
            Id = cardType.Id.Value,
            Name = cardType.Name.Value
        };
    }
}
