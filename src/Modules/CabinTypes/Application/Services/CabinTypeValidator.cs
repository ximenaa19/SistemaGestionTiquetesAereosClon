// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinTypes\Application\Services\CabinTypeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using System.Text.RegularExpressions;
using GestionAerolineas.src.Modules.CabinTypes.Application.Interfaces;
using GestionAerolineas.src.Modules.CabinTypes.Domain.Repository;
using GestionAerolineas.src.Modules.CabinTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinTypes.Application.Services;

public class CabinTypeValidator : ICabinTypeValidator
{
    private readonly ICabinTypeRepository _cabinTypeRepository;

    public CabinTypeValidator(ICabinTypeRepository cabinTypeRepository)
    {
        _cabinTypeRepository = cabinTypeRepository;
    }

    public async Task ValidateNameAsync(CabinTypesName name)
    {
       var regex = new Regex("^[a-zA-Z0-9]+$");

        if (!regex.IsMatch(name.Value))
        {
            throw new Exception("El nombre solo puede contener caracteres alfanuméricos");
        }

        // 🔹 Validar que no exista otro con el mismo nombre
        var existing = await _cabinTypeRepository.GetByNameAsync(name);

        if (existing != null)
        {
            throw new Exception("Ya existe un CabinType con ese nombre");
        }
    }

}
