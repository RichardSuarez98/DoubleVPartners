namespace DoubleVPartnersBackend.DTOs
{
    public class PersonaDTO
    {
        public int idPersona { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string numeroIdentificacion { get; set; }
        public string email { get; set; }
        public string tipoIdentificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
