using GestionAerolineas.src.Modules.Baggage.Application.Interfaces;
using GestionAerolineas.src.Modules.Baggage.Application.Services;
using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

public class PreviewBaggageSurchargeUseCase
{
    private readonly IBaggageRecordRepository _repository;
    private readonly BaggageSurchargeCalculator _calculator;
    private readonly IBaggageValidator _validator;

    public PreviewBaggageSurchargeUseCase(
        IBaggageRecordRepository repository,
        BaggageSurchargeCalculator calculator,
        IBaggageValidator validator)
    {
        _repository = repository;
        _calculator = calculator;
        _validator = validator;
    }

    public async Task<BaggageSurchargeResult> ExecuteAsync(int cabinTypeId, string baggageType, int quantity, decimal totalWeightKg)
    {
        await _validator.ValidateCabinTypeExistsAsync(cabinTypeId);
        _validator.ValidateRegistrationInput(baggageType, quantity, totalWeightKg);

        var cabinTypes = await _repository.GetCabinTypesAsync();
        var cabin = cabinTypes.FirstOrDefault(c => c.Id == cabinTypeId);

        return _calculator.Calculate(cabin!.Name, baggageType, quantity, totalWeightKg);
    }
}
