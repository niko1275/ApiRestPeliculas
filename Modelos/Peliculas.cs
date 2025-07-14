using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ApiRestPeliculas.Modelos
{
    public class Peliculas
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El título es obligatorio")]
        [MaxLength(100, ErrorMessage = "El título no puede tener más de 100 caracteres")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres")]

      
        public string imagen { get; set; } // URL de la imagen
        public int Duracion { get; set; } // Duración en minutos
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;


        public enum TipoClasificacion { siete, trece, dieciseis, dieciocho }
        public TipoClasificacion Clasificacion { get; set; }


        public int categoriaId { get; set; }
        [ForeignKey("categoriaId")]
        public Categoria Categoria { get; set; }
    }
}
