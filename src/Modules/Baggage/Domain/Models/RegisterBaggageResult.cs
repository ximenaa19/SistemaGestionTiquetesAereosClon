using GestionAerolineas.src.Modules.Baggage.Domain.Aggregate;

namespace GestionAerolineas.src.Modules.Baggage.Domain.Models;
// resultado final despues de registrar equipaje
public sealed record RegisterBaggageResult(
    BaggageRecord Record, // registro de equipaje
    BaggageRegistrationContext Context, // datos de registro
    BaggageSurchargeResult Surcharge, // recargos aplicados
    decimal PreviousReservationTotal, // total anterior reserva
    decimal NewReservationTotal); // nuevo total de la reserva
