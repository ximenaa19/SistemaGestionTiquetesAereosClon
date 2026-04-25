// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PaymentMethods\Infrastructure\Entity\PaymentMethodEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.CardTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.PaymentMethodTypes.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.PaymentMethods.Infrastructure.Entity;

public sealed class PaymentMethodEntityConfiguration : IEntityTypeConfiguration<PaymentMethodEntity>
{
    public void Configure(EntityTypeBuilder<PaymentMethodEntity> builder)
    {
        builder.ToTable("paymentmethods");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.PaymentMethodTypeId)
            .HasColumnName("tipo_medio_pago_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.CardTypeId)
            .HasColumnName("tipo_tarjeta_id")
            .HasColumnType("int");

        builder
            .Property(x => x.CardIssuerId)
            .HasColumnName("emisor_tarjeta_id")
            .HasColumnType("int");

        builder
            .Property(x => x.CommercialName)
            .HasColumnName("nombre_comercial")
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.HasIndex(x => x.CommercialName).IsUnique();

        builder
            .HasOne<PaymentMethodTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.PaymentMethodTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<CardTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.CardTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<CardIssuerEntity>()
            .WithMany()
            .HasForeignKey(x => x.CardIssuerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

