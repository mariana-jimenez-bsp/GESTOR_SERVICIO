using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Usuarios.Usuarios
{
    public partial class Usuarios : ComponentBase
    {
        public string esquema = string.Empty;
        public List<mUsuariosParaEditar> usuarios = new List<mUsuariosParaEditar>();
        public List<mClientes> listaClientes = new List<mClientes>();
        public mDatosLicencia licencia = new mDatosLicencia();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        List<mObjetoPermiso> permisos = new List<mObjetoPermiso>();

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            var PermisosClaim = user.Claims.FirstOrDefault(c => c.Type == "permisos");
            if (PermisosClaim != null)
            {
                permisos = JsonConvert.DeserializeObject<List<mObjetoPermiso>>(PermisosClaim.Value);
            }
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
        
        
        private async Task IrAAgregarUsuario()
        {
            if(licencia.CantidadUsuarios <= usuarios.Count)
            {
                await AlertasService.SwalAdvertencia("Límite de Cantidad de Usuarios Alcanzado");
            }
            else
            {
                navigationManager.NavigateTo($"configuraciones/usuario/agregar");
            }
            
        }

        private void IrAEditarUsuario(string codigo)
        {
            navigationManager.NavigateTo($"configuraciones/usuario/editar/{codigo}");
        }
    }
}
