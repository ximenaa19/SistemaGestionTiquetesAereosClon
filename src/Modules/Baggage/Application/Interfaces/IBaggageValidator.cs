namespace GestionAerolineas.src.Modules.Baggage.Application.Interfaces;

public interface IBaggageValidator
{
    Task ValidateCabinTypeExistsAsync(int cabinTypeId);
    void ValidateRegistrationInput(string baggageType, int quantity, decimal totalWeightKg);
}
