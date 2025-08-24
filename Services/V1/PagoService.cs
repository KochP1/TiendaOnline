using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.Data;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;
using TiendaOnline.Models;

namespace TiendaOnline.Services
{

    public class PagoService : IPagoService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PagoService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PagoDto>> ObtenerPagos()
        {
            var pagos = await context.Payments.OrderBy(x => x.PaymentDate).ToListAsync();

            var pagosDto = mapper.Map<IEnumerable<PagoDto>>(pagos);
            return pagosDto;
        }

        public async Task<Payment> ObtenerPagoModelPorId(int id)
        {
            var pago = await context.Payments.FirstOrDefaultAsync(x => x.PaymentId == id);
            return pago;
        }

        public async Task<PagoConOrdenDto> ObtenerPagoPorId(int id)
        {
            var pago = await context.Payments
                .Include(x => x.Order)
                    .ThenInclude(x => x.OrderItems)
                        .ThenInclude(x => x.Product)
                .Include(x => x.Order)
                    .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.PaymentId == id);

            var pagoDto = mapper.Map<PagoConOrdenDto>(pago);
            return pagoDto;
        }

        public async Task<IEnumerable<PagoConOrdenDto>> ObtenerPagosPorUserId(int id)
        {
            var pagos = await context.Payments
                .Where(x => x.Order.UserId == id)
                .Include(x => x.Order)
                    .ThenInclude(x => x.OrderItems)
                        .ThenInclude(x => x.Product)
                .Include(x => x.Order)
                    .ThenInclude(x => x.User)
                .ToListAsync();

            var pagosDto = mapper.Map<IEnumerable<PagoConOrdenDto>>(pagos);
            return pagosDto;
        }
        public async Task<PagoDto> CrearPago(CrearPagoDto crearPagoDto)
        {
            var pago = mapper.Map<Payment>(crearPagoDto);
            pago.TransactionId = Guid.NewGuid().ToString();
            pago.PaymentDate = DateTime.UtcNow;
            context.Payments.Add(pago);
            await context.SaveChangesAsync();

            var pagoDto = mapper.Map<PagoDto>(pago);
            return pagoDto;
        }

        public async Task<bool> PatchPago(JsonPatchDocument<PatchPagoDto> patchDoc, Payment pagoDb)
        {
            try
            {
                var patchPagoDto = mapper.Map<PatchPagoDto>(pagoDb);

                patchDoc.ApplyTo(patchPagoDto);

                mapper.Map(patchPagoDto, pagoDb);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> BorrarPago(int id)
        {
            var pago = await context.Payments.FirstOrDefaultAsync(x => x.PaymentId == id);

            if (pago is null)
            {
                return false;
            }

            context.Payments.Remove(pago);
            await context.SaveChangesAsync();

            return true;
        }
    }
}