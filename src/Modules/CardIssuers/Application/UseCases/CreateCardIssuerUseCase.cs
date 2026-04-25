// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardIssuers\Application\UseCases\CreateCardIssuerUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Application.Interfaces;
using GestionAerolineas.src.Modules.CardIssuers.Domain.Aggregate;
using GestionAerolineas.src.Modules.CardIssuers.Domain.Repositories;
using GestionAerolineas.src.Modules.CardIssuers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CardIssuers.Application.UseCases;

public class CreateCardIssuerUseCase
{
    private readonly ICardIssuerRepository _repository;
    private readonly ICardIssuerValidator _validator;

    public CreateCardIssuerUseCase(
        ICardIssuerRepository repository,
        ICardIssuerValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task ExecuteAsync(string name)
    {
        var nameVO = CardIssuerName.Create(name);

        await _validator.ValidateNameAsync(nameVO);

        var entity = CardIssuer.CreateNew(nameVO);

        await _repository.AddAsync(entity);
    }
}
