using ApiRestPeliculas.Data;
using ApiRestPeliculas.Modelos;
using ApiRestPeliculas.PeliculasMapper;
using ApiRestPeliculas.Repositorio;
using ApiRestPeliculas.Repositorio.IRepositorio;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
if (!string.IsNullOrEmpty(dbPassword))
{
    connectionString = connectionString.Replace("Chuncho777$", dbPassword);
}

builder.Services.AddDbContext<AplicationDbContext>(opciones =>
    opciones.UseNpgsql(connectionString));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger con soporte para JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description=
        "Autentiacion JWT usando el esquema Bearer. \r\n\r\n"+
        "Ingresa la palabra 'Bearer' Segudo de un [Espacio] y despues el token en el campo de abajo. \r\n\r\n"+
        "Ejemplo: \"Bearer TKTKKTK\" ",
        Name="Authorization",
        In= ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "bearer",              // ✅ minúscula y correcto
            Name = "Authorization",         // ✅ es el header correcto
            In = ParameterLocation.Header
        },
        new List<string>()
    }
});
});

//Agregar repositorios
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculasRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

//Hashear de contraseña
builder.Services.AddScoped<PasswordHasher<Usuario>>();

//Agregar AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<PeliculasMapper>());


//CACHE GLOBAL
builder.Services.AddControllers(opt =>
{
    opt.CacheProfiles.Add("Default",
        new Microsoft.AspNetCore.Mvc.CacheProfile
        {
            Duration = 60, // Cache por 60 segundos
            
        });
});

var apiVersionBuilder = builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version")
    );
});

apiVersionBuilder.AddApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["ApiSetting:SecretKey"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        // Mapear el claim "role" al claim type de rol estándar
        RoleClaimType = "role"
    };
});


//identity


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// IMPORTANTE: El orden de los middlewares es crucial
app.UseCors("CorsPolicy");
app.UseAuthentication(); // Debe estar antes de UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();