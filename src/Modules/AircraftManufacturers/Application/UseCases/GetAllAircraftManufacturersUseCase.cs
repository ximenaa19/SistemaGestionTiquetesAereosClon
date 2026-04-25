// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftManufacturers\Application\UseCases\GetAllAircraftManufacturersUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Repositories;

namespace GestionAerolineas.src.Modules.AircraftManufacturers.Application.UseCases;

public class GetAllAircraftManufacturersUseCase
{
    private readonly IAircraftManufacturerRepository _repository;

    public GetAllAircraftManufacturersUseCase(IAircraftManufacturerRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<AircraftManufacturer>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

