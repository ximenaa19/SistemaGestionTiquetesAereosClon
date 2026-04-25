// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\AirportAirline\Application\UseCases\DeleteAirportAirlineUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AirportAirline.Domain.Repositories;
using GestionAerolineas.src.Modules.AirportAirline.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.AirportAirline.Application.UseCases;

public class DeleteAirportAirlineUseCase
{
    private readonly IAirportAirlineRepository _repository;

    public DeleteAirportAirlineUseCase(IAirportAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(AirportAirlineId.Create(id));
        if (entity is null)
            return;

        await _repository.DeleteAsync(entity);
    }
}

