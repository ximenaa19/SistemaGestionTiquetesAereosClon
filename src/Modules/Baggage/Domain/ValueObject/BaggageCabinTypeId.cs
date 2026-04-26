namespace GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

public sealed record BaggageCabinTypeId
{
    public int Value { get; }

    private BaggageCabinTypeId(int value)
    {
        Value = value;
    }

    public static BaggageCabinTypeId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("La clase/cabina es obligatoria.");

        return new BaggageCabinTypeId(value);
    }
}
