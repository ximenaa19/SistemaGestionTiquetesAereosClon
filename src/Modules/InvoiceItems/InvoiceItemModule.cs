// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\InvoiceItemModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Application.UseCases;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.InvoiceItems.Application.Interfaces;
using GestionAerolineas.src.Modules.InvoiceItems.Application.Services;
using GestionAerolineas.src.Modules.InvoiceItems.Application.UseCases;
using GestionAerolineas.src.Modules.InvoiceItems.Infrastructure.Repository;
using GestionAerolineas.src.Modules.InvoiceItems.UI;
using GestionAerolineas.src.Modules.Invoices.Application.UseCases;
using GestionAerolineas.src.Modules.Invoices.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.InvoiceItems;

public static class InvoiceItemModule
{
    public static InvoiceItemMenu Build(AppDbContext context)
    {
        var repository = new InvoiceItemRepository(context);
        var invoiceRepository = new InvoiceRepository(context);
        var typeRepository = new InvoiceItemTypeRepository(context);
        var reservationPassengerRepository = new ReservationPassengerRepository(context);
        var reservationFlightRepository = new ReservationFlightRepository(context);

        var passengerRepository = new PassengerRepository(context);
        var peopleRepository = new PersonRepository(context);

        IInvoiceItemValidator validator = new InvoiceItemValidator(invoiceRepository, typeRepository, reservationPassengerRepository, reservationFlightRepository);

        var create = new CreateInvoiceItemUseCase(repository, validator);
        var getAll = new GetAllInvoiceItemsUseCase(repository);
        var getById = new GetInvoiceItemByIdUseCase(repository);
        var getByInvoiceId = new GetInvoiceItemsByInvoiceIdUseCase(repository);
        var getByTypeId = new GetInvoiceItemsByItemTypeIdUseCase(repository);
        var getByReservationPassengerId = new GetInvoiceItemsByReservationPassengerIdUseCase(repository);
        var update = new UpdateInvoiceItemUseCase(repository, validator);
        var delete = new DeleteInvoiceItemUseCase(repository);

        var getAllInvoices = new GetAllInvoicesUseCase(invoiceRepository);
        var getAllTypes = new GetAllInvoiceItemTypesUseCase(typeRepository);
        var getAllPassengers = new GetAllPassengersUseCase(passengerRepository);
        var getAllPeople = new GetAllPeopleUseCase(peopleRepository);

        return new InvoiceItemMenu(
            create,
            getAll,
            getById,
            getByInvoiceId,
            getByTypeId,
            getByReservationPassengerId,
            update,
            delete,
            getAllInvoices,
            getAllTypes,
            reservationFlightRepository,
            reservationPassengerRepository,
            getAllPassengers,
            getAllPeople);
    }
}

