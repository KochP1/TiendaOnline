using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using TiendaOnline.Data;
using TiendaOnline.DTOS;
using TiendaOnline.Interfaces;
using TiendaOnline.Models;

namespace TiendaOnline.Services
{

    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UsuarioService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerUsuarios()
        {
            var usuarios = await context.Users.OrderBy(x => x.FirstName).ToListAsync();

            var usuariosDto = mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
            return usuariosDto;
        }

        public async Task<User> ObtenerUserModelPorId(int id)
        {
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            return usuario;
        }

        public async Task<UsuarioDto> ObtenerUsuarioPorId(int id)
        {
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            var usuarioDto = mapper.Map<UsuarioDto>(usuario);

            return usuarioDto;
        }

        public async Task<UsuarioDto> CrearUsuario(CrearUsuarioDto crearUsuarioDto)
        {
            var usuario = mapper.Map<User>(crearUsuarioDto);

            context.Users.Add(usuario);
            await context.SaveChangesAsync();

            var carrito = new Cart
            {
                UserId = usuario.UserId,
            };

            context.Carts.Add(carrito);
            await context.SaveChangesAsync();

            var usuarioDto = mapper.Map<UsuarioDto>(usuario);
            return usuarioDto;
        }

        public async Task<bool> BorrarUsuario(int id)
        {
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);

            if (usuario is null)
            {
                return false;
            }

            context.Users.Remove(usuario);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchUsuario(JsonPatchDocument<PatchUsuarioDto> patchDoc, User usuarioDb)
        {
            try
            {
                var usuarioPatchDto = mapper.Map<PatchUsuarioDto>(usuarioDb);

                patchDoc.ApplyTo(usuarioPatchDto);

                mapper.Map(usuarioPatchDto, usuarioDb);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<User> Login(LoginUsuarioDto loginUsuarioDto)
        {
            var usuario = await context.Users.FirstOrDefaultAsync(x => x.Email == loginUsuarioDto.Email);

            return usuario;
        }
    }
}