using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;

namespace TiendaOnline.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]

    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;
        private readonly IMapper mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            this.usuarioService = usuarioService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var usuarios = await usuarioService.ObtenerUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var usuario = await usuarioService.ObtenerUsuarioPorId(id);

                if (usuario is null)
                {
                    return NotFound();
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CrearUsuarioDto crearUsuarioDto)
        {
            try
            {
                var newUsuario = await usuarioService.CrearUsuario(crearUsuarioDto);
                return StatusCode(201, newUsuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await usuarioService.BorrarUsuario(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}