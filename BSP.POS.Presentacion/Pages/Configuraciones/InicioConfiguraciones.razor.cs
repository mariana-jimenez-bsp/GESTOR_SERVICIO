using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Configuraciones
{
    public partial class InicioConfiguraciones : ComponentBase
    {
        public mDatosLicencia licencia = new mDatosLicencia();
        public string esquema = string.Empty;
        public string rol = string.Empty;
        public bool cargaInicial = false;
        private string codigoLicencia = string.Empty;
        public mPerfil perfilActual = new mPerfil();
        List<mObjetoPermiso> permisos = new List<mObjetoPermiso>();
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            var PermisosClaim = user.Claims.FirstOrDefault(c => c.Type == "permisos");
            if (PermisosClaim != null)
            {
                permisos = JsonConvert.DeserializeObject<List<mObjetoPermiso>>(PermisosClaim.Value);
            }
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await LicenciasService.ObtenerDatosDeLicencia();
            if(LicenciasService.licencia != null)
            {
                licencia = LicenciasService.licencia;
            }
            

            
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            codigoLicencia = await LicenciasService.ObtenerCodigoDeLicenciaDesencriptado();
            
            cargaInicial = true;
        }

        private void IrAUsuarios()
        {

            navigationManager.NavigateTo($"Configuraciones/Usuarios");
        }
        private void IrADepartamentos()
        {

            navigationManager.NavigateTo($"Configuraciones/Departamentos");
        }
        private void IrAActividades()
        {

            navigationManager.NavigateTo($"Configuraciones/Actividades");
        }
    }
}
