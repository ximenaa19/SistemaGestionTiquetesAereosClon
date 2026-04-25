// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatuses\Application\UseCases\UpdateReservationStatusUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatuses.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;

public class UpdateReservationStatusUseCase
{
    private readonly IReservationStatusRepository _repository;
    private readonly IReservationStatusValidator _validator;

    public UpdateReservationStatusUseCase(
        IReservationStatusRepository repository,
        IReservationStatusValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var idVO = ReservationStatusId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("El estado de reserva no existe");

        var nameVO = ReservationStatusName.Create(name);

        await _validator.ValidateNameAsync(nameVO, idVO);

        var updated = ReservationStatus.Create(idVO, nameVO);

        await _repository.UpdateAsync(updated);
    }
}
