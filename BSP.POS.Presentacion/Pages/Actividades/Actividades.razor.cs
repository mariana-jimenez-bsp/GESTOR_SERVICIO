using BSP.POS.Presentacion.Models.Actividades;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection.Metadata;
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
            mensajeDescartar = null;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ActividadesService.ObtenerListaDeActividades(esquema);
            if (ActividadesService.ListaActividades != null)
            {
                actividades = ActividadesService.ListaActividades;
            }
            mensajeDescartar = "Se han Descartado los cambios";
        }
        private async Task ActualizarListaActividades()
        {
            mensajeActualizar = null;
            mensajeError = null;
            mensajeDescartar = null;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool seActualizo = await ActividadesService.ActualizarListaDeActividades(actividades, esquema);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.ObtenerListaDeActividades(esquema);
                if (ActividadesService.ListaActividades != null)
                {
                    actividades = ActividadesService.ListaActividades;
                    if (seActualizo)
                    {
                        mensajeActualizar = "Actividades Actualizadas";
                    }
                    else
                    {
                        mensajeError = "Ocurrío un Error vuelva a intentarlo";
                    }
                    
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

        private async Task ActivarScrollBarDeErrores()
        {
            StateHasChanged();
            await Task.Delay(100);
            var isValid = await JSRuntime.InvokeAsync<bool>("HayErroresValidacion", ".validation-message");

            if (!isValid)
            {
                // Si hay errores de validación, activa el scrollbar
                await JSRuntime.InvokeVoidAsync("ActivarScrollViewValidacion", ".validation-message");
            }
        }

        private void IrAAgregarActividad()
        {
            navigationManager.NavigateTo($"actividad/agregar");
        }
    }
}
