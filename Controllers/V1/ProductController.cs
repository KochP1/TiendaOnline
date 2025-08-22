using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;

namespace TiendaOnline.Controllers
{
    [ApiController]
    [Route("api/v1/productos")]
    [Authorize]

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

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await productoService.BorrarProducto(id);

                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<PatchProductDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is required");
            }

            // Obtener entidad desde el servicio
            var productDb = await productoService.ObtenerProductModelPorId(id);
            if (productDb == null)
            {
                return NotFound();
            }

            var productPatchDto = mapper.Map<PatchProductDto>(productDb);
            patchDoc.ApplyTo(productPatchDto, ModelState);

            if (!TryValidateModel(productPatchDto))
            {
                return ValidationProblem(ModelState);
            }

            var success = await productoService.PatchProducto(patchDoc, productDb);

            if (!success)
            {
                return StatusCode(500, "Error al aplicar el patch");
            }

            return NoContent();
        }
    }
}