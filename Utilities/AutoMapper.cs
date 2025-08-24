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
            CreateMap<CarritoDto, Cart>();
            CreateMap<CartItem, CarritoItemDto>().ForMember(dest => dest.Producto, config => config.MapFrom(src => src.Product));
            CreateMap<CarritoItemDto, CartItem>();
            CreateMap<CartItem, CrearCarritoItemDto>().ReverseMap();
            CreateMap<CarritoItemDtoSinProducto, CartItem>().ReverseMap();
            CreateMap<CartItem, PatchCarritoItemDto>().ReverseMap();

            // ORDENES
            CreateMap<CrearOrdenDto, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore());
            CreateMap<Order, CrearOrdenDto>();
            CreateMap<Order, OrdenDto>().ReverseMap();
            CreateMap<OrderItem, OrdenItemDto>().ReverseMap();
            CreateMap<Order, OrdenDetalladaDto>()
                .ForMember(dest => dest.OrdenItems, config => config.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Usuario, config => config.MapFrom(src => src.User));
            CreateMap<OrdenDetalladaDto, Order>();
            CreateMap<CrearOrdenItemDto, OrderItem>()
                .ForMember(dest => dest.OrderItemId, opt => opt.Ignore()) // Si es identity
                .ForMember(dest => dest.Order, opt => opt.Ignore())       // Ignorar la navegación
                .ForMember(dest => dest.Product, opt => opt.Ignore());    // Ignorar la navegación

            CreateMap<OrderItem, CrearOrdenItemDto>();

            // PAGOS
            CreateMap<Payment, PagoConOrdenDto>()
                .ForMember(dest => dest.Orden, config => config.MapFrom(src => src.Order));
            CreateMap<PagoDto, Payment>().ReverseMap();
            CreateMap<CrearPagoDto, Payment>().ReverseMap();
            CreateMap<PatchPagoDto, Payment>().ReverseMap();
        }
    }
}