// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardTypes\Application\UseCases\GetCardTypeByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.CardTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.CardTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CardTypes.Application.UseCases;

public class GetCardTypeByNameUseCase
{
    private readonly ICardTypeRepository _repository;

    public GetCardTypeByNameUseCase(ICardTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<CardType?> ExecuteAsync(string name)
    {
        var nameVO = CardTypeName.Create(name);
        return await _repository.GetByNameAsync(nameVO);
    }
}
