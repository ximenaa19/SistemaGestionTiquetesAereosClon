// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CheckinStatuses\Application\Interfaces\ICheckinStatusValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CheckinStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CheckinStatuses.Application.Interfaces;

public interface ICheckinStatusValidator
{
    Task ValidateNameAsync(CheckinStatusName name, CheckinStatusId? currentId = null);
}
