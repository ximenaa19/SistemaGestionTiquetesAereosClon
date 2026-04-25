// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Application\Interfaces\IAircraftValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Aircraft.Application.Interfaces;

public interface IAircraftValidator
{
    Task ValidateModelExistsAsync(AircraftModelId modelId);
    Task ValidateAirlineExistsAsync(AircraftAirlineId airlineId);
    Task ValidateRegistrationAsync(AircraftRegistration registration, AircraftId? currentId = null);
}

