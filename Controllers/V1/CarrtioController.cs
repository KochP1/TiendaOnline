using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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