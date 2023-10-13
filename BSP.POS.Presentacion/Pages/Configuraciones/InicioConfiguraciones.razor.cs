using BSP.POS.Presentacion.Models.Licencias;
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
            cargaInicial = true;
        }

        private void IrAMisUsuarios()
        {

            navigationManager.NavigateTo($"Configuraciones/Usuarios");
        }
    }
}
