using ApiRestPeliculas.Modelos;
using ApiRestPeliculas.Modelos.Dtos;
using ApiRestPeliculas.Repositorio;
using ApiRestPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestPeliculas.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private readonly IPeliculasRepositorio _PeliculaRepositorio;
        private readonly IMapper _mapper;


        public PeliculaController(IPeliculasRepositorio peliculaRepositorio, IMapper mapper)
        {
            _PeliculaRepositorio = peliculaRepositorio;
            _mapper = mapper;
        }


        [HttpGet]
        [ResponseCache(Duration =20)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObtenerPeliculas()
        {
            var listaPeliculas = _PeliculaRepositorio.ObtenerPeliculas();
            var listaPeliculasDto = new List<PeliculaDto>();
            foreach (var pelicula in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(pelicula));
            }
            return Ok(listaPeliculasDto);
        }



        [HttpGet("{peliculaId:int}", Name = "ObtenerPelicula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ObtenerCategoria(int peliculaId)
        {
            var itemPelicula = _PeliculaRepositorio.ObtenerPelicula(peliculaId);

            if (itemPelicula == null)
            {
                return NotFound();
            }

            var itemPeliculaDTO = _mapper.Map<PeliculaDto>(itemPelicula);
            return Ok(itemPeliculaDTO);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CrearPeliculas([FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (peliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            var pelicula = _mapper.Map<Peliculas>(peliculaDto);


            if (_PeliculaRepositorio.ExistePeliculas(pelicula.Id))
            {
                ModelState.AddModelError("Título", "Ya existe una película con ese título.");
                return Conflict(ModelState);
            }

            if (!_PeliculaRepositorio.CrearPeliculas(pelicula))
            {
                ModelState.AddModelError("", "Ocurrió un error al guardar la película.");
                return StatusCode(500, ModelState);
            }

            
            return CreatedAtRoute("ObtenerPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }
    }
}
