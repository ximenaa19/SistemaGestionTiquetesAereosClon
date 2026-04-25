// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\UI\AdminCreatePersonFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Globalization;
using GestionAerolineas.src.Modules.Addresses.Application.UseCases;
using GestionAerolineas.src.Modules.Airlines.Application.UseCases;
using GestionAerolineas.src.Modules.Airports.Application.UseCases;
using GestionAerolineas.src.Modules.Cities.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.DocumentTypes.Application.UseCases;
using GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.PersonEmails.Application.UseCases;
using GestionAerolineas.src.Modules.RoadTypes.Application.UseCases;
using GestionAerolineas.src.Modules.Staff.Application.UseCases;
using GestionAerolineas.src.Modules.StaffRoles.Application.UseCases;
using GestionAerolineas.src.Modules.SystemRoles.Application.UseCases;
using GestionAerolineas.src.Modules.Users.Application.UseCases;

namespace GestionAerolineas.src.Modules.People.UI;

/// <summary>
/// Flujo guiado para crear una persona desde el menú admin.
/// Además de la persona, crea de forma integrada usuario y, según rol, registro de customer/staff.
/// </summary>
public sealed class AdminCreatePersonFlow
{
    private const string CancelToken = "000000";
    private static readonly DateTime CancelledDate = DateTime.MinValue;

    private readonly CreatePersonUseCase _createPerson;
    private readonly GetPersonByDocumentUseCase _getPersonByDocument;
    private readonly CreatePersonEmailUseCase _createPersonEmail;
    private readonly GetAllDocumentTypesUseCase _getAllDocumentTypes;
    private readonly GetAllEmailDomainsUseCase _getAllEmailDomains;
    private readonly CreateAddressUseCase _createAddress;
    private readonly GetAllAddressesUseCase _getAllAddresses;
    private readonly GetAllRoadTypesUseCase _getAllRoadTypes;
    private readonly GetAllCitiesUseCase _getAllCities;
    private readonly GetAllSystemRolesUseCase _getAllSystemRoles;
    private readonly CreateUserUseCase _createUser;
    private readonly CreateCustomerUseCase _createCustomer;
    private readonly CreateStaffUseCase _createStaff;
    private readonly GetAllStaffRolesUseCase _getAllStaffRoles;
    private readonly GetAllAirlinesUseCase _getAllAirlines;
    private readonly GetAllAirportsUseCase _getAllAirports;

    public AdminCreatePersonFlow(
        CreatePersonUseCase createPerson,
        GetPersonByDocumentUseCase getPersonByDocument,
        CreatePersonEmailUseCase createPersonEmail,
        GetAllDocumentTypesUseCase getAllDocumentTypes,
        GetAllEmailDomainsUseCase getAllEmailDomains,
        CreateAddressUseCase createAddress,
        GetAllAddressesUseCase getAllAddresses,
        GetAllRoadTypesUseCase getAllRoadTypes,
        GetAllCitiesUseCase getAllCities,
        GetAllSystemRolesUseCase getAllSystemRoles,
        CreateUserUseCase createUser,
        CreateCustomerUseCase createCustomer,
        CreateStaffUseCase createStaff,
        GetAllStaffRolesUseCase getAllStaffRoles,
        GetAllAirlinesUseCase getAllAirlines,
        GetAllAirportsUseCase getAllAirports)
    {
        _createPerson = createPerson;
        _getPersonByDocument = getPersonByDocument;
        _createPersonEmail = createPersonEmail;
        _getAllDocumentTypes = getAllDocumentTypes;
        _getAllEmailDomains = getAllEmailDomains;
        _createAddress = createAddress;
        _getAllAddresses = getAllAddresses;
        _getAllRoadTypes = getAllRoadTypes;
        _getAllCities = getAllCities;
        _getAllSystemRoles = getAllSystemRoles;
        _createUser = createUser;
        _createCustomer = createCustomer;
        _createStaff = createStaff;
        _getAllStaffRoles = getAllStaffRoles;
        _getAllAirlines = getAllAirlines;
        _getAllAirports = getAllAirports;
    }

    /// <summary>
    /// Orquesta el proceso completo: captura datos, confirma, persiste y notifica resultados.
    /// </summary>
    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            PrintHeader();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Escribe {CancelToken} para cancelar en cualquier momento.\n");
            Console.ResetColor();

            var draft = await CollectDraftAsync();
            if (draft is null)
                return;

            var choice = ConfirmChoice(draft);
            if (choice == 2)
                continue;
            if (choice == 3)
                return;

            try
            {
                int? addressId = null;
                if (draft.Address is not null)
                {
                    addressId = await CreateAddressAndGetIdAsync(draft.Address);
                }

                await _createPerson.ExecuteAsync(
                    draft.DocumentTypeId,
                    draft.DocumentNumber,
                    draft.FirstNames,
                    draft.LastNames,
                    draft.BirthDate,
                    draft.Gender,
                    addressId);

                var createdPerson = await _getPersonByDocument.ExecuteAsync(draft.DocumentTypeId, draft.DocumentNumber);
                if (createdPerson is null)
                    throw new Exception("No fue posible recuperar la persona creada.");

                if (!string.IsNullOrWhiteSpace(draft.EmailUser) && draft.EmailDomainId.HasValue)
                {
                    await _createPersonEmail.ExecuteAsync(
                        createdPerson.Id.Value,
                        draft.EmailUser!,
                        draft.EmailDomainId.Value,
                        true);
                }

                await _createUser.ExecuteAsync(
                    draft.Username,
                    draft.Password,
                    createdPerson.Id.Value,
                    draft.RoleId);

                if (draft.BusinessRole is RoleKind.Customer)
                {
                    await _createCustomer.ExecuteAsync(createdPerson.Id.Value);
                }

                if (draft.BusinessRole is RoleKind.Staff && draft.Staff is not null)
                {
                    await _createStaff.ExecuteAsync(
                        createdPerson.Id.Value,
                        draft.Staff.StaffRoleId,
                        draft.Staff.AirlineId,
                        draft.Staff.AirportId,
                        draft.Staff.HireDate,
                        draft.Staff.IsActive);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Persona creada correctamente.");
                Console.WriteLine("✔ Usuario creado correctamente.");
                if (draft.BusinessRole is RoleKind.Customer)
                    Console.WriteLine("✔ Registro de customer creado correctamente.");
                if (draft.BusinessRole is RoleKind.Staff)
                    Console.WriteLine("✔ Registro de staff creado correctamente.");
                Console.ResetColor();
                Console.WriteLine($"Persona ID: {createdPerson.Id.Value}");
                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
                return;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Error: {ex.GetBaseException().Message}");
                Console.ResetColor();
                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    /// <summary>
    /// Recolecta todos los datos de entrada de persona, cuenta y rol de negocio.
    /// Si el usuario cancela en cualquier paso, retorna <c>null</c>.
    /// </summary>
    private async Task<PersonDraft?> CollectDraftAsync()
    {
        var documentType = await SelectDocumentTypeAsync();
        if (documentType is null) return null;

        var documentNumber = ReadRequiredText("Numero de documento");
        if (documentNumber is null) return null;

        var firstNames = ReadRequiredText("Nombres");
        if (firstNames is null) return null;

        var lastNames = ReadRequiredText("Apellidos");
        if (lastNames is null) return null;

        var birthDate = ReadOptionalDate("Fecha de nacimiento (yyyy-MM-dd) [opcional]");
        if (birthDate == CancelledDate) return null;

        var gender = ReadOptionalGender("Genero (M/F/N) [opcional]");
        if (gender == CancelToken) return null;
        if (string.IsNullOrWhiteSpace(gender))
            gender = null;

        AddressDraft? address = null;
        var registerAddress = ReadYesNo("Desea registrar una direccion nueva? (S/N)");
        if (registerAddress is null) return null;
        if (registerAddress.Value)
        {
            address = await CollectAddressDraftAsync();
            if (address is null) return null;
        }

        string? emailUser = null;
        int? emailDomainId = null;
        string? emailDomainName = null;
        var registerEmail = ReadYesNo("Desea registrar correo principal? (S/N)");
        if (registerEmail is null) return null;
        if (registerEmail.Value)
        {
            emailUser = ReadRequiredText("Correo principal (usuario sin @)");
            if (emailUser is null) return null;

            var emailDomain = await SelectEmailDomainAsync();
            if (emailDomain is null) return null;
            emailDomainId = emailDomain.Value.id;
            emailDomainName = emailDomain.Value.name;
        }

        var role = await SelectRoleAsync();
        if (role is null) return null;

        var businessRole = ResolveRoleKind(role.Value.name);
        StaffDraft? staff = null;
        if (businessRole is RoleKind.Staff)
        {
            staff = await CollectStaffDraftAsync();
            if (staff is null) return null;
        }

        var username = ReadRequiredText("Username para acceso");
        if (username is null) return null;

        var password = ReadRequiredPassword("Contrasena (minimo 8)");
        if (password is null) return null;

        return new PersonDraft(
            documentType.Value.id,
            documentType.Value.name,
            documentNumber,
            firstNames,
            lastNames,
            birthDate,
            gender,
            address,
            emailUser,
            emailDomainId,
            emailDomainName,
            role.Value.id,
            role.Value.name,
            businessRole,
            staff,
            username,
            password
        );
    }

    /// <summary>
    /// Captura la dirección completa para crearla en la tabla de direcciones.
    /// </summary>
    private async Task<AddressDraft?> CollectAddressDraftAsync()
    {
        var roadType = await SelectRoadTypeAsync();
        if (roadType is null) return null;

        var city = await SelectCityAsync();
        if (city is null) return null;

        var roadName = ReadRequiredText("Nombre de via");
        if (roadName is null) return null;

        var number = ReadOptionalText("Numero [opcional]");
        if (number == CancelToken) return null;

        var complement = ReadOptionalText("Complemento [opcional]");
        if (complement == CancelToken) return null;

        var postal = ReadOptionalText("Codigo postal [opcional]");
        if (postal == CancelToken) return null;

        return new AddressDraft(
            roadType.Value.id,
            roadType.Value.name,
            roadName,
            string.IsNullOrWhiteSpace(number) ? null : number,
            string.IsNullOrWhiteSpace(complement) ? null : complement,
            city.Value.id,
            city.Value.name,
            string.IsNullOrWhiteSpace(postal) ? null : postal
        );
    }

    /// <summary>
    /// Captura los campos adicionales requeridos cuando la persona se registra como staff.
    /// </summary>
    private async Task<StaffDraft?> CollectStaffDraftAsync()
    {
        var staffRoles = (await _getAllStaffRoles.ExecuteAsync())
            .Select(x => (id: x.Id.Value, name: x.Name.Value))
            .OrderBy(x => x.id)
            .ToList();

        var staffRole = SelectById("CARGO STAFF", "Seleccione staff_role_id", staffRoles);
        if (staffRole is null) return null;

        var airlines = (await _getAllAirlines.ExecuteAsync())
            .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.IataCode.Value})"))
            .OrderBy(x => x.id)
            .ToList();

        var airports = (await _getAllAirports.ExecuteAsync())
            .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.IataCode.Value})"))
            .OrderBy(x => x.id)
            .ToList();

        int? airlineId;
        string? airlineName;
        int? airportId;
        string? airportName;

        while (true)
        {
            var selectedAirline = SelectOptionalById("AEROLINEA (OPCIONAL)", "Seleccione airline_id", airlines);
            if (selectedAirline.isCancelled) return null;
            airlineId = selectedAirline.id;
            airlineName = selectedAirline.name;

            var selectedAirport = SelectOptionalById("AEROPUERTO (OPCIONAL)", "Seleccione airport_id", airports);
            if (selectedAirport.isCancelled) return null;
            airportId = selectedAirport.id;
            airportName = selectedAirport.name;

            if (airlineId.HasValue || airportId.HasValue)
                break;

            PrintFieldError("Debes seleccionar aerolinea o aeropuerto (al menos uno).");
        }

        DateTime hireDate;
        while (true)
        {
            var raw = ReadRaw("Fecha ingreso staff (yyyy-MM-dd)");
            if (raw == CancelToken) return null;

            if (DateTime.TryParseExact(raw, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out hireDate))
                break;

            PrintFieldError("Formato invalido. Usa yyyy-MM-dd.");
        }

        var isActive = ReadYesNo("Staff activo? (S/N)");
        if (isActive is null) return null;

        return new StaffDraft(
            staffRole.Value.id,
            staffRole.Value.name,
            airlineId,
            airlineName,
            airportId,
            airportName,
            hireDate,
            isActive.Value
        );
    }

    /// <summary>
    /// Crea una dirección y retorna el id persistido para asociarlo a la persona.
    /// </summary>
    private async Task<int?> CreateAddressAndGetIdAsync(AddressDraft address)
    {
        var before = (await _getAllAddresses.ExecuteAsync()).Select(x => x.Id.Value).DefaultIfEmpty(0).Max();

        await _createAddress.ExecuteAsync(
            address.RoadTypeId,
            address.RoadName,
            address.Number,
            address.Complement,
            address.CityId,
            address.PostalCode);

        var afterList = (await _getAllAddresses.ExecuteAsync()).Select(x => x.Id.Value).ToList();
        var createdId = afterList.Where(x => x > before).DefaultIfEmpty(afterList.DefaultIfEmpty(0).Max()).Max();
        return createdId == 0 ? null : createdId;
    }

    /// <summary>
    /// Obtiene y muestra tipos de documento para selección controlada por id.
    /// </summary>
    private async Task<(int id, string name)?> SelectDocumentTypeAsync()
    {
        var items = (await _getAllDocumentTypes.ExecuteAsync())
            .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.Code.Value})"))
            .OrderBy(x => x.id)
            .ToList();
        return SelectById("TIPO DE DOCUMENTO", "Seleccione tipo_documento_id", items);
    }

    /// <summary>
    /// Obtiene y muestra dominios de correo para evitar entradas libres inconsistentes.
    /// </summary>
    private async Task<(int id, string name)?> SelectEmailDomainAsync()
    {
        var items = (await _getAllEmailDomains.ExecuteAsync())
            .Select(x => (id: x.Id.Value, name: x.Domain.Value))
            .OrderBy(x => x.id)
            .ToList();
        return SelectById("DOMINIO EMAIL", "Seleccione dominio_email_id", items);
    }

    /// <summary>
    /// Obtiene y muestra tipos de vía disponibles para la dirección.
    /// </summary>
    private async Task<(int id, string name)?> SelectRoadTypeAsync()
    {
        var items = (await _getAllRoadTypes.ExecuteAsync())
            .Select(x => (id: x.Id.Value, name: x.Name.Value))
            .OrderBy(x => x.id)
            .ToList();
        return SelectById("TIPO DE VIA", "Seleccione tipo_via_id", items);
    }

    /// <summary>
    /// Obtiene y muestra ciudades disponibles para asociar la dirección.
    /// </summary>
    private async Task<(int id, string name)?> SelectCityAsync()
    {
        var items = (await _getAllCities.ExecuteAsync())
            .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} (region:{x.RegionId.Value})"))
            .OrderBy(x => x.id)
            .ToList();
        return SelectById("CIUDAD", "Seleccione ciudad_id", items);
    }

    /// <summary>
    /// Obtiene y muestra roles del sistema; define el tipo de registro adicional a crear.
    /// </summary>
    private async Task<(int id, string name)?> SelectRoleAsync()
    {
        var items = (await _getAllSystemRoles.ExecuteAsync())
            .Select(x => (id: x.Id.Value, name: x.Name.Value))
            .OrderBy(x => x.id)
            .ToList();
        return SelectById("ROL DE USUARIO", "Seleccione rol_id", items);
    }

    /// <summary>
    /// Presenta una lista numerada y retorna la opción elegida.
    /// Repite la pregunta hasta recibir un id válido o cancelación.
    /// </summary>
    private static (int id, string name)? SelectById(string title, string prompt, List<(int id, string name)> items)
    {
        while (true)
        {
            PrintMenuBox(title, items.Select(x => $"[{x.id}] {x.name}").ToList());
            var raw = ReadRaw(prompt);
            if (raw == CancelToken) return null;

            if (!int.TryParse(raw, out var id))
            {
                PrintFieldError("Debes ingresar un numero valido.");
                continue;
            }

            var found = items.FirstOrDefault(x => x.id == id);
            if (found == default)
            {
                PrintFieldError("El ID no existe en las opciones.");
                continue;
            }

            return found;
        }
    }

    private static (int? id, string? name, bool isCancelled) SelectOptionalById(string title, string prompt, List<(int id, string name)> items)
    {
        while (true)
        {
            var lines = new List<string> { "[0] Ninguno" };
            lines.AddRange(items.Select(x => $"[{x.id}] {x.name}"));
            PrintMenuBox(title, lines);

            var raw = ReadRaw($"{prompt} (0 opcional)");
            if (raw == CancelToken) return (null, null, true);

            if (!int.TryParse(raw, out var id))
            {
                PrintFieldError("Debes ingresar un numero valido.");
                continue;
            }

            if (id == 0)
                return (null, null, false);

            var found = items.FirstOrDefault(x => x.id == id);
            if (found == default)
            {
                PrintFieldError("El ID no existe en las opciones.");
                continue;
            }

            return (found.id, found.name, false);
        }
    }

    /// <summary>
    /// Muestra resumen final y captura decisión del usuario (confirmar, editar o cancelar).
    /// </summary>
    private static int ConfirmChoice(PersonDraft draft)
    {
        while (true)
        {
            Console.Clear();
            PrintHeader();
            PrintMenuBox("RESUMEN", new List<string>
            {
                $"Tipo documento: {draft.DocumentTypeName}",
                $"Numero documento: {draft.DocumentNumber}",
                $"Nombres: {draft.FirstNames}",
                $"Apellidos: {draft.LastNames}",
                $"Fecha nacimiento: {(draft.BirthDate?.ToString("yyyy-MM-dd") ?? "NULL")}",
                $"Genero: {draft.Gender ?? "NULL"}",
                $"Direccion nueva: {(draft.Address is null ? "NO" : $"{draft.Address.RoadTypeName} {draft.Address.RoadName} {draft.Address.Number}")}",
                $"Correo principal: {GetEmailDisplay(draft)}",
                $"Rol: {draft.RoleName}",
                $"Perfil negocio: {GetBusinessRoleDisplay(draft.BusinessRole)}",
                $"Detalle staff: {GetStaffDisplay(draft.Staff)}",
                $"Username: {draft.Username}"
            });

            PrintMenuBox("SELECCIONE UNA OPCION", new List<string>
            {
                "[1] Confirmar",
                "[2] Editar",
                "[3] Cancelar"
            });

            var raw = ReadRaw("Opcion");
            if (raw == CancelToken || raw == "3") return 3;
            if (raw == "2") return 2;
            if (raw == "1") return 1;

            PrintFieldError("Debes ingresar 1, 2 o 3.");
        }
    }

    private static string GetEmailDisplay(PersonDraft draft)
    {
        if (string.IsNullOrWhiteSpace(draft.EmailUser) || !draft.EmailDomainId.HasValue || string.IsNullOrWhiteSpace(draft.EmailDomainName))
            return "NULL";

        return $"{draft.EmailUser}@{draft.EmailDomainName}";
    }

    private static string GetBusinessRoleDisplay(RoleKind kind)
    {
        return kind switch
        {
            RoleKind.Customer => "Customer",
            RoleKind.Staff => "Staff",
            _ => "Ninguno"
        };
    }

    private static string GetStaffDisplay(StaffDraft? staff)
    {
        if (staff is null)
            return "N/A";

        return $"{staff.StaffRoleName} | airline={(staff.AirlineName ?? "NULL")} | airport={(staff.AirportName ?? "NULL")} | ingreso={staff.HireDate:yyyy-MM-dd} | activo={(staff.IsActive ? "SI" : "NO")}";
    }

    /// <summary>
    /// Traduce el nombre del rol a una categoría de negocio (admin/customer/staff/other).
    /// </summary>
    private static RoleKind ResolveRoleKind(string roleName)
    {
        var normalized = roleName.Trim().ToUpperInvariant();
        if (normalized.Contains("CLIENTE") || normalized.Contains("CUSTOMER"))
            return RoleKind.Customer;

        if (normalized.Contains("AGENTE") || normalized.Contains("STAFF"))
            return RoleKind.Staff;

        return RoleKind.None;
    }

    /// <summary>
    /// Lee un texto obligatorio con soporte de token global de cancelación.
    /// </summary>
    private static string? ReadRequiredText(string label)
    {
        while (true)
        {
            var value = ReadRaw(label);
            if (value == CancelToken) return null;
            if (string.IsNullOrWhiteSpace(value))
            {
                PrintFieldError("Este campo es obligatorio.");
                continue;
            }

            return value.Trim();
        }
    }

    private static string ReadOptionalText(string label)
    {
        return ReadRaw(label);
    }

    /// <summary>
    /// Lee contraseña oculta con validación de longitud mínima para proteger calidad de datos.
    /// </summary>
    private static string? ReadRequiredPassword(string label)
    {
        while (true)
        {
            var first = ReadHiddenRequired(label);
            if (first is null) return null;
            if (first.Length < 8)
            {
                PrintFieldError("La contrasena debe tener minimo 8 caracteres.");
                continue;
            }

            var confirm = ReadHiddenRequired("Confirmar contrasena");
            if (confirm is null) return null;
            if (first != confirm)
            {
                PrintFieldError("Las contrasenas no coinciden.");
                continue;
            }

            return first;
        }
    }

    private static string? ReadHiddenRequired(string label)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\n{label}: ");
        Console.ResetColor();

        var buffer = new List<char>();
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                var text = new string(buffer.ToArray()).Trim();
                if (text == CancelToken)
                    return null;

                if (string.IsNullOrWhiteSpace(text))
                {
                    PrintFieldError("Este campo es obligatorio.");
                    return string.Empty;
                }

                return text;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (buffer.Count == 0) continue;
                buffer.RemoveAt(buffer.Count - 1);
                Console.Write("\b \b");
                continue;
            }

            if (!char.IsControl(key.KeyChar))
            {
                buffer.Add(key.KeyChar);
                Console.Write("*");
            }
        }
    }

    /// <summary>
    /// Lee una fecha opcional en formato fijo yyyy-MM-dd.
    /// </summary>
    private static DateTime? ReadOptionalDate(string label)
    {
        while (true)
        {
            var raw = ReadRaw(label);
            if (raw == CancelToken) return CancelledDate;
            if (string.IsNullOrWhiteSpace(raw)) return null;

            if (DateTime.TryParseExact(raw, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return date;

            PrintFieldError("Formato invalido. Usa yyyy-MM-dd.");
        }
    }

    private static string? ReadOptionalGender(string label)
    {
        while (true)
        {
            var raw = ReadRaw(label);
            if (raw == CancelToken) return CancelToken;
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

            var normalized = raw.Trim().ToUpperInvariant();
            if (normalized is "M" or "F" or "N")
                return normalized;

            PrintFieldError("Genero invalido. Usa M, F o N.");
        }
    }

    /// <summary>
    /// Lee una respuesta booleana controlada (S/N) manteniendo cancelación disponible.
    /// </summary>
    private static bool? ReadYesNo(string label)
    {
        while (true)
        {
            var raw = ReadRaw(label);
            if (raw == CancelToken) return null;
            var normalized = raw.Trim().ToUpperInvariant();
            if (normalized is "S" or "SI" or "Y" or "YES") return true;
            if (normalized is "N" or "NO") return false;
            PrintFieldError("Debes ingresar S o N.");
        }
    }

    private static string ReadRaw(string label)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"\n{label}: ");
        Console.ResetColor();
        return (Console.ReadLine() ?? string.Empty).Trim();
    }

    /// <summary>
    /// Imprime cabecera visual del flujo de creación para contexto del operador.
    /// </summary>
    private static void PrintHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine("          CREACION DE PERSONA           ");
        Console.WriteLine("========================================");
        Console.ResetColor();
    }

    private static void PrintMenuBox(string title, IReadOnlyList<string> lines)
    {
        var width = Math.Max(36, lines.DefaultIfEmpty(string.Empty).Max(x => x.Length) + 4);
        var horizontal = new string('═', width);

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"\n╔{horizontal}╗");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"║ {title.PadRight(width - 1)}║");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"╠{horizontal}╣");
        Console.ForegroundColor = ConsoleColor.Gray;
        foreach (var line in lines)
            Console.WriteLine($"║ {line.PadRight(width - 1)}║");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"╚{horizontal}╝");
        Console.ResetColor();
    }

    private static void PrintFieldError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ {message}");
        Console.ResetColor();
    }

    private sealed record AddressDraft(
        int RoadTypeId,
        string RoadTypeName,
        string RoadName,
        string? Number,
        string? Complement,
        int CityId,
        string CityName,
        string? PostalCode
    );

    private sealed record StaffDraft(
        int StaffRoleId,
        string StaffRoleName,
        int? AirlineId,
        string? AirlineName,
        int? AirportId,
        string? AirportName,
        DateTime HireDate,
        bool IsActive
    );

    private sealed record PersonDraft(
        int DocumentTypeId,
        string DocumentTypeName,
        string DocumentNumber,
        string FirstNames,
        string LastNames,
        DateTime? BirthDate,
        string? Gender,
        AddressDraft? Address,
        string? EmailUser,
        int? EmailDomainId,
        string? EmailDomainName,
        int RoleId,
        string RoleName,
        RoleKind BusinessRole,
        StaffDraft? Staff,
        string Username,
        string Password
    );

    private enum RoleKind
    {
        None,
        Customer,
        Staff
    }
}
