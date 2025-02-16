using System.Text.Json;
using DoubleVPartnersBackend.Dao;
using DoubleVPartnersBackend.DTOs;
using DoubleVPartnersBackend.Interface;
using DoubleVPartnersBackend.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DoubleVPartnersBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController: ControllerBase
    {
        private readonly IPersonas personaService;

        // Constructor: Inyectamos el PersonaDAO
        public PersonasController(IPersonas personaService)
        {
            this.personaService = personaService;
        }

        //[Authorize]
        [HttpGet]
        [EndpointSummary("Obtiene el listado de Personas registrados en BD")]
        [EndpointDescription("Obtiene las personas registradas, si no existe la persona en la BD, mostrara un codigo de error")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        public ActionResult<ApiResponse> Get()
        {
            Log.Information("Inicia consulta de Persona");
            ApiResponse resp = personaService.DatosConsulta();
            Log.Information("Response JSON Consulta Personas: {Response}", resp);

            return Ok(resp);
        }

       // [Authorize]
        [HttpGet("{idPersona}")]
        [EndpointSummary("Obtiene una Persona registrada en BD por su ID")]
        [EndpointDescription("Obtiene la persona registrada por su ID. Si no existe la persona, mostrará un código de error.")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponse> GetById(int idPersona)
        {
            Log.Information("Inicia consulta de Persona por idPersona: {idPersona}", idPersona);

            try
            {
                // Llamada al servicio para obtener la persona por id
                var persona = personaService.DatosConsultaPorId(idPersona);

                if (persona != null)
                {
                    Log.Information("Persona encontrada: {Persona}", persona);
                    return Ok(persona);
                }
                else
                {
                    Log.Warning("Persona no encontrada con idPersona: {idPersona}", idPersona);
                    return NotFound(new ApiResponse(new RouteResponse(RouteResponse.Status.NOT_FOUND), "Persona no encontrada."));
                }
            }
            catch (Exception ex)
            {
                Log.Error("Ocurrió un error al intentar consultar la persona. Error: {Error}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(new RouteResponse(RouteResponse.Status.NOT_FOUND), "Error interno del servidor."));
            }
        }

        
        [HttpPost]
        [EndpointSummary("Registra una nueva Persona y Usuario")]
        [EndpointDescription("Inserta una nueva persona y su usuario en la base de datos.")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        public ActionResult<ApiResponse> Post([FromBody] PersonaUsuarioCreacionDTO personaUsuario)
        {
            Log.Information("Inicia registro de Persona y Usuario");

            // Validar si los datos son válidos
            if (personaUsuario == null || string.IsNullOrEmpty(personaUsuario.nombres) || string.IsNullOrEmpty(personaUsuario.apellidos))
            {
                return BadRequest(new ApiResponse(new RouteResponse(RouteResponse.Status.INCOMPLETE_DATA), "Datos incompletos."));
            }

            // Llamada al servicio para insertar la persona
            ApiResponse resp = personaService.InsertarPersonaUsuario(personaUsuario);

            Log.Information("Response JSON Registro Persona: {Response}", resp);
            return Ok(resp);
        }

        [HttpPost("registrar")]
        [EndpointSummary("Registra una nueva Persona")]
        [EndpointDescription("Inserta una nueva persona en la base de datos.")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        public ActionResult<ApiResponse> PostPersona([FromBody] PersonaCreacionDTO persona)
        {
            Log.Information("Inicia registro de Persona y Usuario");

            // Validar si los datos son válidos
            if (persona == null || string.IsNullOrEmpty(persona.nombres) || string.IsNullOrEmpty(persona.apellidos))
            {
                return BadRequest(new ApiResponse(new RouteResponse(RouteResponse.Status.INCOMPLETE_DATA), "Datos incompletos."));
            }

            // Llamada al servicio para insertar la persona
            ApiResponse resp = personaService.InsertarPersona(persona);

            Log.Information("Response JSON Registro Persona: {Response}", resp);
            return Ok(resp);
        }

        // [Authorize]
        [HttpPut("{idPersona}")]
        [EndpointSummary("Actualiza los datos de una Persona")]
        [EndpointDescription("Actualiza la información de una persona existente.")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        public ActionResult<ApiResponse> Put(int idPersona, [FromBody] PersonaCreacionDTO personaDTO)
        {
            Log.Information("Inicia actualización de Persona");

            // Validar si los datos son válidos
            if (personaDTO == null || string.IsNullOrEmpty(personaDTO.nombres) || string.IsNullOrEmpty(personaDTO.apellidos))
            {
                return BadRequest(new ApiResponse(new RouteResponse(RouteResponse.Status.INCOMPLETE_DATA), "Datos incompletos."));
            }

            // Llamada al servicio para actualizar la persona
            ApiResponse resp = personaService.ActualizarPersona(idPersona,personaDTO);

            Log.Information("Response JSON Actualización Persona: {Response}", resp);
            return Ok(resp);
        }

       // [Authorize]
        [HttpDelete("{idPersona}")]
        [EndpointSummary("Elimina una Persona y su Usuario")]
        [EndpointDescription("Elimina una persona junto con su usuario asociado.")]
        [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
        public ActionResult<ApiResponse> Delete(int idPersona)
        {
            Log.Information("Inicia eliminación de Persona y Usuario");

            // Llamada al servicio para eliminar la persona
            ApiResponse resp = personaService.EliminarPersonaUsuario(idPersona);

            Log.Information("Response JSON Eliminación Persona: {Response}", resp);
            return Ok(resp);
        }



    }



}

