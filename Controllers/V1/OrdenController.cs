using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;

namespace TiendaOnline.Controllers
{
    [ApiController]
    [Route("api/v1/ordenes")]

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
    }
}