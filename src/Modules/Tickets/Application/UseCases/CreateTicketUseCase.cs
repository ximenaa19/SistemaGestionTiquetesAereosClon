// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Tickets\Application\UseCases\CreateTicketUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Security.Cryptography;
using GestionAerolineas.src.Modules.Tickets.Application.Interfaces;
using GestionAerolineas.src.Modules.Tickets.Domain.Aggregate;
using GestionAerolineas.src.Modules.Tickets.Domain.Repositories;
using GestionAerolineas.src.Modules.Tickets.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Tickets.Application.UseCases;

public class CreateTicketUseCase
{
    private readonly ITicketRepository _repository;
    private readonly ITicketValidator _validator;

    public CreateTicketUseCase(ITicketRepository repository, ITicketValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Ticket> ExecuteAsync(int reservationPassengerId, DateTime? issuedAt, int statusId)
    {
        var rpIdVO = TicketReservationPassengerId.Create(reservationPassengerId);
        var issuedAtVO = TicketIssuedAt.Create(issuedAt ?? DateTime.Now);
        var statusIdVO = TicketStatusId.Create(statusId);

        await _validator.ValidateReservationPassengerExistsAsync(rpIdVO);
        await _validator.ValidateReservationPassengerIsUniqueAsync(rpIdVO);
        await _validator.ValidateTicketStatusExistsAsync(statusIdVO);
        await _validator.ValidateReservationIsConfirmadaAsync(rpIdVO);

        var code = await GenerateUniqueCodeAsync();
        var entity = Ticket.CreateNew(rpIdVO, code, issuedAtVO, statusIdVO);

        await _repository.AddAsync(entity);

        var created = await _repository.GetByReservationPassengerIdAsync(rpIdVO);
        return created ?? entity;
    }

    private async Task<TicketCode> GenerateUniqueCodeAsync()
    {
        for (int attempt = 0; attempt < 8; attempt++)
        {
            var suffix = RandomNumberGenerator.GetInt32(1000, 9999);
            var candidate = $"TKT-{DateTime.Now:yyMMddHHmmss}-{suffix}";
            var codeVO = TicketCode.Create(candidate);
            var exists = await _repository.ExistsByNormalizedCodeAsync(TicketCode.Normalize(codeVO.Value));
            if (!exists)
                return codeVO;
        }

        throw new Exception("No se pudo generar un codigo_tiquete unico");
    }
}

