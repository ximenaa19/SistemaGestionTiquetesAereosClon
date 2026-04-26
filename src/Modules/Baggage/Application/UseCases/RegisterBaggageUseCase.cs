using GestionAerolineas.src.Modules.Baggage.Application.Interfaces;
using GestionAerolineas.src.Modules.Baggage.Application.Services;
using GestionAerolineas.src.Modules.Baggage.Domain.Aggregate;
using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

public class RegisterBaggageUseCase
{
    private readonly IBaggageRecordRepository _repository;
    private readonly BaggageSurchargeCalculator _calculator;
    private readonly IBaggageValidator _validator;

    public RegisterBaggageUseCase(
        IBaggageRecordRepository repository,
        BaggageSurchargeCalculator calculator,
        IBaggageValidator validator)
    {
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
        if (ticketId <= 0)
            throw new ArgumentException("El tiquete es obligatorio.");

        await _validator.ValidateCabinTypeExistsAsync(cabinTypeId);
        _validator.ValidateRegistrationInput(baggageType, quantity, totalWeightKg);

        var context = await _repository.GetRegistrationContextByTicketIdAsync(ticketId);
        if (context is null)
            throw new InvalidOperationException("No se encontro el tiquete o no esta relacionado con una reserva.");

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
        if (reservationId <= 0)
            throw new ArgumentException("La reserva es obligatoria.");

        await _validator.ValidateCabinTypeExistsAsync(cabinTypeId);
        _validator.ValidateRegistrationInput(baggageType, quantity, totalWeightKg);

        var context = await _repository.GetRegistrationContextByReservationIdAsync(reservationId);
        if (context is null)
            throw new InvalidOperationException("No se encontro la reserva.");

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
        var cabinTypes = await _repository.GetCabinTypesAsync();
        var cabin = cabinTypes.FirstOrDefault(c => c.Id == cabinTypeId);
        if (cabin is null)
            throw new ArgumentException("La clase/cabina seleccionada no existe.");

        var normalizedType = BaggageSurchargeCalculator.NormalizeBaggageType(baggageType);
        var surcharge = _calculator.Calculate(cabin.Name, normalizedType, quantity, totalWeightKg);
        var previousTotal = context.CurrentReservationTotal;

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

        await _repository.AddAsync(record);

        if (surcharge.TotalSurcharge > 0)
            await _repository.AddSurchargeToReservationAsync(context.ReservationId, surcharge.TotalSurcharge);

        return new RegisterBaggageResult(
            record,
            context,
            surcharge,
            previousTotal,
            previousTotal + surcharge.TotalSurcharge);
    }
}
