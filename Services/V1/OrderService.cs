using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.Data;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;
using TiendaOnline.Models;

namespace TiendaOnline.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<OrdenDetalladaDto>> ObtenerOrdenPorId(int id)
        {
            var ordenes = await context.Orders.Where(x => x.UserId == id).Include(x => x.User).Include(x => x.OrderItems).ThenInclude(x => x.Product).ToListAsync();
            var ordenesDto = mapper.Map<IEnumerable<OrdenDetalladaDto>>(ordenes);
            return ordenesDto;
        }

        public async Task<OrdenDto> CrearOrden(CrearOrdenDto crearOrdenDto, int id)
        {
            var carrito = await context.Carts.Include(x => x.CartItems).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.UserId == id);
            var orden = mapper.Map<Order>(crearOrdenDto);
            orden.UserId = id;
            orden.CartId = carrito!.CartId;
            orden.OrderNumber = Guid.NewGuid().ToString();
            orden.OrderTotal = carrito.TotalAmount ?? 0m;

            var ordenItemsDto = CrearOrdenItems(carrito, orden);
            var ordenItems = mapper.Map<ICollection<OrderItem>>(ordenItemsDto);
            orden.OrderItems = ordenItems;

            context.Orders.Add(orden);
            await context.SaveChangesAsync();
            var ordenDto = mapper.Map<OrdenDto>(orden);
            return ordenDto;
        }

        private ICollection<CrearOrdenItemDto> CrearOrdenItems(Cart carrito, Order order)
        {
            var ordenItems = new List<CrearOrdenItemDto>();

            foreach (var item in carrito.CartItems)
            {
                ordenItems.Add(new CrearOrdenItemDto
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Subtotal = item.Subtotal
                });
            }

            return ordenItems;
        }
    }
}