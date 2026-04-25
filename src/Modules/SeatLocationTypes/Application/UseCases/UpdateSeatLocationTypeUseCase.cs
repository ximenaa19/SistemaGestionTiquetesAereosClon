// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\Application\UseCases\UpdateSeatLocationTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SeatLocationTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SeatLocationTypes.Application.UseCases;

public class UpdateSeatLocationTypeUseCase
{
    private readonly ISeatLocationTypeRepository _repository;
    private readonly ISeatLocationTypeValidator _validator;

    public UpdateSeatLocationTypeUseCase(
        ISeatLocationTypeRepository repository,
        ISeatLocationTypeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(int id, string name)
    {
        var idVO = SeatLocationTypeId.Create(id);

        var existing = await _repository.GetByIdAsync(idVO);

        if (existing is null)
            throw new Exception("El tipo de ubicación de asiento no existe");

        var nameVO = SeatLocationTypeName.Create(name);

        await _validator.ValidateNameAsync(nameVO, idVO);

        var updated = SeatLocationType.Create(idVO, nameVO);

        await _repository.UpdateAsync(updated);
    }
}

