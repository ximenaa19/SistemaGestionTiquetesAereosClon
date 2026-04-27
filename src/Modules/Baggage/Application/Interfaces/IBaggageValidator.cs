namespace GestionAerolineas.src.Modules.Baggage.Application.Interfaces;

public interface IBaggageValidator // define que validaciones debe tener el módulo de validación
{
    Task ValidateCabinTypeExistsAsync(int cabinTypeId);
    void ValidateRegistrationInput(string baggageType, int quantity, decimal totalWeightKg);
}
//se separa en una interfaz para que los casos de uso dependan de una abstraccion 
