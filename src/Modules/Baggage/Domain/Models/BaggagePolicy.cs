namespace GestionAerolineas.src.Modules.Baggage.Domain.Models;

//representa la polit5ica aplicada en el registro de equipaje
public sealed record BaggagePolicy(
    string CabinFamily, // familia de cabina (Economy, Business, First)
    string BaggageType, // tipo de equipaje (Handbag, Backpack, etc)
    int AllowedQuantity, // cantidad permitida
    decimal AllowedWeightPerBagKg, // peso permitido por maleta
    decimal AllowedTotalWeightKg, // peso total permitido
    decimal ExtraBagFee, // recargo por exceso de cantidad
    decimal ExcessKgFee); // recargo por exceso de peso (total)
