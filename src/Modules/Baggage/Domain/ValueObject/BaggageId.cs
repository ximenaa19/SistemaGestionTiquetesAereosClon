namespace GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

public sealed record BaggageId
{
    public int Value { get; }

    private BaggageId(int value)
    {
        Value = value;
    }

    public static BaggageId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El id del equipaje debe ser mayor que cero.");

        return new BaggageId(value);
    }

    public static BaggageId CreateEmpty()
    {
        return new BaggageId(0);
    }
}
