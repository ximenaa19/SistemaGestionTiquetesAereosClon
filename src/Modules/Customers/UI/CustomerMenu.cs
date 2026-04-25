// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Customers\UI\CustomerMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Application.UseCases;
using GestionAerolineas.src.Modules.Customers.Domain.Aggregate;
using GestionAerolineas.src.Modules.People.Application.UseCases;

namespace GestionAerolineas.src.Modules.Customers.UI;

public class CustomerMenu
{
    private readonly CreateCustomerUseCase _create;
    private readonly GetAllCustomersUseCase _getAll;
    private readonly GetCustomerByIdUseCase _getById;
    private readonly GetCustomerByPersonIdUseCase _getByPersonId;
    private readonly GetCustomerByPersonNameUseCase _getByPersonName;
    private readonly UpdateCustomerUseCase _update;
    private readonly DeleteCustomerUseCase _delete;

    private readonly GetAllPeopleUseCase _getAllPeople;

    public CustomerMenu(
        CreateCustomerUseCase create,
        GetAllCustomersUseCase getAll,
        GetCustomerByIdUseCase getById,
        GetCustomerByPersonIdUseCase getByPersonId,
        GetCustomerByPersonNameUseCase getByPersonName,
        UpdateCustomerUseCase update,
        DeleteCustomerUseCase delete,
        GetAllPeopleUseCase getAllPeople)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByPersonId = getByPersonId;
        _getByPersonName = getByPersonName;
        _update = update;
        _delete = delete;
        _getAllPeople = getAllPeople;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear customer",
            "Listar customers",
            "Get customer by ID",
            "Get customer by person ID",
            "Get customer by person name",
            "Actualizar customer",
            "Eliminar customer",
            "Salir"
        });

        while (true)
        {
            int option = menu.Show();

            try
            {
                switch (option)
                {
                    case 0:
                        await PrintPeopleAsync();

                        Console.Write("\nIngrese person_id: ");
                        int personId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(personId);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var peopleMap = await GetPersonDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, peopleMap));
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var byId = await _getById.ExecuteAsync(searchId);
                        if (byId is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var peopleMapById = await GetPersonDisplayMapAsync();
                        Console.WriteLine(Format(byId, peopleMapById));
                        break;

                    case 3:
                        await PrintPeopleAsync();

                        Console.Write("\nIngrese person_id: ");
                        int searchPersonId = int.Parse(Console.ReadLine()!);

                        var byPersonId = await _getByPersonId.ExecuteAsync(searchPersonId);
                        if (byPersonId is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var peopleMapByPerson = await GetPersonDisplayMapAsync();
                        Console.WriteLine(Format(byPersonId, peopleMapByPerson));
                        break;

                    case 4:
                        await PrintPeopleAsync();

                        Console.Write("\nIngrese el nombre completo de la persona: ");
                        string searchPersonName = Console.ReadLine()!;

                        var byPersonName = await _getByPersonName.ExecuteAsync(searchPersonName);
                        if (byPersonName is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var peopleMapByName = await GetPersonDisplayMapAsync();
                        Console.WriteLine(Format(byPersonName, peopleMapByName));
                        break;

                    case 5:
                        await PrintPeopleAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo person_id: ");
                        int newPersonId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newPersonId);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 6:
                        Console.Write("Ingrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 7:
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"âŒ Error: {ex.GetBaseException().Message}");
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private async Task PrintPeopleAsync()
    {
        Console.WriteLine("Personas disponibles:");
        var people = (await _getAllPeople.ExecuteAsync()).ToList();

        foreach (var person in people.Take(30))
            Console.WriteLine($"{person.Id.Value} - {person.FirstNames.Value} {person.LastNames.Value} - doc={person.DocumentNumber.Value}");

        if (people.Count > 30)
            Console.WriteLine("(Mostrando solo las primeras 30)");
    }

    private async Task<Dictionary<int, string>> GetPersonDisplayMapAsync()
    {
        var people = await _getAllPeople.ExecuteAsync();
        return people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
    }

    private static string Format(Customer item, Dictionary<int, string> peopleMap)
    {
        string personDisplay = GetDisplay(peopleMap, item.PersonId.Value);
        return $"{item.Id.Value} - person={personDisplay} - createdAt={item.CreatedAt.Value:yyyy-MM-dd HH:mm:ss}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}

