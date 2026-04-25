// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Domain\Repositories\IPassengerTypeRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PassengerTypes.Domain.Repositories;

public interface IPassengerTypeRepository
{
    Task<IEnumerable<PassengerType>> GetAllAsync();
    Task<PassengerType?> GetByIdAsync(PassengerTypeId id);
    Task<PassengerType?> GetByNameAsync(PassengerTypeName name);
    Task AddAsync(PassengerType passengerType);
    Task UpdateAsync(PassengerType passengerType);
    Task DeleteAsync(PassengerType passengerType);
    Task<bool> ExistsAsync(PassengerTypeId id);
}

