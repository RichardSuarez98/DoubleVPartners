using System.ComponentModel.DataAnnotations;

namespace DoubleVPartnersBackend.DTOs
{
    public class CredencialesUsuarioDTO
    {
        [Required]
        public required string usuario { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
