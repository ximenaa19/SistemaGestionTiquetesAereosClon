// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\InvoiceModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.InvoiceItems.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Invoices.Application.Interfaces;
using GestionAerolineas.src.Modules.Invoices.Application.Services;
using GestionAerolineas.src.Modules.Invoices.Application.UseCases;
using GestionAerolineas.src.Modules.Invoices.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Invoices.UI;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Invoices;

public static class InvoiceModule
{
    public static InvoiceMenu Build(AppDbContext context)
    {
        var invoiceRepository = new InvoiceRepository(context);
        var reservationRepository = new ReservationRepository(context);
        var reservationStatusRepository = new ReservationStatusRepository(context);

        var invoiceItemRepository = new InvoiceItemRepository(context);

        IInvoiceValidator validator = new InvoiceValidator(invoiceRepository, reservationRepository, reservationStatusRepository, invoiceItemRepository);

        var create = new CreateInvoiceUseCase(invoiceRepository, validator);
        var getAll = new GetAllInvoicesUseCase(invoiceRepository);
        var getById = new GetInvoiceByIdUseCase(invoiceRepository);
        var getByNumber = new GetInvoiceByNumberUseCase(invoiceRepository);
        var getByReservationId = new GetInvoiceByReservationIdUseCase(invoiceRepository);
        var getByDateRange = new GetInvoicesByIssueDateRangeUseCase(invoiceRepository);
        var getDetailsById = new GetInvoiceDetailsByIdUseCase(invoiceRepository, invoiceItemRepository);
        var update = new UpdateInvoiceUseCase(invoiceRepository, validator);
        var delete = new DeleteInvoiceUseCase(invoiceRepository, validator);

        var customerRepository = new CustomerRepository(context);
        var peopleRepository = new PersonRepository(context);

        var getAllReservations = new GetAllReservationsUseCase(reservationRepository);
        var getAllCustomers = new GetAllCustomersUseCase(customerRepository);
        var getAllPeople = new GetAllPeopleUseCase(peopleRepository);
        var getAllStatuses = new GetAllReservationStatusesUseCase(reservationStatusRepository);

        return new InvoiceMenu(
            create,
            getAll,
            getById,
            getByNumber,
            getByReservationId,
            getByDateRange,
            getDetailsById,
            update,
            delete,
            getAllReservations,
            getAllCustomers,
            getAllPeople,
            getAllStatuses);
    }
}
