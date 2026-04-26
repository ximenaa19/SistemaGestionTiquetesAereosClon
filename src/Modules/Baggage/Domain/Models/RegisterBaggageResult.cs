using GestionAerolineas.src.Modules.Baggage.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Baggage.Domain.Models;

public sealed record RegisterBaggageResult(
    BaggageRecord Record,
    BaggageRegistrationContext Context,
    BaggageSurchargeResult Surcharge,
    decimal PreviousReservationTotal,
    decimal NewReservationTotal);
