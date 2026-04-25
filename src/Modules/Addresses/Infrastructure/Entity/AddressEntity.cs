// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Infrastructure\Entity\AddressEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Addresses.Infrastructure.Entity;

public class AddressEntity
{
    public int Id { get; set; }
    public int RoadTypeId { get; set; }
    public string? RoadName { get; set; }
    public string? Number { get; set; }
    public string? Complement { get; set; }
    public int CityId { get; set; }
    public string? PostalCode { get; set; }
}

