namespace GestionAerolineas.src.Modules.Baggage.Infrastructure.Entity;

public class BaggageRecordEntity
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public int? TicketId { get; set; }
    public int? ReservationPassengerId { get; set; }
    public int? FlightId { get; set; }
    public int? PassengerId { get; set; }
    public int CabinTypeId { get; set; }
    public string? BaggageType { get; set; }
    public int Quantity { get; set; }
    public decimal WeightKg { get; set; }
    public string? Description { get; set; }
    public int AllowedQuantity { get; set; }
    public decimal AllowedWeightPerBagKg { get; set; }
    public decimal AllowedTotalWeightKg { get; set; }
    public int ExcessQuantity { get; set; }
    public decimal ExcessWeightKg { get; set; }
    public decimal QuantitySurcharge { get; set; }
    public decimal WeightSurcharge { get; set; }
    public decimal TotalSurcharge { get; set; }
    public DateTime RegisteredAt { get; set; }
}
