using GestionAerolineas.src.Modules.Baggage.Domain.Aggregate;
using GestionAerolineas.src.Modules.Baggage.Domain.Models;

namespace GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

public interface IBaggageRecordRepository // repositorio para manejar los registros de equipaje y los datos relacionados necesarios para el proceso de registro
{
    Task AddAsync(BaggageRecord record); // guarda un registro nuevo
    Task<BaggageRecord?> GetByIdAsync(int id); // recupera un registro por id
    Task<BaggageRegistrationContext?> GetRegistrationContextByTicketIdAsync(int ticketId); // recupera el contexto de registro por id de tiquete
    Task<BaggageRegistrationContext?> GetRegistrationContextByReservationIdAsync(int reservationId); // recupera el contexto de registro por id de reserva
    Task<IReadOnlyList<CabinTypeOption>> GetCabinTypesAsync(); // recupera las opciones de clase/cabina para mostrar en el formulario
    Task<bool> CabinTypeExistsAsync(int cabinTypeId); // comprueba si la clase/cabina existe
    Task AddSurchargeToReservationAsync(int reservationId, decimal surcharge); // agrega un recargo al total de la reserva, se llama despues de registrar el equipaje para actualizar el total
    Task<IReadOnlyList<BaggageRecordView>> GetByCustomerIdAsync(int customerId); // recupera los registros de equipaje de un cliente, se puede usar para mostrar el historial de equipajes registrados
    Task<IReadOnlyList<BaggageRecordView>> GetByFlightIdAsync(int flightId); // recupera los registros de equipaje de un vuelo, se puede usar para mostrar el equipaje registrado en un vuelo específico
    Task<IReadOnlyList<BaggageRecordView>> GetWithSurchargesAsync(); // recupera los registros de equipaje con los recargos aplicados, se puede usar para mostrar un reporte de equipajes y recargos
}
