using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}