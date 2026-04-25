// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\UseCases\GetCheckinsByPassengerIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;
using GestionAerolineas.src.Modules.Passengers.Domain.ValueObject;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Repository;

namespace GestionAerolineas.src.Modules.Checkins.Application.UseCases;

public class GetCheckinsByPassengerIdUseCase
{
    private readonly ICheckinRepository _repository;
    private readonly PassengerRepository _passengerRepository;

    public GetCheckinsByPassengerIdUseCase(ICheckinRepository repository, PassengerRepository passengerRepository)
    {
        _repository = repository;
        _passengerRepository = passengerRepository;
    }

    public async Task<IEnumerable<Checkin>> ExecuteAsync(int passengerId)
    {
        var exists = await _passengerRepository.ExistsAsync(PassengerId.Create(passengerId));
        if (!exists)
            throw new Exception("El pasajero no existe");

        return await _repository.GetByPassengerIdAsync(passengerId);
    }
}

