using System.ComponentModel.DataAnnotations;

namespace ApiRestPeliculas.Modelos.Dtos
{
    public class CrearCategoriaDto
    {

        [Required(ErrorMessage = "el nombre es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        public string Nombre { get; set; }
       
      
    }
}
