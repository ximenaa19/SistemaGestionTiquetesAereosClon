namespace GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

public sealed record BaggageRegisteredAt
{
    public DateTime Value { get; }

    private BaggageRegisteredAt(DateTime value)
    {
        Value = value;
    }

    public static BaggageRegisteredAt Create(DateTime value)
    {
        if (value == default)
            throw new ArgumentException("La fecha de registro del equipaje es obligatoria.");

        return new BaggageRegisteredAt(value);
    }

    public static BaggageRegisteredAt CreateNow()
    {
        return new BaggageRegisteredAt(DateTime.Now);
    }
}
