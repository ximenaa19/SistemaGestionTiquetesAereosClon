// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardTypes\Application\Services\CardTypeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.CardTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.CardTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CardTypes.Application.Services;

public class CardTypeValidator : ICardTypeValidator
{
    private readonly ICardTypeRepository _repository;

    public CardTypeValidator(ICardTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(CardTypeName name, CardTypeId? currentId = null)
    {
        var existingByName = await _repository.GetByNameAsync(name);

        if (existingByName is not null && existingByName.Id != currentId)
            throw new Exception("Ya existe un tipo de tarjeta con ese nombre");
    }
}
