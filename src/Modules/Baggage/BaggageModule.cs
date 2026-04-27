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

// clase de composicion del modulo: crea repositorio, servicios, casos de uso y menus
public static class BaggageModule
{
    public static BaggageMenu Build(AppDbContext context)
    {
        // repositorio EF que implementa el contrato del dominio
        IBaggageRecordRepository repository = new BaggageRecordRepository(context);
        // servicios y casos de uso comunes para registrar y consultar por cliente
        var services = BuildServices(context);

        // casos de uso propios del menu Admin/Staff
        var getByFlight = new GetBaggageByFlightUseCase(repository);
        var getSurcharges = new GetBaggageSurchargesUseCase(repository);

        // retorna el menu general con todas las operaciones de equipaje
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
        // reutiliza los servicios principales del modulo para el menu del cliente
        var services = BuildServices(context);

        // crea el menu especial que usa el customerId autenticado y no lo pide por consola
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
        // crea una instancia del repositorio para los casos de uso del modulo
        IBaggageRecordRepository repository = new BaggageRecordRepository(context);
        // calculadora con reglas de negocio de recargos
        var calculator = new BaggageSurchargeCalculator();
        // validador que combina value objects y consultas de existencia
        IBaggageValidator validator = new BaggageValidator(repository);

        // agrupa los casos de uso compartidos para no repetir construccion en Build y BuildCustomer
        return new BaggageServices(
            new RegisterBaggageUseCase(repository, calculator, validator),
            new PreviewBaggageSurchargeUseCase(repository, calculator, validator),
            new GetBaggageByCustomerUseCase(repository),
            new GetCabinTypesForBaggageUseCase(repository),
            new GetBaggageRegistrationContextUseCase(repository));
    }

    // record privado para devolver juntas las dependencias comunes del modulo
    private sealed record BaggageServices(
        RegisterBaggageUseCase Register,
        PreviewBaggageSurchargeUseCase Preview,
        GetBaggageByCustomerUseCase GetByCustomer,
        GetCabinTypesForBaggageUseCase GetCabinTypes,
        GetBaggageRegistrationContextUseCase GetRegistrationContext);
}
