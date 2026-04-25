// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\UI\AdminDeletePersonFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.People.UI;

/// <summary>
/// Flujo administrativo para eliminación controlada de personas.
/// Presenta opciones, valida existencia y solicita confirmación explícita.
/// </summary>
public sealed class AdminDeletePersonFlow
{
    private readonly GetAllPeopleUseCase _getAll;
    private readonly GetPersonByIdUseCase _getById;
    private readonly DeletePersonUseCase _delete;

    public AdminDeletePersonFlow(
        GetAllPeopleUseCase getAll,
        GetPersonByIdUseCase getById,
        DeletePersonUseCase delete)
    {
        _getAll = getAll;
        _getById = getById;
        _delete = delete;
    }

    /// <summary>
    /// Ejecuta el ciclo de borrado desde consola con confirmación previa.
    /// </summary>
    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("ELIMINACION DE PERSONA");
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");

            var hasData = await PrintPeopleOptionsAsync();
            if (!hasData)
            {
                Pause();
                return;
            }

            var id = ReadRequiredInt("ID de persona a eliminar");
            if (!id.HasValue) return;

            var existing = await _getById.ExecuteAsync(id.Value);
            if (existing is null)
            {
                AdminFlowConsole.PrintError("No existe una persona con ese ID.");
                Pause();
                continue;
            }

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Persona ID: {existing.Id.Value}",
                $"Documento: {existing.DocumentNumber.Value}",
                $"Nombre: {existing.FirstNames.Value} {existing.LastNames.Value}",
                "Accion: ELIMINAR registro"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _delete.ExecuteAsync(id.Value);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Persona eliminada correctamente.");
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
    /// Muestra personas existentes para facilitar la selección de id a eliminar.
    /// </summary>
    private async Task<bool> PrintPeopleOptionsAsync()
    {
        var people = (await _getAll.ExecuteAsync())
            .Select(x => $"[{x.Id.Value}] {x.FirstNames.Value} {x.LastNames.Value} - doc={x.DocumentNumber.Value}")
            .ToList();

        if (people.Count == 0)
        {
            AdminFlowConsole.PrintError("No hay personas para eliminar.");
            return false;
        }

        var lines = people.Take(30).ToList();
        if (people.Count > 30)
            lines.Add($"... y {people.Count - 30} mas");

        AdminFlowConsole.PrintMenuBox("PERSONAS DISPONIBLES", lines);
        return true;
    }

    /// <summary>
    /// Lee un entero obligatorio con posibilidad de cancelar.
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
    /// Pausa estándar después de cada resultado del flujo.
    /// </summary>
    private static void Pause()
    {
        Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();
    }
}
