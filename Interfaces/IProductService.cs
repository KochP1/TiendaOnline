using Microsoft.AspNetCore.JsonPatch;
using TiendaOnline.DTOS;
using TiendaOnline.Models;

namespace TiendaOnline.Interfaces
{
    public interface IProductoService
    {
        Task<bool> BorrarProducto(int id);
        Task<ProductoDto> CrearProducto(CrearProductoDto crearProductoDto);
        Task<IEnumerable<ProductoDto>> ObtenerProductos();
        Task<ProductoDto> ObtenerProductPorId(int id);
        Task<bool> PatchProducto(JsonPatchDocument<PatchProductDto> patchDoc, Product productDb);
    }
}