// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Seed\CatalogSeed.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.AvailabilityStatuses.Infrastructure.Entity;
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.CardIssuers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.CardTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.CheckinStatuses.Infrastructure.Entity;
using GestionAerolineas.src.Modules.DocumentTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.EmailDomains.Infrastructure.Entity;
using GestionAerolineas.src.Modules.FlightRoles.Infrastructure.Entity;
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Entity;
using GestionAerolineas.src.Modules.FlightStatusTransitions.Infrastructure.Entity;
using GestionAerolineas.src.Modules.InvoiceItemTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.PassengerTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.PaymentMethods.Infrastructure.Entity;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.PaymentStates.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Permissions.Infrastructure.Entity;
using GestionAerolineas.src.Modules.PhoneCodes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Entity;
using GestionAerolineas.src.Modules.RoadTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.RolePermissions.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Seasons.Infrastructure.Entity;
using GestionAerolineas.src.Modules.SeatLocationTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.StaffRoles.Infrastructure.Entity;
using GestionAerolineas.src.Modules.SystemRoles.Infrastructure.Entity;
using GestionAerolineas.src.Modules.TicketStatuses.Infrastructure.Entity;
using GestionAerolineas.src.shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestionAerolineas.src.shared.Seed;

public static class CatalogSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedRoadTypesAsync(context);
        await SeedDocumentTypesAsync(context);
        await SeedEmailDomainsAsync(context);
        await SeedPhoneCodesAsync(context);
        await SeedSeasonsAsync(context);

        await SeedPassengerTypesAsync(context);
        await SeedCabinTypesAsync(context);
        await SeedSeatLocationTypesAsync(context);

        await SeedAvailabilityStatusesAsync(context);
        await SeedStaffRolesAsync(context);
        await SeedFlightRolesAsync(context);

        await SeedFlightStatesAndTransitionsAsync(context);
        await SeedReservationStatusesAndTransitionsAsync(context);

        await SeedTicketStatusesAsync(context);
        await SeedCheckinStatusesAsync(context);
        await SeedInvoiceItemTypesAsync(context);

        await SeedPaymentCatalogsAsync(context);
        await SeedAuthCatalogsAsync(context);
    }

    private static async Task SeedRoadTypesAsync(AppDbContext context)
    {
        var desired = new[] { "Calle", "Carrera", "Avenida", "Diagonal", "Transversal" };
        var existing = await context.RoadTypes.AsNoTracking().ToListAsync();

        var maxId = existing.Count == 0 ? 0 : existing.Max(x => x.Id);
        foreach (var name in desired)
        {
            var norm = SeedHelpers.Normalize(name);
            if (existing.Any(x => SeedHelpers.Normalize(x.Name) == norm))
                continue;

            maxId++;
            context.RoadTypes.Add(new RoadTypeEntity { Id = maxId, Name = name });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedDocumentTypesAsync(AppDbContext context)
    {
        var existing = await context.DocumentTypes.AsNoTracking().ToListAsync();
        var desired = new[]
        {
            new { Name = "Cédula de ciudadanía", Code = "CC" },
            new { Name = "Tarjeta de identidad", Code = "TI" },
            new { Name = "Pasaporte", Code = "PASAPORTE" }
        };

        foreach (var d in desired)
        {
            var codeNorm = SeedHelpers.Normalize(d.Code);
            if (existing.Any(x => SeedHelpers.Normalize(x.Code) == codeNorm))
                continue;

            context.DocumentTypes.Add(new DocumentTypeEntity { Name = d.Name, Code = d.Code });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedEmailDomainsAsync(AppDbContext context)
    {
        var existing = await context.EmailDomains.AsNoTracking().ToListAsync();
        var desired = new[] { "gmail.com", "outlook.com", "hotmail.com", "yahoo.com" };

        foreach (var domain in desired)
        {
            var norm = SeedHelpers.Normalize(domain);
            if (existing.Any(x => SeedHelpers.Normalize(x.Domain) == norm))
                continue;

            context.EmailDomains.Add(new EmailDomainEntity { Domain = domain });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedPhoneCodesAsync(AppDbContext context)
    {
        var existing = await context.PhoneCodes.AsNoTracking().ToListAsync();
        var desired = new[]
        {
            new { Code = "+57", Country = "Colombia" },
            new { Code = "+1", Country = "Estados Unidos" },
            new { Code = "+34", Country = "España" }
        };

        foreach (var item in desired)
        {
            var norm = SeedHelpers.Normalize(item.Code);
            if (existing.Any(x => SeedHelpers.Normalize(x.CountryCode) == norm))
                continue;

            context.PhoneCodes.Add(new PhoneCodeEntity { CountryCode = item.Code, CountryName = item.Country });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedSeasonsAsync(AppDbContext context)
    {
        var existing = await context.Seasons.AsNoTracking().ToListAsync();
        var desired = new[]
        {
            new { Name = "Baja", Description = "Temporada baja", Factor = 1.0m },
            new { Name = "Media", Description = "Temporada media", Factor = 1.2m },
            new { Name = "Alta", Description = "Temporada alta", Factor = 1.5m }
        };

        foreach (var s in desired)
        {
            var norm = SeedHelpers.Normalize(s.Name);
            if (existing.Any(x => SeedHelpers.Normalize(x.Name) == norm))
                continue;

            context.Seasons.Add(new SeasonEntity { Name = s.Name, Description = s.Description, PriceFactor = s.Factor });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedPassengerTypesAsync(AppDbContext context)
    {
        var existing = await context.PassengerTypes.AsNoTracking().ToListAsync();
        var desired = new[]
        {
            new PassengerTypeEntity { Name = "Adulto", AgeMin = 12, AgeMax = null },
            new PassengerTypeEntity { Name = "Niño", AgeMin = 2, AgeMax = 11 },
            new PassengerTypeEntity { Name = "Infante", AgeMin = 0, AgeMax = 1 }
        };

        foreach (var pt in desired)
        {
            var norm = SeedHelpers.Normalize(pt.Name);
            if (existing.Any(x => SeedHelpers.Normalize(x.Name) == norm))
                continue;

            context.PassengerTypes.Add(pt);
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedCabinTypesAsync(AppDbContext context)
    {
        var desired = new[] { "Económica", "Premium", "Ejecutiva", "Primera" };
        var existing = await context.CabinTypes.AsNoTracking().ToListAsync();

        var maxId = existing.Count == 0 ? 0 : existing.Max(x => x.Id);
        foreach (var name in desired)
        {
            var norm = SeedHelpers.Normalize(name);
            if (existing.Any(x => SeedHelpers.Normalize(x.Name) == norm))
                continue;

            maxId++;
            context.CabinTypes.Add(new CabinTypeEntity { Id = maxId, Name = name });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedSeatLocationTypesAsync(AppDbContext context)
    {
        await EnsureNameListAsync(context, context.SeatLocationTypes, x => x.Name, n => new SeatLocationTypeEntity { Name = n }, new[]
        {
            "Ventana", "Pasillo", "Centro"
        });
    }

    private static async Task SeedAvailabilityStatusesAsync(AppDbContext context)
    {
        await EnsureNameListAsync(context, context.AvailabilityStatuses, x => x.Name, n => new AvailabilityStatusEntity { Name = n }, new[]
        {
            "Disponible", "Asignado", "Vacaciones", "Licencia", "Baja"
        });
    }

    private static async Task SeedStaffRolesAsync(AppDbContext context)
    {
        await EnsureNameListAsync(context, context.StaffRoles, x => x.Name, n => new StaffRoleEntity { Name = n }, new[]
        {
            "Piloto", "Copiloto", "Agente Check-In", "Administrativo", "Auxiliar de Vuelo"
        });
    }

    private static async Task SeedFlightRolesAsync(AppDbContext context)
    {
        await EnsureNameListAsync(context, context.FlightRoles, x => x.Name, n => new FlightRoleEntity { Name = n }, new[]
        {
            "Comandante", "Copiloto", "Jefe de Cabina", "Auxiliar de Vuelo"
        });
    }

    private static async Task SeedFlightStatesAndTransitionsAsync(AppDbContext context)
    {
        await EnsureNameListAsync(context, context.FlightStates, x => x.Name, n => new FlightStateEntity { Name = n }, new[]
        {
            "Programado", "Abordando", "En vuelo", "Cancelado", "Completado", "Reprogramado"
        });

        var states = await context.FlightStates.AsNoTracking().ToListAsync();
        int Id(string name)
        {
            var norm = SeedHelpers.Normalize(name);
            return states.First(s => SeedHelpers.Normalize(s.Name) == norm).Id;
        }

        var pairs = new (string from, string to)[]
        {
            ("Programado", "Abordando"),
            ("Abordando", "En vuelo"),
            ("En vuelo", "Completado"),
            ("Programado", "Cancelado"),
            ("Abordando", "Cancelado"),
            ("Programado", "Reprogramado"),
            ("Reprogramado", "Programado")
        };

        var existing = await context.FlightStatusTransitions.AsNoTracking().ToListAsync();
        foreach (var (from, to) in pairs)
        {
            var origin = Id(from);
            var dest = Id(to);
            if (existing.Any(x => x.OriginStateId == origin && x.DestinationStateId == dest))
                continue;

            context.FlightStatusTransitions.Add(new FlightStatusTransitionEntity
            {
                OriginStateId = origin,
                DestinationStateId = dest
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedReservationStatusesAndTransitionsAsync(AppDbContext context)
    {
        await EnsureNameListAsync(context, context.ReservationStatuses, x => x.Name, n => new ReservationStatusEntity { Name = n }, new[]
        {
            "Pendiente", "Confirmada", "Cancelada", "Vencida"
        });

        var statuses = await context.ReservationStatuses.AsNoTracking().ToListAsync();
        int Id(string name)
        {
            var norm = SeedHelpers.Normalize(name);
            return statuses.First(s => SeedHelpers.Normalize(s.Name) == norm).Id;
        }

        var pairs = new (string from, string to)[]
        {
            ("Pendiente", "Confirmada"),
            ("Pendiente", "Cancelada"),
            ("Pendiente", "Vencida"),
            ("Confirmada", "Cancelada")
        };

        var existing = await context.ReservationStatusTransitions.AsNoTracking().ToListAsync();
        foreach (var (from, to) in pairs)
        {
            var origin = Id(from);
            var dest = Id(to);
            if (existing.Any(x => x.OriginStatusId == origin && x.DestinationStatusId == dest))
                continue;

            context.ReservationStatusTransitions.Add(new ReservationStatusTransitionEntity
            {
                OriginStatusId = origin,
                DestinationStatusId = dest
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedTicketStatusesAsync(AppDbContext context)
    {
        await EnsureNameListAsync(context, context.TicketStatuses, x => x.Name, n => new TicketStatusEntity { Name = n }, new[]
        {
            "Emitido", "Anulado", "Usado"
        });
    }

    private static async Task SeedCheckinStatusesAsync(AppDbContext context)
    {
        await EnsureNameListAsync(context, context.CheckinStatuses, x => x.Name, n => new CheckinStatusEntity { Name = n }, new[]
        {
            "Pendiente", "Realizado", "No presentado"
        });
    }

    private static async Task SeedInvoiceItemTypesAsync(AppDbContext context)
    {
        await EnsureNameListAsync(context, context.InvoiceItemTypes, x => x.Name, n => new InvoiceItemTypeEntity { Name = n }, new[]
        {
            "Tiquete base",
            "Equipaje adicional",
            "Upgrade cabina",
            "Selección de asiento",
            "Comidas especiales"
        });
    }

    private static async Task SeedPaymentCatalogsAsync(AppDbContext context)
    {
        await EnsureNameListAsync(context, context.PaymentStates, x => x.Name, n => new PaymentStateEntity { Name = n }, new[]
        {
            "Pendiente", "Pagado", "Rechazado", "Reembolsado"
        });

        await EnsureNameListAsync(context, context.PaymentMethodTypes, x => x.Name, n => new PaymentMethodTypeEntity { Name = n }, new[]
        {
            "Efectivo", "Tarjeta", "Transferencia", "PSE"
        });

        await EnsureNameListAsync(context, context.CardTypes, x => x.Name, n => new CardTypeEntity { Name = n }, new[]
        {
            "Crédito", "Débito", "Prepago"
        });

        await EnsureNameListAsync(context, context.CardIssuers, x => x.Name, n => new CardIssuerEntity { Name = n }, new[]
        {
            "Visa", "Mastercard", "Amex", "Diners"
        });

        var types = await context.PaymentMethodTypes.AsNoTracking().ToListAsync();
        int TypeId(string name) => types.First(x => SeedHelpers.Normalize(x.Name) == SeedHelpers.Normalize(name)).Id;

        var cardTypes = await context.CardTypes.AsNoTracking().ToListAsync();
        int CardTypeId(string name) => cardTypes.First(x => SeedHelpers.Normalize(x.Name) == SeedHelpers.Normalize(name)).Id;

        var existing = await context.PaymentMethods.AsNoTracking().ToListAsync();

        var desired = new[]
        {
            new PaymentMethodEntity { PaymentMethodTypeId = TypeId("Efectivo"), CardTypeId = null, CardIssuerId = null, CommercialName = "Efectivo" },
            new PaymentMethodEntity { PaymentMethodTypeId = TypeId("Tarjeta"), CardTypeId = CardTypeId("Crédito"), CardIssuerId = null, CommercialName = "Tarjeta de crédito" },
            new PaymentMethodEntity { PaymentMethodTypeId = TypeId("Tarjeta"), CardTypeId = CardTypeId("Débito"), CardIssuerId = null, CommercialName = "Tarjeta débito" },
            new PaymentMethodEntity { PaymentMethodTypeId = TypeId("Transferencia"), CardTypeId = null, CardIssuerId = null, CommercialName = "Transferencia" },
            new PaymentMethodEntity { PaymentMethodTypeId = TypeId("PSE"), CardTypeId = null, CardIssuerId = null, CommercialName = "PSE" }
        };

        foreach (var pm in desired)
        {
            var norm = SeedHelpers.Normalize(pm.CommercialName);
            if (existing.Any(x => SeedHelpers.Normalize(x.CommercialName) == norm))
                continue;

            context.PaymentMethods.Add(pm);
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedAuthCatalogsAsync(AppDbContext context)
    {
        await AuthSeed.EnsureSystemRolesAsync(context);

        var rolesExisting = await context.SystemRoles.AsNoTracking().ToListAsync();
        var rolesDesired = new[]
        {
            new SystemRoleEntity { Name = "Admin", Description = "Acceso total al sistema" },
            new SystemRoleEntity { Name = "Agente", Description = "Operación de reservas, pagos, check-in" },
            new SystemRoleEntity { Name = "Cliente", Description = "Acceso limitado a consultas propias" }
        };

        foreach (var r in rolesDesired)
        {
            var norm = SeedHelpers.Normalize(r.Name);
            if (rolesExisting.Any(x => SeedHelpers.Normalize(x.Name) == norm))
                continue;
            context.SystemRoles.Add(r);
        }

        await context.SaveChangesAsync();

        var permsExisting = await context.Permissions.AsNoTracking().ToListAsync();
        var permsDesired = new[]
        {
            new PermissionEntity { Name = "FULL_ACCESS", Description = "Permiso total (modo escolar)" },
            new PermissionEntity { Name = "MANAGE_RESERVATIONS", Description = "Crear/consultar/actualizar reservas" },
            new PermissionEntity { Name = "MANAGE_PAYMENTS", Description = "Registrar y consultar pagos" },
            new PermissionEntity { Name = "MANAGE_CHECKINS", Description = "Registrar y consultar check-ins" }
        };

        foreach (var p in permsDesired)
        {
            var norm = SeedHelpers.Normalize(p.Name);
            if (permsExisting.Any(x => SeedHelpers.Normalize(x.Name) == norm))
                continue;
            context.Permissions.Add(p);
        }

        await context.SaveChangesAsync();

        var roles = await context.SystemRoles.AsNoTracking().ToListAsync();
        var perms = await context.Permissions.AsNoTracking().ToListAsync();

        int RoleId(string name) => roles.First(r => SeedHelpers.Normalize(r.Name) == SeedHelpers.Normalize(name)).Id;
        int PermId(string name) => perms.First(p => SeedHelpers.Normalize(p.Name) == SeedHelpers.Normalize(name)).Id;

        var admin = RoleId("Admin");
        var agente = RoleId("Agente");
        var cliente = RoleId("Cliente");

        var full = PermId("FULL_ACCESS");
        var res = PermId("MANAGE_RESERVATIONS");
        var pay = PermId("MANAGE_PAYMENTS");
        var chk = PermId("MANAGE_CHECKINS");

        var existingRp = await context.RolePermissions.AsNoTracking().ToListAsync();
        var desiredRp = new (int roleId, int permId)[]
        {
            (admin, full),
            (admin, res),
            (admin, pay),
            (admin, chk),
            (agente, res),
            (agente, pay),
            (agente, chk),
            (cliente, res)
        };

        foreach (var (roleId, permId) in desiredRp)
        {
            if (existingRp.Any(x => x.RoleId == roleId && x.PermissionId == permId))
                continue;

            context.RolePermissions.Add(new RolePermissionEntity { RoleId = roleId, PermissionId = permId });
        }

        await context.SaveChangesAsync();
    }

    private static async Task EnsureNameListAsync<TEntity>(
        AppDbContext context,
        DbSet<TEntity> set,
        Func<TEntity, string?> nameSelector,
        Func<string, TEntity> create,
        IEnumerable<string> desired)
        where TEntity : class
    {
        var existing = await set.AsNoTracking().ToListAsync();
        foreach (var value in desired)
        {
            var norm = SeedHelpers.Normalize(value);
            if (existing.Any(x => SeedHelpers.Normalize(nameSelector(x)) == norm))
                continue;

            set.Add(create(value));
        }

        await context.SaveChangesAsync();
    }
}
