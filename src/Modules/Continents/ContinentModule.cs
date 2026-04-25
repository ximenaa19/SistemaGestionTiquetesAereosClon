// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Continents\ContinentModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Continents.Application.Interfaces;
using GestionAerolineas.src.Modules.Continents.Application.Services;
using GestionAerolineas.src.Modules.Continents.Application.UseCases;
using GestionAerolineas.src.Modules.Continents.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Continents.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Continents;

public static class ContinentModule
{
    public static ContinentMenu Build(AppDbContext context)
    {
        var repository = new ContinentRepository(context);
        IContinentValidator validator = new ContinentValidator(repository);

        var create = new CreateContinentUseCase(repository, validator);
        var getAll = new GetAllContinentsUseCase(repository);
        var getById = new GetContinentByIdUseCase(repository);
        var getByName = new GetContinentByNameUseCase(repository);
        var update = new UpdateContinentUseCase(repository, validator);
        var delete = new DeleteContinentUseCase(repository);

        return new ContinentMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
