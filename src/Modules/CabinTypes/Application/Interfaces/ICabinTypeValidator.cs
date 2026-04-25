// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinTypes\Application\Interfaces\ICabinTypeValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using GestionAerolineas.src.Modules.CabinTypes.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CabinTypes.Application.Interfaces;

public interface ICabinTypeValidator
{
    Task ValidateNameAsync(CabinTypesName name);

}
