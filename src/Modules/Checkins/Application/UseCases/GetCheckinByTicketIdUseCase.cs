// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Checkins\Application\UseCases\GetCheckinByTicketIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Checkins.Application.Interfaces;
using GestionAerolineas.src.Modules.Checkins.Domain.Aggregate;
using GestionAerolineas.src.Modules.Checkins.Domain.Repositories;
using GestionAerolineas.src.Modules.Checkins.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Checkins.Application.UseCases;

public class GetCheckinByTicketIdUseCase
{
    private readonly ICheckinRepository _repository;
    private readonly ICheckinValidator _validator;

    public GetCheckinByTicketIdUseCase(ICheckinRepository repository, ICheckinValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Checkin?> ExecuteAsync(int ticketId)
    {
        var ticketIdVO = CheckinTicketId.Create(ticketId);
        await _validator.ValidateTicketExistsAsync(ticketIdVO);
        return await _repository.GetByTicketIdAsync(ticketIdVO);
    }
}
