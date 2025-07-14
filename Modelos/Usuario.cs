using System.ComponentModel.DataAnnotations;

namespace ApiRestPeliculas.Modelos
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Rol { get; set; }
    }
}