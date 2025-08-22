using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    }
}