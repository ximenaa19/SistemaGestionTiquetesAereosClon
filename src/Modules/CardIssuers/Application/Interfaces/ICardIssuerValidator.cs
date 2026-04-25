// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardIssuers\Application\Interfaces\ICardIssuerValidator.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using GestionAerolineas.src.Modules.CardIssuers.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.CardIssuers.Application.Interfaces;

public interface ICardIssuerValidator
{
    Task ValidateNameAsync(CardIssuerName name);
}
