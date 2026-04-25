// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Domain\Aggregate\CabinConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Domain.Aggregate;

public class CabinConfigurationAggregate
{
    public CabinConfigurationId Id { get; private set; }
    public CabinConfigurationAircraftId AircraftId { get; private set; }
    public CabinConfigurationCabinTypeId CabinTypeId { get; private set; }
    public CabinConfigurationStartRow StartRow { get; private set; }
    public CabinConfigurationEndRow EndRow { get; private set; }
    public CabinConfigurationSeatsPerRow SeatsPerRow { get; private set; }
    public CabinConfigurationSeatLetters SeatLetters { get; private set; }

    private CabinConfigurationAggregate(
        CabinConfigurationId id,
        CabinConfigurationAircraftId aircraftId,
        CabinConfigurationCabinTypeId cabinTypeId,
        CabinConfigurationStartRow startRow,
        CabinConfigurationEndRow endRow,
        CabinConfigurationSeatsPerRow seatsPerRow,
        CabinConfigurationSeatLetters seatLetters)
    {
        Id = id;
        AircraftId = aircraftId;
        CabinTypeId = cabinTypeId;
        StartRow = startRow;
        EndRow = endRow;
        SeatsPerRow = seatsPerRow;
        SeatLetters = seatLetters;
    }

    public static CabinConfigurationAggregate Create(
        CabinConfigurationId id,
        CabinConfigurationAircraftId aircraftId,
        CabinConfigurationCabinTypeId cabinTypeId,
        CabinConfigurationStartRow startRow,
        CabinConfigurationEndRow endRow,
        CabinConfigurationSeatsPerRow seatsPerRow,
        CabinConfigurationSeatLetters seatLetters)
    {
        return new CabinConfigurationAggregate(id, aircraftId, cabinTypeId, startRow, endRow, seatsPerRow, seatLetters);
    }

    public static CabinConfigurationAggregate CreateNew(
        CabinConfigurationAircraftId aircraftId,
        CabinConfigurationCabinTypeId cabinTypeId,
        CabinConfigurationStartRow startRow,
        CabinConfigurationEndRow endRow,
        CabinConfigurationSeatsPerRow seatsPerRow,
        CabinConfigurationSeatLetters seatLetters)
    {
        return new CabinConfigurationAggregate(
            CabinConfigurationId.CreateEmpty(),
            aircraftId,
            cabinTypeId,
            startRow,
            endRow,
            seatsPerRow,
            seatLetters
        );
    }
}
