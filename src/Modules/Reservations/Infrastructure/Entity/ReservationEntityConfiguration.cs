// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Reservations\Infrastructure\Entity\ReservationEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Customers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationStatuses.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;

public sealed class ReservationEntityConfiguration : IEntityTypeConfiguration<ReservationEntity>
{
    public void Configure(EntityTypeBuilder<ReservationEntity> builder)
    {
        builder.ToTable("reservations");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.Code)
            .HasColumnName("codigo_reserva")
            .HasColumnType("varchar(30)");

        builder.HasIndex(x => x.Code).IsUnique();

        builder
            .Property(x => x.CustomerId)
            .HasColumnName("cliente_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.ReservedAt)
            .HasColumnName("fecha_reserva")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.StatusId)
            .HasColumnName("estado_reserva_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.TotalAmount)
            .HasColumnName("valor_total")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder
            .Property(x => x.ExpiresAt)
            .HasColumnName("vence_en")
            .HasColumnType("datetime");

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("creado_en")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("actualizado_en")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();

        builder
            .HasOne<CustomerEntity>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<ReservationStatusEntity>()
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

