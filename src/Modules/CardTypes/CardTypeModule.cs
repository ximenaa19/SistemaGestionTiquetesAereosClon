// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardTypes\CardTypeModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.CardTypes.Application.Services;
using GestionAerolineas.src.Modules.CardTypes.Application.UseCases;
using GestionAerolineas.src.Modules.CardTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.CardTypes.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.CardTypes;

public static class CardTypeModule
{
    public static CardTypeMenu Build(AppDbContext context)
    {
        var repository = new CardTypeRepository(context);
        ICardTypeValidator validator = new CardTypeValidator(repository);

        var create = new CreateCardTypeUseCase(repository, validator);
        var getAll = new GetAllCardTypesUseCase(repository);
        var getById = new GetCardTypeByIdUseCase(repository);
        var getByName = new GetCardTypeByNameUseCase(repository);
        var update = new UpdateCardTypeUseCase(repository, validator);
        var delete = new DeleteCardTypeUseCase(repository);

        return new CardTypeMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
