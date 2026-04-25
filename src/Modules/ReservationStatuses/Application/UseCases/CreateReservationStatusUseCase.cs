// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatuses\Application\UseCases\CreateReservationStatusUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatuses.Application.Interfaces;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Aggregate;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.Repositories;
using GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;

public class CreateReservationStatusUseCase
{
    private readonly IReservationStatusRepository _repository;
    private readonly IReservationStatusValidator _validator;

    public CreateReservationStatusUseCase(
        IReservationStatusRepository repository,
        IReservationStatusValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name)
    {
        var nameVO = ReservationStatusName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var entity = ReservationStatus.CreateNew(nameVO);

        await _repository.AddAsync(entity);
    }
}

