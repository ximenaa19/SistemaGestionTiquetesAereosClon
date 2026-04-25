// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\UseCases\GetCheckinsByCheckedAtRangeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Checkins.Application.UseCases;

public class GetCheckinsByCheckedAtRangeUseCase
{
    private readonly ICheckinRepository _repository;

    public GetCheckinsByCheckedAtRangeUseCase(ICheckinRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Checkin>> ExecuteAsync(DateTime fromInclusive, DateTime toInclusive)
    {
        if (fromInclusive > toInclusive)
            throw new Exception("El rango de fechas no es valido");

        return _repository.GetByCheckedAtRangeAsync(fromInclusive, toInclusive);
    }
}

