// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardIssuers\Application\Services\CardIssuerValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Application.Interfaces;
using GestionAerolineas.src.Modules.CardIssuers.Domain.Repositories;
using GestionAerolineas.src.Modules.CardIssuers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CardIssuers.Application.Services;

public class CardIssuerValidator : ICardIssuerValidator
{
    private readonly ICardIssuerRepository _repository;

    public CardIssuerValidator(ICardIssuerRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(CardIssuerName name)
    {
        var existing = await _repository.GetByNameAsync(name);

        if (existing != null)
            throw new Exception("Ya existe un CardIssuer con ese nombre");
    }
}
