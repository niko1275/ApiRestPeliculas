using System.ComponentModel.DataAnnotations;
using static ApiRestPeliculas.Modelos.Peliculas;

namespace ApiRestPeliculas.Modelos.Dtos
{
    public class PeliculaDto
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [MaxLength(100, ErrorMessage = "El título no puede tener más de 100 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres")]
        public string Descripcion { get; set; }

        public string? imagen { get; set; }


        public int Duracion { get; set; }

        [Required(ErrorMessage = "La clasificación es obligatoria")]
        public TipoClasificacion Clasificacion { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int categoriaId { get; set; }
    }
}
