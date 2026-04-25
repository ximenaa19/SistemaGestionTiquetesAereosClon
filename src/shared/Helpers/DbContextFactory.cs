// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\shared\Helpers\DbContextFactory.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GestionAerolineas.src.shared.Helpers;
using GestionAerolineas.src.shared.Context;

namespace GestionAerolineas.src.shared.Helpers;

public class DbContextFactory
{
    public static AppDbContext Create()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        string? ConnectionString =
            Environment.GetEnvironmentVariable("MYSQL_CONNECTION")
            ?? config.GetConnectionString("MySqlDB");

        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new InvalidOperationException("No connection string found");
        }

        var detectedVersion = MySqlVersionResolver.DetectVersion(ConnectionString);
        var minVersion = new Version(8, 0, 0);
        if (detectedVersion < minVersion)
            throw new NotSupportedException(
                $"Versión de MySQL no soportada: {detectedVersion}. Requiere {minVersion} o superior."
            );

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(ConnectionString, new MySqlServerVersion(detectedVersion))
            .Options;
        return new AppDbContext(options);
    }
}


