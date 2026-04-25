// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\StaffAvailability\Application\UseCases\CreateStaffAvailabilityUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.StaffAvailability.Application.Interfaces;
using GestionAerolineas.src.Modules.StaffAvailability.Domain.Aggregate;
using GestionAerolineas.src.Modules.StaffAvailability.Domain.Repositories;
using GestionAerolineas.src.Modules.StaffAvailability.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.StaffAvailability.Application.UseCases;

public class CreateStaffAvailabilityUseCase
{
    private readonly IStaffAvailabilityRepository _repository;
    private readonly IStaffAvailabilityValidator _validator;

    public CreateStaffAvailabilityUseCase(IStaffAvailabilityRepository repository, IStaffAvailabilityValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int staffId, int statusId, DateTime startDateTime, DateTime endDateTime, string? observation)
    {
        var staffIdVO = StaffAvailabilityStaffId.Create(staffId);
        var statusIdVO = StaffAvailabilityStatusId.Create(statusId);
        var startVO = StaffAvailabilityStartDateTime.Create(startDateTime);
        var endVO = StaffAvailabilityEndDateTime.Create(endDateTime);
        var observationVO = StaffAvailabilityObservation.Create(observation);

        await _validator.ValidateStaffExistsAndActiveAsync(staffIdVO);
        await _validator.ValidateStatusExistsAsync(statusIdVO);
        _validator.ValidateDateRange(startVO, endVO);
        await _validator.ValidateNoOverlapAsync(staffIdVO, startVO, endVO);

        var entity = StaffAvailabilityBlock.CreateNew(staffIdVO, statusIdVO, startVO, endVO, observationVO);
        await _repository.AddAsync(entity);
    }
}
