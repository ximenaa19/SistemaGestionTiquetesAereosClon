// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinTypes\Application\UseCases\GetAllCabinTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using GestionAerolineas.src.Modules.CabinTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.CabinTypes.Domain.Repository;

namespace GestionAerolineas.src.Modules.CabinTypes.Application.UseCases;

public class GetAllCabinTypeUseCase
{
    private readonly ICabinTypeRepository _repository;

    public GetAllCabinTypeUseCase(ICabinTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CabinType>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }

}
