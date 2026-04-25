// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonPhones\UI\PersonPhoneMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.PersonPhones.Application.UseCases;
using GestionAerolineas.src.Modules.PersonPhones.Domain.Aggregate;
using GestionAerolineas.src.Modules.PhoneCodes.Application.UseCases;

namespace GestionAerolineas.src.Modules.PersonPhones.UI;

public class PersonPhoneMenu
{
    private readonly CreatePersonPhoneUseCase _create;
    private readonly GetAllPersonPhonesUseCase _getAll;
    private readonly GetPersonPhoneByIdUseCase _getById;
    private readonly GetPersonPhoneByPersonAndPhoneUseCase _getByPhone;
    private readonly UpdatePersonPhoneUseCase _update;
    private readonly DeletePersonPhoneUseCase _delete;

    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllPhoneCodesUseCase _getAllPhoneCodes;

    public PersonPhoneMenu(
        CreatePersonPhoneUseCase create,
        GetAllPersonPhonesUseCase getAll,
        GetPersonPhoneByIdUseCase getById,
        GetPersonPhoneByPersonAndPhoneUseCase getByPhone,
        UpdatePersonPhoneUseCase update,
        DeletePersonPhoneUseCase delete,
        GetAllPeopleUseCase getAllPeople,
        GetAllPhoneCodesUseCase getAllPhoneCodes)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByPhone = getByPhone;
        _update = update;
        _delete = delete;
        _getAllPeople = getAllPeople;
        _getAllPhoneCodes = getAllPhoneCodes;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear person phone",
            "Listar person phones",
            "Get person phone by ID",
            "Get person phone by person+phone",
            "Actualizar person phone",
            "Eliminar person phone",
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
                        await PrintPhoneCodesAsync();

                        Console.Write("\nIngrese persona_id: ");
                        int personId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese codigo_telefono_id: ");
                        int phoneCodeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese numero_telefono: ");
                        string phoneNumber = Console.ReadLine()!;

                        Console.Write("Ingrese es_principal (true/false) [default=false]: ");
                        var primaryInput = Console.ReadLine();
                        bool isPrimary = string.IsNullOrWhiteSpace(primaryInput) ? false : bool.Parse(primaryInput);

                        await _create.ExecuteAsync(personId, phoneCodeId, phoneNumber, isPrimary);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var peopleMap = await GetPersonDisplayMapAsync();
                        var phoneCodeMap = await GetPhoneCodeDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, peopleMap, phoneCodeMap));
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
                        var phoneCodeMapById = await GetPhoneCodeDisplayMapAsync();
                        Console.WriteLine(Format(byId, peopleMapById, phoneCodeMapById));
                        break;

                    case 3:
                        await PrintPeopleAsync();
                        await PrintPhoneCodesAsync();

                        Console.Write("\nIngrese persona_id: ");
                        int searchPersonId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese codigo_telefono_id: ");
                        int searchPhoneCodeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese numero_telefono: ");
                        string searchPhoneNumber = Console.ReadLine()!;

                        var byPhone = await _getByPhone.ExecuteAsync(searchPersonId, searchPhoneCodeId, searchPhoneNumber);
                        if (byPhone is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var peopleMapByPhone = await GetPersonDisplayMapAsync();
                        var phoneCodeMapByPhone = await GetPhoneCodeDisplayMapAsync();
                        Console.WriteLine(Format(byPhone, peopleMapByPhone, phoneCodeMapByPhone));
                        break;

                    case 4:
                        await PrintPeopleAsync();
                        await PrintPhoneCodesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese persona_id: ");
                        int newPersonId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese codigo_telefono_id: ");
                        int newPhoneCodeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese numero_telefono: ");
                        string newPhoneNumber = Console.ReadLine()!;

                        Console.Write("Ingrese es_principal (true/false): ");
                        bool newIsPrimary = bool.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newPersonId, newPhoneCodeId, newPhoneNumber, newIsPrimary);
                        Console.WriteLine("âœ” Actualizado");
                        break;

                    case 5:
                        Console.Write("Ingrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("âœ” Eliminado");
                        break;

                    case 6:
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
        Console.WriteLine("People disponibles:");
        var people = (await _getAllPeople.ExecuteAsync()).ToList();

        foreach (var person in people.Take(30))
            Console.WriteLine($"{person.Id.Value} - {person.FirstNames.Value} {person.LastNames.Value} - doc={person.DocumentNumber.Value}");

        if (people.Count > 30)
            Console.WriteLine("(Mostrando solo las primeras 30)");
    }

    private async Task PrintPhoneCodesAsync()
    {
        Console.WriteLine("\nCodigos de telefono disponibles:");
        var codes = (await _getAllPhoneCodes.ExecuteAsync()).ToList();

        foreach (var code in codes.Take(30))
            Console.WriteLine($"{code.Id.Value} - {code.CountryCode.Value} - {code.CountryName.Value}");

        if (codes.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task<Dictionary<int, string>> GetPersonDisplayMapAsync()
    {
        var people = await _getAllPeople.ExecuteAsync();
        return people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
    }

    private async Task<Dictionary<int, string>> GetPhoneCodeDisplayMapAsync()
    {
        var codes = await _getAllPhoneCodes.ExecuteAsync();
        return codes.ToDictionary(c => c.Id.Value, c => c.CountryCode.Value);
    }

    private static string Format(PersonPhone item, Dictionary<int, string> peopleMap, Dictionary<int, string> phoneCodeMap)
    {
        string personDisplay = GetDisplay(peopleMap, item.PersonId.Value);
        string codeDisplay = GetDisplay(phoneCodeMap, item.PhoneCodeId.Value);
        var primaryDisplay = item.IsPrimary.Value ? "primary" : "secondary";

        return $"{item.Id.Value} - person={personDisplay} [{item.PersonId.Value}] - phone={codeDisplay} {item.PhoneNumber.Value} [{item.PhoneCodeId.Value}] - {primaryDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }
}


