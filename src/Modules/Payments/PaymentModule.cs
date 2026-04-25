// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\PaymentModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentMethods.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PaymentStates.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Payments.Application.Interfaces;
using GestionAerolineas.src.Modules.Payments.Application.Services;
using GestionAerolineas.src.Modules.Payments.Application.UseCases;
using GestionAerolineas.src.Modules.Payments.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Payments.UI;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Payments;

public static class PaymentModule
{
    public static PaymentMenu Build(AppDbContext context)
    {
        var repository = new PaymentRepository(context);

        var reservationRepository = new ReservationRepository(context);
        var reservationStatusRepository = new ReservationStatusRepository(context);
        var reservationStatusTransitionRepository = new ReservationStatusTransitionRepository(context);
        var paymentStateRepository = new PaymentStateRepository(context);
        var paymentMethodRepository = new PaymentMethodRepository(context);

        IPaymentValidator validator = new PaymentValidator(
            repository,
            reservationRepository,
            reservationStatusRepository,
            paymentStateRepository,
            paymentMethodRepository);

        var create = new CreatePaymentUseCase(
            repository,
            validator,
            reservationRepository,
            reservationStatusRepository,
            reservationStatusTransitionRepository,
            paymentStateRepository);

        var getAll = new GetAllPaymentsUseCase(repository);
        var getById = new GetPaymentByIdUseCase(repository);
        var getByReservationId = new GetPaymentsByReservationIdUseCase(repository);
        var getByReservationCode = new GetPaymentsByReservationCodeUseCase(reservationRepository, repository);
        var getByStateId = new GetPaymentsByStateIdUseCase(repository);
        var getByMethodId = new GetPaymentsByMethodIdUseCase(repository);
        var getByDateRange = new GetPaymentsByDateRangeUseCase(repository);
        var update = new UpdatePaymentUseCase(repository, validator, reservationRepository, reservationStatusRepository, reservationStatusTransitionRepository, paymentStateRepository);
        var delete = new DeletePaymentUseCase(repository, validator);

        var getAllReservations = new GetAllReservationsUseCase(reservationRepository);

        var customerRepository = new CustomerRepository(context);
        var getAllCustomers = new GetAllCustomersUseCase(customerRepository);

        var peopleRepository = new PersonRepository(context);
        var getAllPeople = new GetAllPeopleUseCase(peopleRepository);

        var getAllReservationStatuses = new GetAllReservationStatusesUseCase(reservationStatusRepository);
        var getAllPaymentStates = new GetAllPaymentStatesUseCase(paymentStateRepository);
        var getAllPaymentMethods = new GetAllPaymentMethodsUseCase(paymentMethodRepository);

        return new PaymentMenu(
            create,
            getAll,
            getById,
            getByReservationId,
            getByReservationCode,
            getByStateId,
            getByMethodId,
            getByDateRange,
            update,
            delete,
            getAllReservations,
            getAllCustomers,
            getAllPeople,
            getAllReservationStatuses,
            getAllPaymentStates,
            getAllPaymentMethods);
    }
}

