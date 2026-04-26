namespace GestionAerolineas.src.Modules.Baggage.Domain.Models;

public sealed record BaggagePolicy(
    string CabinFamily,
    string BaggageType,
    int AllowedQuantity,
    decimal AllowedWeightPerBagKg,
    decimal AllowedTotalWeightKg,
    decimal ExtraBagFee,
    decimal ExcessKgFee);
