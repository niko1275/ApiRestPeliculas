using System.ComponentModel.DataAnnotations;
using static ApiRestPeliculas.Modelos.Peliculas;

using System.ComponentModel.DataAnnotations;
using static ApiRestPeliculas.Modelos.Peliculas;

namespace ApiRestPeliculas.Modelos.Dtos
{
    public class CrearPeliculaDto
    {
      
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public string? imagen { get; set; }

        public IFormFile Imagens { get; set; }

        public int Duracion { get; set; }

       
        public TipoClasificacion Clasificacion { get; set; }

       
        public int categoriaId { get; set; }
    }
}
