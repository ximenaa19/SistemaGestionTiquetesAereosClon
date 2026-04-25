// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\Domain\Aggregate\Customer.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Customers.Domain.Aggregate;

public class Customer
{
    public CustomerId Id { get; private set; }
    public CustomerPersonId PersonId { get; private set; }
    public CustomerCreatedAt CreatedAt { get; private set; }

    private Customer(CustomerId id, CustomerPersonId personId, CustomerCreatedAt createdAt)
    {
        Id = id;
        PersonId = personId;
        CreatedAt = createdAt;
    }

    public static Customer Create(CustomerId id, CustomerPersonId personId, CustomerCreatedAt createdAt)
    {
        return new Customer(id, personId, createdAt);
    }

    public static Customer CreateNew(CustomerPersonId personId)
    {
        return new Customer(CustomerId.CreateEmpty(), personId, CustomerCreatedAt.CreateNow());
    }
}
