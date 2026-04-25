// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\Infrastructure\Entity\SeatLocationTypeEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.SeatLocationTypes.Infrastructure.Entity;

public class SeatLocationTypeEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

