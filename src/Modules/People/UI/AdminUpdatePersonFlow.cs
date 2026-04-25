// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\UI\AdminUpdatePersonFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Application.UseCases;
using GestionAerolineas.src.Modules.DocumentTypes.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.People.UI;

/// <summary>
/// Flujo administrativo para actualizar personas existentes.
/// Lista opciones disponibles, valida id objetivo y reaplica captura de datos controlada.
/// </summary>
public sealed class AdminUpdatePersonFlow
{
    private readonly GetAllPeopleUseCase _getAll;
    private readonly GetPersonByIdUseCase _getById;
    private readonly UpdatePersonUseCase _update;
    private readonly GetAllDocumentTypesUseCase _getAllDocumentTypes;
    private readonly GetAllAddressesUseCase _getAllAddresses;

    public AdminUpdatePersonFlow(
        GetAllPeopleUseCase getAll,
        GetPersonByIdUseCase getById,
        UpdatePersonUseCase update,
        GetAllDocumentTypesUseCase getAllDocumentTypes,
        GetAllAddressesUseCase getAllAddresses)
    {
        _getAll = getAll;
        _getById = getById;
        _update = update;
        _getAllDocumentTypes = getAllDocumentTypes;
        _getAllAddresses = getAllAddresses;
    }

    /// <summary>
    /// Ejecuta el proceso de edición de persona de punta a punta.
    /// </summary>
    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("ACTUALIZACION DE PERSONA");
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            await PrintPeopleOptionsAsync();

            var personId = ReadRequiredInt("ID de persona");
            if (!personId.HasValue) return;

            var existing = await _getById.ExecuteAsync(personId.Value);
            if (existing is null)
            {
                AdminFlowConsole.PrintError("No existe una persona con ese ID.");
                Pause();
                continue;
            }

            var documentTypes = (await _getAllDocumentTypes.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.Name.Value} ({x.Code.Value})"))
                .OrderBy(x => x.id)
                .ToList();
            var documentType = AdminFlowConsole.SelectById("TIPO DE DOCUMENTO", "Seleccione tipo_documento_id", documentTypes);
            if (documentType is null) return;

            var addresses = (await _getAllAddresses.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.RoadTypeId.Value}-{x.RoadName.Value} #{x.Number.Value ?? "SN"}"))
                .OrderBy(x => x.id)
                .ToList();

            int? addressId;
            while (true)
            {
                var lines = new List<string> { "[0] Sin direccion" };
                lines.AddRange(addresses.Select(a => $"[{a.id}] {a.name}"));
                AdminFlowConsole.PrintMenuBox("DIRECCION (OPCIONAL)", lines);
                var raw = AdminFlowConsole.ReadRaw("Seleccione direccion_id");
                if (raw == AdminFlowConsole.CancelToken) return;
                if (!int.TryParse(raw, out var value))
                {
                    AdminFlowConsole.PrintError("Debes ingresar un numero valido.");
                    continue;
                }

                if (value == 0)
                {
                    addressId = null;
                    break;
                }

                if (addresses.Any(a => a.id == value))
                {
                    addressId = value;
                    break;
                }

                AdminFlowConsole.PrintError("El ID no existe en la lista.");
            }

            var documentNumber = AdminFlowConsole.ReadRequiredText("Numero de documento");
            if (documentNumber is null) return;
            var firstNames = AdminFlowConsole.ReadRequiredText("Nombres");
            if (firstNames is null) return;
            var lastNames = AdminFlowConsole.ReadRequiredText("Apellidos");
            if (lastNames is null) return;
            var birthDate = AdminFlowConsole.ReadOptionalDate("Fecha nacimiento (yyyy-MM-dd) [opcional]");
            if (birthDate == DateTime.MinValue) return;

            string? gender;
            while (true)
            {
                var raw = AdminFlowConsole.ReadOptionalText("Genero (M/F/N) [opcional]");
                if (raw == AdminFlowConsole.CancelToken) return;
                if (string.IsNullOrWhiteSpace(raw))
                {
                    gender = null;
                    break;
                }

                var normalized = raw.Trim().ToUpperInvariant();
                if (normalized is "M" or "F" or "N")
                {
                    gender = normalized;
                    break;
                }

                AdminFlowConsole.PrintError("Genero invalido. Usa M, F o N.");
            }

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Persona ID: {personId.Value}",
                $"Tipo documento: {documentType.Value.name}",
                $"Numero documento: {documentNumber}",
                $"Nombres: {firstNames}",
                $"Apellidos: {lastNames}",
                $"Fecha nacimiento: {(birthDate?.ToString("yyyy-MM-dd") ?? "NULL")}",
                $"Genero: {gender ?? "NULL"}",
                $"Direccion ID: {(addressId?.ToString() ?? "NULL")}"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _update.ExecuteAsync(
                    personId.Value,
                    documentType.Value.id,
                    documentNumber,
                    firstNames,
                    lastNames,
                    birthDate,
                    gender,
                    addressId);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Persona actualizada correctamente.");
                Console.ResetColor();
                Pause();
                return;
            }
            catch (Exception ex)
            {
                AdminFlowConsole.PrintError(ex.GetBaseException().Message);
                Pause();
            }
        }
    }

    /// <summary>
    /// Lee un entero obligatorio con soporte de cancelación global.
    /// </summary>
    private static int? ReadRequiredInt(string label)
    {
        while (true)
        {
            var raw = AdminFlowConsole.ReadRaw(label);
            if (raw == AdminFlowConsole.CancelToken) return null;
            if (int.TryParse(raw, out var value)) return value;
            AdminFlowConsole.PrintError("Debes ingresar un numero valido.");
        }
    }

    /// <summary>
    /// Pausa estándar para permitir revisar mensajes antes de volver al flujo.
    /// </summary>
    private static void Pause()
    {
        Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();
    }

    /// <summary>
    /// Imprime la lista de personas para que el operador seleccione cuál editar.
    /// </summary>
    private async Task PrintPeopleOptionsAsync()
    {
        var people = (await _getAll.ExecuteAsync())
            .Select(x => $"[{x.Id.Value}] {x.FirstNames.Value} {x.LastNames.Value} - doc={x.DocumentNumber.Value}")
            .ToList();

        if (people.Count == 0)
        {
            AdminFlowConsole.PrintError("No hay personas para actualizar.");
            return;
        }

        var lines = people.Take(30).ToList();
        if (people.Count > 30)
            lines.Add($"... y {people.Count - 30} mas");

        AdminFlowConsole.PrintMenuBox("PERSONAS DISPONIBLES", lines);
    }
}
