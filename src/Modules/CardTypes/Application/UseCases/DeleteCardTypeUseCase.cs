// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardTypes\Application\UseCases\DeleteCardTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.CardTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CardTypes.Application.UseCases;

public class DeleteCardTypeUseCase
{
    private readonly ICardTypeRepository _repository;

    public DeleteCardTypeUseCase(ICardTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var cardTypeId = CardTypeId.Create(id);
        var cardType = await _repository.GetByIdAsync(cardTypeId);

        if (cardType is null)
            throw new KeyNotFoundException($"CardType con id '{cardTypeId.Value}' no existe.");

        await _repository.DeleteAsync(cardType);
    }
}
