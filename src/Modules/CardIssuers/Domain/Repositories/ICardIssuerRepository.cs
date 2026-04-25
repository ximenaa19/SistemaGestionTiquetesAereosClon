// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardIssuers\Domain\Repositories\ICardIssuerRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Domain.Aggregate;
using GestionAerolineas.src.Modules.CardIssuers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CardIssuers.Domain.Repositories;

public interface ICardIssuerRepository
{
    Task<IEnumerable<CardIssuer>> GetAllAsync();
    Task<CardIssuer?> GetByIdAsync(CardIssuerId id);
    Task<CardIssuer?> GetByNameAsync(CardIssuerName name);
    Task AddAsync(CardIssuer cardIssuer);
    Task UpdateAsync(CardIssuer cardIssuer);
    Task DeleteAsync(CardIssuer cardIssuer);
    Task<bool> ExistsAsync(CardIssuerId id);
}
