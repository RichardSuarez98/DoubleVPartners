using System.Text;
using AutoMapper;
using DoubleVPartnersBackend.Conection;
using DoubleVPartnersBackend.Dao;
using DoubleVPartnersBackend.Interface;
using DoubleVPartnersBackend.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
//using PeliculasAPI;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();



// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()         // Escribir en consola
    .WriteTo.File("app.log")   // Escribir en un archivo
    .CreateLogger();

// Usar Serilog en lugar del logger predeterminado
builder.Host.UseSerilog();

builder.Services.AddScoped<SqlConnectionManager>(); // Registramos PersonaDAO
builder.Services.AddScoped<PersonaDAO>(); // Registramos PersonaDAO
builder.Services.AddScoped<UsuarioDAO>(); // Registramos PersonaDAO
builder.Services.AddScoped<IPersonas, PersonaService>();
builder.Services.AddScoped<IUsuarios, UsuarioService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title="DoubleVPartners",
        Description = "Está es una prueba técnica de DoubleVPartners",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Email = "richard.suarezjacome@gmail.com",
            Name = "Richard Suáre",
            Url = new Uri("https://gavilan.blog")
        }
    });

    // Configuración para el JWT en Swagger
    opciones.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization", // El nombre de la cabecera que llevará el token
        In = Microsoft.OpenApi.Models.ParameterLocation.Header, // El token estará en la cabecera
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer", // Especificamos que se usa el esquema Bearer
        BearerFormat = "JWT", // Formato de token que se espera
        Description = "Por favor ingresa el token JWT con el prefijo 'Bearer' antes de la cadena del token."
    });

    opciones.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer" // El ID que definimos antes
                }
            },
            new string[] { }
        }
    });


});



/*builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer("name=DefaultConnection", sqlServer =>
sqlServer.UseNetTopologySuite()
));*/
//builder.Services.addswa

//Configurar la ubicaciond para cines
builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

builder.Services.AddOutputCache(opciones =>
{
    opciones.DefaultExpirationTimeSpan=TimeSpan.FromSeconds(15);
});


//configurando cors
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(opcionesCORS =>
    {
        opcionesCORS.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders("cantidad-total-registros");
    });
});

builder.Services.AddHttpContextAccessor();
///builder.Services.AddSingleton<IRepositorio,RepositorioEnMemoria>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
/*app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DoubleVPartners API V1");
    options.RoutePrefix = string.Empty; // Esto es opcional, hace que Swagger UI esté disponible en la raíz
});
*/
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//Imagenes
app.UseStaticFiles();

app.UseCors();

app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
