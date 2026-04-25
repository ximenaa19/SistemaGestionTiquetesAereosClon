// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Invoices\Infrastructure\Entity\InvoiceEntityConfiguration.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Invoices.Infrastructure.Entity;

public sealed class InvoiceEntityConfiguration : IEntityTypeConfiguration<InvoiceEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceEntity> builder)
    {
        builder.ToTable("invoices");

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
            .Property(x => x.InvoiceNumber)
            .HasColumnName("numero_factura")
            .HasColumnType("varchar(30)")
            .IsRequired();

        builder
            .Property(x => x.IssuedAt)
            .HasColumnName("fecha_emision")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder
            .Property(x => x.Subtotal)
            .HasColumnName("subtotal")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .Property(x => x.Taxes)
            .HasColumnName("impuestos")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .Property(x => x.Total)
            .HasColumnName("total")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("creado_en")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.HasIndex(x => x.ReservationId).IsUnique();
        builder.HasIndex(x => x.InvoiceNumber).IsUnique();

        builder
            .HasOne<ReservationEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
