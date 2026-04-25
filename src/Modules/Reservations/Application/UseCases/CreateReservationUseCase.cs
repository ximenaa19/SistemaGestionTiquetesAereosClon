// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Application\UseCases\CreateReservationUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reservations.Application.Interfaces;
using GestionAerolineas.src.Modules.Reservations.Application.Services;
using GestionAerolineas.src.Modules.Reservations.Domain.Aggregate;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Reservations.Application.UseCases;

public class CreateReservationUseCase
{
    private readonly IReservationRepository _repository;
    private readonly IReservationValidator _validator;

    public CreateReservationUseCase(IReservationRepository repository, IReservationValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Reservation> ExecuteAsync(int customerId, int statusId, DateTime? expiresAt)
    {
        var customerIdVO = ReservationCustomerId.Create(customerId);
        var statusIdVO = ReservationStatusId.Create(statusId);
        var expiresAtVO = ReservationExpiresAt.CreateOptional(expiresAt);

        await _validator.ValidateCustomerExistsAsync(customerIdVO);
        await _validator.ValidateStatusExistsAsync(statusIdVO);

        ReservationCode code;
        while (true)
        {
            code = ReservationCodeGenerator.Generate();
            var exists = await _repository.ExistsByNormalizedCodeAsync(ReservationCode.Normalize(code.Value));
            if (!exists)
                break;
        }

        var entity = Reservation.CreateNew(code, customerIdVO, statusIdVO, expiresAtVO);
        _validator.ValidateExpiresAt(entity.ReservedAt, entity.ExpiresAt);

        await _repository.AddAsync(entity);

        var created = await _repository.GetByCodeAsync(code);
        if (created is null)
            throw new Exception("No se pudo recuperar la reserva creada");

        return created;
    }
}
