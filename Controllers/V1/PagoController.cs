using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;

namespace TiendaOnline.Controllers
{
    [ApiController]
    [Route("api/v1/pagos")]
    [Authorize]

    public class PagoController : ControllerBase
    {
        private readonly IPagoService pagoService;
        private readonly IMapper mapper;

        public PagoController(IPagoService pagoService, IMapper mapper)
        {
            this.pagoService = pagoService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var pagos = await pagoService.ObtenerPagos();
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var pago = await pagoService.ObtenerPagoPorId(id);

                if (pago is null)
                {
                    return NotFound();
                }
                return Ok(pago);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("getByUserId")]
        public async Task<ActionResult> GetByUserId()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                var pagos = await pagoService.ObtenerPagosPorUserId(int.Parse(userId!));
                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CrearPagoDto crearPagoDto)
        {
            try
            {
                var pago = await pagoService.CrearPago(crearPagoDto);
                return StatusCode(201, pago);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<PatchPagoDto> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest("Patch document is required");
                }

                var pagoDb = await pagoService.ObtenerPagoModelPorId(id);
                if (pagoDb == null)
                {
                    return NotFound();
                }

                var patchPagoDto = mapper.Map<PatchPagoDto>(pagoDb);
                patchDoc.ApplyTo(patchPagoDto, ModelState);

                if (!TryValidateModel(patchPagoDto))
                {
                    return ValidationProblem(ModelState);
                }

                var success = await pagoService.PatchPago(patchDoc, pagoDb);

                if (!success)
                {
                    return StatusCode(500, "Error al aplicar el patch");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await pagoService.BorrarPago(id);

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