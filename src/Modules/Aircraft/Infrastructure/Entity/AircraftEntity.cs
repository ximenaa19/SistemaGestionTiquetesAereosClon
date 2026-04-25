// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Aircraft\Infrastructure\Entity\AircraftEntity.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
namespace GestionAerolineas.src.Modules.Aircraft.Infrastructure.Entity;

public class AircraftEntity
{
    public int Id { get; set; }
    public int ModelId { get; set; }
    public int AirlineId { get; set; }
    public string? Registration { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public bool IsActive { get; set; }
}

