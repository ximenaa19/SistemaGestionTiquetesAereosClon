// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PhoneCodes\UI\PhoneCodeMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using System.Threading.Tasks;
using GestionAerolineas.src.Modules.PhoneCodes.Application.UseCases;

namespace GestionAerolineas.src.Modules.PhoneCodes.UI;

public class PhoneCodeMenu
{
    private readonly CreatePhoneCodeUseCase _create;
    private readonly GetAllPhoneCodesUseCase _getAll;
    private readonly GetPhoneCodeByIdUseCase _getById;
    private readonly GetPhoneCodeByCountryNameUseCase _getByCountry;
    private readonly UpdatePhoneCodeUseCase _update;
    private readonly DeletePhoneCodeUseCase _delete;

    public PhoneCodeMenu(
        CreatePhoneCodeUseCase create,
        GetAllPhoneCodesUseCase getAll,
        GetPhoneCodeByIdUseCase getById,
        GetPhoneCodeByCountryNameUseCase getByCountry,
        UpdatePhoneCodeUseCase update,
        DeletePhoneCodeUseCase delete)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByCountry = getByCountry;
        _update = update;
        _delete = delete;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear phone code",
            "Listar phone codes",
            "Get phone code by ID",
            "Get phone code by country",
            "Actualizar phone code",
            "Eliminar phone code",
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
                        Console.Write("Ingrese el codigo pais (Ej: +57): ");
                        string code = Console.ReadLine()!;

                        Console.Write("Ingrese el nombre del pais: ");
                        string name = Console.ReadLine()!;

                        await _create.ExecuteAsync(code, name);
                        Console.WriteLine("Creado");
                        break;

                    case 1:
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine($"{item.Id.Value} - {item.CountryCode.Value} - {item.CountryName.Value}");
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var result = await _getById.ExecuteAsync(searchId);

                        Console.WriteLine(result == null
                            ? "No encontrado"
                            : $"{result.Id.Value} - {result.CountryCode.Value} - {result.CountryName.Value}");
                        break;

                    case 3:
                        Console.Write("Ingrese el pais: ");
                        string searchCountry = Console.ReadLine()!;

                        var resultByCountry = await _getByCountry.ExecuteAsync(searchCountry);

                        Console.WriteLine(resultByCountry == null
                            ? "No encontrado"
                            : $"{resultByCountry.Id.Value} - {resultByCountry.CountryCode.Value} - {resultByCountry.CountryName.Value}");
                        break;

                    case 4:
                        Console.Write("Ingrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo codigo pais (Ej: +57): ");
                        string newCode = Console.ReadLine()!;

                        Console.Write("Ingrese el nuevo nombre del pais: ");
                        string newName = Console.ReadLine()!;

                        await _update.ExecuteAsync(updateId, newCode, newName);
                        Console.WriteLine("Actualizado");
                        break;

                    case 5:
                        Console.Write("Ingrese el ID: ");
                        int deleteId = int.Parse(Console.ReadLine()!);

                        await _delete.ExecuteAsync(deleteId);
                        Console.WriteLine("Eliminado");
                        break;

                    case 6:
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetBaseException().Message}");
            }

            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}


