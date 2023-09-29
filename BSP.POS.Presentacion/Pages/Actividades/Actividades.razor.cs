using BSP.POS.Presentacion.Models.Actividades;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Actividades
{
    public partial class Actividades: ComponentBase
    {
        public string esquema = string.Empty;
        public List<mActividades> actividades = new List<mActividades>();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        List<string> permisos;
        public string mensajeActualizar;
        public string mensajeDescartar;
        public string mensajeError;
        private bool estadoActividadNueva = false;
        private bool estadoActividadCancelada = false;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await ActividadesService.ObtenerListaDeActividades(esquema);
            if (ActividadesService.ListaActividades != null)
            {
                actividades = ActividadesService.ListaActividades;

            }

            
            cargaInicial = true;
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

        private async Task DescartarCambios()
        {
            mensajeActualizar = null;
            estadoActividadNueva = false;
            mensajeDescartar = null;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ActividadesService.ObtenerListaDeActividades(esquema);
            if (ActividadesService.ListaActividades != null)
            {
                actividades = ActividadesService.ListaActividades;
            }
        }
        private async Task ActualizarListaActividades()
        {
            mensajeActualizar = null;
            mensajeError = null;
            estadoActividadNueva = false;
            mensajeDescartar = null;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.ActualizarListaDeActividades(actividades, esquema);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.ObtenerListaDeActividades(esquema);
                if (ActividadesService.ListaActividades != null)
                {
                    actividades = ActividadesService.ListaActividades;
                    mensajeActualizar = "Actividades Actualizadas";
                }
            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }

        }

        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }

        bool activarModalAgregarActividad = false;

        async Task ClickHandlerAgregarActividad(bool activar)
        {
            activarModalAgregarActividad = activar;
            if (!activar)
            {
                mensajeActualizar = null;
                mensajeDescartar = null;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.ObtenerListaDeActividades(esquema);
                if (ActividadesService.ListaActividades != null)
                {
                    actividades = ActividadesService.ListaActividades;
                }
            }
            if (activar)
            {
                estadoActividadNueva = false;
                estadoActividadCancelada = false;
            }
            StateHasChanged();
        }
        public void CambiarEstadoActividadNueva(bool estado)
        {
            estadoActividadNueva = estado;
        }
        public void CambiarEstadoActividadCancelada(bool estado)
        {
            estadoActividadCancelada = estado;
        }
    }
}
