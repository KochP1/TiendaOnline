using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;

namespace TiendaOnline.Controllers
{
    [ApiController]
    [Route("api/v1/carritos")]
    [Authorize]

    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService carritoService;
        private readonly IMapper mapper;

        public CarritoController(ICarritoService carritoService, IMapper mapper)
        {
            this.carritoService = carritoService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                var carritoDto = await carritoService.ObtenerCarritoPorId(int.Parse(userId!));
                return Ok(carritoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostCarritoItem(CrearCarritoItemDto crearCarritoItemDto)
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                var newCarritoItem = await carritoService.AñadirItem(int.Parse(userId!), crearCarritoItemDto);
                return StatusCode(201, newCarritoItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<PatchCarritoItemDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is required");
            }

            var cartItemDb = await carritoService.ObtenerCartItemPorId(id);
            if (cartItemDb == null)
            {
                return NotFound();
            }

            var patchCarritoItemDto = mapper.Map<PatchCarritoItemDto>(cartItemDb);
            patchDoc.ApplyTo(patchCarritoItemDto, ModelState);

            if (!TryValidateModel(patchCarritoItemDto))
            {
                return ValidationProblem(ModelState);
            }

            var success = await carritoService.PatchCarritoItem(patchDoc, cartItemDb);

            if (!success)
            {
                return StatusCode(500, "Error al aplicar el patch");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await carritoService.BorrarItem(id);

                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}