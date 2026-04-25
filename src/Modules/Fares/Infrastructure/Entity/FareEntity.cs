// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Infrastructure\Entity\FareEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Fares.Infrastructure.Entity;

public class FareEntity
{
    public int Id { get; set; }
    public int RouteId { get; set; }
    public int CabinTypeId { get; set; }
    public int PassengerTypeId { get; set; }
    public int SeasonId { get; set; }
    public decimal BasePrice { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }
}

