// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\UI\PersonEmailMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.PersonEmails.Application.UseCases;
using GestionAerolineas.src.Modules.PersonEmails.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.PersonEmails.UI;

public class PersonEmailMenu
{
    private readonly CreatePersonEmailUseCase _create;
    private readonly GetAllPersonEmailsUseCase _getAll;
    private readonly GetPersonEmailByIdUseCase _getById;
    private readonly GetPersonEmailByPersonAndEmailUseCase _getByAddress;
    private readonly UpdatePersonEmailUseCase _update;
    private readonly DeletePersonEmailUseCase _delete;

    private readonly GetAllPeopleUseCase _getAllPeople;
    private readonly GetAllEmailDomainsUseCase _getAllEmailDomains;

    public PersonEmailMenu(
        CreatePersonEmailUseCase create,
        GetAllPersonEmailsUseCase getAll,
        GetPersonEmailByIdUseCase getById,
        GetPersonEmailByPersonAndEmailUseCase getByAddress,
        UpdatePersonEmailUseCase update,
        DeletePersonEmailUseCase delete,
        GetAllPeopleUseCase getAllPeople,
        GetAllEmailDomainsUseCase getAllEmailDomains)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByAddress = getByAddress;
        _update = update;
        _delete = delete;
        _getAllPeople = getAllPeople;
        _getAllEmailDomains = getAllEmailDomains;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear person email",
            "Listar person emails",
            "Get person email by ID",
            "Get person email by person+email",
            "Actualizar person email",
            "Eliminar person email",
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
                        await PrintEmailDomainsAsync();

                        Console.Write("\nIngrese persona_id: ");
                        int personId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese usuario_email (sin @): ");
                        string user = Console.ReadLine()!;

                        Console.Write("Ingrese dominio_email_id: ");
                        int domainId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese es_principal (true/false) [default=false]: ");
                        var primaryInput = Console.ReadLine();
                        bool isPrimary = string.IsNullOrWhiteSpace(primaryInput) ? false : bool.Parse(primaryInput);

                        await _create.ExecuteAsync(personId, user, domainId, isPrimary);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var peopleMap = await GetPersonDisplayMapAsync();
                        var domainMap = await GetDomainDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, peopleMap, domainMap));
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
                        var domainMapById = await GetDomainDisplayMapAsync();
                        Console.WriteLine(Format(byId, peopleMapById, domainMapById));
                        break;

                    case 3:
                        await PrintPeopleAsync();
                        await PrintEmailDomainsAsync();

                        Console.Write("\nIngrese persona_id: ");
                        int searchPersonId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese usuario_email (sin @): ");
                        string searchUser = Console.ReadLine()!;

                        Console.Write("Ingrese dominio_email_id: ");
                        int searchDomainId = int.Parse(Console.ReadLine()!);

                        var byAddress = await _getByAddress.ExecuteAsync(searchPersonId, searchUser, searchDomainId);
                        if (byAddress is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var peopleMapByAddress = await GetPersonDisplayMapAsync();
                        var domainMapByAddress = await GetDomainDisplayMapAsync();
                        Console.WriteLine(Format(byAddress, peopleMapByAddress, domainMapByAddress));
                        break;

                    case 4:
                        await PrintPeopleAsync();
                        await PrintEmailDomainsAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese persona_id: ");
                        int newPersonId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese usuario_email (sin @): ");
                        string newUser = Console.ReadLine()!;

                        Console.Write("Ingrese dominio_email_id: ");
                        int newDomainId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese es_principal (true/false): ");
                        bool newIsPrimary = bool.Parse(Console.ReadLine()!);

                        await _update.ExecuteAsync(updateId, newPersonId, newUser, newDomainId, newIsPrimary);
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

    private async Task PrintEmailDomainsAsync()
    {
        Console.WriteLine("\nDominios disponibles:");
        var domains = (await _getAllEmailDomains.ExecuteAsync()).ToList();

        foreach (var domain in domains.Take(30))
            Console.WriteLine($"{domain.Id.Value} - {domain.Domain.Value}");

        if (domains.Count > 30)
            Console.WriteLine("(Mostrando solo los primeros 30)");
    }

    private async Task<Dictionary<int, string>> GetPersonDisplayMapAsync()
    {
        var people = await _getAllPeople.ExecuteAsync();
        return people.ToDictionary(p => p.Id.Value, p => $"{p.FirstNames.Value} {p.LastNames.Value}");
    }

    private async Task<Dictionary<int, string>> GetDomainDisplayMapAsync()
    {
        var domains = await _getAllEmailDomains.ExecuteAsync();
        return domains.ToDictionary(d => d.Id.Value, d => d.Domain.Value);
    }

    private static string Format(PersonEmail item, Dictionary<int, string> peopleMap, Dictionary<int, string> domainMap)
    {
        string personDisplay = GetDisplay(peopleMap, item.PersonId.Value);
        string domainDisplay = GetDisplay(domainMap, item.EmailDomainId.Value);
        var primaryDisplay = item.IsPrimary.Value ? "primary" : "secondary";

        return $"{item.Id.Value} - person={personDisplay} [{item.PersonId.Value}] - email={item.User.Value}@{domainDisplay} [{item.EmailDomainId.Value}] - {primaryDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? display : $"#{id}";
    }
}


