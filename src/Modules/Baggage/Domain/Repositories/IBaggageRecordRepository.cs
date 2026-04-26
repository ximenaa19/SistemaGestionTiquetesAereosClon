using GestionAerolineas.src.Modules.Baggage.Domain.Aggregate;
using GestionAerolineas.src.Modules.Baggage.Domain.Models;

namespace GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

public interface IBaggageRecordRepository
{
    Task AddAsync(BaggageRecord record);
    Task<BaggageRegistrationContext?> GetRegistrationContextByTicketIdAsync(int ticketId);
    Task<BaggageRegistrationContext?> GetRegistrationContextByReservationIdAsync(int reservationId);
    Task<IReadOnlyList<CabinTypeOption>> GetCabinTypesAsync();
    Task<bool> CabinTypeExistsAsync(int cabinTypeId);
    Task AddSurchargeToReservationAsync(int reservationId, decimal surcharge);
    Task<IReadOnlyList<BaggageRecordView>> GetByCustomerIdAsync(int customerId);
    Task<IReadOnlyList<BaggageRecordView>> GetByFlightIdAsync(int flightId);
    Task<IReadOnlyList<BaggageRecordView>> GetWithSurchargesAsync();
}
