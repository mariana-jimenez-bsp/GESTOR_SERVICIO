using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Usuarios.Usuarios
{
    public partial class Usuarios : ComponentBase
    {
        public string esquema = string.Empty;
        public List<mUsuariosParaEditar> usuarios = new List<mUsuariosParaEditar>();
        public List<mClientes> listaClientes = new List<mClientes>();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        public string codigoUsuario = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await RefrescarListaDeUsuarios();
            cargaInicial = true;


        }

        


        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }
        private async Task RefrescarListaDeUsuarios()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaDeUsuariosParaEditar(esquema);
            if (UsuariosService.ListaDeUsuariosParaEditar != null)
            {
                usuarios = UsuariosService.ListaDeUsuariosParaEditar;
            }
        }
        bool actividarModalAgregarUsuario = false;

        async Task ClickHandlerAgregarUsuario(bool activar)
        {
            actividarModalAgregarUsuario = activar;
            if (!activar)
            {
                await RefrescarListaDeUsuarios();
            }
            StateHasChanged();
        }
        private async Task EnviarCodigo(bool activar, string codigo)
        {
            if (activar)
            {
                codigoUsuario = codigo;
                await ClickHandlerEditarUsuario(activar);
            }
        }
        bool actividarModalEditarUsuario = false;

        async Task ClickHandlerEditarUsuario(bool activar)
        {
            
           
            if (!activar)
            {
                await RefrescarListaDeUsuarios();
                codigoUsuario = string.Empty;
            }
            actividarModalEditarUsuario = activar;
            StateHasChanged();
        }
    }
}
