using GestionAerolineas.src.Modules.Baggage.Application.Interfaces;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;
using GestionAerolineas.src.Modules.Baggage.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Baggage.Application.Services;

public class BaggageValidator : IBaggageValidator
{
    private readonly IBaggageRecordRepository _repository;

    public BaggageValidator(IBaggageRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateCabinTypeExistsAsync(int cabinTypeId)
    {
        if (cabinTypeId <= 0)
            throw new ArgumentException("La clase/cabina es obligatoria."); // valida forma 

        var exists = await _repository.CabinTypeExistsAsync(cabinTypeId);
        if (!exists)
            throw new ArgumentException("La clase/cabina seleccionada no existe."); // valida existencia en la base de datos
    }

    public void ValidateRegistrationInput(string baggageType, int quantity, decimal totalWeightKg) // usa value object. si algun dato es invalido, lanza excepcion
    {
        BaggageType.Create(baggageType);
        BaggageQuantity.Create(quantity);
        BaggageWeightKg.Create(totalWeightKg);
    }
}
 