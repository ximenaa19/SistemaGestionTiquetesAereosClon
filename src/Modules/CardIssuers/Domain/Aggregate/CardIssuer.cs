// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardIssuers\Domain\Aggregate\CardIssuer.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CardIssuers.Domain.Aggregate;

public class CardIssuer
{
    public CardIssuerId Id { get; private set; }
    public CardIssuerName Name { get; private set; }

    private CardIssuer(CardIssuerId id, CardIssuerName name)
    {
        Id = id;
        Name = name;
    }

    public static CardIssuer Create(CardIssuerId id, CardIssuerName name)
    {
        return new CardIssuer(id, name);
    }

    public static CardIssuer CreateNew(CardIssuerName name)
    {
        return new CardIssuer(CardIssuerId.CreateEmpty(), name);
    }
}
