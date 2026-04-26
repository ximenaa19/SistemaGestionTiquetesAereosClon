using GestionAerolineas.src.Modules.Baggage.Application.Services;
using GestionAerolineas.src.Modules.Baggage.Application.UseCases;
using GestionAerolineas.src.Modules.Baggage.Application.Interfaces;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;
using GestionAerolineas.src.Modules.Baggage.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Baggage.UI;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Tickets.Application.UseCases;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Baggage;

public static class BaggageModule
{
    public static BaggageMenu Build(AppDbContext context)
    {
        IBaggageRecordRepository repository = new BaggageRecordRepository(context);
        var services = BuildServices(context);

        var getByFlight = new GetBaggageByFlightUseCase(repository);
        var getSurcharges = new GetBaggageSurchargesUseCase(repository);

        return new BaggageMenu(
            services.Register,
            services.Preview,
            services.GetByCustomer,
            getByFlight,
            getSurcharges,
            services.GetCabinTypes,
            services.GetRegistrationContext);
    }

    public static CustomerBaggageMenu BuildCustomer(
        AppDbContext context,
        int customerId,
        GetReservationsByCustomerIdUseCase getReservationsByCustomerId,
        GetTicketsByReservationCodeUseCase getTicketsByReservationCode)
    {
        var services = BuildServices(context);

        return new CustomerBaggageMenu(
            customerId,
            services.Register,
            services.Preview,
            services.GetByCustomer,
            services.GetCabinTypes,
            services.GetRegistrationContext,
            getReservationsByCustomerId,
            getTicketsByReservationCode);
    }

    private static BaggageServices BuildServices(AppDbContext context)
    {
        IBaggageRecordRepository repository = new BaggageRecordRepository(context);
        var calculator = new BaggageSurchargeCalculator();
        IBaggageValidator validator = new BaggageValidator(repository);

        return new BaggageServices(
            new RegisterBaggageUseCase(repository, calculator, validator),
            new PreviewBaggageSurchargeUseCase(repository, calculator, validator),
            new GetBaggageByCustomerUseCase(repository),
            new GetCabinTypesForBaggageUseCase(repository),
            new GetBaggageRegistrationContextUseCase(repository));
    }

    private sealed record BaggageServices(
        RegisterBaggageUseCase Register,
        PreviewBaggageSurchargeUseCase Preview,
        GetBaggageByCustomerUseCase GetByCustomer,
        GetCabinTypesForBaggageUseCase GetCabinTypes,
        GetBaggageRegistrationContextUseCase GetRegistrationContext);
}
