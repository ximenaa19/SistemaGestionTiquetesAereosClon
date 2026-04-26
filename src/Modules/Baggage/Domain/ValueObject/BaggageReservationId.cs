namespace GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

public sealed record BaggageReservationId
{
    public int Value { get; }

    private BaggageReservationId(int value)
    {
        Value = value;
    }

    public static BaggageReservationId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        return new BaggageReservationId(value);
    }
}
