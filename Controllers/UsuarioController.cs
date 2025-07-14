
using ApiRestPeliculas.Modelos.Dtos;
using ApiRestPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioRepositorio UsuarioRepo;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, IMapper mapper)
        {
            UsuarioRepo = usuarioRepositorio;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult ObtenerUsuarios()
        {
            UsuarioRepo.ObtenerUsuarios();
            var listaUsuariosDto = new List<UsuarioDto>();
            foreach (var usuario in UsuarioRepo.ObtenerUsuarios())
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(usuario));
            }
            return Ok(listaUsuariosDto);

        }


        [HttpGet("{usuarioId:int}", Name = "ObtenerUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ObtenerUsuario(int usuarioId)
        {
            var itemUsuario = UsuarioRepo.ObtenerUsuario(usuarioId);
            if (itemUsuario == null)
            {
                return NotFound();
            }
            var itemUsuarioDto = _mapper.Map<UsuarioDto>(itemUsuario);
            return Ok(itemUsuarioDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            if (usuarioRegistroDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var usuarioCreado = await UsuarioRepo.Registro(usuarioRegistroDto);

                // Aquí podrías usar CreatedAtRoute si tienes un endpoint como "ObtenerUsuario"
                return Created("", usuarioCreado); // O simplemente usar Ok(usuarioCreado);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }



        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto loginDto)
        {
            if (loginDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var respuestaLogin = await UsuarioRepo.Login(loginDto);

            if (respuestaLogin == null)
            {
                return Unauthorized(new { mensaje = "Nombre de usuario o contraseña incorrectos" });
            }

            return Ok(respuestaLogin); // Devuelve token y datos del usuario
        }
    }
}
