using GestionAerolineas.src.Modules.Baggage.Application.Interfaces;
using GestionAerolineas.src.Modules.Baggage.Application.Services;
using GestionAerolineas.src.Modules.Baggage.Domain.Models;
using GestionAerolineas.src.Modules.Baggage.Domain.Repositories;

namespace GestionAerolineas.src.Modules.Baggage.Application.UseCases;

// caso de uso para calcular el recargo antes de guardar el registro
public class PreviewBaggageSurchargeUseCase
{
    // repositorio para validar y obtener el nombre de la cabina
    private readonly IBaggageRecordRepository _repository;
    // servicio que contiene las reglas de negocio del calculo
    private readonly BaggageSurchargeCalculator _calculator;
    // validador de cabina, tipo de equipaje, cantidad y peso
    private readonly IBaggageValidator _validator;

    public PreviewBaggageSurchargeUseCase(
        IBaggageRecordRepository repository,
        BaggageSurchargeCalculator calculator,
        IBaggageValidator validator)
    {
        // guarda dependencias para que el caso de uso coordine validacion y calculo
        _repository = repository;
        _calculator = calculator;
        _validator = validator;
    }

    public async Task<BaggageSurchargeResult> ExecuteAsync(int cabinTypeId, string baggageType, int quantity, decimal totalWeightKg)
    {
        // valida que la cabina exista en base de datos
        await _validator.ValidateCabinTypeExistsAsync(cabinTypeId);
        // valida tipo de equipaje, cantidad y peso usando value objects
        _validator.ValidateRegistrationInput(baggageType, quantity, totalWeightKg);

        // obtiene la lista de cabinas para recuperar el nombre de la seleccionada
        var cabinTypes = await _repository.GetCabinTypesAsync();
        var cabin = cabinTypes.FirstOrDefault(c => c.Id == cabinTypeId);

        // calcula el recargo sin guardar nada; se usa para mostrar confirmacion al usuario
        return _calculator.Calculate(cabin!.Name, baggageType, quantity, totalWeightKg);
    }
}
