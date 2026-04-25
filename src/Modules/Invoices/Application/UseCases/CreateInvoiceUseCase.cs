// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Application\UseCases\CreateInvoiceUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Security.Cryptography;
using GestionAerolineas.src.Modules.Invoices.Application.Interfaces;
using GestionAerolineas.src.Modules.Invoices.Domain.Aggregate;
using GestionAerolineas.src.Modules.Invoices.Domain.Repositories;
using GestionAerolineas.src.Modules.Invoices.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Invoices.Application.UseCases;

public class CreateInvoiceUseCase
{
    private readonly IInvoiceRepository _repository;
    private readonly IInvoiceValidator _validator;

    public CreateInvoiceUseCase(IInvoiceRepository repository, IInvoiceValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Invoice> ExecuteAsync(int reservationId, DateTime? issuedAt)
    {
        var reservationIdVO = InvoiceReservationId.Create(reservationId);
        var issuedAtVO = InvoiceIssuedAt.Create(issuedAt ?? DateTime.Now);

        await _validator.ValidateReservationExistsAsync(reservationIdVO);
        await _validator.ValidateReservationAllowsInvoiceAsync(reservationIdVO);
        await _validator.ValidateReservationIsUniqueAsync(reservationIdVO);

        var number = await GenerateUniqueNumberAsync();
        var invoice = Invoice.CreateNew(reservationIdVO, number, issuedAtVO);

        await _repository.AddAsync(invoice);

        var created = await _repository.GetByReservationIdAsync(reservationIdVO);
        return created ?? invoice;
    }

    private async Task<InvoiceNumber> GenerateUniqueNumberAsync()
    {
        for (int attempt = 0; attempt < 6; attempt++)
        {
            var suffix = RandomNumberGenerator.GetInt32(1000, 9999);
            var candidate = $"INV-{DateTime.Now:yyyyMMdd-HHmmss}-{suffix}";
            var numberVO = InvoiceNumber.Create(candidate);

            var normalized = InvoiceNumber.Normalize(numberVO.Value);
            var exists = await _repository.ExistsByNormalizedNumberAsync(normalized);
            if (!exists)
                return numberVO;
        }

        throw new Exception("No se pudo generar un numero de factura unico");
    }
}

