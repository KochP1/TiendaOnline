using TiendaOnline.DTOS;

namespace TiendaOnline.Interfaces
{
    public interface ICarritoService
    {
        Task<CarritoDto> ObtenerCarritoPorId(int id);
        Task<CarritoItemDtoSinProducto> AñadirItem(int id, CrearCarritoItemDto crearCarritoItemDto);
        Task<bool> BorrarItem(int id);
    }
}