// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentStates\Domain\Aggregate\PaymentState.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentStates.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PaymentStates.Domain.Aggregate;

public class PaymentState
{
    public PaymentStateId Id { get; private set; }
    public PaymentStateName Name { get; private set; }

    private PaymentState(PaymentStateId id, PaymentStateName name)
    {
        Id = id;
        Name = name;
    }

    public static PaymentState Create(PaymentStateId id, PaymentStateName name)
    {
        return new PaymentState(id, name);
    }

    public static PaymentState CreateNew(PaymentStateName name)
    {
        return new PaymentState(PaymentStateId.CreateEmpty(), name);
    }
}
