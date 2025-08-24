using Microsoft.AspNetCore.JsonPatch;
using TiendaOnline.DTOS;
using TiendaOnline.Models;

namespace TiendaOnline.Interfaces
{
        public interface IEntregaService
    {
        Task<bool> BorrarEntrega(int id);
        Task<EntregaDto> CrearEntrega(CrearEntregaDto crearEntregaDto);
        Task<Shipping> ObtenerEntregaModelPorId(int id);
        Task<EntregaDetalladaDto> ObtenerEntregaPorId(int id);
        Task<IEnumerable<EntregaDetalladaDto>> ObtenerEntregaPorUserId(int id);
        Task<IEnumerable<EntregaDto>> ObtenerEntregas();
        Task<bool> PatchEntrega(JsonPatchDocument<PatchEntregaDto> patchDoc, Shipping entregaDb);
    }
}