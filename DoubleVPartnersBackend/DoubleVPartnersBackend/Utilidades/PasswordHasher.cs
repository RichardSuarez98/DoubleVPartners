
namespace DoubleVPartnersBackend.Utilidades
{
    using BCrypt.Net;

    public class PasswordHasher
    {
        // Método para encriptar la contraseña
        public static string HashPassword(string password)
        {
            // Genera un hash de la contraseña
            return BCrypt.HashPassword(password, BCrypt.GenerateSalt(12)); // 12 es el costo de trabajo (work factor)
        }
        
        public static bool VerificarHashPassword(string pass,string storedPasswordHash)
        {
            // Genera un hash de la contraseña
            return BCrypt.Verify(pass, storedPasswordHash); // 12 es el costo de trabajo (work factor)
        }
    }
}
