// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Continents\Infrastructure\Entity\ContinentEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;

namespace GestionAerolineas.src.Modules.Continents.Infrastructure.Entity;

public class ContinentEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
}


