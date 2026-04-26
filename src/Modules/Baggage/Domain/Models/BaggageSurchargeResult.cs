namespace GestionAerolineas.src.Modules.Baggage.Domain.Models;

public sealed record BaggageSurchargeResult(
    BaggagePolicy Policy,
    int ExcessQuantity,
    decimal ExcessWeightKg,
    decimal QuantitySurcharge,
    decimal WeightSurcharge,
    decimal TotalSurcharge);
