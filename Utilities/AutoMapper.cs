using AutoMapper;
using TiendaOnline.DTOS;
using TiendaOnline.Models;

namespace TiendaOnline.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CrearProductoDto, Product>().ReverseMap();
            CreateMap<ProductoDto, Product>().ReverseMap();
            CreateMap<PatchProductDto, Product>().ReverseMap();
        }
    }
}