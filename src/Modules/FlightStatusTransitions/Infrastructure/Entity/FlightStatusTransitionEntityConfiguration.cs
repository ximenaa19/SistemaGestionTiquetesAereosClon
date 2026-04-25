// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\FlightStatusTransitions\Infrastructure\Entity\FlightStatusTransitionEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.FlightStates.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.FlightStatusTransitions.Infrastructure.Entity;

public sealed class FlightStatusTransitionEntityConfiguration : IEntityTypeConfiguration<FlightStatusTransitionEntity>
{
    public void Configure(EntityTypeBuilder<FlightStatusTransitionEntity> builder)
    {
        builder.ToTable("flightstatustransitions");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.OriginStateId)
            .HasColumnName("estado_origen_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.DestinationStateId)
            .HasColumnName("estado_destino_id")
            .HasColumnType("int")
            .IsRequired();

        builder.HasIndex(x => new { x.OriginStateId, x.DestinationStateId }).IsUnique();

        builder
            .HasOne<FlightStateEntity>()
            .WithMany()
            .HasForeignKey(x => x.OriginStateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<FlightStateEntity>()
            .WithMany()
            .HasForeignKey(x => x.DestinationStateId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

