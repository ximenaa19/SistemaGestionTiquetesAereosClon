// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\ReservationStatusTransitions\Infrastructure\Entity\ReservationStatusTransitionEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.ReservationStatusTransitions.Infrastructure.Entity;

public sealed class ReservationStatusTransitionEntityConfiguration : IEntityTypeConfiguration<ReservationStatusTransitionEntity>
{
    public void Configure(EntityTypeBuilder<ReservationStatusTransitionEntity> builder)
    {
        builder.ToTable("transiciones_estado_reserva");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.OriginStatusId)
            .HasColumnName("estado_origen_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.DestinationStatusId)
            .HasColumnName("estado_destino_id")
            .HasColumnType("int")
            .IsRequired();

        builder.HasIndex(x => new { x.OriginStatusId, x.DestinationStatusId }).IsUnique();

        builder
            .HasOne<ReservationStatusEntity>()
            .WithMany()
            .HasForeignKey(x => x.OriginStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<ReservationStatusEntity>()
            .WithMany()
            .HasForeignKey(x => x.DestinationStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
