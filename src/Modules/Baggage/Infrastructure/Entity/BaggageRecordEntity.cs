namespace GestionAerolineas.src.Modules.Baggage.Infrastructure.Entity;

// entidad plana que Entity Framework usa para mapear la tabla baggage_records
public class BaggageRecordEntity
{
    // llave primaria generada por la base de datos
    public int Id { get; set; }
    // reserva obligatoria a la que pertenece el equipaje
    public int ReservationId { get; set; }
    // tiquete opcional; existe cuando el registro se hace por tiquete
    public int? TicketId { get; set; }
    // relacion opcional entre reserva y pasajero
    public int? ReservationPassengerId { get; set; }
    // vuelo opcional asociado al equipaje
    public int? FlightId { get; set; }
    // pasajero opcional asociado al equipaje
    public int? PassengerId { get; set; }
    // cabina/clase usada para calcular la politica
    public int CabinTypeId { get; set; }
    // tipo normalizado: MANO o BODEGA
    public string? BaggageType { get; set; }
    // cantidad de maletas registradas
    public int Quantity { get; set; }
    // peso total registrado en kilogramos
    public decimal WeightKg { get; set; }
    // observacion opcional escrita por el usuario
    public string? Description { get; set; }
    // cantidad permitida por la politica aplicada al momento del registro
    public int AllowedQuantity { get; set; }
    // peso permitido por cada maleta segun la politica
    public decimal AllowedWeightPerBagKg { get; set; }
    // peso total permitido segun la politica
    public decimal AllowedTotalWeightKg { get; set; }
    // cantidad de maletas que exceden lo permitido
    public int ExcessQuantity { get; set; }
    // kilos excedidos despues de aplicar reglas de peso
    public decimal ExcessWeightKg { get; set; }
    // valor cobrado por maletas adicionales
    public decimal QuantitySurcharge { get; set; }
    // valor cobrado por exceso de peso
    public decimal WeightSurcharge { get; set; }
    // suma de recargo por cantidad y recargo por peso
    public decimal TotalSurcharge { get; set; }
    // fecha en la que se registro el equipaje
    public DateTime RegisteredAt { get; set; }
}
