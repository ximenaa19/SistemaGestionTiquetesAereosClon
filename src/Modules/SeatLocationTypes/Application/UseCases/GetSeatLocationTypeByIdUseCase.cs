// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\Application\UseCases\GetSeatLocationTypeByIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.SeatLocationTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.SeatLocationTypes.Application.UseCases;

public class GetSeatLocationTypeByIdUseCase
{
    private readonly ISeatLocationTypeRepository _repository;

    public GetSeatLocationTypeByIdUseCase(ISeatLocationTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<SeatLocationType?> ExecuteAsync(int id)
    {
        var idVO = SeatLocationTypeId.Create(id);
        return await _repository.GetByIdAsync(idVO);
    }
}

