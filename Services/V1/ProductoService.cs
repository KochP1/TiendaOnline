using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.Data;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;
using TiendaOnline.Models;

namespace TiendaOnline.Services
{

    public class ProductoService : IProductoService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProductoService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductoDto>> ObtenerProductos()
        {
            var productos = await context.Products
                            .OrderBy(x => x.Name)
                            .ToListAsync();

            var productsDto = mapper.Map<IEnumerable<ProductoDto>>(productos);

            return productsDto;
        }

        public async Task<ProductoDto> ObtenerProductPorId(int id)
        {
            var producto = await context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            var productoDto = mapper.Map<ProductoDto>(producto);
            return productoDto;
        }

        public async Task<Product> ObtenerProductModelPorId(int id)
        {
            var producto = await context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            return producto;
        }

        public async Task<ProductoDto> CrearProducto(CrearProductoDto crearProductoDto)
        {
            var product = mapper.Map<Product>(crearProductoDto);
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var productDto = mapper.Map<ProductoDto>(product);

            return productDto;
        }

        public async Task<bool> BorrarProducto(int id)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            if (product is null)
            {
                return false;
            }

            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchProducto(JsonPatchDocument<PatchProductDto> patchDoc, Product productDb)
        {
            try
            {
                var productPatchDto = mapper.Map<PatchProductDto>(productDb);

                patchDoc.ApplyTo(productPatchDto);

                mapper.Map(productPatchDto, productDb);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}