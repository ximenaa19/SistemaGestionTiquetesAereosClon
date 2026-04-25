// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PassengerTypes\Application\UseCases\CreatePassengerTypeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PassengerTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Aggregate;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.PassengerTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;

public class CreatePassengerTypeUseCase
{
    private readonly IPassengerTypeRepository _repository;
    private readonly IPassengerTypeValidator _validator;

    public CreatePassengerTypeUseCase(
        IPassengerTypeRepository repository,
        IPassengerTypeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name, int? ageMin, int? ageMax)
    {
        ValidateAges(ageMin, ageMax);

        var nameVO = PassengerTypeName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var entity = PassengerType.CreateNew(nameVO, ageMin, ageMax);

        await _repository.AddAsync(entity);
    }

    private static void ValidateAges(int? ageMin, int? ageMax)
    {
        if (ageMin is < 0)
            throw new Exception("edad_min no puede ser negativa");

        if (ageMax is < 0)
            throw new Exception("edad_max no puede ser negativa");

        if (ageMin.HasValue && ageMax.HasValue && ageMin.Value > ageMax.Value)
            throw new Exception("edad_min no puede ser mayor que edad_max");
    }
}

