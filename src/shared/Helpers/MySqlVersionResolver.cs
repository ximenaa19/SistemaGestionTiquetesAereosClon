// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Helpers\MySqlVersionResolver.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using MySqlConnector;

namespace GestionAerolineas.src.shared.Helpers;

public class MySqlVersionResolver
{
    public static Version DetectVersion(string connectionString)
    {
        using var conn = new MySqlConnection(connectionString);
        conn.Open();
        var raw = conn.ServerVersion;
        if (raw == null)
        {
            throw new InvalidOperationException("Unable to retrieve server version.");
        }
        var clean = raw.Split('-')[0];
        return Version.Parse(clean);
    }
}


