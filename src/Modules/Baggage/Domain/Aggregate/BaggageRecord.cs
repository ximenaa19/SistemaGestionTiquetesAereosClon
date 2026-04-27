using GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Baggage.Domain.Aggregate;

public class BaggageRecord
{
    public BaggageId Id { get; private set; }
    public BaggageReservationId ReservationId { get; private set; }
    public int? TicketId { get; private set; } 
    public int? ReservationPassengerId { get; private set; }
    public int? FlightId { get; private set; }
    public int? PassengerId { get; private set; }
    public BaggageCabinTypeId CabinTypeId { get; private set; }
    public BaggageType BaggageType { get; private set; }
    public BaggageQuantity Quantity { get; private set; }
    public BaggageWeightKg WeightKg { get; private set; }
    public string? Description { get; private set; }

    // politica aplicada en el registro
    public int AllowedQuantity { get; private set; } // cantidad permitida
    public decimal AllowedWeightPerBagKg { get; private set; }// peso permitido por maleta
    public decimal AllowedTotalWeightKg { get; private set; } // peso total permitido
    public int ExcessQuantity { get; private set; } // cuantas maletas exceden la cantidad permitida
    public decimal ExcessWeightKg { get; private set; } // cuantos kg exceden el peso permitido (total)
    public BaggageSurchargeAmount QuantitySurcharge { get; private set; } // recargo por cantidad
    public BaggageSurchargeAmount WeightSurcharge { get; private set; } // recargo por peso
    public BaggageSurchargeAmount TotalSurcharge { get; private set; } // recargo total
    public BaggageRegisteredAt RegisteredAt { get; private set; } // fecha de registro

    // constructor privado para forzar el uso de los métodos de creación
    private BaggageRecord(
        BaggageId id,
        BaggageReservationId reservationId,
        int? ticketId,
        int? reservationPassengerId,
        int? flightId,
        int? passengerId,
        BaggageCabinTypeId cabinTypeId,
        BaggageType baggageType,
        BaggageQuantity quantity,
        BaggageWeightKg weightKg,
        string? description,
        int allowedQuantity,
        decimal allowedWeightPerBagKg,
        decimal allowedTotalWeightKg,
        int excessQuantity,
        decimal excessWeightKg,
        BaggageSurchargeAmount quantitySurcharge,
        BaggageSurchargeAmount weightSurcharge,
        BaggageSurchargeAmount totalSurcharge,
        BaggageRegisteredAt registeredAt)
    {
        Id = id;
        ReservationId = reservationId;
        TicketId = ticketId;
        ReservationPassengerId = reservationPassengerId;
        FlightId = flightId;
        PassengerId = passengerId;
        CabinTypeId = cabinTypeId;
        BaggageType = baggageType;
        Quantity = quantity;
        WeightKg = weightKg;
        Description = description;
        AllowedQuantity = allowedQuantity;
        AllowedWeightPerBagKg = allowedWeightPerBagKg;
        AllowedTotalWeightKg = allowedTotalWeightKg;
        ExcessQuantity = excessQuantity;
        ExcessWeightKg = excessWeightKg;
        QuantitySurcharge = quantitySurcharge;
        WeightSurcharge = weightSurcharge;
        TotalSurcharge = totalSurcharge;
        RegisteredAt = registeredAt;
    }

    // crea un registro nuevo con validación de negocio
    public static BaggageRecord Create(
        int id,
        int reservationId,
        int? ticketId,
        int? reservationPassengerId,
        int? flightId,
        int? passengerId,
        int cabinTypeId,
        string baggageType,
        int quantity,
        decimal weightKg,
        string? description,
        int allowedQuantity,
        decimal allowedWeightPerBagKg,
        decimal allowedTotalWeightKg,
        int excessQuantity,
        decimal excessWeightKg,
        decimal quantitySurcharge,
        decimal weightSurcharge,
        decimal totalSurcharge,
        DateTime registeredAt)
    {
        return new BaggageRecord(
            id <= 0 ? BaggageId.CreateEmpty() : BaggageId.Create(id), // crea un id si no se le pasa uno
            BaggageReservationId.Create(reservationId),
            ticketId,
            reservationPassengerId,
            flightId,
            passengerId,
            BaggageCabinTypeId.Create(cabinTypeId),
            BaggageType.Create(baggageType),
            BaggageQuantity.Create(quantity),
            BaggageWeightKg.Create(weightKg),
            string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            allowedQuantity,
            decimal.Round(allowedWeightPerBagKg, 2), // redondea los valores de los decimales
            decimal.Round(allowedTotalWeightKg, 2), // para evitar errores de redondeo
            Math.Max(0, excessQuantity), // si el exceso es negativo lo convierte a cero
            decimal.Round(Math.Max(0, excessWeightKg), 2),
            BaggageSurchargeAmount.Create(quantitySurcharge),
            BaggageSurchargeAmount.Create(weightSurcharge),
            BaggageSurchargeAmount.Create(totalSurcharge),
            BaggageRegisteredAt.Create(registeredAt));
    }
     // crea un registro nuevo sin validación de negocio, para casos donde ya se han calculado los valores y solo se quiere crear el objeto
    public static BaggageRecord CreateNew(
        int reservationId,
        int? ticketId,
        int? reservationPassengerId,
        int? flightId,
        int? passengerId,
        int cabinTypeId,
        string baggageType,
        int quantity,
        decimal weightKg,
        string? description,
        int allowedQuantity,
        decimal allowedWeightPerBagKg,
        decimal allowedTotalWeightKg,
        int excessQuantity,
        decimal excessWeightKg,
        decimal quantitySurcharge,
        decimal weightSurcharge,
        decimal totalSurcharge)
    {
        return Create(
            0,
            reservationId,
            ticketId,
            reservationPassengerId,
            flightId,
            passengerId,
            cabinTypeId,
            baggageType,
            quantity,
            weightKg,
            description,
            allowedQuantity,
            allowedWeightPerBagKg,
            allowedTotalWeightKg,
            excessQuantity,
            excessWeightKg,
            quantitySurcharge,
            weightSurcharge,
            totalSurcharge,
            DateTime.Now);
    }
}
