using AutoMapper;
using BCrypt.Net;
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
            CreateMap<CrearUsuarioDto, User>().ForMember(dest => dest.PasswordHash, config => config.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.PasswordHash)));
            CreateMap<User, CrearUsuarioDto>();
            CreateMap<PatchUsuarioDto, User>().ReverseMap();
            CreateMap<CredencialesUsuarioDto, User>().ReverseMap();
        }
    }
}