namespace GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

public sealed record BaggageType
{
    public string Value { get; }

    private BaggageType(string value)
    {
        Value = value;
    }

    public static BaggageType Create(string value)
    {
        var normalized = (value ?? string.Empty).Trim().ToUpperInvariant();

        return normalized switch
        {
            "1" or "MANO" or "EQUIPAJE DE MANO" or "CABINA" => new BaggageType("MANO"),
            "2" or "BODEGA" or "EQUIPAJE EN BODEGA" or "HOLD" => new BaggageType("BODEGA"),
            _ => throw new ArgumentException("Tipo de equipaje invalido. Usa MANO o BODEGA.")
        };
    }
}
