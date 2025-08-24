using TiendaOnline.DTOS;

namespace TiendaOnline.Interfaces
{
    public interface IOrderService
    {
        Task<OrdenDto> CrearOrden(CrearOrdenDto crearOrdenDto, int id);
        Task<IEnumerable<OrdenDetalladaDto>> ObtenerOrdenPorId(int id);
    }

}