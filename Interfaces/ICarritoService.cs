using TiendaOnline.DTOS;

namespace TiendaOnline.Interfaces
{
    public interface ICarritoService
    {
        Task<CarritoDto> ObtenerCarritoPorId(int id);
    }
}