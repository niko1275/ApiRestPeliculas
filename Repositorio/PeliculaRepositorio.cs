using ApiRestPeliculas.Data;
using ApiRestPeliculas.Modelos;
using ApiRestPeliculas.Repositorio.IRepositorio;

namespace ApiRestPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculasRepositorio
    {
        private readonly AplicationDbContext _context;

        public PeliculaRepositorio(AplicationDbContext context)
        {
            _context = context;
        }
        public bool ActualizarPeliculas(Peliculas pelicula)
        {
           pelicula.FechaCreacion = DateTime.UtcNow;
           _context.Peliculas.Update(pelicula);
           return Guardar();   
        }

        
        public bool CrearPeliculas(Peliculas pelicula)
        {
            Peliculas peliculaExistente = _context.Peliculas.FirstOrDefault(p => p.Titulo == pelicula.Titulo);
            if (peliculaExistente != null)
            {
                return false; // La película ya existe
            }
            _context.Peliculas.Add(pelicula);
            pelicula.FechaCreacion = DateTime.UtcNow;
            return Guardar();
        }

        public bool EliminarPeliculas(Peliculas pelicula)
        {
            _context.Peliculas.Remove(pelicula);
            return Guardar();
        }

        public bool ExistePeliculas(int id)
        {
            return _context.Peliculas.Any(p => p.Id == id);
        }

        public bool ExistePeliculas(string nombre)
        {
            return _context.Peliculas.Any(p => p.Titulo.ToLower().Trim() == nombre.ToLower().Trim());
        }

        public bool Guardar()
        {
            try
            {
                return _context.SaveChanges() >= 0;
            }
            catch (Exception ex)
            {
                // Aquí puedes registrar el error con un logger, si tienes uno
                Console.WriteLine($"Error al guardar en la base de datos: {ex.Message}");
                // También podrías relanzar la excepción si quieres manejarla más arriba
                return false;
            }
        }

       
        public Peliculas ObtenerPelicula(int PeliculaId)
        {
            Peliculas pelicula = _context.Peliculas.FirstOrDefault(p => p.Id == PeliculaId);
         
            return pelicula;
        }

        public ICollection<Peliculas> ObtenerPeliculas()
        {
               
             return _context.Peliculas.ToList();
        }
    }
}
