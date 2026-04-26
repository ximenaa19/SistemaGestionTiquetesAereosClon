using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Baggage.Infrastructure.Entity;

public sealed class BaggageRecordEntityConfiguration : IEntityTypeConfiguration<BaggageRecordEntity>
{
    public void Configure(EntityTypeBuilder<BaggageRecordEntity> builder)
    {
        builder.ToTable("baggage_records");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.ReservationId)
            .HasColumnName("reserva_id")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.TicketId)
            .HasColumnName("tiquete_id")
            .HasColumnType("int");

        builder.Property(x => x.ReservationPassengerId)
            .HasColumnName("reserva_pasajero_id")
            .HasColumnType("int");

        builder.Property(x => x.FlightId)
            .HasColumnName("vuelo_id")
            .HasColumnType("int");

        builder.Property(x => x.PassengerId)
            .HasColumnName("pasajero_id")
            .HasColumnType("int");

        builder.Property(x => x.CabinTypeId)
            .HasColumnName("tipo_cabina_id")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.BaggageType)
            .HasColumnName("tipo_equipaje")
            .HasColumnType("varchar(30)")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("cantidad")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.WeightKg)
            .HasColumnName("peso_kg")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("descripcion")
            .HasColumnType("varchar(250)");

        builder.Property(x => x.AllowedQuantity)
            .HasColumnName("cantidad_permitida")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.AllowedWeightPerBagKg)
            .HasColumnName("peso_permitido_por_maleta_kg")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.AllowedTotalWeightKg)
            .HasColumnName("peso_total_permitido_kg")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.ExcessQuantity)
            .HasColumnName("exceso_cantidad")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.ExcessWeightKg)
            .HasColumnName("exceso_peso_kg")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(x => x.QuantitySurcharge)
            .HasColumnName("recargo_cantidad")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.WeightSurcharge)
            .HasColumnName("recargo_peso")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.TotalSurcharge)
            .HasColumnName("recargo_total")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.RegisteredAt)
            .HasColumnName("fecha_registro")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasIndex(x => x.ReservationId);
        builder.HasIndex(x => x.TicketId);
        builder.HasIndex(x => x.FlightId);
        builder.HasIndex(x => x.PassengerId);

        builder.HasOne<ReservationEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<TicketEntity>()
            .WithMany()
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<ReservationPassengerEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReservationPassengerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<FlightEntity>()
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<PassengerEntity>()
            .WithMany()
            .HasForeignKey(x => x.PassengerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<CabinTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.CabinTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
