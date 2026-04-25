// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Application\Services\CabinConfigurationValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Aircraft.Infrastructure.Repository;
using GestionAerolineas.src.Modules.CabinConfiguration.Application.Interfaces;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.Repositories;
using GestionAerolineas.src.Modules.CabinConfiguration.Domain.ValueObject;
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.CabinConfiguration.Application.Services;

public class CabinConfigurationValidator : ICabinConfigurationValidator
{
    private readonly ICabinConfigurationRepository _repository;
    private readonly AircraftRepository _aircraftRepository;
    private readonly CabinTypeRepository _cabinTypeRepository;

    public CabinConfigurationValidator(
        ICabinConfigurationRepository repository,
        AircraftRepository aircraftRepository,
        CabinTypeRepository cabinTypeRepository)
    {
        _repository = repository;
        _aircraftRepository = aircraftRepository;
        _cabinTypeRepository = cabinTypeRepository;
    }

    public async Task ValidateAircraftExistsAsync(CabinConfigurationAircraftId aircraftId)
    {
        var exists = await _aircraftRepository.ExistsAsync(
            GestionAerolineas.src.Modules.Aircraft.Domain.ValueObject.AircraftId.Create(aircraftId.Value));

        if (!exists)
            throw new Exception("La aeronave no existe");
    }

    public async Task ValidateCabinTypeExistsAsync(CabinConfigurationCabinTypeId cabinTypeId)
    {
        var exists = await _cabinTypeRepository.ExistsAsync(
            GestionAerolineas.src.Modules.CabinTypes.Domain.ValueObject.CabinTypesId.Create(cabinTypeId.Value));

        if (!exists)
            throw new Exception("El tipo de cabina no existe");
    }

    public async Task ValidateUniqueCabinTypeInAircraftAsync(
        CabinConfigurationAircraftId aircraftId,
        CabinConfigurationCabinTypeId cabinTypeId,
        CabinConfigurationId? currentId = null)
    {
        var exists = await _repository.ExistsByAircraftAndCabinTypeAsync(
            aircraftId.Value,
            cabinTypeId.Value,
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe una configuracion para ese tipo de cabina en esta aeronave");
    }

    public Task ValidateRowRangeAsync(CabinConfigurationStartRow startRow, CabinConfigurationEndRow endRow)
    {
        if (endRow.Value < startRow.Value)
            throw new Exception("fila_fin no puede ser menor que fila_inicio");

        return Task.CompletedTask;
    }

    public Task ValidateSeatsAndLettersAsync(CabinConfigurationSeatsPerRow seatsPerRow, CabinConfigurationSeatLetters seatLetters)
    {
        if (seatLetters.Value.Length != seatsPerRow.Value)
            throw new Exception("letras_asientos debe tener la misma cantidad que asientos_por_fila");

        return Task.CompletedTask;
    }

    public async Task ValidateNoRowOverlapAsync(
        CabinConfigurationAircraftId aircraftId,
        CabinConfigurationStartRow startRow,
        CabinConfigurationEndRow endRow,
        CabinConfigurationId? currentId = null)
    {
        var existing = (await _repository.GetByAircraftIdAsync(aircraftId)).ToList();

        foreach (var item in existing)
        {
            if (currentId != null && item.Id.Value == currentId.Value)
                continue;

            if (Overlaps(startRow.Value, endRow.Value, item.StartRow.Value, item.EndRow.Value))
            {
                throw new Exception(
                    $"Rango de filas se solapa con otra configuracion (id={item.Id.Value}) " +
                    $"[{item.StartRow.Value}-{item.EndRow.Value}].");
            }
        }
    }

    private static bool Overlaps(int aStart, int aEnd, int bStart, int bEnd)
    {
        return aStart <= bEnd && bStart <= aEnd;
    }
}
