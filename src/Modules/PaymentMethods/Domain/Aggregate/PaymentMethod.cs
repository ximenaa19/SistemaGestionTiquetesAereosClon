// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Domain\Aggregate\PaymentMethod.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentMethods.Domain.Aggregate;

public class PaymentMethod
{
    public PaymentMethodId Id { get; private set; }
    public PaymentMethodTypeId PaymentMethodTypeId { get; private set; }
    public CardTypeId? CardTypeId { get; private set; }
    public CardIssuerId? CardIssuerId { get; private set; }
    public PaymentMethodCommercialName CommercialName { get; private set; }

    private PaymentMethod(
        PaymentMethodId id,
        PaymentMethodTypeId paymentMethodTypeId,
        CardTypeId? cardTypeId,
        CardIssuerId? cardIssuerId,
        PaymentMethodCommercialName commercialName)
    {
        Id = id;
        PaymentMethodTypeId = paymentMethodTypeId;
        CardTypeId = cardTypeId;
        CardIssuerId = cardIssuerId;
        CommercialName = commercialName;
    }

    public static PaymentMethod Create(
        PaymentMethodId id,
        PaymentMethodTypeId paymentMethodTypeId,
        CardTypeId? cardTypeId,
        CardIssuerId? cardIssuerId,
        PaymentMethodCommercialName commercialName)
    {
        return new PaymentMethod(id, paymentMethodTypeId, cardTypeId, cardIssuerId, commercialName);
    }

    public static PaymentMethod CreateNew(
        PaymentMethodTypeId paymentMethodTypeId,
        CardTypeId? cardTypeId,
        CardIssuerId? cardIssuerId,
        PaymentMethodCommercialName commercialName)
    {
        return new PaymentMethod(PaymentMethodId.CreateEmpty(), paymentMethodTypeId, cardTypeId, cardIssuerId, commercialName);
    }
}

