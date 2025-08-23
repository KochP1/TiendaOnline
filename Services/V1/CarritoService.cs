using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.Data;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;
using TiendaOnline.Models;

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

        public async Task<CarritoItemDtoSinProducto> AñadirItem(int id, CrearCarritoItemDto crearCarritoItemDto)
        {
            var carrito = await context.Carts.FirstOrDefaultAsync(x => x.UserId == id);

            var carritoItem = mapper.Map<CartItem>(crearCarritoItemDto);

            carritoItem.CartId = carrito!.CartId;

            context.CartItems.Add(carritoItem);
            await context.SaveChangesAsync();

            var newCarritoItemDto = mapper.Map<CarritoItemDtoSinProducto>(carritoItem);
            return newCarritoItemDto;
        }

        public async Task<bool> BorrarItem(int id)
        {
            var record = await context.CartItems.FirstOrDefaultAsync(x => x.CartItemId == id);

            if (record is null)
            {
                return false;
            }

            context.CartItems.Remove(record);
            await context.SaveChangesAsync();
            return true;
        }
    }
}