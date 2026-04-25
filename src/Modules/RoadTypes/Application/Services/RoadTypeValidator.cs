// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\RoadTypes\Application\Services\RoadTypeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Text.RegularExpressions;
using GestionAerolineas.src.Modules.RoadTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.RoadTypes.Domain.Repositories;
using GestionAerolineas.src.Modules.RoadTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.RoadTypes.Application.Services;

public class RoadTypeValidator : IRoadTypeValidator
{
    private readonly IRoadTypeRepository _repository;

    public RoadTypeValidator(IRoadTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ValidateNameAsync(RoadTypeName name)
    {
        // 🔹 Validar que solo tenga caracteres alfanuméricos
        var regex = new Regex("^[a-zA-Z0-9]+$");

        if (!regex.IsMatch(name.Value))
        {
            throw new Exception("El nombre solo puede contener caracteres alfanuméricos");
        }

        // 🔹 Validar que no exista otro con el mismo nombre
        var existing = await _repository.GetByNameAsync(name);

        if (existing != null)
        {
            throw new Exception("Ya existe un RoadType con ese nombre");
        }
    }
}