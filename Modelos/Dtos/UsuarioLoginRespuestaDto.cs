using System.ComponentModel.DataAnnotations;

namespace ApiRestPeliculas.Modelos.Dtos
{
    public class UsuarioLoginRespuestaDto
    {
        public UsuarioDto Usuario { get; set; }
        public string Token { get; set; }

        public string role { get; set; }

    }
}