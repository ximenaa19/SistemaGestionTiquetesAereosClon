// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Countries\Application\UseCases\GetCountryByIsoCodeUseCase.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.Countries.Domain.Aggregate;
using GestionAerolineas.src.Modules.Countries.Domain.Repositories;
using GestionAerolineas.src.Modules.Countries.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Countries.Application.UseCases;

public class GetCountryByIsoCodeUseCase
{
    private readonly ICountryRepository _repository;

    public GetCountryByIsoCodeUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public Task<Country?> ExecuteAsync(string isoCode)
    {
        var isoVO = CountryCodigoIso.Create(isoCode);
        return _repository.GetByIsoCodeAsync(isoVO);
    }
}

