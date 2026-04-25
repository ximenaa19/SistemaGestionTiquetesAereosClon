// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftManufacturers\Application\UseCases\GetAircraftManufacturerByNameUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Repositories;
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftManufacturers.Application.UseCases;

public class GetAircraftManufacturerByNameUseCase
{
    private readonly IAircraftManufacturerRepository _repository;

    public GetAircraftManufacturerByNameUseCase(IAircraftManufacturerRepository repository)
    {
        _repository = repository;
    }

    public Task<AircraftManufacturer?> ExecuteAsync(string name)
    {
        var nameVO = AircraftManufacturerName.Create(name);
        return _repository.GetByNameAsync(nameVO);
    }
}

