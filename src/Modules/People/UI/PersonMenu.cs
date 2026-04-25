// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\UI\PersonMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Application.UseCases;
using GestionAerolineas.src.Modules.DocumentTypes.Application.UseCases;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.People.UI;

public class PersonMenu
{
    private readonly CreatePersonUseCase _create;
    private readonly GetAllPeopleUseCase _getAll;
    private readonly GetPersonByIdUseCase _getById;
    private readonly GetPersonByDocumentUseCase _getByDocument;
    private readonly UpdatePersonUseCase _update;
    private readonly DeletePersonUseCase _delete;

    private readonly GetAllDocumentTypesUseCase _getAllDocumentTypes;
    private readonly GetAllAddressesUseCase _getAllAddresses;

    public PersonMenu(
        CreatePersonUseCase create,
        GetAllPeopleUseCase getAll,
        GetPersonByIdUseCase getById,
        GetPersonByDocumentUseCase getByDocument,
        UpdatePersonUseCase update,
        DeletePersonUseCase delete,
        GetAllDocumentTypesUseCase getAllDocumentTypes,
        GetAllAddressesUseCase getAllAddresses)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByDocument = getByDocument;
        _update = update;
        _delete = delete;
        _getAllDocumentTypes = getAllDocumentTypes;
        _getAllAddresses = getAllAddresses;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear person",
            "Listar people",
            "Get person by ID",
            "Get person by document",
            "Actualizar person",
            "Eliminar person",
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
                        await PrintDocumentTypesAsync();
                        await PrintAddressesAsync();

                        Console.Write("\nIngrese tipo_documento_id: ");
                        int documentTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese numero_documento: ");
                        string documentNumber = Console.ReadLine()!;

                        Console.Write("Ingrese nombres: ");
                        string firstNames = Console.ReadLine()!;

                        Console.Write("Ingrese apellidos: ");
                        string lastNames = Console.ReadLine()!;

                        Console.Write("Ingrese fecha_nacimiento (yyyy-MM-dd) [opcional]: ");
                        var birthDateInput = Console.ReadLine();
                        DateTime? birthDate = string.IsNullOrWhiteSpace(birthDateInput)
                            ? null
                            : DateTime.Parse(birthDateInput!);

                        Console.Write("Ingrese genero (M/F/N) [opcional]: ");
                        var gender = Console.ReadLine();

                        Console.Write("Ingrese direccion_id [opcional]: ");
                        var addressInput = Console.ReadLine();
                        int? addressId = string.IsNullOrWhiteSpace(addressInput) ? null : int.Parse(addressInput!);

                        await _create.ExecuteAsync(
                            documentTypeId,
                            documentNumber,
                            firstNames,
                            lastNames,
                            birthDate,
                            gender,
                            addressId
                        );
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var documentTypeMap = await GetDocumentTypeDisplayMapAsync();
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine(Format(item, documentTypeMap));
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

                        var docTypeMapById = await GetDocumentTypeDisplayMapAsync();
                        Console.WriteLine(Format(byId, docTypeMapById));
                        break;

                    case 3:
                        await PrintDocumentTypesAsync();

                        Console.Write("\nIngrese tipo_documento_id: ");
                        int searchDocTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese numero_documento: ");
                        string searchDocNumber = Console.ReadLine()!;

                        var byDocument = await _getByDocument.ExecuteAsync(searchDocTypeId, searchDocNumber);
                        if (byDocument is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var docTypeMapByDocument = await GetDocumentTypeDisplayMapAsync();
                        Console.WriteLine(Format(byDocument, docTypeMapByDocument));
                        break;

                    case 4:
                        await PrintDocumentTypesAsync();
                        await PrintAddressesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese tipo_documento_id: ");
                        int newDocumentTypeId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese numero_documento: ");
                        string newDocumentNumber = Console.ReadLine()!;

                        Console.Write("Ingrese nombres: ");
                        string newFirstNames = Console.ReadLine()!;

                        Console.Write("Ingrese apellidos: ");
                        string newLastNames = Console.ReadLine()!;

                        Console.Write("Ingrese fecha_nacimiento (yyyy-MM-dd) [opcional]: ");
                        var newBirthDateInput = Console.ReadLine();
                        DateTime? newBirthDate = string.IsNullOrWhiteSpace(newBirthDateInput)
                            ? null
                            : DateTime.Parse(newBirthDateInput!);

                        Console.Write("Ingrese genero (M/F/N) [opcional]: ");
                        var newGender = Console.ReadLine();

                        Console.Write("Ingrese direccion_id [opcional]: ");
                        var newAddressInput = Console.ReadLine();
                        int? newAddressId = string.IsNullOrWhiteSpace(newAddressInput) ? null : int.Parse(newAddressInput!);

                        await _update.ExecuteAsync(
                            updateId,
                            newDocumentTypeId,
                            newDocumentNumber,
                            newFirstNames,
                            newLastNames,
                            newBirthDate,
                            newGender,
                            newAddressId
                        );
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

    private async Task PrintDocumentTypesAsync()
    {
        Console.WriteLine("Tipos de documento disponibles:");
        var types = await _getAllDocumentTypes.ExecuteAsync();
        foreach (var type in types)
            Console.WriteLine($"{type.Id.Value} - {type.Name.Value} - code={type.Code.Value}");
    }

    private async Task PrintAddressesAsync()
    {
        Console.WriteLine("\nDirecciones disponibles (puede ser NULL):");
        var addresses = (await _getAllAddresses.ExecuteAsync()).ToList();

        foreach (var address in addresses.Take(20))
            Console.WriteLine($"{address.Id.Value} - city_id={address.CityId.Value} - {address.RoadName.Value} {address.Number.Value}");

        if (addresses.Count > 20)
            Console.WriteLine("(Mostrando solo las primeras 20)");
    }

    private async Task<Dictionary<int, string>> GetDocumentTypeDisplayMapAsync()
    {
        var types = await _getAllDocumentTypes.ExecuteAsync();
        return types.ToDictionary(t => t.Id.Value, t => t.Code.Value);
    }

    private static string Format(Person item, Dictionary<int, string> documentTypeMap)
    {
        string docTypeDisplay = GetDisplay(documentTypeMap, item.DocumentTypeId.Value);
        var birthDateDisplay = item.BirthDate.Value?.ToString("yyyy-MM-dd") ?? "NULL";
        var genderDisplay = item.Gender.Value ?? "NULL";
        var addressDisplay = item.AddressId.Value?.ToString() ?? "NULL";

        return $"{item.Id.Value} - docType={docTypeDisplay} - docNumber={item.DocumentNumber.Value} - {item.FirstNames.Value} {item.LastNames.Value} - birthDate={birthDateDisplay} - gender={genderDisplay} - address_id={addressDisplay}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }
}

