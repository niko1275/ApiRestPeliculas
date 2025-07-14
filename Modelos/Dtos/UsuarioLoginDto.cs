using System.ComponentModel.DataAnnotations;

namespace ApiRestPeliculas.Modelos.Dtos
{
    public class UsuarioLoginDto
    {
       

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }

    }
}