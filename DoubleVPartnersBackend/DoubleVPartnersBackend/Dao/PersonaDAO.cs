using System.Data;
using DoubleVPartnersBackend.Conection;
using DoubleVPartnersBackend.DTOs;
using DoubleVPartnersBackend.Utilidades;
using Microsoft.Data.SqlClient;
using Serilog;

namespace DoubleVPartnersBackend.Dao
{
    public class PersonaDAO
    {
        private readonly SqlConnectionManager _connectionManager;

        public PersonaDAO(SqlConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }


        public List<PersonaDTO> listarPersona(int? idPersona = null)
        {
            List<PersonaDTO> personas = null;
            string storedProcedure = "sp_c_personac";  // Nombre del SP

            // Agregamos el parámetro idPersona al array de parámetros
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@idPersona", SqlDbType.Int)
        {
            Value = (object)idPersona ?? DBNull.Value, // Si idPersona es null, lo tratamos como DBNull.Value
            IsNullable = true
        },
        new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output },
        new SqlParameter("@codigo", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            try
            {
                // Ejecutar el SP
                DataTable result = _connectionManager.ExecuteStoredProcedure(storedProcedure, parameters);

                // Obtener los parámetros de salida
                string mensaje = parameters[1].Value.ToString();
                int codigo = (int)parameters[2].Value;

                // Si el código de salida es 0, procesamos los resultados
                if (codigo == 0)
                {
                    Log.Information($"Mensaje: {mensaje}");

                    if (result != null && result.Rows.Count > 0)
                    {
                        personas = new List<PersonaDTO>();

                        foreach (DataRow row in result.Rows)
                        {
                            personas.Add(new PersonaDTO
                            {
                                idPersona = DbNullHelper.GetValue<int>(row, "idPersona"),
                                nombres = DbNullHelper.GetValue<string>(row, "nombres", string.Empty),
                                apellidos = DbNullHelper.GetValue<string>(row, "apellidos", string.Empty),
                                numeroIdentificacion = DbNullHelper.GetValue<string>(row, "numeroIdentificacion", string.Empty),
                                email = DbNullHelper.GetValue<string>(row, "email", string.Empty),
                                tipoIdentificacion = DbNullHelper.GetValue<string>(row, "tipoIdentificacion", string.Empty),
                                fechaCreacion = DbNullHelper.GetValue<DateTime>(row, "fechaCreacion", DateTime.MinValue)
                            });
                        }
                    }
                }
                Log.Information($"Mensaje: {mensaje}");
                Log.Information($"Código: {codigo}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error al ejecutar el SP: {ex.Message}");
            }

            return personas;
        }


        public bool InsertarPersonaUsuario(PersonaUsuarioCreacionDTO personaUsuario)
        {
            string storedProcedure = "sp_i_persona_usuario";  // Nombre del SP
            string passwordEncrypted=PasswordHasher.HashPassword(personaUsuario.pass);
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nombres", SqlDbType.NVarChar, 100) { Value = personaUsuario.nombres },
                new SqlParameter("@apellidos", SqlDbType.NVarChar, 100) { Value = personaUsuario.apellidos },
                new SqlParameter("@numeroIdentificacion", SqlDbType.NVarChar, 20) { Value = personaUsuario.numeroIdentificacion },
                new SqlParameter("@email", SqlDbType.NVarChar, 100) { Value = personaUsuario.email },
                new SqlParameter("@tipoIdentificacion", SqlDbType.NVarChar, 50) { Value = personaUsuario.tipoIdentificacion },
                new SqlParameter("@usuario", SqlDbType.NVarChar, 50) { Value = personaUsuario.usuario },
                new SqlParameter("@pass", SqlDbType.NVarChar, 100) { Value = passwordEncrypted },
                // Parámetros de salida
                new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output },
                new SqlParameter("@codigo", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            try
            {
                // Ejecutar el SP
                DataTable result = _connectionManager.ExecuteStoredProcedure(storedProcedure, parameters);

                // Obtener los parámetros de salida
                string mensaje = parameters[7].Value.ToString();  // @mensaje
                int codigo = (int)parameters[8].Value;           // @codigo

                // Comprobar el código de salida para saber si la operación fue exitosa
                if (codigo == 0)
                {
                    Log.Information($"Persona y usuario registrados correctamente.");
                    return true;  // Indica que el registro fue exitoso
                }
                else
                {
                    Log.Error($"Error al registrar persona y usuario. Mensaje: {mensaje}");
                    return false;  // En caso de error
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Ocurrió un error al ejecutar el SP: {ex.Message}");
                return false;
            }
        }

        public bool InsertarPersona(PersonaCreacionDTO persona)
        {
            string storedProcedure = "sp_i_persona";  // Nombre del SP
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@nombres", SqlDbType.NVarChar, 100) { Value = persona.nombres },
                new SqlParameter("@apellidos", SqlDbType.NVarChar, 100) { Value = persona.apellidos },
                new SqlParameter("@numeroIdentificacion", SqlDbType.NVarChar, 20) { Value = persona.numeroIdentificacion },
                new SqlParameter("@email", SqlDbType.NVarChar, 100) { Value = persona.email },
                new SqlParameter("@tipoIdentificacion", SqlDbType.NVarChar, 50) { Value = persona.tipoIdentificacion },
                // Parámetros de salida
                new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output },
                new SqlParameter("@codigo", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            try
            {
                // Ejecutar el SP
                DataTable result = _connectionManager.ExecuteStoredProcedure(storedProcedure, parameters);

                // Obtener los parámetros de salida
                string mensaje = parameters[5].Value.ToString();  // @mensaje
                int codigo = (int)parameters[6].Value;           // @codigo

                // Comprobar el código de salida para saber si la operación fue exitosa
                if (codigo == 0)
                {
                    Log.Information($"Persona registrado correctamente.");
                    return true;  // Indica que el registro fue exitoso
                }
                else
                {
                    Log.Error($"Error al registrar persona. Mensaje: {mensaje}");
                    return false;  // En caso de error
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Ocurrió un error al ejecutar el SP: {ex.Message}");
                return false;
            }
        }


        public bool ActualizarPersona(int idPersona,PersonaCreacionDTO personaDTO)
        {
            string storedProcedure = "sp_a_persona";  // Nombre del SP
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@idPersona", SqlDbType.Int) { Value = idPersona },
                new SqlParameter("@nombres", SqlDbType.NVarChar, 100) { Value = personaDTO.nombres },
                new SqlParameter("@apellidos", SqlDbType.NVarChar, 100) { Value = personaDTO.apellidos },
                new SqlParameter("@numeroIdentificacion", SqlDbType.NVarChar, 20) { Value = personaDTO.numeroIdentificacion },
                new SqlParameter("@email", SqlDbType.NVarChar, 100) { Value = personaDTO.email },
                new SqlParameter("@tipoIdentificacion", SqlDbType.NVarChar, 50) { Value = personaDTO.tipoIdentificacion },
                // Parámetros de salida
                new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output },
                new SqlParameter("@codigo", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            try
            {
                // Ejecutar el SP
                DataTable result = _connectionManager.ExecuteStoredProcedure(storedProcedure, parameters);

                // Obtener los parámetros de salida
                string mensaje = parameters[6].Value.ToString();  // @mensaje
                int codigo = (int)parameters[7].Value;           // @codigo

                // Comprobar el código de salida para saber si la operación fue exitosa
                if (codigo == 0)
                {
                    Log.Information($"Persona y usuario actualizados correctamente.");
                    return true;  // Indica que la actualización fue exitosa
                }
                else
                {
                    Log.Error($"Error al actualizar persona y usuario. Mensaje: {mensaje}");
                    return false;  // En caso de error
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Ocurrió un error al ejecutar el SP: {ex.Message}");
                return false;
            }
        }


        public bool EliminarPersonaUsuario(int idPersona)
        {
            string storedProcedure = "sp_e_persona_usuario";  // Nombre del SP
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@idPersona", SqlDbType.Int) { Value = idPersona },
                // Parámetros de salida
                new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output },
                new SqlParameter("@codigo", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            try
            {
                // Ejecutar el SP
                DataTable result = _connectionManager.ExecuteStoredProcedure(storedProcedure, parameters);

                // Obtener los parámetros de salida
                string mensaje = parameters[1].Value.ToString();  // @mensaje
                int codigo = (int)parameters[2].Value;           // @codigo

                // Comprobar el código de salida para saber si la operación fue exitosa
                if (codigo == 0)
                {
                    Log.Information($"Persona y usuario eliminados correctamente.");
                    return true;  // Indica que la eliminación fue exitosa
                }
                else
                {
                    Log.Error($"Error al eliminar persona y usuario. Mensaje: {mensaje}");
                    return false;  // En caso de error
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Ocurrió un error al ejecutar el SP: {ex.Message}");
                return false;
            }
        }





    }



}
