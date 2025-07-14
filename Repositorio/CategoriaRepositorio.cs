using ApiRestPeliculas.Data;
using ApiRestPeliculas.Modelos;
using ApiRestPeliculas.Repositorio.IRepositorio;

namespace ApiRestPeliculas.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly AplicationDbContext _context;

        public CategoriaRepositorio(AplicationDbContext context)
        {
            _context = context;
        }
        public bool EliminarCategoria(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            return Guardar();
        }
        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.UtcNow;
            _context.Categorias.Update(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            categoria.FechaCreacion = DateTime.UtcNow;
            return Guardar();
        }

        public bool ExisteCategoria(int id)
        {
            _context.Categorias.Any(c => c.Id == id);
            return _context.Categorias.Any(c => c.Id == id);

        }

        public bool ExisteCategoria(string nombre)
        {
            _context.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return _context.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
        }

        public bool Guardar()
        {
            return _context.SaveChanges() >= 0 ? true : false;

        }

        public Categoria ObtenerCategoria(int CategoriaId)
        {

            return _context.Categorias.FirstOrDefault(c => c.Id == CategoriaId);

        }

        public ICollection<Categoria> ObtenerCategorias()
        {

            return _context.Categorias.OrderBy(c => c.Nombre).ToList();

        }

       
    }
}
