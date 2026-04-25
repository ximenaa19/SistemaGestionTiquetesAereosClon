// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\Infrastructure\Entity\RoadTypeEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;

namespace GestionAerolineas.src.Modules.RoadTypes.Infrastructure.Entity;

public class RoadTypeEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }

}
