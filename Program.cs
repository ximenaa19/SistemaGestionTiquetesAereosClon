// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: Program.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses;
using GestionAerolineas.src.Modules.Aircraft;
using GestionAerolineas.src.Modules.AircraftManufacturers;
using GestionAerolineas.src.Modules.AircraftModels;
using GestionAerolineas.src.Modules.Airlines;
using GestionAerolineas.src.Modules.AirportAirline;
using GestionAerolineas.src.Modules.Airports;
using GestionAerolineas.src.Modules.Auth;
using GestionAerolineas.src.Modules.AvailabilityStatuses;
using GestionAerolineas.src.Modules.CabinConfiguration;
using GestionAerolineas.src.Modules.CabinTypes;
using GestionAerolineas.src.Modules.CardIssuers;
using GestionAerolineas.src.Modules.CardTypes;
using GestionAerolineas.src.Modules.Checkins;
using GestionAerolineas.src.Modules.CheckinStatuses;
using GestionAerolineas.src.Modules.Cities;
using GestionAerolineas.src.Modules.Continents;
using GestionAerolineas.src.Modules.Countries;
using GestionAerolineas.src.Modules.Customers;
using GestionAerolineas.src.Modules.DocumentTypes;
using GestionAerolineas.src.Modules.EmailDomains;
using GestionAerolineas.src.Modules.Fares;
using GestionAerolineas.src.Modules.FlightAssignments;
using GestionAerolineas.src.Modules.FlightRoles;
using GestionAerolineas.src.Modules.Flights;
using GestionAerolineas.src.Modules.FlightSeats;
using GestionAerolineas.src.Modules.FlightStates;
using GestionAerolineas.src.Modules.FlightStatusTransitions;
using GestionAerolineas.src.Modules.InvoiceItems;
using GestionAerolineas.src.Modules.InvoiceItemTypes;
using GestionAerolineas.src.Modules.Invoices;
using GestionAerolineas.src.Modules.Passengers;
using GestionAerolineas.src.Modules.PassengerTypes;
using GestionAerolineas.src.Modules.PaymentMethods;
using GestionAerolineas.src.Modules.PaymentMethodTypes;
using GestionAerolineas.src.Modules.Payments;
using GestionAerolineas.src.Modules.PaymentStates;
using GestionAerolineas.src.Modules.People;
using GestionAerolineas.src.Modules.Permissions;
using GestionAerolineas.src.Modules.PersonEmails;
using GestionAerolineas.src.Modules.PersonPhones;
using GestionAerolineas.src.Modules.PhoneCodes;
using GestionAerolineas.src.Modules.Regions;
using GestionAerolineas.src.Modules.Reservations;
using GestionAerolineas.src.Modules.ReservationFlights;
using GestionAerolineas.src.Modules.ReservationPassengers;
using GestionAerolineas.src.Modules.ReservationStatuses;
using GestionAerolineas.src.Modules.ReservationStatusTransitions;
using GestionAerolineas.src.Modules.RoadTypes;
using GestionAerolineas.src.Modules.RolePermissions;
using GestionAerolineas.src.Modules.Reports;
using GestionAerolineas.src.Modules.Routes;
using GestionAerolineas.src.Modules.RouteStops;
using GestionAerolineas.src.Modules.Seasons;
using GestionAerolineas.src.Modules.SeatLocationTypes;
using GestionAerolineas.src.Modules.Sessions;
using GestionAerolineas.src.Modules.Staff;
using GestionAerolineas.src.Modules.StaffAvailability;
using GestionAerolineas.src.Modules.StaffRoles;
using GestionAerolineas.src.Modules.SystemRoles;
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Tickets;
using GestionAerolineas.src.Modules.TicketStatuses;
using GestionAerolineas.src.Modules.Users;
using GestionAerolineas.src.Modules.Users.Application.UseCases;
using GestionAerolineas.src.Modules.Users.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Reservations.Application.Services;
using GestionAerolineas.src.Modules.Reservations.Application.UseCases;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationFlights.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationFlights.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Tickets.Application.UseCases;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Repository;
using GestionAerolineas.src.Modules.TicketStatuses.Application.UseCases;
using GestionAerolineas.src.Modules.TicketStatuses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Payments.Application.UseCases;
using GestionAerolineas.src.Modules.Payments.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PaymentStates.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentMethods.Infrastructure.Repository;
using GestionAerolineas.src.shared.Helpers;
using GestionAerolineas.src.shared.Seed;
using GestionAerolineas.src.shared.Ui.RoleMenus;

try
{
    var context = DbContextFactory.Create();

    if (!context.Database.CanConnect())
    {
        Console.WriteLine("No se pudo conectar a la base de datos");
        return;
    }

    Console.WriteLine("Conexion exitosa");
    await SeedRunner.SeedMasterAndCatalogsAsync(context);
    Console.WriteLine("Seed de catalogos y datos maestros listo\n");

    var continentMenu = ContinentModule.Build(context);
    var countryMenu = CountryModule.Build(context);
    var customerMenu = CustomerModule.Build(context);
    var aircraftManufacturerMenu = AircraftManufacturerModule.Build(context);
    var aircraftModelMenu = AircraftModelModule.Build(context);
    var aircraftMenu = AircraftModule.Build(context);
    var airportMenu = AirportModule.Build(context);
    var airportAirlineMenu = AirportAirlineModule.Build(context);
    var airlineMenu = AirlineModule.Build(context);
    var routeMenu = RouteModule.Build(context);
    var routeStopMenu = RouteStopModule.Build(context);
    var fareMenu = FareModule.Build(context);
    var staffMenu = StaffModule.Build(context);
    var staffAvailabilityMenu = StaffAvailabilityModule.Build(context);
    var flightMenu = FlightModule.Build(context);
    var flightSeatMenu = FlightSeatModule.Build(context);
    var flightAssignmentMenu = FlightAssignmentModule.Build(context);
    var reservationMenu = ReservationModule.Build(context);
    var reservationFlightMenu = ReservationFlightModule.Build(context);
    var reservationPassengerMenu = ReservationPassengerModule.Build(context);
    var ticketMenu = TicketModule.Build(context);
    var checkinMenu = CheckinModule.Build(context);
    var paymentMenu = PaymentModule.Build(context);
    var invoiceMenu = InvoiceModule.Build(context);
    var invoiceItemMenu = InvoiceItemModule.Build(context);
    var regionMenu = RegionModule.Build(context);
    var cityMenu = CityModule.Build(context);
    var addressMenu = AddressModule.Build(context);
    var availabilityStatusMenu = AvailabilityStatusModule.Build(context);
    var cardTypeMenu = CardTypeModule.Build(context);
    var cardIssuerMenu = CardIssuerModule.Build(context);
    var checkinStatusMenu = CheckinStatusModule.Build(context);
    var emailDomainMenu = EmailDomainModule.Build(context);
    var personEmailMenu = PersonEmailModule.Build(context);
    var personPhoneMenu = PersonPhoneModule.Build(context);
    var roadTypeMenu = RoadTypeModule.Build(context);
    var documentTypeMenu = DocumentTypeModule.Build(context);
    var flightRoleMenu = FlightRoleModule.Build(context);
    var flightStateMenu = FlightStateModule.Build(context);
    var flightStatusTransitionMenu = FlightStatusTransitionModule.Build(context);
    var invoiceItemTypeMenu = InvoiceItemTypeModule.Build(context);
    var paymentMethodTypeMenu = PaymentMethodTypeModule.Build(context);
    var paymentMethodMenu = PaymentMethodModule.Build(context);
    var paymentStateMenu = PaymentStateModule.Build(context);
    var permissionMenu = PermissionModule.Build(context);
    var phoneCodeMenu = PhoneCodeModule.Build(context);
    var reservationStatusMenu = ReservationStatusModule.Build(context);
    var seasonMenu = SeasonModule.Build(context);
    var seatLocationTypeMenu = SeatLocationTypeModule.Build(context);
    var sessionMenu = SessionModule.Build(context);
    var staffRoleMenu = StaffRoleModule.Build(context);
    var systemRoleMenu = SystemRoleModule.Build(context);
    var ticketStatusMenu = TicketStatusModule.Build(context);
    var userMenu = UserModule.Build(context);
    var authMenu = AuthModule.Build(context);
    var personMenu = PersonModule.Build(context);
    var adminCreatePersonFlow = PersonModule.BuildAdminCreateFlow(context);
    var adminUpdatePersonFlow = PersonModule.BuildAdminUpdateFlow(context);
    var adminDeletePersonFlow = PersonModule.BuildAdminDeleteFlow(context);
    var adminCreateAirlineFlow = AirlineModule.BuildAdminCreateFlow(context);
    var adminUpdateAirlineFlow = AirlineModule.BuildAdminUpdateFlow(context);
    var adminDeleteAirlineFlow = AirlineModule.BuildAdminDeleteFlow(context);
    var adminCreateAirportFlow = AirportModule.BuildAdminCreateFlow(context);
    var adminUpdateAirportFlow = AirportModule.BuildAdminUpdateFlow(context);
    var adminDeleteAirportFlow = AirportModule.BuildAdminDeleteFlow(context);
    var adminCreateRouteFlow = RouteModule.BuildAdminCreateFlow(context);
    var adminUpdateRouteFlow = RouteModule.BuildAdminUpdateFlow(context);
    var adminDeleteRouteFlow = RouteModule.BuildAdminDeleteFlow(context);
    var adminCreateAircraftFlow = AircraftModule.BuildAdminCreateFlow(context);
    var adminUpdateAircraftFlow = AircraftModule.BuildAdminUpdateFlow(context);
    var adminDeleteAircraftFlow = AircraftModule.BuildAdminDeleteFlow(context);
    var reportsMenu = ReportsModule.Build(context);
    var passengerMenu = PassengerModule.Build(context);
    var cabinTypeMenu = CabinTypeModule.Build(context);
    var cabinConfigurationMenu = CabinConfigurationModule.Build(context);
    var passengerTypeMenu = PassengerTypeModule.Build(context);
    var reservationStatusTransitionMenu = ReservationStatusTransitionModule.Build(context);
    var rolePermissionMenu = RolePermissionModule.Build(context);

    var authResult = await authMenu.StartAsync();
    if (authResult is null)
        return;

    var roleRepository = new SystemRoleRepository(context);
    var getRoleById = new GetSystemRoleByIdUseCase(roleRepository);
    var role = await getRoleById.ExecuteAsync(authResult.RoleId);
    var roleName = role?.Name.Value?.Trim() ?? string.Empty;

    var adminAirOperationMenu = new RoleMenu("ADMIN - OPERACION AEREA", new List<RoleMenuOption>
    {
        new("Vuelos", () => flightMenu.StartAsync()),
        new("Asignaciones de vuelo", () => flightAssignmentMenu.StartAsync()),
        new("Asientos de vuelo", () => flightSeatMenu.StartAsync()),
        new("Rutas", () => routeMenu.StartAsync()),
        new("Escalas de ruta", () => routeStopMenu.StartAsync()),
        new("Aeropuertos", () => airportMenu.StartAsync()),
        new("Aeropuerto-Aerolinea", () => airportAirlineMenu.StartAsync()),
        new("Aerolineas", () => airlineMenu.StartAsync()),
        new("Aeronaves", () => aircraftMenu.StartAsync()),
        new("Modelos de aeronave", () => aircraftModelMenu.StartAsync()),
        new("Fabricantes de aeronave", () => aircraftManufacturerMenu.StartAsync()),
        new("Configuracion de cabina", () => cabinConfigurationMenu.StartAsync()),
        new("Tarifas", () => fareMenu.StartAsync())
    }, "Volver");

    var adminCommercialMenu = new RoleMenu("ADMIN - COMERCIAL Y VENTAS", new List<RoleMenuOption>
    {
        new("Reservas", () => reservationMenu.StartAsync()),
        new("Reservas por vuelo", () => reservationFlightMenu.StartAsync()),
        new("Pasajeros por reserva", () => reservationPassengerMenu.StartAsync()),
        new("Pagos", () => paymentMenu.StartAsync()),
        new("Facturas", () => invoiceMenu.StartAsync()),
        new("Items de factura", () => invoiceItemMenu.StartAsync()),
        new("Tiquetes", () => ticketMenu.StartAsync()),
        new("Check-ins", () => checkinMenu.StartAsync())
    }, "Volver");

    var adminPeopleMenu = new RoleMenu("ADMIN - PERSONAS Y ORGANIZACION", new List<RoleMenuOption>
    {
        new("Personas", () => personMenu.StartAsync()),
        new("Clientes", () => customerMenu.StartAsync()),
        new("Pasajeros", () => passengerMenu.StartAsync()),
        new("Staff", () => staffMenu.StartAsync()),
        new("Disponibilidad de staff", () => staffAvailabilityMenu.StartAsync()),
        new("Correos de persona", () => personEmailMenu.StartAsync()),
        new("Telefonos de persona", () => personPhoneMenu.StartAsync()),
        new("Usuarios", () => userMenu.StartAsync()),
        new("Sesiones", () => sessionMenu.StartAsync())
    }, "Volver");

    var adminSecurityMenu = new RoleMenu("ADMIN - SEGURIDAD Y PERMISOS", new List<RoleMenuOption>
    {
        new("Roles del sistema", () => systemRoleMenu.StartAsync()),
        new("Permisos", () => permissionMenu.StartAsync()),
        new("Permisos por rol", () => rolePermissionMenu.StartAsync()),
        new("Roles de staff", () => staffRoleMenu.StartAsync()),
        new("Roles de vuelo", () => flightRoleMenu.StartAsync())
    }, "Volver");

    var adminCatalogMenu = new RoleMenu("ADMIN - CATALOGOS MAESTROS", new List<RoleMenuOption>
    {
        new("Continentes", () => continentMenu.StartAsync()),
        new("Paises", () => countryMenu.StartAsync()),
        new("Regiones", () => regionMenu.StartAsync()),
        new("Ciudades", () => cityMenu.StartAsync()),
        new("Direcciones", () => addressMenu.StartAsync()),
        new("Tipos de via", () => roadTypeMenu.StartAsync()),
        new("Tipos de documento", () => documentTypeMenu.StartAsync()),
        new("Codigos telefonicos", () => phoneCodeMenu.StartAsync()),
        new("Dominios de correo", () => emailDomainMenu.StartAsync()),
        new("Tipos de pasajero", () => passengerTypeMenu.StartAsync()),
        new("Tipos de cabina", () => cabinTypeMenu.StartAsync()),
        new("Ubicaciones de asiento", () => seatLocationTypeMenu.StartAsync()),
        new("Estados de disponibilidad", () => availabilityStatusMenu.StartAsync()),
        new("Estados de reserva", () => reservationStatusMenu.StartAsync()),
        new("Transiciones de reserva", () => reservationStatusTransitionMenu.StartAsync()),
        new("Estados de vuelo", () => flightStateMenu.StartAsync()),
        new("Transiciones de vuelo", () => flightStatusTransitionMenu.StartAsync()),
        new("Estados de tiquete", () => ticketStatusMenu.StartAsync()),
        new("Estados de check-in", () => checkinStatusMenu.StartAsync()),
        new("Estados de pago", () => paymentStateMenu.StartAsync()),
        new("Tipos de metodo de pago", () => paymentMethodTypeMenu.StartAsync()),
        new("Metodos de pago", () => paymentMethodMenu.StartAsync()),
        new("Tipos de tarjeta", () => cardTypeMenu.StartAsync()),
        new("Franquicias de tarjeta", () => cardIssuerMenu.StartAsync()),
        new("Tipos de item de factura", () => invoiceItemTypeMenu.StartAsync()),
        new("Temporadas", () => seasonMenu.StartAsync())
    }, "Volver");

    var adminSystemMenu = new RoleMenu("ADMIN - SISTEMA", new List<RoleMenuOption>
    {
        new("Ejecutar seed maestros y catalogos", async () =>
        {
            await SeedRunner.SeedMasterAndCatalogsAsync(context);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✔ Seed completado");
            Console.ResetColor();
            Console.WriteLine("Presiona una tecla para continuar...");
            Console.ReadKey();
        })
    }, "Volver");

    var adminPersonCrudMenu = new RoleMenu("ADMIN - PERSONA", new List<RoleMenuOption>
    {
        new("Crear persona", () => adminCreatePersonFlow.StartAsync()),
        new("Actualizar persona", () => adminUpdatePersonFlow.StartAsync()),
        new("Eliminar persona", () => adminDeletePersonFlow.StartAsync())
    }, "Volver");

    var adminAirlineCrudMenu = new RoleMenu("ADMIN - AEREOLINEA", new List<RoleMenuOption>
    {
        new("Crear aerolinea", () => adminCreateAirlineFlow.StartAsync()),
        new("Actualizar aerolinea", () => adminUpdateAirlineFlow.StartAsync()),
        new("Eliminar aerolinea", () => adminDeleteAirlineFlow.StartAsync())
    }, "Volver");

    var adminAirportCrudMenu = new RoleMenu("ADMIN - AEROPUERTO", new List<RoleMenuOption>
    {
        new("Crear aeropuerto", () => adminCreateAirportFlow.StartAsync()),
        new("Actualizar aeropuerto", () => adminUpdateAirportFlow.StartAsync()),
        new("Eliminar aeropuerto", () => adminDeleteAirportFlow.StartAsync())
    }, "Volver");

    var adminRouteCrudMenu = new RoleMenu("ADMIN - RUTA", new List<RoleMenuOption>
    {
        new("Crear ruta", () => adminCreateRouteFlow.StartAsync()),
        new("Actualizar ruta", () => adminUpdateRouteFlow.StartAsync()),
        new("Eliminar ruta", () => adminDeleteRouteFlow.StartAsync())
    }, "Volver");

    var adminAircraftCrudMenu = new RoleMenu("ADMIN - AERONAVE", new List<RoleMenuOption>
    {
        new("Crear aeronave", () => adminCreateAircraftFlow.StartAsync()),
        new("Actualizar aeronave", () => adminUpdateAircraftFlow.StartAsync()),
        new("Eliminar aeronave", () => adminDeleteAircraftFlow.StartAsync())
    }, "Volver");

    var adminSecondaryMenu = new RoleMenu("ADMIN - MENU SECUNDARIO", new List<RoleMenuOption>
    {
        new("Operacion aerea", () => adminAirOperationMenu.StartAsync()),
        new("Comercial y ventas", () => adminCommercialMenu.StartAsync()),
        new("Personas y organizacion", () => adminPeopleMenu.StartAsync()),
        new("Seguridad y permisos", () => adminSecurityMenu.StartAsync()),
        new("Catalogos maestros", () => adminCatalogMenu.StartAsync()),
        new("Sistema", () => adminSystemMenu.StartAsync())
    }, "Volver");

    var adminMenu = new AdminRoleMenu(new List<RoleMenuOption>
    {
        new("Persona", () => adminPersonCrudMenu.StartAsync()),
        new("Aereolinea", () => adminAirlineCrudMenu.StartAsync()),
        new("Aeropuerto", () => adminAirportCrudMenu.StartAsync()),
        new("Ruta", () => adminRouteCrudMenu.StartAsync()),
        new("Aeronave", () => adminAircraftCrudMenu.StartAsync()),
        new("Reportes (LINQ)", () => reportsMenu.StartAsync()),
        new("Menu secundario", () => adminSecondaryMenu.StartAsync())
    });

    var staffRoleMenuUi = new StaffRoleMenu(new List<RoleMenuOption>
    {
        new("Vuelos", () => flightMenu.StartAsync()),
        new("Reservas", () => reservationMenu.StartAsync()),
        new("Reservas por vuelo", () => reservationFlightMenu.StartAsync()),
        new("Pasajeros por reserva", () => reservationPassengerMenu.StartAsync()),
        new("Check-ins", () => checkinMenu.StartAsync()),
        new("Pagos", () => paymentMenu.StartAsync()),
        new("Tiquetes", () => ticketMenu.StartAsync()),
        new("Sesiones", () => sessionMenu.StartAsync())
    });

    var customerSecondaryMenu = new RoleMenu("CLIENTE - MENU SECUNDARIO", new List<RoleMenuOption>
    {
        new("Vuelos", () => flightMenu.StartAsync()),
        new("Reservas (modulo completo)", () => reservationMenu.StartAsync()),
        new("Reservas por vuelo", () => reservationFlightMenu.StartAsync()),
        new("Pasajeros por reserva", () => reservationPassengerMenu.StartAsync()),
        new("Tiquetes (modulo completo)", () => ticketMenu.StartAsync()),
        new("Pagos (modulo completo)", () => paymentMenu.StartAsync()),
        new("Check-ins (modulo completo)", () => checkinMenu.StartAsync()),
        new("Clientes (modulo completo)", () => customerMenu.StartAsync())
    }, "Volver");

    var customerProfileMenu = new RoleMenu("CLIENTE - PERFIL BASICO", new List<RoleMenuOption>
    {
        new("Gestionar correos", () => personEmailMenu.StartAsync()),
        new("Gestionar telefonos", () => personPhoneMenu.StartAsync())
    }, "Volver");

    if (roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
    {
        await adminMenu.StartAsync();
    }
    else if (roleName.Equals("Agente", StringComparison.OrdinalIgnoreCase) ||
             roleName.Equals("Staff", StringComparison.OrdinalIgnoreCase))
    {
        await staffRoleMenuUi.StartAsync();
    }
    else if (roleName.Equals("Cliente", StringComparison.OrdinalIgnoreCase) ||
             roleName.Equals("Customer", StringComparison.OrdinalIgnoreCase))
    {
        var userRepository = new UserRepository(context);
        var customerRepository = new CustomerRepository(context);
        var passengerRepository = new PassengerRepository(context);

        var getUserById = new GetUserByIdUseCase(userRepository);
        var getCustomerByPersonId = new GetCustomerByPersonIdUseCase(customerRepository);
        var getPassengerByPersonId = new GetPassengerByPersonIdUseCase(passengerRepository);

        var user = await getUserById.ExecuteAsync(authResult.UserId);
        if (user is null || !user.PersonId.Value.HasValue)
        {
            Console.WriteLine("No se encontro persona asociada al usuario cliente.");
            Console.WriteLine("Presiona una tecla para salir...");
            Console.ReadKey();
            return;
        }

        var personId = user.PersonId.Value.Value;
        var customer = await getCustomerByPersonId.ExecuteAsync(personId);
        if (customer is null)
        {
            Console.WriteLine("La persona del usuario no tiene registro en customers.");
            Console.WriteLine("Presiona una tecla para salir...");
            Console.ReadKey();
            return;
        }

        var passenger = await getPassengerByPersonId.ExecuteAsync(personId);

        var reservationRepository = new ReservationRepository(context);
        var reservationFlightRepository = new ReservationFlightRepository(context);
        var reservationPassengerRepository = new ReservationPassengerRepository(context);
        var reservationStatusRepository = new ReservationStatusRepository(context);
        var reservationStatusTransitionRepository = new ReservationStatusTransitionRepository(context);
        var ticketRepository = new TicketRepository(context);
        var ticketStatusRepository = new TicketStatusRepository(context);
        var paymentRepository = new PaymentRepository(context);
        var paymentStateRepository = new PaymentStateRepository(context);
        var paymentMethodRepository = new PaymentMethodRepository(context);

        var getReservationsByCustomerId = new GetReservationsByCustomerIdUseCase(reservationRepository);
        var getReservationDetailsById = new GetReservationDetailsByIdUseCase(
            reservationRepository,
            reservationFlightRepository,
            reservationPassengerRepository);
        var getTicketsByReservationCode = new GetTicketsByReservationCodeUseCase(ticketRepository);
        var getPaymentsByReservationCode = new GetPaymentsByReservationCodeUseCase(reservationRepository, paymentRepository);
        var getAllReservationStatuses = new GetAllReservationStatusesUseCase(reservationStatusRepository);
        var getAllTicketStatuses = new GetAllTicketStatusesUseCase(ticketStatusRepository);
        var getAllPaymentStates = new GetAllPaymentStatesUseCase(paymentStateRepository);
        var getAllPaymentMethods = new GetAllPaymentMethodsUseCase(paymentMethodRepository);

        var reservationValidator = new ReservationValidator(
            reservationRepository,
            customerRepository,
            reservationStatusRepository,
            reservationStatusTransitionRepository);
        var updateReservationStatus = new GestionAerolineas.src.Modules.Reservations.Application.UseCases.UpdateReservationStatusUseCase(
            reservationRepository,
            reservationValidator);

        var customerSelfServiceMenu = new CustomerSelfServiceMenu(
            customer.Id.Value,
            personId,
            passenger?.Id.Value,
            authResult.Username,
            getReservationsByCustomerId,
            getReservationDetailsById,
            getTicketsByReservationCode,
            getPaymentsByReservationCode,
            getAllReservationStatuses,
            getAllTicketStatuses,
            getAllPaymentStates,
            getAllPaymentMethods,
            updateReservationStatus,
            () => flightMenu.StartAsync(),
            () => reservationMenu.StartAsync(),
            () => checkinMenu.StartAsync(),
            () => customerProfileMenu.StartAsync(),
            () => customerSecondaryMenu.StartAsync());

        await customerSelfServiceMenu.StartAsync();
    }
    else
    {
        Console.WriteLine($"Rol no mapeado para menu principal: '{roleName}'.");
        Console.WriteLine("Presiona una tecla para salir...");
        Console.ReadKey();
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error inesperado: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.Error.WriteLine($"Detalle: {ex.InnerException.Message}");
    }
}
