using GestionAerolineas.src.Modules.Baggage.Application.Interfaces;
using GestionAerolineas.src.Modules.Baggage.Application.Services;
using GestionAerolineas.src.Modules.Baggage.Domain.Aggregate;
using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

// caso de uso principal: registra equipaje y suma recargos a la reserva si aplica
public class RegisterBaggageUseCase
{
    // repositorio para consultar contexto, guardar equipaje y actualizar reserva
    private readonly IBaggageRecordRepository _repository;
    // calculadora con las reglas de negocio de cantidad, peso y recargos
    private readonly BaggageSurchargeCalculator _calculator;
    // validador de entradas antes de crear el registro
    private readonly IBaggageValidator _validator;

    public RegisterBaggageUseCase(
        IBaggageRecordRepository repository,
        BaggageSurchargeCalculator calculator,
        IBaggageValidator validator)
    {
        // recibe las dependencias por constructor para mantener el caso de uso desacoplado
        _repository = repository;
        _calculator = calculator;
        _validator = validator;
    }

    public async Task<RegisterBaggageResult> ExecuteByTicketAsync(
        int ticketId,
        int cabinTypeId,
        string baggageType,
        int quantity,
        decimal totalWeightKg,
        string? description)
    {
        // valida que se haya enviado un tiquete valido
        if (ticketId <= 0)
            throw new ArgumentException("El tiquete es obligatorio.");

        // valida que la cabina exista y que tipo/cantidad/peso sean correctos
        await _validator.ValidateCabinTypeExistsAsync(cabinTypeId);
        _validator.ValidateRegistrationInput(baggageType, quantity, totalWeightKg);

        // busca la reserva, vuelo y pasajero relacionados con el tiquete
        var context = await _repository.GetRegistrationContextByTicketIdAsync(ticketId);
        if (context is null)
            throw new InvalidOperationException("No se encontro el tiquete o no esta relacionado con una reserva.");

        // reutiliza el flujo comun de registro con el contexto encontrado
        return await RegisterAsync(context, cabinTypeId, baggageType, quantity, totalWeightKg, description);
    }

    public async Task<RegisterBaggageResult> ExecuteByReservationAsync(
        int reservationId,
        int cabinTypeId,
        string baggageType,
        int quantity,
        decimal totalWeightKg,
        string? description)
    {
        // valida que se haya enviado una reserva valida
        if (reservationId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        // valida cabina, tipo de equipaje, cantidad y peso total
        await _validator.ValidateCabinTypeExistsAsync(cabinTypeId);
        _validator.ValidateRegistrationInput(baggageType, quantity, totalWeightKg);

        // obtiene el contexto de la reserva antes de guardar el equipaje
        var context = await _repository.GetRegistrationContextByReservationIdAsync(reservationId);
        if (context is null)
            throw new InvalidOperationException("No se encontro la reserva.");

        // ejecuta el registro usando el mismo flujo usado por tiquete
        return await RegisterAsync(context, cabinTypeId, baggageType, quantity, totalWeightKg, description);
    }

    private async Task<RegisterBaggageResult> RegisterAsync(
        BaggageRegistrationContext context,
        int cabinTypeId,
        string baggageType,
        int quantity,
        decimal totalWeightKg,
        string? description)
    {
        // obtiene cabinas para recuperar el nombre de la cabina seleccionada
        var cabinTypes = await _repository.GetCabinTypesAsync();
        var cabin = cabinTypes.FirstOrDefault(c => c.Id == cabinTypeId);
        if (cabin is null)
            throw new ArgumentException("La clase/cabina seleccionada no existe.");

        // normaliza el tipo para guardar siempre MANO o BODEGA
        var normalizedType = BaggageSurchargeCalculator.NormalizeBaggageType(baggageType);
        // calcula politica aplicada, excesos y recargos
        var surcharge = _calculator.Calculate(cabin.Name, normalizedType, quantity, totalWeightKg);
        // guarda el total anterior para mostrarlo en la respuesta
        var previousTotal = context.CurrentReservationTotal;

        // crea el agregado de dominio con contexto, datos capturados y resultado del calculo
        var record = BaggageRecord.CreateNew(
            context.ReservationId,
            context.TicketId,
            context.ReservationPassengerId,
            context.FlightId,
            context.PassengerId,
            cabinTypeId,
            normalizedType,
            quantity,
            totalWeightKg,
            description,
            surcharge.Policy.AllowedQuantity,
            surcharge.Policy.AllowedWeightPerBagKg,
            surcharge.Policy.AllowedTotalWeightKg,
            surcharge.ExcessQuantity,
            surcharge.ExcessWeightKg,
            surcharge.QuantitySurcharge,
            surcharge.WeightSurcharge,
            surcharge.TotalSurcharge);

        // persiste el registro en la tabla baggage_records
        await _repository.AddAsync(record);

        // si hubo recargo, lo suma al valor total de la reserva
        if (surcharge.TotalSurcharge > 0)
            await _repository.AddSurchargeToReservationAsync(context.ReservationId, surcharge.TotalSurcharge);

        // devuelve datos completos para que la UI muestre confirmacion y nuevo total
        return new RegisterBaggageResult(
            record,
            context,
            surcharge,
            previousTotal,
            previousTotal + surcharge.TotalSurcharge);
    }
}
