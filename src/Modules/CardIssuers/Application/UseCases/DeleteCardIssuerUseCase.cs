// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardIssuers\Application\UseCases\DeleteCardIssuerUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Domain.Repositories;
using GestionAerolineas.src.Modules.CardIssuers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CardIssuers.Application.UseCases;

public class DeleteCardIssuerUseCase
{
    private readonly ICardIssuerRepository _repository;

    public DeleteCardIssuerUseCase(ICardIssuerRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var cardIssuerId = CardIssuerId.Create(id);
        var cardIssuer = await _repository.GetByIdAsync(cardIssuerId);

        if (cardIssuer is null)
        {
            throw new KeyNotFoundException($"CardIssuer con id '{cardIssuerId.Value}' no existe.");
        }

        await _repository.DeleteAsync(cardIssuer);
    }
}
