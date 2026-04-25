// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Domain\Aggregate\PersonEmail.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonEmails.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PersonEmails.Domain.Aggregate;

public class PersonEmail
{
    public PersonEmailId Id { get; private set; }
    public PersonEmailPersonId PersonId { get; private set; }
    public PersonEmailUser User { get; private set; }
    public PersonEmailDomainId EmailDomainId { get; private set; }
    public PersonEmailIsPrimary IsPrimary { get; private set; }

    private PersonEmail(
        PersonEmailId id,
        PersonEmailPersonId personId,
        PersonEmailUser user,
        PersonEmailDomainId emailDomainId,
        PersonEmailIsPrimary isPrimary)
    {
        Id = id;
        PersonId = personId;
        User = user;
        EmailDomainId = emailDomainId;
        IsPrimary = isPrimary;
    }

    public static PersonEmail Create(
        PersonEmailId id,
        PersonEmailPersonId personId,
        PersonEmailUser user,
        PersonEmailDomainId emailDomainId,
        PersonEmailIsPrimary isPrimary)
    {
        return new PersonEmail(id, personId, user, emailDomainId, isPrimary);
    }

    public static PersonEmail CreateNew(
        PersonEmailPersonId personId,
        PersonEmailUser user,
        PersonEmailDomainId emailDomainId,
        PersonEmailIsPrimary? isPrimary = null)
    {
        return new PersonEmail(
            PersonEmailId.CreateEmpty(),
            personId,
            user,
            emailDomainId,
            isPrimary ?? PersonEmailIsPrimary.Create(false)
        );
    }
}

