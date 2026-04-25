// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\Infrastructure\Entity\PassengerEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Passengers.Infrastructure.Entity;

public class PassengerEntity
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public int PassengerTypeId { get; set; }
}
