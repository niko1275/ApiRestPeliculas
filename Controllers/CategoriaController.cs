using ApiRestPeliculas.Modelos;
using ApiRestPeliculas.Modelos.Dtos;
using ApiRestPeliculas.Repositorio;
using ApiRestPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestPeliculas.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaRepositorio categoriaRepositorio, IMapper mapper)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ObtenerCategorias()
        {
            var listaCategorias = _categoriaRepositorio.ObtenerCategorias();
            var listaCategoriasDto = new List<CategoriaDto>();
            foreach (var categoria in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(categoria));
            }
            return Ok(listaCategoriasDto);

        }


        [HttpGet("{categoriaId:int}", Name = "ObtenerCategoria")]
        [ResponseCache(CacheProfileName="default")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ObtenerCategoria(int categoriaId)
        {
            var itemCategoria = _categoriaRepositorio.ObtenerCategoria(categoriaId);

            if (itemCategoria == null)
            {
                return NotFound();
            }

            var itemcategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
            return Ok(itemcategoriaDto);

        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult CrearCategoria([FromBody] CrearCategoriaDto crearCategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (crearCategoriaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_categoriaRepositorio.ExisteCategoria(crearCategoriaDto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);

            if (!_categoriaRepositorio.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo Salio mal guardando el registro{categoria.Nombre}");
                return StatusCode(404, ModelState);

            }

            return CreatedAtRoute("ObtenerCategoria", new { categoriaId = categoria.Id }, categoria);

        }




        [Authorize(Roles = "admin")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCategoria(int id, [FromBody] CategoriaDto actualizarCategoriaDto)
        {
            // Validar entrada
            if (actualizarCategoriaDto == null)
                return BadRequest("Los datos son requeridos");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(id == 0 || actualizarCategoriaDto.Id != id)
            {
                return BadRequest(ModelState);
            }

            var categoria = _mapper.Map<Categoria>(actualizarCategoriaDto);

            if (!_categoriaRepositorio.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{categoria.Nombre}");
                return StatusCode(404, ModelState);
            }

            return Ok(categoria);

        }
    }

}

