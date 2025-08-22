using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;

namespace TiendaOnline.Controllers
{
    [ApiController]
    [Route("api/v1/productos")]

    public class ProdudctController : ControllerBase
    {
        private readonly IProductoService productoService;
        private readonly IMapper mapper;

        public ProdudctController(IProductoService productoService, IMapper mapper)
        {
            this.productoService = productoService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var products = await productoService.ObtenerProductos();
                return Ok(products);
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
                var product = await productoService.ObtenerProductPorId(id);

                if (product is null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CrearProductoDto crearProductoDto)
        {
            try
            {
                var newProduct = await productoService.CrearProducto(crearProductoDto);
                return StatusCode(201, newProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }
    }
}