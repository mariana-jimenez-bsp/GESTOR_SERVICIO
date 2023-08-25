using BSP.POS.Presentacion.Models.Actividades;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Modals.Creates
{
    public partial class CrearActividad : ComponentBase
    {
        public string esquema = string.Empty;
        public mActividades activadNueva = new mActividades();
        public string mensajeAgregado;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
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

        public async Task AgregarActividad()
        {
            mensajeAgregado = null;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ActividadesService.AgregarActividad(activadNueva, esquema);
            mensajeAgregado = "Actividad Agregada";
        }

    }
}
