// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinTypes\Application\UseCases\DeleteCabinTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using GestionAerolineas.src.Modules.CabinTypes.Domain.Repository;
using GestionAerolineas.src.Modules.CabinTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinTypes.Application.UseCases;

public class DeleteCabinTypeUseCase
{
    private readonly ICabinTypeRepository _repository;

    public DeleteCabinTypeUseCase(ICabinTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var cabinTypeId = CabinTypesId.Create(id);
        var cabinType = await _repository.GetByIdAsync(cabinTypeId);

        if (cabinType is null)
        {
            throw new KeyNotFoundException($"CabinType con id '{cabinTypeId.Value}' no existe.");
        }

        await _repository.DeleteAsync(cabinType);
    }

}
