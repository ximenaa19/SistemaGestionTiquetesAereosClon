// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\SeatLocationTypes\SeatLocationTypeModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.SeatLocationTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.SeatLocationTypes.Application.Services;
using GestionAerolineas.src.Modules.SeatLocationTypes.Application.UseCases;
using GestionAerolineas.src.Modules.SeatLocationTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.SeatLocationTypes.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.SeatLocationTypes;

public static class SeatLocationTypeModule
{
    public static SeatLocationTypeMenu Build(AppDbContext context)
    {
        var repository = new SeatLocationTypeRepository(context);
        ISeatLocationTypeValidator validator = new SeatLocationTypeValidator(repository);

        var create = new CreateSeatLocationTypeUseCase(repository, validator);
        var getAll = new GetAllSeatLocationTypesUseCase(repository);
        var getById = new GetSeatLocationTypeByIdUseCase(repository);
        var getByName = new GetSeatLocationTypeByNameUseCase(repository);
        var update = new UpdateSeatLocationTypeUseCase(repository, validator);
        var delete = new DeleteSeatLocationTypeUseCase(repository);

        return new SeatLocationTypeMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}

