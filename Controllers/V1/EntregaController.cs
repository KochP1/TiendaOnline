using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;

namespace TiendaOnline.Controllers
{
    [ApiController]
    [Route("api/v1/entregas")]
    [Authorize]

    public class EntregasController : ControllerBase
    {
        private readonly IEntregaService entregaService;
        private readonly IMapper mapper;

        public EntregasController(IEntregaService entregaService, IMapper mapper)
        {
            this.entregaService = entregaService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var entregas = await entregaService.ObtenerEntregas();
                return Ok(entregas);
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
                var entrega = await entregaService.ObtenerEntregaPorId(id);
                return Ok(entrega);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("obtenerPorUserId")]
        public async Task<ActionResult> GetByUserId()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                var entrega = await entregaService.ObtenerEntregaPorUserId(int.Parse(userId!));
                return Ok(entrega);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CrearEntregaDto crearEntregaDto)
        {
            var entrega = await entregaService.CrearEntrega(crearEntregaDto);
            return StatusCode(201, entrega);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<PatchEntregaDto> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                {
                    return BadRequest("Patch document is required");
                }

                var entregaDb = await entregaService.ObtenerEntregaModelPorId(id);
                if (entregaDb == null)
                {
                    return NotFound();
                }

                var patchEntregaDto = mapper.Map<PatchEntregaDto>(entregaDb);
                patchDoc.ApplyTo(patchEntregaDto, ModelState);

                if (!TryValidateModel(patchEntregaDto))
                {
                    return ValidationProblem(ModelState);
                }

                var success = await entregaService.PatchEntrega(patchDoc, entregaDb);

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
                var result = await entregaService.BorrarEntrega(id);

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