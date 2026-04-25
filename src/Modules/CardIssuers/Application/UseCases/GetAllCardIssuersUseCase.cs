// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardIssuers\Application\UseCases\GetAllCardIssuersUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Domain.Aggregate;
using GestionAerolineas.src.Modules.CardIssuers.Domain.Repositories;

namespace GestionAerolineas.src.Modules.CardIssuers.Application.UseCases;

public class GetAllCardIssuersUseCase
{
    private readonly ICardIssuerRepository _repository;

    public GetAllCardIssuersUseCase(ICardIssuerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CardIssuer>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
