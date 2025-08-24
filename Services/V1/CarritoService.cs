using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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

        public async Task<CartItem> ObtenerCartItemPorId(int id)
        {
            var cartItem = await context.CartItems.FirstOrDefaultAsync(x => x.CartItemId == id);
            return cartItem;
        }

        public async Task<CarritoItemDtoSinProducto> AñadirItem(int id, CrearCarritoItemDto crearCarritoItemDto)
        {
            var carrito = await context.Carts.FirstOrDefaultAsync(x => x.UserId == id);

            var carritoItem = mapper.Map<CartItem>(crearCarritoItemDto);

            carritoItem.CartId = carrito!.CartId;

            carrito.TotalAmount = (carritoItem.UnitPrice * carritoItem.Quantity) + (carrito.TotalAmount ?? 0);
            context.CartItems.Add(carritoItem);
            context.Carts.Update(carrito);
            await context.SaveChangesAsync();

            var newCarritoItemDto = mapper.Map<CarritoItemDtoSinProducto>(carritoItem);
            return newCarritoItemDto;
        }

        public async Task<bool> BorrarItem(int id, int userId)
        {
            var record = await context.CartItems.FirstOrDefaultAsync(x => x.CartItemId == id);
            var carrito = await context.Carts.FirstOrDefaultAsync(x => x.UserId == userId);

            if (record is null)
            {
                return false;
            }

            await ActualizarTotalCarrito(carrito!, record);
            context.CartItems.Remove(record);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchCarritoItem(JsonPatchDocument<PatchCarritoItemDto> patchDoc, CartItem cartItemDb)
        {
            try
            {
                var patchCarritoItemDto = mapper.Map<PatchCarritoItemDto>(cartItemDb);

                patchDoc.ApplyTo(patchCarritoItemDto);

                mapper.Map(patchCarritoItemDto, cartItemDb);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task ActualizarTotalCarrito(Cart carrito, CartItem cartItem)
        {
            carrito.TotalAmount = (carrito.TotalAmount ?? 0) - (cartItem.Subtotal ?? 0);
            context.Carts.Update(carrito);
            await context.SaveChangesAsync();
        }
    }
}