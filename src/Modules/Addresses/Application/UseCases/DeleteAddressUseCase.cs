// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Addresses\Application\UseCases\DeleteAddressUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Addresses.Domain.Repositories;
using GestionAerolineas.src.Modules.Addresses.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Addresses.Application.UseCases;

public class DeleteAddressUseCase
{
    private readonly IAddressRepository _repository;

    public DeleteAddressUseCase(IAddressRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(AddressId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

