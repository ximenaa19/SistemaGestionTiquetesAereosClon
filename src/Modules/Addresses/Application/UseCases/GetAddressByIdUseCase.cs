// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Application\UseCases\GetAddressByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Domain.Aggregate;
using GestionAerolineas.src.Modules.Addresses.Domain.Repositories;
using GestionAerolineas.src.Modules.Addresses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Addresses.Application.UseCases;

public class GetAddressByIdUseCase
{
    private readonly IAddressRepository _repository;

    public GetAddressByIdUseCase(IAddressRepository repository)
    {
        _repository = repository;
    }

    public Task<Address?> ExecuteAsync(int id)
    {
        return _repository.GetByIdAsync(AddressId.Create(id));
    }
}

