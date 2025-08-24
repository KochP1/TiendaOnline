using Microsoft.AspNetCore.JsonPatch;
using TiendaOnline.DTOS;
using TiendaOnline.Models;

namespace TiendaOnline.Interfaces
{
    public interface IOrderService
    {
        Task<OrdenDto> CrearOrden(CrearOrdenDto crearOrdenDto, int id);
        Task<IEnumerable<OrdenDetalladaDto>> ObtenerOrdenPorId(int id);
        Task<bool> PatchOrden(JsonPatchDocument<PatchOrden> patchDoc, Order ordenDb);
        Task<Order> ObtenerOrdenModelPorId(int id);
        Task<bool> BorrarOrden(int id);
    }

}