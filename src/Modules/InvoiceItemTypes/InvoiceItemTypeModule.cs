// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItemTypes\InvoiceItemTypeModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Application.Services;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Application.UseCases;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.InvoiceItemTypes.UI;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.InvoiceItemTypes;

public static class InvoiceItemTypeModule
{
    public static InvoiceItemTypeMenu Build(AppDbContext context)
    {
        var repository = new InvoiceItemTypeRepository(context);
        IInvoiceItemTypeValidator validator = new InvoiceItemTypeValidator(repository);

        var create = new CreateInvoiceItemTypeUseCase(repository, validator);
        var getAll = new GetAllInvoiceItemTypesUseCase(repository);
        var getById = new GetInvoiceItemTypeByIdUseCase(repository);
        var getByName = new GetInvoiceItemTypeByNameUseCase(repository);
        var update = new UpdateInvoiceItemTypeUseCase(repository, validator);
        var delete = new DeleteInvoiceItemTypeUseCase(repository);

        return new InvoiceItemTypeMenu(
            create,
            getAll,
            getById,
            getByName,
            update,
            delete
        );
    }
}
