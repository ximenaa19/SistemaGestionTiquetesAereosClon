// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Domain\ValueObject\EmailDomainId.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;

namespace GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;

public sealed record EmailDomainId
{
    public int Value { get; }

    private EmailDomainId(int value)
    {
        Value = value;
    }

    public static EmailDomainId Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("El valor no puede ser menor a 1");

        return new EmailDomainId(value);
    }

    public static EmailDomainId CreateEmpty()
    {
        return new EmailDomainId(0);
    }
}

