using DoubleVPartnersBackend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DoubleVPartnersBackend.Interface
{
    public interface IPersonas
    {
        ApiResponse DatosConsulta();
        ApiResponse DatosConsultaPorId(int idPersona);
        ApiResponse InsertarPersonaUsuario(PersonaUsuarioCreacionDTO personaUsuario);
        ApiResponse InsertarPersona(PersonaCreacionDTO persona);

        ApiResponse ActualizarPersona(int idPersona, PersonaCreacionDTO personaDTO);
        ApiResponse EliminarPersonaUsuario(int idPersona);

    }
}
