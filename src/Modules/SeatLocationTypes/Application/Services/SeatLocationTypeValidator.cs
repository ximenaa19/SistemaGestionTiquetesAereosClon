// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\Application\Services\SeatLocationTypeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SeatLocationTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SeatLocationTypes.Application.Services;

public class SeatLocationTypeValidator : ISeatLocationTypeValidator
{
    private readonly ISeatLocationTypeRepository _repository;

    public SeatLocationTypeValidator(ISeatLocationTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(SeatLocationTypeName name, SeatLocationTypeId? currentId = null)
    {
        var existing = await _repository.GetByNameAsync(name);

        if (existing != null && (currentId is null || existing.Id.Value != currentId.Value))
            throw new Exception("Ya existe un tipo de ubicación de asiento con ese nombre");
    }
}

