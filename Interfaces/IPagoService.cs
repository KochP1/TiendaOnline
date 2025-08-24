using Microsoft.AspNetCore.JsonPatch;
using TiendaOnline.DTOS;
using TiendaOnline.Models;

namespace TiendaOnline.Interfaces
{
    public interface IPagoService
    {
        Task<bool> BorrarPago(int id);
        Task<PagoDto> CrearPago(CrearPagoDto crearPagoDto);
        Task<Payment> ObtenerPagoModelPorId(int id);
        Task<PagoConOrdenDto> ObtenerPagoPorId(int id);
        Task<IEnumerable<PagoDto>> ObtenerPagos();
        Task<IEnumerable<PagoConOrdenDto>> ObtenerPagosPorUserId(int id);
        Task<bool> PatchPago(JsonPatchDocument<PatchPagoDto> patchDoc, Payment pagoDb);
    }
}