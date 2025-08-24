using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;

namespace TiendaOnline.Controllers
{
    [ApiController]
    [Route("api/v1/ordenes")]
    [Authorize]

    public class OrdenController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdenController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CrearOrdenDto crearOrdenDto)
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                var ordenDto = await orderService.CrearOrden(crearOrdenDto, int.Parse(userId!));
                return StatusCode(201, ordenDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                var ordenes = await orderService.ObtenerOrdenPorId(int.Parse(userId!));
                return Ok(ordenes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchOrden(int id, JsonPatchDocument<PatchOrden> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is required");
            }

            var ordenDb = await orderService.ObtenerOrdenModelPorId(id);
            if (ordenDb == null)
            {
                return NotFound();
            }

            var patchOrden = mapper.Map<PatchOrden>(ordenDb);
            patchDoc.ApplyTo(patchOrden, ModelState);

            if (!TryValidateModel(patchOrden))
            {
                return ValidationProblem(ModelState);
            }

            var success = await orderService.PatchOrden(patchDoc, ordenDb);

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
                var result = await orderService.BorrarOrden(id);
                
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