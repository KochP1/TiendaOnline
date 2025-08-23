using Microsoft.AspNetCore.JsonPatch;
using TiendaOnline.DTOS;
using TiendaOnline.Models;

namespace TiendaOnline.Interfaces
{
    public interface ICarritoService
    {
        Task<CarritoDto> ObtenerCarritoPorId(int id);
        Task<CarritoItemDtoSinProducto> AñadirItem(int id, CrearCarritoItemDto crearCarritoItemDto);
        Task<bool> BorrarItem(int id);
        Task<CartItem> ObtenerCartItemPorId(int id);
        Task<bool> PatchCarritoItem(JsonPatchDocument<PatchCarritoItemDto> patchDoc, CartItem cartItemDb);
    }
}