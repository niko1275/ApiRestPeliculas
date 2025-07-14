using ApiRestPeliculas.Data;
using ApiRestPeliculas.Modelos;
using ApiRestPeliculas.Modelos.Dtos;
using ApiRestPeliculas.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiRestPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        private readonly AplicationDbContext _context;
        private string clavesecreta;

        public UsuarioRepositorio(AplicationDbContext context,IConfiguration config)
        {
            _context = context;
            clavesecreta = config.GetValue<string>("ApiSetting:SecretKey");
        }

   
        public bool ActualizarUsuario(Usuario usuario)
        {
          _context.Usuarios.Update(usuario);
           return Guardar();
        }

        public Usuario CrearUsuario(Usuario usuario)
        {
            return _context.Usuarios.Add(usuario).Entity;
        }

        public bool EliminarUsuario(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            return Guardar();
        }

        public bool ExisteUsuario(int id)
        {
            _context.Usuarios.Any(u => u.Id == id);
            return _context.Usuarios.Any(u => u.Id == id);

        }

        public bool ExisteUsuario(string nombre)
        {
            _context.Usuarios.Any(u => u.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return _context.Usuarios.Any(u => u.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        }

        public bool Guardar()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public Usuario ObtenerUsuario(int usuarioId)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<Usuario> ObtenerUsuarios()
        {
            return _context.Usuarios.OrderBy(u => u.Nombre).ToList();

        }

   

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLogin)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Nombre == usuarioLogin.Nombre);

            if (usuario == null)
            {
                return null; 
            }

            var hasher = new PasswordHasher<Usuario>();
            var resultado = hasher.VerifyHashedPassword(usuario, usuario.Password, usuarioLogin.Password);

            if (resultado == PasswordVerificationResult.Failed)
            {
                return null; // Contraseña incorrecta
            }

            // Usuario autenticado correctamente

            var usuarioDto = new UsuarioDto
            {
              
                Nombre = usuario.Nombre,
                NombreUsuario = usuario.NombreUsuario,
                Rol = usuario.Rol
            };

          

            var tokenmanejado = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(clavesecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                     {
                        new Claim("id", usuario.Id.ToString()),
                        new Claim("nombre", usuario.Nombre),
                        new Claim(ClaimTypes.Role, usuario.Rol) 
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenmanejado.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuariologinrps = new UsuarioLoginRespuestaDto
            {
                Usuario = usuarioDto,
                Token = tokenmanejado.WriteToken(token)
            };

            return  usuariologinrps;
        }

        public async Task<UsuarioDto> Registro(UsuarioRegistroDto UsuarioRegistroDto)
        {
            // Verificar si el usuario ya existe
            if (_context.Usuarios.Any(u => u.NombreUsuario == UsuarioRegistroDto.NombreUsuario))
            {
                throw new Exception("El usuario ya existe.");
            }

            // Crear entidad Usuario desde el DTO
            var usuario = new Usuario
            {
                NombreUsuario = UsuarioRegistroDto.NombreUsuario,
                Nombre = UsuarioRegistroDto.Nombre,
                Rol = UsuarioRegistroDto.Rol
            };

            // Hashear la contraseña
            var hasher = new PasswordHasher<Usuario>();
            usuario.Password = hasher.HashPassword(usuario, UsuarioRegistroDto.Password);

            // Guardar en base de datos
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Mapear a DTO de respuesta
            var usuarioDto = new UsuarioDto
            {

                NombreUsuario = usuario.NombreUsuario,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol
            };

            return usuarioDto;
        }
    }
}
