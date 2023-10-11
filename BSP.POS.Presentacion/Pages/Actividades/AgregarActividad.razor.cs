using BSP.POS.Presentacion.Models.Actividades;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Actividades
{
    public partial class AgregarActividad: ComponentBase
    {
        
        public string esquema = string.Empty;
        public mActividades activadNueva = new mActividades();

        public string mensajeError;
        private bool actividadAgregada = false;
        private bool descartarCambios = false;
        private bool cargaInicial = false;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            cargaInicial = true;
        }

        
       

        private void CambioActividad(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                activadNueva.Actividad = e.Value.ToString();

            }

        }
        private void CambioCIReferencia(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                activadNueva.CI_referencia = e.Value.ToString();

            }

        }
        private void CambioHoras(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                activadNueva.horas = e.Value.ToString();

            }

        }
        private async Task DescartarCambios()
        {
            descartarCambios = false;
            activadNueva = new mActividades();
            StateHasChanged();
            await Task.Delay(100);
            descartarCambios = true;
        }
        private async Task AgregarActividadNueva()
        {
            mensajeError = null;
            actividadAgregada = false;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.AgregarActividad(activadNueva, esquema);
                actividadAgregada = true;
                activadNueva = new mActividades();
            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }

        }

        private void IrAActividades()
        {
            navigationManager.NavigateTo($"actividades");
        }
    }
}
