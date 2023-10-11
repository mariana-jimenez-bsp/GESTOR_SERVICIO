using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Licencias;
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
        public mLicencia licencia = new mLicencia();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        public string codigoUsuario = string.Empty;
        private bool estadoUsuarioNuevo = false;
        private bool estadoUsuarioActualizado = false;
        private bool estadoUsuarioNuevoCancelado = false;
        private bool estadoUsuarioActualizadoCancelado = false;
        private bool limiteDeUsuarios = false;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await RefrescarListaDeUsuarios();
            await LicenciasService.ObtenerDatosDeLicencia();
            if (LicenciasService.licencia != null)
            {
                licencia = LicenciasService.licencia;

            }
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
            if (activar)
            {
                estadoUsuarioNuevo = false;
                estadoUsuarioNuevoCancelado = false;
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
            if (activar)
            {
                estadoUsuarioActualizado = false;
                estadoUsuarioActualizadoCancelado = false;
            }
            actividarModalEditarUsuario = activar;
            StateHasChanged();
        }
        public void CambiarEstadoUsuarioNuevo(bool estado)
        {
            estadoUsuarioNuevo = estado;
        }

        public void CambiarEstadoUsuarioActualizado(bool estado)
        {
            estadoUsuarioActualizado = estado;
        }
        public void CambiarEstadoUsuarioNuevoCancelado(bool estado)
        {
            estadoUsuarioNuevoCancelado = estado;
        }
        public void CambiarEstadoUsuarioActualizadoCancelado(bool estado)
        {
            estadoUsuarioActualizadoCancelado = estado;
        }
        private async Task IrAAgregarUsuario()
        {
            limiteDeUsuarios = false;
            StateHasChanged();
            await Task.Delay(100);
            if(licencia.CantidadUsuarios <= usuarios.Count)
            {
                limiteDeUsuarios = true;
            }
            else
            {
                navigationManager.NavigateTo($"configuraciones/usuario/agregar");
            }
            
        }
    }
}
