namespace GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

public sealed record BaggageQuantity
{
    public int Value { get; }

    private BaggageQuantity(int value)
    {
        Value = value;
    }

    public static BaggageQuantity Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que cero.");

        return new BaggageQuantity(value);
    }
}
