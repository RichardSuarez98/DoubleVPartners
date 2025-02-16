using DoubleVPartnersBackend.DTOs;

namespace DoubleVPartnersBackend.Interface
{
    public interface IUsuarios
    {
        ApiResponse ActualizarPassword(UsuarioCreacionDTO usuarioCreacionDTO);
        ApiResponse DatosConsultaPorId(int idPersona);
        ApiResponse Authenticate(string usuario, string pass);
    }
}
