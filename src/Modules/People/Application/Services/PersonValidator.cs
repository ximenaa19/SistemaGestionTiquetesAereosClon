// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\People\Application\Services\PersonValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Domain.ValueObject;
using GestionAerolineas.src.Modules.Addresses.Infrastructure.Repository;
using GestionAerolineas.src.Modules.DocumentTypes.Domain.ValueObject;
using GestionAerolineas.src.Modules.DocumentTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.Interfaces;
using GestionAerolineas.src.Modules.People.Domain.Repositories;
using GestionAerolineas.src.Modules.People.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.People.Application.Services;

public class PersonValidator : IPersonValidator
{
    private readonly IPersonRepository _repository;
    private readonly DocumentTypeRepository _documentTypeRepository;
    private readonly AddressRepository _addressRepository;

    public PersonValidator(
        IPersonRepository repository,
        DocumentTypeRepository documentTypeRepository,
        AddressRepository addressRepository)
    {
        _repository = repository;
        _documentTypeRepository = documentTypeRepository;
        _addressRepository = addressRepository;
    }

    public async Task ValidateDocumentTypeExistsAsync(PersonDocumentTypeId documentTypeId)
    {
        var exists = await _documentTypeRepository.ExistsAsync(DocumentTypeId.Create(documentTypeId.Value));
        if (!exists)
            throw new Exception("El tipo de documento no existe");
    }

    public async Task ValidateAddressExistsAsync(PersonAddressId addressId)
    {
        if (addressId.Value is null)
            return;

        var exists = await _addressRepository.ExistsAsync(AddressId.Create(addressId.Value.Value));
        if (!exists)
            throw new Exception("La direccion no existe");
    }

    public async Task ValidateUniqueDocumentAsync(
        PersonDocumentTypeId documentTypeId,
        PersonDocumentNumber documentNumber,
        PersonId? currentId = null)
    {
        var normalizedCandidate = PersonDocumentNumber.Normalize(documentNumber.Value);
        var exists = await _repository.ExistsByNormalizedDocumentInTypeAsync(
            documentTypeId.Value,
            normalizedCandidate,
            currentId?.Value);

        if (exists)
            throw new Exception("Ya existe una persona con ese tipo y numero de documento");
    }
}

