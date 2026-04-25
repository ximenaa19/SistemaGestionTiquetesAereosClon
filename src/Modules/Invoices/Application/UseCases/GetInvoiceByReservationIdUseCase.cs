// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Application\UseCases\GetInvoiceByReservationIdUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Invoices.Domain.Aggregate;
using GestionAerolineas.src.Modules.Invoices.Domain.Repositories;
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Invoices.Application.UseCases;

public class GetInvoiceByReservationIdUseCase
{
    private readonly IInvoiceRepository _repository;

    public GetInvoiceByReservationIdUseCase(IInvoiceRepository repository)
    {
        _repository = repository;
    }

    public Task<Invoice?> ExecuteAsync(int reservationId)
    {
        return _repository.GetByReservationIdAsync(InvoiceReservationId.Create(reservationId));
    }
}

