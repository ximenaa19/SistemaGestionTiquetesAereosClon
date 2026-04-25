// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Continents\Application\Interfaces\IContinentValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Continents.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Continents.Application.Interfaces;

public interface IContinentValidator
{
    Task ValidateNameAsync(ContinentName name);
}
