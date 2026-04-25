// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Seed\SeedRunner.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.shared.Seed;

public static class SeedRunner
{
    public static async Task SeedMasterAndCatalogsAsync(AppDbContext context)
    {
        await using var tx = await context.Database.BeginTransactionAsync();
        try
        {
            await CatalogSeed.SeedAsync(context);
            await MasterDataSeed.SeedAsync(context);
            await OperationalSampleSeed.SeedAsync(context);

            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}

