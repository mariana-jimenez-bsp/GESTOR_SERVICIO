using BSP.POS.Presentacion.Models.Actividades;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Modals
{
    public partial class ModalActividades : ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        public List<mActividades> actividades = new List<mActividades>();
        public string esquema = string.Empty;
        private void OpenModal()
        {
            ActivarModal = true;
        }

        private async Task CloseModal()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ActividadesService.ObtenerListaDeActividades(esquema);
            actividades = ActividadesService.ListaActividades;
            await OnClose.InvokeAsync(false);

        }
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await ActividadesService.ObtenerListaDeActividades(esquema);
            if (ActividadesService.ListaActividades != null)
            {
                actividades = ActividadesService.ListaActividades;
            }

        }


        private void CambioActividadNombre(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var actividad in actividades)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.Actividad = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioCIReferencia(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var actividad in actividades)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.CI_referencia = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioHoras(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var actividad in actividades)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.horas = e.Value.ToString();
                    }
                }
            }
        }

        private async Task ActualizarListaActividades()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ActividadesService.ActualizarListaDeActividades(actividades, esquema);
            await CloseModal();
        }
        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }
        private string activeTab = "lista"; // Pestaña activa inicialmente

        private async Task ChangeTab(string tabId)
        {
            activeTab = tabId;
            if (activeTab == "lista")
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.ObtenerListaDeActividades(esquema);
                if (ActividadesService.ListaActividades != null)
                {
                    actividades = ActividadesService.ListaActividades;
                }
            }
        }

        private string GetTabLinkClass(string tabId)
        {
            return activeTab == tabId ? "active" : "";
        }
    }
}
