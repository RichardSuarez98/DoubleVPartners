using DoubleVPartnersBackend.Dao;
using DoubleVPartnersBackend.DTOs;
using DoubleVPartnersBackend.Interface;
using Serilog;

namespace DoubleVPartnersBackend.Servicios
{
    public class PersonaService : IPersonas
    {
        private readonly PersonaDAO contextDao;

        public PersonaService(PersonaDAO contextDao)
        {
            this.contextDao = contextDao;
        }

        public ApiResponse DatosConsulta()
        {
            try
            {
                var mensajeSalida = contextDao.listarPersona();

                if (mensajeSalida == null || mensajeSalida.Count == 0)
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA, "No hay registros por mostrar"));
                }
                return new ApiResponse(new RouteResponse(RouteResponse.Status.OK), mensajeSalida);
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
                return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA, "Error en la operación."));

            }

        }


        public ApiResponse InsertarPersonaUsuario(PersonaUsuarioCreacionDTO personaUsuario)
        {
            try
            {
                bool resultado = contextDao.InsertarPersonaUsuario(personaUsuario);

                if (resultado)
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.OK), "Persona y usuario registrados correctamente.");
                }
                else
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error al registrar persona y usuario.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
                return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error en la operación.");
            }
        }

        public ApiResponse InsertarPersona(PersonaCreacionDTO persona)
        {
            try
            {
                bool resultado = contextDao.InsertarPersona(persona);

                if (resultado)
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.OK), "Persona registrado correctamente.");
                }
                else
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error al registrar persona.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
                return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error en la operación.");
            }
        }


        public ApiResponse ActualizarPersona(int idPersona,PersonaCreacionDTO personaDTO)
        {
            try
            {
                bool resultado = contextDao.ActualizarPersona(idPersona,personaDTO);

                if (resultado)
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.OK), "Persona y usuario actualizados correctamente.");
                }
                else
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error al actualizar persona y usuario.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
                return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error en la operación.");
            }
        }

        public ApiResponse EliminarPersonaUsuario(int idPersona)
        {
            try
            {
                bool resultado = contextDao.EliminarPersonaUsuario(idPersona);

                if (resultado)
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.OK), "Persona y usuario eliminados correctamente.");
                }
                else
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error al eliminar persona y usuario.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
                return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error en la operación.");
            }
        }

        public ApiResponse DatosConsultaPorId(int idPersona)
        {
            try
            {
                var mensajeSalida = contextDao.listarPersona(idPersona);

                if (mensajeSalida == null || mensajeSalida.Count == 0)
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA, "No hay registros por mostrar"));
                }
                return new ApiResponse(new RouteResponse(RouteResponse.Status.OK), mensajeSalida);
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
                return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA, "Error en la operación."));

            }
        }



    }
}
