// [DocHeader]
// M?dulo: General
// Capa: General
// Archivo: src\Modules\Users\Application\Services\UserPasswordHasher.cs
// Responsabilidad: Agrupa l?gica espec?fica del m?dulo respetando la arquitectura por capas del proyecto.
// Flujo: Participa en el flujo general de construcci?n y ejecuci?n del sistema de gesti?n a?rea.
using System.Security.Cryptography;
using GestionAerolineas.src.Modules.Users.Domain.ValueObject;

namespace GestionAerolineas.src.Modules.Users.Application.Services;

public static class UserPasswordHasher
{
    private const int SaltSizeBytes = 16;
    private const int KeySizeBytes = 32;
    private const int Iterations = 100_000;

    public static UserPasswordHash Hash(string plainPassword)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
            throw new ArgumentException("La contraseña no puede estar vacía");

        var trimmed = plainPassword.Trim();
        if (trimmed.Length < 8)
            throw new ArgumentException("La contraseña debe tener al menos 8 caracteres");

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSizeBytes);
        byte[] key = Rfc2898DeriveBytes.Pbkdf2(
            password: trimmed,
            salt: salt,
            iterations: Iterations,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: KeySizeBytes);

        var encoded = $"PBKDF2${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
        return UserPasswordHash.Create(encoded);
    }

    public static bool Verify(string plainPassword, UserPasswordHash storedHash)
    {
        if (string.IsNullOrWhiteSpace(plainPassword))
            return false;

        if (storedHash is null || string.IsNullOrWhiteSpace(storedHash.Value))
            return false;

        if (!storedHash.Value.StartsWith("PBKDF2$", StringComparison.Ordinal))
            return false;

        var parts = storedHash.Value.Split('$');
        if (parts.Length != 4)
            return false;

        if (!int.TryParse(parts[1], out int iterations) || iterations <= 0)
            return false;

        byte[] salt;
        byte[] expectedKey;
        try
        {
            salt = Convert.FromBase64String(parts[2]);
            expectedKey = Convert.FromBase64String(parts[3]);
        }
        catch
        {
            return false;
        }

        byte[] actualKey = Rfc2898DeriveBytes.Pbkdf2(
            password: plainPassword.Trim(),
            salt: salt,
            iterations: iterations,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: expectedKey.Length);

        return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
    }
}
