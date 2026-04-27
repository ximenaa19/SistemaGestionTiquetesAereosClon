using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Baggage.Application.Services;
// reglas y calculo de recargos
public class BaggageSurchargeCalculator
{
    public const string CarryOnType = "MANO";
    public const string CheckedType = "BODEGA"; // constantes para las clases/cabinas

    public BaggageSurchargeResult Calculate(
        string cabinTypeName,
        string baggageType,
        int quantity,
        decimal totalWeightKg)
    { //validaciones basicas de entrada. si algun dato es invalido, lanza excepcion
        if (quantity <= 0)
            throw new ArgumentException("La cantidad de maletas debe ser mayor que cero.");

        if (totalWeightKg <= 0)
            throw new ArgumentException("El peso debe ser mayor que cero.");

        var normalizedType = BaggageType.Create(baggageType).Value; // normaliza el tipo de equipaje
        var policy = ResolvePolicy(cabinTypeName, normalizedType); //escoge la politica seguy el tipo de equipaje

        var excessQuantity = Math.Max(0, quantity - policy.AllowedQuantity); // calcula exceso de cantidad
        var weightPerBag = decimal.Round(totalWeightKg / quantity, 2); // calcula peso por maleta
        var excessByTotalWeight = Math.Max(0, totalWeightKg - policy.AllowedTotalWeightKg); // calcula exceso por peso total
        var excessByBagWeight = Math.Max(0, weightPerBag - policy.AllowedWeightPerBagKg) * quantity; // calcula exceso por peso de maleta
        var excessWeight = decimal.Round(Math.Max(excessByTotalWeight, excessByBagWeight), 2); // calcula exceso total
        var quantitySurcharge = decimal.Round(excessQuantity * policy.ExtraBagFee, 2); // calcula recargo por cantidad
        var weightSurcharge = decimal.Round(excessWeight * policy.ExcessKgFee, 2); // calcula recargo por peso

        return new BaggageSurchargeResult(
            policy,
            excessQuantity,
            excessWeight,
            quantitySurcharge,
            weightSurcharge,
            quantitySurcharge + weightSurcharge);
    }

    public static string NormalizeBaggageType(string baggageType)
    {
        return BaggageType.Create(baggageType).Value;
    }
// reglas de negocio por cabina
    private static BaggagePolicy ResolvePolicy(string cabinTypeName, string baggageType)
    {
        var cabinFamily = ResolveCabinFamily(cabinTypeName);

        return (cabinFamily, baggageType) switch
        {
            ("PRIMERA", CarryOnType) => new BaggagePolicy(cabinFamily, baggageType, 2, 12m, 24m, 90000m, 18000m),
            ("PRIMERA", CheckedType) => new BaggagePolicy(cabinFamily, baggageType, 3, 32m, 96m, 130000m, 22000m),
            ("EJECUTIVA", CarryOnType) => new BaggagePolicy(cabinFamily, baggageType, 1, 12m, 12m, 80000m, 16000m),
            ("EJECUTIVA", CheckedType) => new BaggagePolicy(cabinFamily, baggageType, 2, 32m, 64m, 120000m, 20000m),
            (_, CarryOnType) => new BaggagePolicy("ECONOMICA", baggageType, 1, 10m, 10m, 70000m, 15000m),
            _ => new BaggagePolicy("ECONOMICA", baggageType, 1, 23m, 23m, 100000m, 18000m)
        };
    }
 // detecta la familia de cabina con nombre
    private static string ResolveCabinFamily(string cabinTypeName)
    {
        var name = (cabinTypeName ?? string.Empty).Trim().ToUpperInvariant();

        if (name.Contains("FIRST") || name.Contains("PRIMER"))
            return "PRIMERA";

        if (name.Contains("BUSINESS") || name.Contains("EJECUT") || name.Contains("PREMIUM"))
            return "EJECUTIVA";

        return "ECONOMICA";
    }
}
