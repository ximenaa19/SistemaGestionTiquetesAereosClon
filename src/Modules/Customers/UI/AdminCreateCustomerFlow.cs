// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\UI\AdminCreateCustomerFlow.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.shared.Ui;

namespace GestionAerolineas.src.Modules.Customers.UI;

public sealed class AdminCreateCustomerFlow
{
    private readonly CreateCustomerUseCase _createCustomer;
    private readonly GetAllPeopleUseCase _getAllPeople;

    public AdminCreateCustomerFlow(
        CreateCustomerUseCase createCustomer,
        GetAllPeopleUseCase getAllPeople)
    {
        _createCustomer = createCustomer;
        _getAllPeople = getAllPeople;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.Clear();
            AdminFlowConsole.PrintHeader("CREACIÓN DE CUSTOMER");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Escribe {AdminFlowConsole.CancelToken} para cancelar.\n");
            Console.ResetColor();

            var people = (await _getAllPeople.ExecuteAsync())
                .Select(x => (id: x.Id.Value, name: $"{x.FirstNames.Value} {x.LastNames.Value} - doc={x.DocumentNumber.Value}"))
                .OrderBy(x => x.id).ToList();

            var selected = AdminFlowConsole.SelectById("PERSONAS", "Seleccione person_id", people);
            if (selected is null) return;

            var choice = AdminFlowConsole.ReadConfirmChoice(new List<string>
            {
                $"Persona: {selected.Value.name}"
            });
            if (choice == 2) continue;
            if (choice == 3) return;

            try
            {
                await _createCustomer.ExecuteAsync(selected.Value.id);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✔ Customer creado correctamente.");
                Console.ResetColor();
                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
                return;
            }
            catch (Exception ex)
            {
                AdminFlowConsole.PrintError(ex.GetBaseException().Message);
                Console.WriteLine("Presiona una tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}

