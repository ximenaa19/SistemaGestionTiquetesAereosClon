namespace GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

public sealed record BaggageWeightKg
{
    public decimal Value { get; }

    private BaggageWeightKg(decimal value)
    {
        Value = value;
    }

    public static BaggageWeightKg Create(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("El peso debe ser mayor que cero.");

        return new BaggageWeightKg(decimal.Round(value, 2));
    }
}
