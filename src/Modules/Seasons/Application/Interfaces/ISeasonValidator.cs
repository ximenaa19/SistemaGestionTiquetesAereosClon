// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Seasons\Application\Interfaces\ISeasonValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Seasons.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Seasons.Application.Interfaces;

public interface ISeasonValidator
{
    Task ValidateNameAsync(SeasonName name, SeasonId? currentId = null);
}
