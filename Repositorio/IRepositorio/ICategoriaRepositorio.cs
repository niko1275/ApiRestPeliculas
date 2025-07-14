using ApiRestPeliculas.Modelos;


namespace ApiRestPeliculas.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio
    {
        ICollection<Categoria> ObtenerCategorias();
        Categoria ObtenerCategoria(int CategoriaId);
        bool ExisteCategoria(int id);

        bool ExisteCategoria(string nombre);
        bool CrearCategoria(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);

        bool EliminarCategoria(Categoria categoria);

        bool Guardar();
    }
}
