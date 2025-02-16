using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DoubleVPartnersBackend.DTOs;
using DoubleVPartnersBackend.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;


namespace DoubleVPartnersBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
       /* private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;*/
        private readonly IUsuarios usuarioService;

        // Constructor: Inyectamos el PersonaDAO
        public UsuariosController(/*UserManager<IdentityUser> userManager,
                                  SignInManager<IdentityUser> signInManager,
                                  IConfiguration configuration,*/
            IUsuarios usuarioService)
        {
           /*/ this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;*/
            this.usuarioService = usuarioService;
        }


        /*[HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar(CredencialesUsuarioDTO credencialesUsuarioDTO)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuarioDTO.usuario,
            };

            var resultado = await userManager.CreateAsync(usuario,credencialesUsuarioDTO.Password);
            if (resultado.Succeeded)
            {
                return await ConstruirToken(usuario);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login(CredencialesUsuarioDTO credencialesUsuarioDTO)
        {
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDTO.usuario);
            if (usuario is null)
            {
                var errores = ConstruirLoginIncorrecto();
                return BadRequest(errores);
            }
            var resultado = await signInManager.CheckPasswordSignInAsync(usuario,
                credencialesUsuarioDTO.Password,lockoutOnFailure:false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(usuario);
            }
            else
            {
                var errores= ConstruirLoginIncorrecto();
                return BadRequest(errores);
            }
        }

        private IEnumerable<IdentityError> ConstruirLoginIncorrecto()
        {
            var identity = new IdentityError() { Description = "Login incorrecto" };
            var errors = new List<IdentityError>();
            errors.Add(identity);
            return errors;
        }

        private async Task<RespuestaAutenticacionDTO> ConstruirToken(IdentityUser identityUser)//,
           // IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            var claims = new List<Claim>
            {
                new Claim("usuario",identityUser.UserName),
                new Claim("lo que yo quiera","cualquier valor"),
            };

            var claimsDB = await userManager.GetClaimsAsync(identityUser);
            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]!));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var tokenDeSeguirdad = new JwtSecurityToken(issuer:null, audience: null, claims: claims,
                expires:expiracion, signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguirdad);

            return new RespuestaAutenticacionDTO { 
                Token = token, 
                Expiracion = expiracion,            
            };
        }*/

        [HttpGet("{idUsuario}")]
        [EndpointSummary("Obtiene una Persona registrada en BD por su ID")]
        [EndpointDescription("Obtiene la persona registrada por su ID. Si no existe la persona, mostrará un código de error.")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponse> GetById(int idUsuario)
        {
            Log.Information("Inicia consulta de Usuario por idUsuario: {idUsuario}", idUsuario);

            try
            {
                // Llamada al servicio para obtener la persona por id
                var usuario = usuarioService.DatosConsultaPorId(idUsuario);

                if (usuario != null)
                {
                    Log.Information("Usuario encontrado: {Usuario}", idUsuario);
                    return Ok(usuario);
                }
                else
                {
                    Log.Warning("Usuario no encontrado con idUsuario: {idUsuario}", idUsuario);
                    return NotFound(new ApiResponse(new RouteResponse(RouteResponse.Status.NOT_FOUND), "Usuario no encontrada."));
                }
            }
            catch (Exception ex)
            {
                Log.Error("Ocurrió un error al intentar consultar la persona. Error: {Error}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(new RouteResponse(RouteResponse.Status.NOT_FOUND), "Error interno del servidor."));
            }
        }




        [HttpPut]
        [EndpointSummary("Actualiza la contraseña de un Usuario")]
        [EndpointDescription("Actualiza la información de un usuario existente.")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        public ActionResult<ApiResponse> Put([FromBody] UsuarioCreacionDTO usuarioCreacionDTO)
        {
            Log.Information("Inicia actualización de Contraseña");

            // Validar si los datos son válidos
            if (usuarioCreacionDTO == null || string.IsNullOrEmpty(usuarioCreacionDTO.usuario))
            {
                return BadRequest(new ApiResponse(new RouteResponse(RouteResponse.Status.INCOMPLETE_DATA), "Datos incompletos."));
            }

            // Llamada al servicio para actualizar la persona
            ApiResponse resp = usuarioService.ActualizarPassword(usuarioCreacionDTO);

            Log.Information("Response JSON Actualización contraseña: {Response}", resp);
            return Ok(resp);
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] CredencialesUsuarioDTO request)
        {
            try
            {
                // Validar el modelo de entrada
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Datos de entrada inválidos."));
                }

                // Llamar al servicio de autenticación
                var response = usuarioService.Authenticate(request.usuario, request.Password);

                // Devolver la respuesta
                /* if (response.RouteResponse.ResponseStatus == RouteResponse.Status.OK)
                 {
                     return Ok(response);
                 }
                 else
                 {
                     return Unauthorized(response);
                 }*/
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error($"Error en el controlador AuthController: {ex.Message}");
                return StatusCode(500, new ApiResponse(new RouteResponse(RouteResponse.Status.CONFLICT_WITH_DATA), "Error interno del servidor."));
            }
        }





    }
}
