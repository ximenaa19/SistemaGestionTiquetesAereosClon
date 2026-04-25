// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\PaymentMethodModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Application.UseCases;
using GestionAerolineas.src.Modules.CardIssuers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.CardTypes.Application.UseCases;
using GestionAerolineas.src.Modules.CardTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PaymentMethods.Application.Interfaces;
using GestionAerolineas.src.Modules.PaymentMethods.Application.Services;
using GestionAerolineas.src.Modules.PaymentMethods.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentMethods.Infrastructure.Repository;
using GestionAerolineas.src.Modules.PaymentMethods.UI;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Application.UseCases;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.PaymentMethods;

public static class PaymentMethodModule
{
    public static PaymentMethodMenu Build(AppDbContext context)
    {
        var repository = new PaymentMethodRepository(context);
        IPaymentMethodValidator validator = new PaymentMethodValidator(repository);

        var create = new CreatePaymentMethodUseCase(repository, validator);
        var getAll = new GetAllPaymentMethodsUseCase(repository);
        var getById = new GetPaymentMethodByIdUseCase(repository);
        var getByCommercialName = new GetPaymentMethodByCommercialNameUseCase(repository);
        var update = new UpdatePaymentMethodUseCase(repository, validator);
        var delete = new DeletePaymentMethodUseCase(repository);

        var paymentMethodTypeRepository = new PaymentMethodTypeRepository(context);
        var cardTypeRepository = new CardTypeRepository(context);
        var cardIssuerRepository = new CardIssuerRepository(context);

        var getAllPaymentMethodTypes = new GetAllPaymentMethodTypesUseCase(paymentMethodTypeRepository);
        var getAllCardTypes = new GetAllCardTypesUseCase(cardTypeRepository);
        var getAllCardIssuers = new GetAllCardIssuersUseCase(cardIssuerRepository);

        return new PaymentMethodMenu(
            create,
            getAll,
            getById,
            getByCommercialName,
            update,
            delete,
            getAllPaymentMethodTypes,
            getAllCardTypes,
            getAllCardIssuers
        );
    }
}

