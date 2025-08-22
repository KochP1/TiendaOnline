using Microsoft.AspNetCore.JsonPatch;
using TiendaOnline.DTOS;
using TiendaOnline.Models;

namespace TiendaOnline.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> BorrarUsuario(int id);
        Task<UsuarioDto> CrearUsuario(CrearUsuarioDto crearUsuarioDto);
        Task<User> ObtenerUserModelPorId(int id);
        Task<UsuarioDto> ObtenerUsuarioPorId(int id);
        Task<IEnumerable<UsuarioDto>> ObtenerUsuarios();
        Task<bool> PatchUsuario(JsonPatchDocument<PatchUsuarioDto> patchDoc, User usuarioDb);
    }
}