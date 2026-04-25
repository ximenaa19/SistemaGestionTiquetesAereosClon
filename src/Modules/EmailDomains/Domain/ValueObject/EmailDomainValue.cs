// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Domain\ValueObject\EmailDomainValue.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Text.RegularExpressions;

namespace GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;

public sealed record EmailDomainValue
{
    public string Value { get; }

    private EmailDomainValue(string value)
    {
        Value = value;
    }

    public static EmailDomainValue Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El dominio no puede estar vacío");

        var normalized = value.Trim().ToLowerInvariant();

        // Dominio básico (ej: gmail.com, mi-dominio.co)
        var regex = new Regex("^[a-z0-9]([a-z0-9-]{0,61}[a-z0-9])?(\\.[a-z0-9]([a-z0-9-]{0,61}[a-z0-9])?)+$");

        if (!regex.IsMatch(normalized))
            throw new ArgumentException("Dominio inválido");

        return new EmailDomainValue(normalized);
    }

    public override string ToString() => Value;
}

