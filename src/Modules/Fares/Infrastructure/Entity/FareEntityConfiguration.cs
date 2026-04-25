// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Fares\Infrastructure\Entity\FareEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.PassengerTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Routes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Seasons.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Fares.Infrastructure.Entity;

public sealed class FareEntityConfiguration : IEntityTypeConfiguration<FareEntity>
{
    public void Configure(EntityTypeBuilder<FareEntity> builder)
    {
        builder.ToTable("fares");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.RouteId)
            .HasColumnName("ruta_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.CabinTypeId)
            .HasColumnName("tipo_cabina_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.PassengerTypeId)
            .HasColumnName("tipo_pasajero_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.SeasonId)
            .HasColumnName("temporada_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.BasePrice)
            .HasColumnName("precio_base")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder
            .Property(x => x.ValidFrom)
            .HasColumnName("vigencia_desde")
            .HasColumnType("date");

        builder
            .Property(x => x.ValidUntil)
            .HasColumnName("vigencia_hasta")
            .HasColumnType("date");

        builder
            .HasIndex(x => new { x.RouteId, x.CabinTypeId, x.PassengerTypeId, x.SeasonId });

        builder
            .HasOne<RouteEntity>()
            .WithMany()
            .HasForeignKey(x => x.RouteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<CabinTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.CabinTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<PassengerTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.PassengerTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<SeasonEntity>()
            .WithMany()
            .HasForeignKey(x => x.SeasonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

