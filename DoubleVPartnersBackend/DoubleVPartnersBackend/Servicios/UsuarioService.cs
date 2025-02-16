using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DoubleVPartnersBackend.Conection;
using DoubleVPartnersBackend.Dao;
using DoubleVPartnersBackend.DTOs;
using DoubleVPartnersBackend.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace DoubleVPartnersBackend.Servicios
{
    public class UsuarioService: IUsuarios
    {
        private readonly UsuarioDAO contextDao;
        private readonly IConfiguration _configuration;

        public UsuarioService(UsuarioDAO contextDao,
            IConfiguration configuration)
        {
            this.contextDao = contextDao;
            _configuration = configuration;
        }

        public ApiResponse Authenticate(string usuario, string pass)
        {
            RespuestaAutenticacionDTO response = null;
            try
            {
                // Validar las credenciales del usuario utilizando el stored procedure
                bool loginExitoso = contextDao.LoginUsuario(usuario, pass);

                if (!loginExitoso)
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.INTERNAL_ERROR), "Credenciales inválidas.");
                }

                // Generar el token JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, usuario.ToString()),
                    new Claim("esadmin", "true"), // Agrega un claim personalizado para "esadmin"
                    //new Claim(ClaimTypes.Role, "esadmin") // Aquí puedes agregar roles si los tienes
                    //new Claim(ClaimTypes.Role, "User") // Aquí puedes agregar roles si los tienes
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"])),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"]
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                response = new RespuestaAutenticacionDTO
                {
                    Token = tokenString,
                    Expiracion = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]!))
                };
                return new ApiResponse(new RouteResponse(RouteResponse.Status.OK), response);
            }
            catch (Exception ex)
            {
                Log.Error($"Error en la autenticación: {ex.Message}");
                return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error en la operación.");
            }
        }



        public ApiResponse ActualizarPassword(UsuarioCreacionDTO usuarioCreacionDTO)
        {
            try
            {
                bool resultado = contextDao.ActualizarPassword(usuarioCreacionDTO);

                if (resultado)
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.OK), "Contraseña actualizados correctamente.");
                }
                else
                {
                    return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error al actualizar contraseña.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
                return new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error en la operación.");
            }
        }

        public ApiResponse DatosConsultaPorId(int idUsuario)
            {
                try
                {
                    var mensajeSalida = contextDao.mostrarUsuario(idUsuario);

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
