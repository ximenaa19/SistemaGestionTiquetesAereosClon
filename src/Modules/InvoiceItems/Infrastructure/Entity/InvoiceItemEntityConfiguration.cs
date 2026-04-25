// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\InvoiceItems\Infrastructure\Entity\InvoiceItemEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.InvoiceItemTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Invoices.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.InvoiceItems.Infrastructure.Entity;

public sealed class InvoiceItemEntityConfiguration : IEntityTypeConfiguration<InvoiceItemEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceItemEntity> builder)
    {
        builder.ToTable("invoiceitems");

        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder
            .Property(x => x.InvoiceId)
            .HasColumnName("factura_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.ItemTypeId)
            .HasColumnName("tipo_item_id")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasColumnName("descripcion")
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder
            .Property(x => x.Quantity)
            .HasColumnName("cantidad")
            .HasColumnType("int")
            .HasDefaultValue(1)
            .IsRequired();

        builder
            .Property(x => x.UnitPrice)
            .HasColumnName("precio_unitario")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder
            .Property(x => x.Subtotal)
            .HasColumnName("subtotal")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder
            .Property(x => x.ReservationPassengerId)
            .HasColumnName("reserva_pasajero_id")
            .HasColumnType("int");

        builder
            .HasOne<InvoiceEntity>()
            .WithMany()
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<InvoiceItemTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.ItemTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<ReservationPassengerEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReservationPassengerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

