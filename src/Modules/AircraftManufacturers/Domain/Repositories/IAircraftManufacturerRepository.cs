// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AircraftManufacturers\Domain\Repositories\IAircraftManufacturerRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Aggregate;
using GestionAerolineas.src.Modules.AircraftManufacturers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AircraftManufacturers.Domain.Repositories;

public interface IAircraftManufacturerRepository
{
    Task<IEnumerable<AircraftManufacturer>> GetAllAsync();
    Task<AircraftManufacturer?> GetByIdAsync(AircraftManufacturerId id);
    Task<AircraftManufacturer?> GetByNameAsync(AircraftManufacturerName name);
    Task AddAsync(AircraftManufacturer manufacturer);
    Task UpdateAsync(AircraftManufacturer manufacturer);
    Task DeleteAsync(AircraftManufacturer manufacturer);
    Task<bool> ExistsAsync(AircraftManufacturerId id);
}

