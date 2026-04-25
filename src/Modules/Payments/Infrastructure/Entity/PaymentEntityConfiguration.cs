// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Payments\Infrastructure\Entity\PaymentEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PaymentMethods.Infrastructure.Entity;
using GestionAerolineas.src.Modules.PaymentStates.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Payments.Infrastructure.Entity;

public sealed class PaymentEntityConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.ReservationId)
            .HasColumnName("reserva_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.Amount)
            .HasColumnName("monto")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder
            .Property(x => x.PaidAt)
            .HasColumnName("fecha_pago")
            .HasColumnType("datetime")
            .IsRequired();

        builder
            .Property(x => x.StateId)
            .HasColumnName("estado_pago_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.MethodId)
            .HasColumnName("metodo_pago_id")
            .HasColumnType("int")
            .IsRequired();

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
            .HasOne<ReservationEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<PaymentStateEntity>()
            .WithMany()
            .HasForeignKey(x => x.StateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<PaymentMethodEntity>()
            .WithMany()
            .HasForeignKey(x => x.MethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

