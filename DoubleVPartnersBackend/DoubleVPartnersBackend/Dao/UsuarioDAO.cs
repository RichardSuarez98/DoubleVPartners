using DoubleVPartnersBackend.Conection;
using DoubleVPartnersBackend.DTOs;
using DoubleVPartnersBackend.Utilidades;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Data;

namespace DoubleVPartnersBackend.Dao
{
    public class UsuarioDAO
    {

        private readonly SqlConnectionManager _connectionManager;

        public UsuarioDAO(SqlConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }


        public List<UsuarioDTO> mostrarUsuario(int? idUsuario = null)
        {
            List<UsuarioDTO> personas = null;
            string storedProcedure = "sp_c_usuario";  

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@idUsuario", SqlDbType.Int)
                {
                    Value = (object)idUsuario ?? DBNull.Value, 
                    IsNullable = true
                },
                new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output },
                new SqlParameter("@codigo", SqlDbType.Int) { Direction = ParameterDirection.Output }
                    };

            try
            {
                DataTable result = _connectionManager.ExecuteStoredProcedure(storedProcedure, parameters);

                string mensaje = parameters[1].Value.ToString();
                int codigo = (int)parameters[2].Value;

                if (codigo == 0)
                {
                    Log.Information($"Mensaje: {mensaje}");

                    if (result != null && result.Rows.Count > 0)
                    {
                        personas = new List<UsuarioDTO>();

                        foreach (DataRow row in result.Rows)
                        {
                            personas.Add(new UsuarioDTO
                            {
                                idUsuario = DbNullHelper.GetValue<int>(row, "idUsuario"),
                                usuario = DbNullHelper.GetValue<string>(row, "usuario", string.Empty),
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


        public bool ActualizarPassword(UsuarioCreacionDTO usuarioCreacionDTO)
        {
            string storedProcedure = "sp_a_usuario";  
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@idUsuario", SqlDbType.Int) { Value = usuarioCreacionDTO.idUsuario },
                new SqlParameter("@pass", SqlDbType.NVarChar, 100) { Value = usuarioCreacionDTO.pass },
                // Parámetros de salida
                new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output },
                new SqlParameter("@codigo", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            try
            {
                DataTable result = _connectionManager.ExecuteStoredProcedure(storedProcedure, parameters);

                string mensaje = parameters[2].Value.ToString();  // @mensaje
                int codigo = (int)parameters[3].Value;           // @codigo

                if (codigo == 0)
                {
                    Log.Information($"Contraseña actualizada correctamente.");
                    return true;  
                }
                else
                {
                    Log.Error($"Error al actualizar contraseña. Mensaje: {mensaje}");
                    return false;  
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Ocurrió un error al ejecutar el SP: {ex.Message}");
                return false;
            }
        }


        /*public bool LoginUsuario(string usuario, string pass)
        {
            string storedProcedure = "sp_login";  // Nombre del stored procedure
            string mensaje = "";
            string passwordEncrypted = PasswordHasher.HashPassword(pass);
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@usuario", SqlDbType.VarChar,20) { Value = usuario },
                new SqlParameter("@pass", SqlDbType.VarChar, 255) { Value = passwordEncrypted },
                new SqlParameter("@mensaje", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output },
                new SqlParameter("@codigo", SqlDbType.Int) { Direction = ParameterDirection.Output }
                    };

            try
            {
                // Ejecutar el stored procedure
                _connectionManager.ExecuteStoredProcedure(storedProcedure, parameters);

                // Obtener los valores de salida
                 mensaje = parameters[2].Value.ToString();  // @mensaje
                int codigo = (int)parameters[3].Value;     // @codigo

                if (codigo == 0)
                {
                    Log.Information($"Login exitoso. {mensaje}");
                    return true;
                }
                else
                {
                    Log.Error($"Error en login. Mensaje: {mensaje}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Error al ejecutar el SP: {ex.Message}";
                Log.Error(mensaje);
                return false;
            }
        }*/

        public bool LoginUsuario(string usuario, string pass)
        {
            string storedProcedure = "sp_obtener_hash";  // Nombre del stored procedure
            string mensaje = "";

            SqlParameter[] parameters = new SqlParameter[]
            {/*
               @usuario VARCHAR(20),
	@hash varchar(255) OUTPUT,
    @mensaje NVARCHAR(255) OUTPUT,  -- Variable de salida para el mensaje
    @codigo INT OUTPUT   
              */
                new SqlParameter("@usuario", SqlDbType.VarChar, 20) { Value = usuario },
                new SqlParameter("@hash", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output },
                new SqlParameter("@mensaje", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output },
                new SqlParameter("@codigo", SqlDbType.Int) { Direction = ParameterDirection.Output }
            };

            try
            {
                // Ejecutar el stored procedure para obtener el hash almacenado
                _connectionManager.ExecuteStoredProcedure(storedProcedure, parameters);

                // Obtener el hash almacenado
                string storedPasswordHash = parameters[1].Value?.ToString();
                mensaje = parameters[2].Value.ToString();  // @mensaje
                int codigo = (int)parameters[3].Value;
                Log.Information(mensaje + " - " + codigo.ToString());
                // Validar si el usuario existe
                if (storedPasswordHash == null)
                {
                    Log.Error("Usuario no encontrado.");
                    return false;
                }

                // Validar la contraseña usando BCrypt
                if (PasswordHasher.VerificarHashPassword(pass, storedPasswordHash))
                {
                    Log.Information("Login exitoso.");
                    return true;
                }
                else
                {
                    Log.Error("Error en login. Contraseña incorrecta.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Error al ejecutar el SP: {ex.Message}";
                Log.Error(mensaje);
                return false;
            }
        }


    }
}
