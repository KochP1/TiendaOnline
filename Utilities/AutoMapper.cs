using AutoMapper;
using TiendaOnline.DTOS;
using TiendaOnline.Models;

namespace TiendaOnline.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            // PRODUCTOS
            CreateMap<CrearProductoDto, Product>().ReverseMap();
            CreateMap<ProductoDto, Product>().ReverseMap();
            CreateMap<PatchProductDto, Product>().ReverseMap();

            // USUARIOS
            CreateMap<UsuarioDto, User>().ReverseMap();
            CreateMap<CrearUsuarioDto, User>()
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.PasswordHash)))
                .ForMember(dest => dest.Carts,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UserId,
                    opt => opt.Ignore());
            CreateMap<User, CrearUsuarioDto>();
            CreateMap<PatchUsuarioDto, User>().ReverseMap();
            CreateMap<CredencialesUsuarioDto, User>().ReverseMap();

            // CARRITO
            CreateMap<Cart, CarritoDto>().ForMember(dest => dest.CarritoItems, config => config.MapFrom(src => src.CartItems));
            CreateMap<CartItem, CarritoItemDto>().ForMember(dest => dest.Producto, config => config.MapFrom(src => src.Product));
        }
    }
}