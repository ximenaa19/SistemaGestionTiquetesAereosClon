// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\UI\EmailDomainMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Application.UseCases;

namespace GestionAerolineas.src.Modules.EmailDomains.UI;

public class EmailDomainMenu
{
    private readonly CreateEmailDomainUseCase _create;
    private readonly GetAllEmailDomainsUseCase _getAll;
    private readonly GetEmailDomainByIdUseCase _getById;
    private readonly GetEmailDomainByDomainUseCase _getByDomain;
    private readonly UpdateEmailDomainUseCase _update;
    private readonly DeleteEmailDomainUseCase _delete;

    public EmailDomainMenu(
        CreateEmailDomainUseCase create,
        GetAllEmailDomainsUseCase getAll,
        GetEmailDomainByIdUseCase getById,
        GetEmailDomainByDomainUseCase getByDomain,
        UpdateEmailDomainUseCase update,
        DeleteEmailDomainUseCase delete)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByDomain = getByDomain;
        _update = update;
        _delete = delete;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear email domain",
            "Listar email domains",
            "Get email domain by ID",
            "Get email domain by domain",
            "Actualizar email domain",
            "Eliminar email domain",
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
                        Console.Write("Ingrese el dominio: ");
                        string domain = Console.ReadLine()!;

                        await _create.ExecuteAsync(domain);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var list = await _getAll.ExecuteAsync();

                        foreach (var item in list)
                            Console.WriteLine($"{item.Id.Value} - {item.Domain.Value}");
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var result = await _getById.ExecuteAsync(searchId);

                        Console.WriteLine(result == null
                            ? "No encontrado"
                            : $"{result.Id.Value} - {result.Domain.Value}");
                        break;

                    case 3:
                        Console.Write("Ingrese el dominio: ");
                        string searchDomain = Console.ReadLine()!;

                        var resultByDomain = await _getByDomain.ExecuteAsync(searchDomain);

                        Console.WriteLine(resultByDomain == null
                            ? "No encontrado"
                            : $"{resultByDomain.Id.Value} - {resultByDomain.Domain.Value}");
                        break;

                    case 4:
                        Console.Write("Ingrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese el nuevo dominio: ");
                        string newDomain = Console.ReadLine()!;

                        await _update.ExecuteAsync(updateId, newDomain);
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
}


