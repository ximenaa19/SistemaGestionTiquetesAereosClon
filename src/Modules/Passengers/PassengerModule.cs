// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Passengers\PassengerModule.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Passengers.Application.Interfaces;
using GestionAerolineas.src.Modules.Passengers.Application.Services;
using GestionAerolineas.src.Modules.Passengers.Application.UseCases;
using GestionAerolineas.src.Modules.Passengers.Infrastructure.Repository;
using GestionAerolineas.src.Modules.Passengers.UI;
using GestionAerolineas.src.Modules.PassengerTypes.Application.UseCases;
using GestionAerolineas.src.Modules.PassengerTypes.Infrastructure.Repository;
using GestionAerolineas.src.Modules.People.Application.UseCases;
using GestionAerolineas.src.Modules.People.Infrastructure.Repository;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.Modules.Passengers;

public static class PassengerModule
{
    public static PassengerMenu Build(AppDbContext context)
    {
        var repository = new PassengerRepository(context);

        var personRepository = new PersonRepository(context);
        var passengerTypeRepository = new PassengerTypeRepository(context);

        IPassengerValidator validator = new PassengerValidator(repository, personRepository, passengerTypeRepository);

        var create = new CreatePassengerUseCase(repository, validator);
        var getAll = new GetAllPassengersUseCase(repository);
        var getById = new GetPassengerByIdUseCase(repository);
        var getByPersonId = new GetPassengerByPersonIdUseCase(repository);
        var getByPersonName = new GetPassengerByPersonNameUseCase(repository);
        var update = new UpdatePassengerUseCase(repository, validator);
        var delete = new DeletePassengerUseCase(repository);

        var getAllPeople = new GetAllPeopleUseCase(personRepository);
        var getAllPassengerTypes = new GetAllPassengerTypesUseCase(passengerTypeRepository);

        return new PassengerMenu(
            create,
            getAll,
            getById,
            getByPersonId,
            getByPersonName,
            update,
            delete,
            getAllPeople,
            getAllPassengerTypes
        );
    }
}
