// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethodTypes\PaymentMethodTypeModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethodTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Application.Services;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PaymentMethodTypes.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.PaymentMethodTypes;

public static class PaymentMethodTypeModule
{
    public static PaymentMethodTypeMenu Build(AppDbContext context)
    {
        var repository = new PaymentMethodTypeRepository(context);
        IPaymentMethodTypeValidator validator = new PaymentMethodTypeValidator(repository);

        var create = new CreatePaymentMethodTypeUseCase(repository, validator);
        var getAll = new GetAllPaymentMethodTypesUseCase(repository);
        var getById = new GetPaymentMethodTypeByIdUseCase(repository);
        var getByName = new GetPaymentMethodTypeByNameUseCase(repository);
        var update = new UpdatePaymentMethodTypeUseCase(repository, validator);
        var delete = new DeletePaymentMethodTypeUseCase(repository);

        return new PaymentMethodTypeMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
