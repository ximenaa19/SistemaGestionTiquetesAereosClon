namespace GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

public sealed record BaggageSurchargeAmount
{
    public decimal Value { get; }

    private BaggageSurchargeAmount(decimal value)
    {
        Value = value;
    }

    public static BaggageSurchargeAmount Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("El recargo no puede ser negativo.");

        return new BaggageSurchargeAmount(decimal.Round(value, 2));
    }
}
