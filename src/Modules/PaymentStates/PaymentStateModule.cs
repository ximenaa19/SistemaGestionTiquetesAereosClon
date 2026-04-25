// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentStates\PaymentStateModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentStates.Application.Interfaces;
using GestionAerolineas.src.Modules.PaymentStates.Application.Services;
using GestionAerolineas.src.Modules.PaymentStates.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentStates.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PaymentStates.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.PaymentStates;

public static class PaymentStateModule
{
    public static PaymentStateMenu Build(AppDbContext context)
    {
        var repository = new PaymentStateRepository(context);
        IPaymentStateValidator validator = new PaymentStateValidator(repository);

        var create = new CreatePaymentStateUseCase(repository, validator);
        var getAll = new GetAllPaymentStatesUseCase(repository);
        var getById = new GetPaymentStateByIdUseCase(repository);
        var getByName = new GetPaymentStateByNameUseCase(repository);
        var update = new UpdatePaymentStateUseCase(repository, validator);
        var delete = new DeletePaymentStateUseCase(repository);

        return new PaymentStateMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
