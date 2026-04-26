using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Baggage.Application.Services;

public class BaggageSurchargeCalculator
{
    public const string CarryOnType = "MANO";
    public const string CheckedType = "BODEGA";

    public BaggageSurchargeResult Calculate(
        string cabinTypeName,
        string baggageType,
        int quantity,
        decimal totalWeightKg)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad de maletas debe ser mayor que cero.");

        if (totalWeightKg <= 0)
            throw new ArgumentException("El peso debe ser mayor que cero.");

        var normalizedType = BaggageType.Create(baggageType).Value;
        var policy = ResolvePolicy(cabinTypeName, normalizedType);

        var excessQuantity = Math.Max(0, quantity - policy.AllowedQuantity);
        var weightPerBag = decimal.Round(totalWeightKg / quantity, 2);
        var excessByTotalWeight = Math.Max(0, totalWeightKg - policy.AllowedTotalWeightKg);
        var excessByBagWeight = Math.Max(0, weightPerBag - policy.AllowedWeightPerBagKg) * quantity;
        var excessWeight = decimal.Round(Math.Max(excessByTotalWeight, excessByBagWeight), 2);
        var quantitySurcharge = decimal.Round(excessQuantity * policy.ExtraBagFee, 2);
        var weightSurcharge = decimal.Round(excessWeight * policy.ExcessKgFee, 2);

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
