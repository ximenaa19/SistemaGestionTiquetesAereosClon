// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Domain\Repositories\IAddressRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Domain.Aggregate;
using GestionAerolineas.src.Modules.Addresses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Addresses.Domain.Repositories;

public interface IAddressRepository
{
    Task<IEnumerable<Address>> GetAllAsync();
    Task<Address?> GetByIdAsync(AddressId id);
    Task AddAsync(Address address);
    Task UpdateAsync(Address address);
    Task DeleteAsync(Address address);
    Task<bool> ExistsAsync(AddressId id);
}

