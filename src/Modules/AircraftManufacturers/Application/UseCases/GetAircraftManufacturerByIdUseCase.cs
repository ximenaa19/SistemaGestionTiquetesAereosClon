// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftManufacturers\Application\UseCases\GetAircraftManufacturerByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Repositories;
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftManufacturers.Application.UseCases;

public class GetAircraftManufacturerByIdUseCase
{
    private readonly IAircraftManufacturerRepository _repository;

    public GetAircraftManufacturerByIdUseCase(IAircraftManufacturerRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftManufacturer?> ExecuteAsync(int id)
    {
        var idVO = AircraftManufacturerId.Create(id);
        return _repository.GetByIdAsync(idVO);
    }
}

