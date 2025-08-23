using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.Data;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;

namespace TiendaOnline.Services
{

    public class CarritoService : ICarritoService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CarritoService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CarritoDto> ObtenerCarritoPorId(int id)
        {
            var carrito = await context.Carts.Where(x => x.UserId == id).Include(x => x.CartItems).ThenInclude(x => x.Product).FirstOrDefaultAsync();

            var carritoDto = mapper.Map<CarritoDto>(carrito);

            return carritoDto;
        }
    }
}