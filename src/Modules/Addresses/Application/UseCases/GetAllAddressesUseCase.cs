// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Application\UseCases\GetAllAddressesUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Domain.Aggregate;
using GestionAerolineas.src.Modules.Addresses.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Addresses.Application.UseCases;

public class GetAllAddressesUseCase
{
    private readonly IAddressRepository _repository;

    public GetAllAddressesUseCase(IAddressRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Address>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

