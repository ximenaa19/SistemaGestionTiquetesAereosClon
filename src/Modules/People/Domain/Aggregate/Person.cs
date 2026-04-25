// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Domain\Aggregate\Person.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.People.Domain.Aggregate;

public class Person
{
    public PersonId Id { get; private set; }
    public PersonDocumentTypeId DocumentTypeId { get; private set; }
    public PersonDocumentNumber DocumentNumber { get; private set; }
    public PersonFirstNames FirstNames { get; private set; }
    public PersonLastNames LastNames { get; private set; }
    public PersonBirthDate BirthDate { get; private set; }
    public PersonGender Gender { get; private set; }
    public PersonAddressId AddressId { get; private set; }

    private Person(
        PersonId id,
        PersonDocumentTypeId documentTypeId,
        PersonDocumentNumber documentNumber,
        PersonFirstNames firstNames,
        PersonLastNames lastNames,
        PersonBirthDate birthDate,
        PersonGender gender,
        PersonAddressId addressId)
    {
        Id = id;
        DocumentTypeId = documentTypeId;
        DocumentNumber = documentNumber;
        FirstNames = firstNames;
        LastNames = lastNames;
        BirthDate = birthDate;
        Gender = gender;
        AddressId = addressId;
    }

    public static Person Create(
        PersonId id,
        PersonDocumentTypeId documentTypeId,
        PersonDocumentNumber documentNumber,
        PersonFirstNames firstNames,
        PersonLastNames lastNames,
        PersonBirthDate birthDate,
        PersonGender gender,
        PersonAddressId addressId)
    {
        return new Person(id, documentTypeId, documentNumber, firstNames, lastNames, birthDate, gender, addressId);
    }

    public static Person CreateNew(
        PersonDocumentTypeId documentTypeId,
        PersonDocumentNumber documentNumber,
        PersonFirstNames firstNames,
        PersonLastNames lastNames,
        PersonBirthDate birthDate,
        PersonGender gender,
        PersonAddressId addressId)
    {
        return new Person(
            PersonId.CreateEmpty(),
            documentTypeId,
            documentNumber,
            firstNames,
            lastNames,
            birthDate,
            gender,
            addressId
        );
    }
}

