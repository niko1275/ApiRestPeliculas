using Microsoft.EntityFrameworkCore;
using ApiRestPeliculas.Modelos;

namespace ApiRestPeliculas.Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options)
            : base(options)
        {

        }
        //Aqui van los modelos
        // DbSet representa una tabla en la base de datos
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Peliculas> Peliculas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

    }
}
