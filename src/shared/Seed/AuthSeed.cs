// [DocHeader]
// Modulo: General
// Capa: General
// Archivo: src\shared\Seed\AuthSeed.cs
// Responsabilidad: Garantiza los roles minimos necesarios para autenticacion.
// Flujo: Lo usa el seed general para permitir registrar usuarios aun con base recien creada.
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.shared.Seed;

public static class AuthSeed
{
    public static async Task EnsureSystemRolesAsync(AppDbContext context)
    {
        var existingRoles = await context.SystemRoles.AsNoTracking().ToListAsync();
        var desiredRoles = new[]
        {
            new SystemRoleEntity { Name = "Admin", Description = "Acceso total al sistema" },
            new SystemRoleEntity { Name = "Agente", Description = "Operacion de reservas, pagos, check-in" },
            new SystemRoleEntity { Name = "Cliente", Description = "Acceso limitado a consultas propias" }
        };

        foreach (var role in desiredRoles)
        {
            var normalized = SeedHelpers.Normalize(role.Name);
            if (existingRoles.Any(x => SeedHelpers.Normalize(x.Name) == normalized))
                continue;

            context.SystemRoles.Add(role);
        }

        await context.SaveChangesAsync();
    }
}
