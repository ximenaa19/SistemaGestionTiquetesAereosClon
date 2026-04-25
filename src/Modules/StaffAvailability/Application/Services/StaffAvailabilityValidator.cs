// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Application\Services\StaffAvailabilityValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Domain.ValueObject;
using GestionAerolineas.src.Modules.AvailabilityStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Staff.Domain.ValueObject;
using GestionAerolineas.src.Modules.Staff.Infrastructure.Repository;
using GestionAerolineas.src.Modules.StaffAvailability.Application.Interfaces;
using GestionAerolineas.src.Modules.StaffAvailability.Domain.Repositories;
using GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffAvailability.Application.Services;

public class StaffAvailabilityValidator : IStaffAvailabilityValidator
{
    private readonly IStaffAvailabilityRepository _repository;
    private readonly StaffRepository _staffRepository;
    private readonly AvailabilityStatusRepository _availabilityStatusRepository;

    public StaffAvailabilityValidator(
        IStaffAvailabilityRepository repository,
        StaffRepository staffRepository,
        AvailabilityStatusRepository availabilityStatusRepository)
    {
        _repository = repository;
        _staffRepository = staffRepository;
        _availabilityStatusRepository = availabilityStatusRepository;
    }

    public async Task ValidateStaffExistsAndActiveAsync(StaffAvailabilityStaffId staffId)
    {
        var staff = await _staffRepository.GetByIdAsync(StaffId.Create(staffId.Value));
        if (staff is null)
            throw new Exception("El staff no existe");

        if (!staff.IsActive.Value)
            throw new Exception("No se puede crear disponibilidad para staff inactivo");
    }

    public async Task ValidateStatusExistsAsync(StaffAvailabilityStatusId statusId)
    {
        var exists = await _availabilityStatusRepository.ExistsAsync(AvailabilityStatusId.Create(statusId.Value));
        if (!exists)
            throw new Exception("El estado de disponibilidad no existe");
    }

    public void ValidateDateRange(StaffAvailabilityStartDateTime start, StaffAvailabilityEndDateTime end)
    {
        if (end.Value <= start.Value)
            throw new Exception("La fecha_fin debe ser mayor que la fecha_inicio");
    }

    public async Task ValidateNoOverlapAsync(
        StaffAvailabilityStaffId staffId,
        StaffAvailabilityStartDateTime start,
        StaffAvailabilityEndDateTime end,
        StaffAvailabilityId? currentId = null)
    {
        var existsOverlap = await _repository.ExistsOverlapAsync(staffId.Value, start.Value, end.Value, currentId?.Value);
        if (existsOverlap)
            throw new Exception("El rango de fechas se solapa con otro bloque de disponibilidad del mismo staff");
    }
}

