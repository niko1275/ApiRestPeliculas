using System.ComponentModel.DataAnnotations;

namespace ApiRestPeliculas.Modelos.Dtos
{
    public class CategoriaDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "el nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }
        // Puedes agregar más propiedades si es necesario
    }
}
