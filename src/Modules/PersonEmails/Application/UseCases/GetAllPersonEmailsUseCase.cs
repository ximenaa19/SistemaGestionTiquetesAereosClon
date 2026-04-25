// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\PersonEmails\Application\UseCases\GetAllPersonEmailsUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.PersonEmails.Domain.Aggregate;
using GestionAerolineas.src.Modules.PersonEmails.Domain.Repositories;

namespace GestionAerolineas.src.Modules.PersonEmails.Application.UseCases;

public class GetAllPersonEmailsUseCase
{
    private readonly IPersonEmailRepository _repository;

    public GetAllPersonEmailsUseCase(IPersonEmailRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<PersonEmail>> ExecuteAsync()
    {
        return _repository.GetAllAsync();
    }
}

