namespace GestionAerolineas.src.Modules.Baggage.Domain.Models;

//resultado de la aplicacion de recargos
// se usa para mostrar previsualizacion de los resultados
public sealed record BaggageSurchargeResult(
    BaggagePolicy Policy, // politica aplicada
    int ExcessQuantity, // cantidad excedente
    decimal ExcessWeightKg, // peso excedente
    decimal QuantitySurcharge, // recargo por cantidad
    decimal WeightSurcharge, // recargo por peso
    decimal TotalSurcharge); // recargo total
