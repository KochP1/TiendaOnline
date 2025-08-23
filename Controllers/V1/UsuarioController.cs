using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]

    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper, IConfiguration configuration)
        {
            this.usuarioService = usuarioService;
            this.mapper = mapper;
            this.configuration = configuration;
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

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUsuarioDto loginUsuarioDto)
        {
            try
            {
                var usuario = await usuarioService.Login(loginUsuarioDto);

                if (!BCrypt.Net.BCrypt.Verify(loginUsuarioDto.Password, usuario.PasswordHash))
                {
                    Console.WriteLine("Me ejecuto");
                    return Unauthorized();
                }

                if (usuario is null)
                {
                    Console.WriteLine("Usuario nulo");
                    return NotFound();
                }

                var response = ConstruirToken(usuario);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpPatch]
        public async Task<ActionResult> Patch(JsonPatchDocument<PatchUsuarioDto> patchDoc)
        {
            var userId = User.FindFirst("userId")?.Value;
            if (patchDoc == null)
            {
                return BadRequest("Patch document is required");
            }

            var usuarioDb = await usuarioService.ObtenerUserModelPorId(int.Parse(userId!));
            if (usuarioDb == null)
            {
                return NotFound();
            }

            var patchUsuarioDto = mapper.Map<PatchUsuarioDto>(usuarioDb);
            patchDoc.ApplyTo(patchUsuarioDto, ModelState);

            if (!TryValidateModel(patchUsuarioDto))
            {
                return ValidationProblem(ModelState);
            }

            var success = await usuarioService.PatchUsuario(patchDoc, usuarioDb);

            if (!success)
            {
                return StatusCode(500, "Error al aplicar el patch");
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete()
        {
            var userId = User.FindFirst("userId")?.Value;
            var result = await usuarioService.BorrarUsuario(int.Parse(userId!));

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        private RespuestaAutentificacionDto ConstruirToken(User usuario)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", usuario.UserId.ToString()),
                new Claim("email", usuario.Email),
                new Claim("nombre", usuario.FirstName),
                new Claim("apellido", usuario.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Si tienes roles personalizados, puedes agregarlos así:
            // claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            // claims.Add(new Claim(ClaimTypes.Role, "User"));

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]!));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var tokenSeguridad = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad);

            return new RespuestaAutentificacionDto
            {
                Token = token,
                Expiracion = expiracion
            };
        }
    }
}