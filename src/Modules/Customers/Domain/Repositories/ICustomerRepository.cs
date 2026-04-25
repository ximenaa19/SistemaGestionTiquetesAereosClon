// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Domain\Repositories\ICustomerRepository.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Domain.Aggregate;
using GestionAerolineas.src.Modules.Customers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Customers.Domain.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(CustomerId id);
    Task<Customer?> GetByPersonIdAsync(CustomerPersonId personId);
    Task<Customer?> GetByPersonNameAsync(CustomerPersonName personName);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
    Task<bool> ExistsAsync(CustomerId id);
    Task<bool> ExistsByPersonIdAsync(CustomerPersonId personId, CustomerId? excludingId = null);
}
