using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Licencias;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
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
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
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

        private void IrAMisUsuarios()
        {

            navigationManager.NavigateTo($"Configuraciones/Usuarios");
        }
    }
}
