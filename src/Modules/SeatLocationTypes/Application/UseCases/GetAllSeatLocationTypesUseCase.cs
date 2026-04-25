// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\Application\UseCases\GetAllSeatLocationTypesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Repositories;

namespace GestionAerolineas.src.Modules.SeatLocationTypes.Application.UseCases;

public class GetAllSeatLocationTypesUseCase
{
    private readonly ISeatLocationTypeRepository _repository;

    public GetAllSeatLocationTypesUseCase(ISeatLocationTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SeatLocationType>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}

