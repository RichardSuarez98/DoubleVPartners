namespace DoubleVPartnersBackend.DTOs
{
    public class PersonaUsuarioCreacionDTO
    {
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string numeroIdentificacion { get; set; }
        public string email { get; set; }
        public string tipoIdentificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuario { get; set; }
        public string pass { get; set; }

    }
}
