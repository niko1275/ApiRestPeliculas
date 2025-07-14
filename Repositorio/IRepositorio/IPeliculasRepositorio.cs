using ApiRestPeliculas.Modelos;


namespace ApiRestPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculasRepositorio
    {
        // ✅ Obtener todas las películas
        ICollection<Peliculas> ObtenerPeliculas();

        // ✅ Obtener una película por ID
        Peliculas ObtenerPelicula(int peliculaId);
        bool ExistePeliculas(int id);

        bool ExistePeliculas(string nombre);
        bool CrearPeliculas(Peliculas pelicula);
        bool ActualizarPeliculas(Peliculas pelicula);

        bool EliminarPeliculas(Peliculas pelicula);

        bool Guardar();
    }
}
