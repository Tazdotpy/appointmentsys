using System;
using System.Security.Cryptography;
using System.Text;

namespace AppointmentSystem.Data.Utils;

public static class SecurityHelper
{
    public static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes); // UPPERCASE hex string
    }
}
