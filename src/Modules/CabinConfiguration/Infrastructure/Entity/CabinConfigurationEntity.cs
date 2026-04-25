// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\CabinConfiguration\Infrastructure\Entity\CabinConfigurationEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.CabinConfiguration.Infrastructure.Entity;

public class CabinConfigurationEntity
{
    public int Id { get; set; }
    public int AircraftId { get; set; }
    public int CabinTypeId { get; set; }
    public int StartRow { get; set; }
    public int EndRow { get; set; }
    public int SeatsPerRow { get; set; }
    public string? SeatLetters { get; set; }
}

