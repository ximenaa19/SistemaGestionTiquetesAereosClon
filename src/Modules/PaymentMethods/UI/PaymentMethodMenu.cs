// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\UI\PaymentMethodMenu.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Application.UseCases;
using GestionAerolineas.src.Modules.CardTypes.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Application.UseCases;

namespace GestionAerolineas.src.Modules.PaymentMethods.UI;

public class PaymentMethodMenu
{
    private readonly CreatePaymentMethodUseCase _create;
    private readonly GetAllPaymentMethodsUseCase _getAll;
    private readonly GetPaymentMethodByIdUseCase _getById;
    private readonly GetPaymentMethodByCommercialNameUseCase _getByCommercialName;
    private readonly UpdatePaymentMethodUseCase _update;
    private readonly DeletePaymentMethodUseCase _delete;

    private readonly GetAllPaymentMethodTypesUseCase _getAllPaymentMethodTypes;
    private readonly GetAllCardTypesUseCase _getAllCardTypes;
    private readonly GetAllCardIssuersUseCase _getAllCardIssuers;

    public PaymentMethodMenu(
        CreatePaymentMethodUseCase create,
        GetAllPaymentMethodsUseCase getAll,
        GetPaymentMethodByIdUseCase getById,
        GetPaymentMethodByCommercialNameUseCase getByCommercialName,
        UpdatePaymentMethodUseCase update,
        DeletePaymentMethodUseCase delete,
        GetAllPaymentMethodTypesUseCase getAllPaymentMethodTypes,
        GetAllCardTypesUseCase getAllCardTypes,
        GetAllCardIssuersUseCase getAllCardIssuers)
    {
        _create = create;
        _getAll = getAll;
        _getById = getById;
        _getByCommercialName = getByCommercialName;
        _update = update;
        _delete = delete;
        _getAllPaymentMethodTypes = getAllPaymentMethodTypes;
        _getAllCardTypes = getAllCardTypes;
        _getAllCardIssuers = getAllCardIssuers;
    }

    public async Task StartAsync()
    {
        var menu = new ConsoleMenu(new[]
        {
            "Crear payment method",
            "Listar payment methods",
            "Get payment method by ID",
            "Get payment method by commercial name",
            "Actualizar payment method",
            "Eliminar payment method",
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
                        await PrintDependenciesAsync();

                        Console.Write("\nIngrese tipo_medio_pago_id: ");
                        int paymentMethodTypeId = int.Parse(Console.ReadLine()!);

                        int? cardTypeId = ReadNullableInt("Ingrese tipo_tarjeta_id (opcional): ");
                        int? cardIssuerId = ReadNullableInt("Ingrese emisor_tarjeta_id (opcional): ");

                        Console.Write("Ingrese nombre_comercial: ");
                        string commercialName = Console.ReadLine()!;

                        await _create.ExecuteAsync(paymentMethodTypeId, cardTypeId, cardIssuerId, commercialName);
                        Console.WriteLine("âœ” Creado");
                        break;

                    case 1:
                        var methodTypeMap = await GetPaymentMethodTypeDisplayMapAsync();
                        var cardTypeMap = await GetCardTypeDisplayMapAsync();
                        var issuerMap = await GetCardIssuerDisplayMapAsync();

                        var list = await _getAll.ExecuteAsync();
                        foreach (var item in list)
                            Console.WriteLine(Format(item.Id.Value, item.PaymentMethodTypeId.Value, item.CardTypeId?.Value, item.CardIssuerId?.Value, item.CommercialName.Value, methodTypeMap, cardTypeMap, issuerMap));
                        break;

                    case 2:
                        Console.Write("Ingrese el ID: ");
                        int searchId = int.Parse(Console.ReadLine()!);

                        var result = await _getById.ExecuteAsync(searchId);
                        if (result is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var methodTypeMapById = await GetPaymentMethodTypeDisplayMapAsync();
                        var cardTypeMapById = await GetCardTypeDisplayMapAsync();
                        var issuerMapById = await GetCardIssuerDisplayMapAsync();

                        Console.WriteLine(Format(result.Id.Value, result.PaymentMethodTypeId.Value, result.CardTypeId?.Value, result.CardIssuerId?.Value, result.CommercialName.Value, methodTypeMapById, cardTypeMapById, issuerMapById));
                        break;

                    case 3:
                        Console.Write("Ingrese nombre_comercial: ");
                        string searchName = Console.ReadLine()!;

                        var resultByName = await _getByCommercialName.ExecuteAsync(searchName);
                        if (resultByName is null)
                        {
                            Console.WriteLine("No encontrado");
                            break;
                        }

                        var methodTypeMapByName = await GetPaymentMethodTypeDisplayMapAsync();
                        var cardTypeMapByName = await GetCardTypeDisplayMapAsync();
                        var issuerMapByName = await GetCardIssuerDisplayMapAsync();

                        Console.WriteLine(Format(resultByName.Id.Value, resultByName.PaymentMethodTypeId.Value, resultByName.CardTypeId?.Value, resultByName.CardIssuerId?.Value, resultByName.CommercialName.Value, methodTypeMapByName, cardTypeMapByName, issuerMapByName));
                        break;

                    case 4:
                        await PrintDependenciesAsync();

                        Console.Write("\nIngrese el ID: ");
                        int updateId = int.Parse(Console.ReadLine()!);

                        Console.Write("Ingrese nuevo tipo_medio_pago_id: ");
                        int newPaymentMethodTypeId = int.Parse(Console.ReadLine()!);

                        int? newCardTypeId = ReadNullableInt("Ingrese nuevo tipo_tarjeta_id (opcional): ");
                        int? newCardIssuerId = ReadNullableInt("Ingrese nuevo emisor_tarjeta_id (opcional): ");

                        Console.Write("Ingrese nuevo nombre_comercial: ");
                        string newCommercialName = Console.ReadLine()!;

                        await _update.ExecuteAsync(updateId, newPaymentMethodTypeId, newCardTypeId, newCardIssuerId, newCommercialName);
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

    private async Task PrintDependenciesAsync()
    {
        Console.WriteLine("PaymentMethodTypes disponibles:");
        var methodTypes = await _getAllPaymentMethodTypes.ExecuteAsync();
        foreach (var item in methodTypes)
            Console.WriteLine($"{item.Id.Value} - {item.Name.Value}");

        Console.WriteLine("\nCardTypes disponibles:");
        var cardTypes = await _getAllCardTypes.ExecuteAsync();
        foreach (var item in cardTypes)
            Console.WriteLine($"{item.Id.Value} - {item.Name.Value}");

        Console.WriteLine("\nCardIssuers disponibles:");
        var issuers = await _getAllCardIssuers.ExecuteAsync();
        foreach (var item in issuers)
            Console.WriteLine($"{item.Id.Value} - {item.Name.Value}");
    }

    private async Task<Dictionary<int, string>> GetPaymentMethodTypeDisplayMapAsync()
    {
        var items = await _getAllPaymentMethodTypes.ExecuteAsync();
        return items.ToDictionary(x => x.Id.Value, x => x.Name.Value);
    }

    private async Task<Dictionary<int, string>> GetCardTypeDisplayMapAsync()
    {
        var items = await _getAllCardTypes.ExecuteAsync();
        return items.ToDictionary(x => x.Id.Value, x => x.Name.Value);
    }

    private async Task<Dictionary<int, string>> GetCardIssuerDisplayMapAsync()
    {
        var items = await _getAllCardIssuers.ExecuteAsync();
        return items.ToDictionary(x => x.Id.Value, x => x.Name.Value);
    }

    private static string Format(
        int id,
        int paymentMethodTypeId,
        int? cardTypeId,
        int? cardIssuerId,
        string commercialName,
        Dictionary<int, string> methodTypeMap,
        Dictionary<int, string> cardTypeMap,
        Dictionary<int, string> issuerMap)
    {
        var methodTypeDisplay = GetDisplay(methodTypeMap, paymentMethodTypeId);
        var cardTypeDisplay = cardTypeId is null ? "null" : GetDisplay(cardTypeMap, cardTypeId.Value);
        var issuerDisplay = cardIssuerId is null ? "null" : GetDisplay(issuerMap, cardIssuerId.Value);

        return $"{id} - tipo_medio_pago={methodTypeDisplay} - tipo_tarjeta={cardTypeDisplay} - emisor_tarjeta={issuerDisplay} - nombre_comercial={commercialName}";
    }

    private static string GetDisplay(Dictionary<int, string> map, int id)
    {
        return map.TryGetValue(id, out var display) ? $"{display} [{id}]" : $"#{id}";
    }

    private static int? ReadNullableInt(string prompt)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (!int.TryParse(input, out var value))
            throw new Exception("Valor invÃ¡lido");

        return value;
    }
}


