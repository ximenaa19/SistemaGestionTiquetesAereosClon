using GestionAerolineas.src.Modules.CabinTypes.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Flights.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.ReservationPassengers.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Reservations.Infrastructure.Entity;
using GestionAerolineas.src.Modules.Tickets.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionAerolineas.src.Modules.Baggage.Infrastructure.Entity;

// configuracion de Entity Framework para crear y relacionar la tabla baggage_records
public sealed class BaggageRecordEntityConfiguration : IEntityTypeConfiguration<BaggageRecordEntity>
{
    public void Configure(EntityTypeBuilder<BaggageRecordEntity> builder)
    {
        // nombre fisico de la tabla en MySQL
        builder.ToTable("baggage_records");

        // llave primaria del registro de equipaje
        builder.HasKey(x => x.Id);

        // id autoincremental generado al insertar
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();

        // reserva obligatoria; todo equipaje debe pertenecer a una reserva
        builder.Property(x => x.ReservationId)
            .HasColumnName("reserva_id")
            .HasColumnType("int")
            .IsRequired();

        // tiquete opcional para registros hechos por tiquete
        builder.Property(x => x.TicketId)
            .HasColumnName("tiquete_id")
            .HasColumnType("int");

        // relacion opcional con el pasajero dentro de la reserva
        builder.Property(x => x.ReservationPassengerId)
            .HasColumnName("reserva_pasajero_id")
            .HasColumnType("int");

        // vuelo opcional asociado al registro
        builder.Property(x => x.FlightId)
            .HasColumnName("vuelo_id")
            .HasColumnType("int");

        // pasajero opcional asociado al equipaje
        builder.Property(x => x.PassengerId)
            .HasColumnName("pasajero_id")
            .HasColumnType("int");

        // cabina obligatoria porque define la politica de equipaje
        builder.Property(x => x.CabinTypeId)
            .HasColumnName("tipo_cabina_id")
            .HasColumnType("int")
            .IsRequired();

        // tipo de equipaje normalizado: MANO o BODEGA
        builder.Property(x => x.BaggageType)
            .HasColumnName("tipo_equipaje")
            .HasColumnType("varchar(30)")
            .IsRequired();

        // cantidad de maletas registradas
        builder.Property(x => x.Quantity)
            .HasColumnName("cantidad")
            .HasColumnType("int")
            .IsRequired();

        // peso total registrado en kg
        builder.Property(x => x.WeightKg)
            .HasColumnName("peso_kg")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // descripcion opcional del equipaje
        builder.Property(x => x.Description)
            .HasColumnName("descripcion")
            .HasColumnType("varchar(250)");

        // politica guardada como historico: cantidad permitida
        builder.Property(x => x.AllowedQuantity)
            .HasColumnName("cantidad_permitida")
            .HasColumnType("int")
            .IsRequired();

        // politica guardada como historico: peso por maleta
        builder.Property(x => x.AllowedWeightPerBagKg)
            .HasColumnName("peso_permitido_por_maleta_kg")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // politica guardada como historico: peso total permitido
        builder.Property(x => x.AllowedTotalWeightKg)
            .HasColumnName("peso_total_permitido_kg")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // exceso por cantidad calculado al registrar
        builder.Property(x => x.ExcessQuantity)
            .HasColumnName("exceso_cantidad")
            .HasColumnType("int")
            .IsRequired();

        // exceso por peso calculado al registrar
        builder.Property(x => x.ExcessWeightKg)
            .HasColumnName("exceso_peso_kg")
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // valor monetario por exceso de cantidad
        builder.Property(x => x.QuantitySurcharge)
            .HasColumnName("recargo_cantidad")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        // valor monetario por exceso de peso
        builder.Property(x => x.WeightSurcharge)
            .HasColumnName("recargo_peso")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        // recargo total aplicado a la reserva
        builder.Property(x => x.TotalSurcharge)
            .HasColumnName("recargo_total")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        // fecha con valor por defecto desde MySQL si no se envia una fecha
        builder.Property(x => x.RegisteredAt)
            .HasColumnName("fecha_registro")
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .IsRequired();

        // indices para acelerar consultas frecuentes por reserva, tiquete, vuelo y pasajero
        builder.HasIndex(x => x.ReservationId);
        builder.HasIndex(x => x.TicketId);
        builder.HasIndex(x => x.FlightId);
        builder.HasIndex(x => x.PassengerId);

        // relacion obligatoria con reservations; se restringe borrar reservas con equipaje
        builder.HasOne<ReservationEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        // relacion opcional con tickets; si se borra el tiquete, se conserva el historico del equipaje
        builder.HasOne<TicketEntity>()
            .WithMany()
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.SetNull);

        // relacion opcional con reservationpassengers
        builder.HasOne<ReservationPassengerEntity>()
            .WithMany()
            .HasForeignKey(x => x.ReservationPassengerId)
            .OnDelete(DeleteBehavior.SetNull);

        // relacion opcional con flights
        builder.HasOne<FlightEntity>()
            .WithMany()
            .HasForeignKey(x => x.FlightId)
            .OnDelete(DeleteBehavior.SetNull);

        // relacion opcional con passengers
        builder.HasOne<PassengerEntity>()
            .WithMany()
            .HasForeignKey(x => x.PassengerId)
            .OnDelete(DeleteBehavior.SetNull);

        // relacion obligatoria con CabinTypes; no se debe borrar una cabina usada por equipaje
        builder.HasOne<CabinTypeEntity>()
            .WithMany()
            .HasForeignKey(x => x.CabinTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
