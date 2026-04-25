// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Application\Services\ReservationValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Application.Interfaces;
using GestionAerolineas.src.Modules.Reservations.Domain.Repositories;
using GestionAerolineas.src.Modules.Reservations.Domain.ValueObject;
using CustomerId = GestionAerolineas.src.Modules.Customers.Domain.ValueObject.CustomerId;
using ReservationStatusTransitionOriginId = GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject.ReservationStatusOriginId;
using ReservationStatusTransitionDestinationId = GestionAerolineas.src.Modules.ReservationStatusTransitions.Domain.ValueObject.ReservationStatusDestinationId;
using ReservationStatusIdEntity = GestionAerolineas.src.Modules.ReservationStatuses.Domain.ValueObject.ReservationStatusId;

namespace GestionAerolineas.src.Modules.Reservations.Application.Services;

public class ReservationValidator : IReservationValidator
{
    private readonly IReservationRepository _reservationRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly ReservationStatusRepository _statusRepository;
    private readonly ReservationStatusTransitionRepository _transitionRepository;

    public ReservationValidator(
        IReservationRepository reservationRepository,
        CustomerRepository customerRepository,
        ReservationStatusRepository statusRepository,
        ReservationStatusTransitionRepository transitionRepository)
    {
        _reservationRepository = reservationRepository;
        _customerRepository = customerRepository;
        _statusRepository = statusRepository;
        _transitionRepository = transitionRepository;
    }

    public async Task ValidateCustomerExistsAsync(ReservationCustomerId customerId)
    {
        var exists = await _customerRepository.ExistsAsync(CustomerId.Create(customerId.Value));
        if (!exists)
            throw new Exception("El cliente no existe");
    }

    public async Task ValidateStatusExistsAsync(ReservationStatusId statusId)
    {
        var exists = await _statusRepository.ExistsAsync(ReservationStatusIdEntity.Create(statusId.Value));
        if (!exists)
            throw new Exception("El estado de reserva no existe");
    }

    public async Task ValidateCodeUniqueAsync(ReservationCode code, ReservationId? currentId = null)
    {
        var normalized = ReservationCode.Normalize(code.Value);
        var exists = await _reservationRepository.ExistsByNormalizedCodeAsync(normalized, currentId?.Value);
        if (exists)
            throw new Exception("El codigo_reserva ya existe");
    }

    public void ValidateExpiresAt(ReservationReservedAt reservedAt, ReservationExpiresAt expiresAt)
    {
        if (expiresAt.Value.HasValue && expiresAt.Value.Value <= reservedAt.Value)
            throw new Exception("vence_en debe ser mayor que fecha_reserva");
    }

    public async Task ValidateStatusTransitionAsync(ReservationStatusId currentStatusId, ReservationStatusId newStatusId)
    {
        if (currentStatusId.Value == newStatusId.Value)
            return;

        var transition = await _transitionRepository.GetByPairAsync(
            ReservationStatusTransitionOriginId.Create(currentStatusId.Value),
            ReservationStatusTransitionDestinationId.Create(newStatusId.Value));

        if (transition is null)
            throw new Exception("Transicion de estado no permitida");
    }
}

