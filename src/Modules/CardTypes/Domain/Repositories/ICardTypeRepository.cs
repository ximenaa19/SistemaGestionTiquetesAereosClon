// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardTypes\Domain\Repositories\ICardTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.CardTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CardTypes.Domain.Repositories;

public interface ICardTypeRepository
{
    Task<IEnumerable<CardType>> GetAllAsync();
    Task<CardType?> GetByIdAsync(CardTypeId id);
    Task<CardType?> GetByNameAsync(CardTypeName name);
    Task AddAsync(CardType cardType);
    Task UpdateAsync(CardType cardType);
    Task DeleteAsync(CardType cardType);
    Task<bool> ExistsAsync(CardTypeId id);
}
