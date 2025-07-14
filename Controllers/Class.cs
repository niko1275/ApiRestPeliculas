using Microsoft.AspNetCore.Mvc;
using ApiRestPeliculas.Data; // Asegúrate que este namespace apunte a tu DbContext
using Microsoft.EntityFrameworkCore;

namespace ApiRestPeliculas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly AplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public TestController(AplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                // Obtener información de la conexión
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                var connection = _context.Database.GetDbConnection();
                
                var canConnect = await _context.Database.CanConnectAsync();
                
                if (canConnect)
                {
                    return Ok(new { 
                        success = true, 
                        message = "✅ Conectado exitosamente a la base de datos",
                        database = connection.Database,
                        server = connection.DataSource,
                        connectionString = connectionString?.Substring(0, Math.Min(50, connectionString?.Length ?? 0)) + "..."
                    });
                }
                else
                {
                    return BadRequest(new { 
                        success = false, 
                        message = "❌ No se pudo conectar a la base de datos",
                        connectionString = connectionString?.Substring(0, Math.Min(50, connectionString?.Length ?? 0)) + "..."
                    });
                }
            }
            catch (Exception ex)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                return StatusCode(500, new { 
                    success = false, 
                    message = "❌ Error al conectar con la base de datos",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message,
                    connectionString = connectionString?.Substring(0, Math.Min(50, connectionString?.Length ?? 0)) + "..."
                });
            }
        }

        [HttpGet("debug")]
        public IActionResult DebugConnection()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                var connection = _context.Database.GetDbConnection();
                
                return Ok(new { 
                    success = true,
                    connectionString = connectionString,
                    connectionState = connection.State.ToString(),
                    database = connection.Database,
                    server = connection.DataSource,
                    hasPassword = !string.IsNullOrEmpty(connectionString) && connectionString.Contains("Password=")
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "❌ Error al obtener información de debug",
                    error = ex.Message
                });
            }
        }

        [HttpGet("migrations")]
        public async Task<IActionResult> CheckMigrations()
        {
            try
            {
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
                
                return Ok(new { 
                    success = true,
                    pendingMigrations = pendingMigrations.ToArray(),
                    appliedMigrations = appliedMigrations.ToArray(),
                    hasPendingMigrations = pendingMigrations.Any()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "❌ Error al verificar migraciones",
                    error = ex.Message
                });
            }
        }

        [HttpPost("apply-migrations")]
        public async Task<IActionResult> ApplyMigrations()
        {
            try
            {
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                
                if (!pendingMigrations.Any())
                {
                    return Ok(new { 
                        success = true, 
                        message = "✅ No hay migraciones pendientes" 
                    });
                }

                await _context.Database.MigrateAsync();
                
                return Ok(new { 
                    success = true, 
                    message = "✅ Migraciones aplicadas correctamente",
                    appliedMigrations = pendingMigrations.ToArray()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "❌ Error al aplicar migraciones",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("test-ssl")]
        public async Task<IActionResult> TestSSLConnections()
        {
            var results = new List<object>();
            
            // Probar diferentes configuraciones SSL
            var connections = new[]
            {
                new { Name = "DefaultConnection", ConnectionString = _configuration.GetConnectionString("DefaultConnection") },
                new { Name = "ConnectionSSL", ConnectionString = _configuration.GetConnectionString("ConnectionSSL") },
                new { Name = "ConnectionPrefer", ConnectionString = _configuration.GetConnectionString("ConnectionPrefer") },
                new { Name = "ConnectionAWS", ConnectionString = _configuration.GetConnectionString("ConnectionAWS") }
            };

            foreach (var conn in connections)
            {
                try
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AplicationDbContext>();
                    optionsBuilder.UseNpgsql(conn.ConnectionString);
                    
                    using var testContext = new AplicationDbContext(optionsBuilder.Options);
                    var canConnect = await testContext.Database.CanConnectAsync();
                    
                    results.Add(new
                    {
                        connectionName = conn.Name,
                        success = canConnect,
                        message = canConnect ? "✅ Conectado" : "❌ Falló la conexión"
                    });
                }
                catch (Exception ex)
                {
                    results.Add(new
                    {
                        connectionName = conn.Name,
                        success = false,
                        message = "❌ Error",
                        error = ex.Message
                    });
                }
            }
            
            return Ok(new { results });
        }
    }
}