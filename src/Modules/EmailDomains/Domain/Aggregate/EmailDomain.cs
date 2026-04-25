// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\EmailDomains\Domain\Aggregate\EmailDomain.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.EmailDomains.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.EmailDomains.Domain.Aggregate;

public class EmailDomain
{
    public EmailDomainId Id { get; private set; }
    public EmailDomainValue Domain { get; private set; }

    private EmailDomain(EmailDomainId id, EmailDomainValue domain)
    {
        Id = id;
        Domain = domain;
    }

    public static EmailDomain Create(EmailDomainId id, EmailDomainValue domain)
    {
        return new EmailDomain(id, domain);
    }

    public static EmailDomain CreateNew(EmailDomainValue domain)
    {
        return new EmailDomain(EmailDomainId.CreateEmpty(), domain);
    }
}

