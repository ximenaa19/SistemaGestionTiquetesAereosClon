// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Application\Interfaces\ICabinConfigurationValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Application.Interfaces;

public interface ICabinConfigurationValidator
{
    Task ValidateAircraftExistsAsync(CabinConfigurationAircraftId aircraftId);
    Task ValidateCabinTypeExistsAsync(CabinConfigurationCabinTypeId cabinTypeId);
    Task ValidateUniqueCabinTypeInAircraftAsync(CabinConfigurationAircraftId aircraftId, CabinConfigurationCabinTypeId cabinTypeId, CabinConfigurationId? currentId = null);
    Task ValidateRowRangeAsync(CabinConfigurationStartRow startRow, CabinConfigurationEndRow endRow);
    Task ValidateSeatsAndLettersAsync(CabinConfigurationSeatsPerRow seatsPerRow, CabinConfigurationSeatLetters seatLetters);
    Task ValidateNoRowOverlapAsync(CabinConfigurationAircraftId aircraftId, CabinConfigurationStartRow startRow, CabinConfigurationEndRow endRow, CabinConfigurationId? currentId = null);
}

