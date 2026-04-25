// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Application\UseCases\UpdatePersonUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.People.Application.Interfaces;
using GestionAerolineas.src.Modules.People.Domain.Aggregate;
using GestionAerolineas.src.Modules.People.Domain.Repositories;
using GestionAerolineas.src.Modules.People.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.People.Application.UseCases;

public class UpdatePersonUseCase
{
    private readonly IPersonRepository _repository;
    private readonly IPersonValidator _validator;

    public UpdatePersonUseCase(IPersonRepository repository, IPersonValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(
        int id,
        int documentTypeId,
        string documentNumber,
        string firstNames,
        string lastNames,
        DateTime? birthDate,
        string? gender,
        int? addressId)
    {
        var idVO = PersonId.Create(id);
        var documentTypeVO = PersonDocumentTypeId.Create(documentTypeId);
        var documentNumberVO = PersonDocumentNumber.Create(documentNumber);
        var firstNamesVO = PersonFirstNames.Create(firstNames);
        var lastNamesVO = PersonLastNames.Create(lastNames);
        var birthDateVO = PersonBirthDate.Create(birthDate);
        var genderVO = PersonGender.Create(gender);
        var addressIdVO = PersonAddressId.Create(addressId);

        await _validator.ValidateDocumentTypeExistsAsync(documentTypeVO);
        await _validator.ValidateAddressExistsAsync(addressIdVO);
        await _validator.ValidateUniqueDocumentAsync(documentTypeVO, documentNumberVO, idVO);

        var entity = Person.Create(
            idVO,
            documentTypeVO,
            documentNumberVO,
            firstNamesVO,
            lastNamesVO,
            birthDateVO,
            genderVO,
            addressIdVO
        );

        await _repository.UpdateAsync(entity);
    }
}

