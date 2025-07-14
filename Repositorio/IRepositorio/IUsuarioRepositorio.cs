using ApiRestPeliculas.Modelos;
using ApiRestPeliculas.Modelos.Dtos;


namespace ApiRestPeliculas.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> ObtenerUsuarios();

        Usuario ObtenerUsuario(int usuarioId);


     
        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLogin);

        Task<UsuarioDto> Registro(UsuarioRegistroDto UsuarioRegistroDto);
        bool ExisteUsuario(int id);
            
        bool ExisteUsuario(string nombre);

        Usuario CrearUsuario(Usuario usuario);

        bool ActualizarUsuario(Usuario usuario);

        bool EliminarUsuario(Usuario usuario);

        bool Guardar();

    }
}
