using ApiRestPeliculas.Modelos;
using ApiRestPeliculas.Modelos.Dtos;
using AutoMapper;

namespace ApiRestPeliculas.PeliculasMapper
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper() {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CrearCategoriaDto>().ReverseMap();

            CreateMap<Peliculas,PeliculaDto>().ReverseMap();
            CreateMap<Usuario,UsuarioDto>().ReverseMap();


        }
    }
}
