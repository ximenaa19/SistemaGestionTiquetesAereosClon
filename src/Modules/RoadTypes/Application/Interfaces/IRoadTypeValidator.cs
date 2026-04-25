// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\Application\Interfaces\IRoadTypeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.RoadTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RoadTypes.Application.Interfaces;

public interface IRoadTypeValidator
{
    Task ValidateNameAsync(RoadTypeName name);
}