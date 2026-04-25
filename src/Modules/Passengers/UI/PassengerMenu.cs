// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\UI\PassengerMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Domain.Aggregate;
using GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;

namespace GestionAerolineas.src.Modules.Passengers.UI;

public class PassengerMenu
{
    private readonly CreatePassengerUseCase _create;
    private readonly GetAllPassengersUseCase _getAll;
    private readonly GetPassengerByIdUseCase _getById;
    private readonly GetPassengerByPersonIdUseCase _getByPersonId;
    private readonly GetPassengerByPersonNameUseCase _getByPersonName;
    private readonly UpdatePassengerUseCase _update;
    private readonly DeletePassengerUseCase _delete;

    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllPassengerTypesUseCase _getAllPassengerTypes;

    public PassengerMenu(
        CreatePassengerUseCase create,
        GetAllPassengersUseCase getAll,
        GetPassengerByIdUseCase getById,
        GetPassengerByPersonIdUseCase getByPersonId,
        GetPassengerByPersonNameUseCase getByPersonName,
        UpdatePassengerUseCase update,
        DeletePassengerUseCase delete,
        GetAllPeopleUseCase getAllPeople,
        GetAllPassengerTypesUseCase getAllPassengerTypes)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByPersonId = getByPersonId;
        _getByPersonName = getByPersonName;
        _update = update;
        _delete = delete;
        _getAllPeople = getAllPeople;
        _getAllPassengerTypes = getAllPassengerTypes;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear passenger",
            "Listar passengers",
            "Get passenger by ID",
            "Get passenger by person ID",
            "Get passenger by person name",
            "Actualizar passenger",
            "Eliminar passenger",
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
                        await PrintPassengerTypesAsync();

                        Console.Write("\nIngrese person_id: ");
                        int personId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese passenger_type_id: ");
                        int passengerTypeId = int.Parse(Console.ReadLine()!);

                        await _create.ExecuteAsync(personId, passengerTypeId);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var peopleMap = await GetPersonDisplayMapAsync();
                        var passengerTypeMap = await GetPassengerTypeDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, peopleMap, passengerTypeMap));
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
                        var passengerTypeMapById = await GetPassengerTypeDisplayMapAsync();
                        Console.WriteLine(Format(byId, peopleMapById, passengerTypeMapById));
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
                        var passengerTypeMapByPerson = await GetPassengerTypeDisplayMapAsync();
                        Console.WriteLine(Format(byPersonId, peopleMapByPerson, passengerTypeMapByPerson));
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
                        var passengerTypeMapByName = await GetPassengerTypeDisplayMapAsync();
                        Console.WriteLine(Format(byPersonName, peopleMapByName, passengerTypeMapByName));
                        break;

                    case 5:
                        await PrintPeopleAsync();
                        await PrintPassengerTypesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo person_id: ");
                        int newPersonId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo passenger_type_id: ");
                        int newPassengerTypeId = int.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newPersonId, newPassengerTypeId);
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

    private async Task PrintPassengerTypesAsync()
    {
        Console.WriteLine("\nTipos de pasajero disponibles:");
        var passengerTypes = await _getAllPassengerTypes.ExecuteAsync();

        foreach (var passengerType in passengerTypes)
            Console.WriteLine($"{passengerType.Id.Value} - {passengerType.Name.Value}");
    }

    private async Task<Dictionary<int, string>> GetPersonDisplayMapAsync()
    {
        var people = await _getAllPeople.ExecuteAsync();
        return people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
    }

    private async Task<Dictionary<int, string>> GetPassengerTypeDisplayMapAsync()
    {
        var passengerTypes = await _getAllPassengerTypes.ExecuteAsync();
        return passengerTypes.ToDictionary(pt => pt.Id.Value, pt => pt.Name.Value);
    }

    private static string Format(Passenger item, Dictionary<int, string> peopleMap, Dictionary<int, string> passengerTypeMap)
    {
        string personDisplay = GetDisplay(peopleMap, item.PersonId.Value);
        string passengerTypeDisplay = GetDisplay(passengerTypeMap, item.PassengerTypeId.Value);

        return $"{item.Id.Value} - person={personDisplay} - passengerType={passengerTypeDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}

