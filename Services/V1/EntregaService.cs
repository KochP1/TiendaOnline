using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.Data;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;
using TiendaOnline.Models;

namespace TiendaOnline.Services
{

    public class EntregaService : IEntregaService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EntregaService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EntregaDto>> ObtenerEntregas()
        {
            var entregas = await context.Shippings.OrderBy(x => x.ShippingId).ToListAsync();
            var entregasDto = mapper.Map<IEnumerable<EntregaDto>>(entregas);
            return entregasDto;
        }

        public async Task<Shipping> ObtenerEntregaModelPorId(int id)
        {
            var entrega = await context.Shippings.FirstOrDefaultAsync(x => x.ShippingId == id);
            return entrega;
        }

        public async Task<EntregaDetalladaDto> ObtenerEntregaPorId(int id)
        {
            var entrega = await context.Shippings
                .Include(x => x.Order)
                    .ThenInclude(x => x.User)
                .Include(x => x.Order)
                    .ThenInclude(x => x.OrderItems).ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.ShippingId == id);

            var entregaDto = mapper.Map<EntregaDetalladaDto>(entrega);
            return entregaDto;
        }

        public async Task<IEnumerable<EntregaDetalladaDto>> ObtenerEntregaPorUserId(int id)
        {
            var entregas = await context.Shippings
                .Where(x => x.Order.UserId == id)
                .Include(x => x.Order)
                    .ThenInclude(x => x.User)
                .Include(x => x.Order)
                    .ThenInclude(x => x.OrderItems).ThenInclude(x => x.Product)
                .ToListAsync();

            var entregasDto = mapper.Map<IEnumerable<EntregaDetalladaDto>>(entregas);
            return entregasDto;
        }

        public async Task<EntregaDto> CrearEntrega(CrearEntregaDto crearEntregaDto)
        {
            var entrega = mapper.Map<Shipping>(crearEntregaDto);
            entrega.TrackingNumber = Guid.NewGuid().ToString();
            context.Shippings.Add(entrega);
            await context.SaveChangesAsync();

            var entregaDto = mapper.Map<EntregaDto>(entrega);
            return entregaDto;
        }

        public async Task<bool> PatchEntrega(JsonPatchDocument<PatchEntregaDto> patchDoc, Shipping entregaDb)
        {
            try
            {
                var patchEntregaDto = mapper.Map<PatchEntregaDto>(entregaDb);

                patchDoc.ApplyTo(patchEntregaDto);

                mapper.Map(patchEntregaDto, entregaDb);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> BorrarEntrega(int id)
        {
            var entrega = await context.Shippings.FirstOrDefaultAsync(x => x.ShippingId == id);

            if (entrega is null)
            {
                return false;
            }

            context.Shippings.Remove(entrega);
            await context.SaveChangesAsync();
            return true;
        }
    }
}