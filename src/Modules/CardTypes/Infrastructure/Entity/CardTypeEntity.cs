// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CardTypes\Infrastructure\Entity\CardTypeEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.CardTypes.Infrastructure.Entity;

public class CardTypeEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
