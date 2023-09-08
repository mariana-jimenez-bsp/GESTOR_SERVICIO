using BSP.POS.Presentacion.Models.Observaciones;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class ModalObservaciones: ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public string esquema { get; set; } = string.Empty;
        [Parameter] public string consecutivo { get; set; } = string.Empty;
        [Parameter] public string usuarioActual { get; set; } = string.Empty;
        public List<mObservaciones> listaDeObservaciones = new List<mObservaciones>();
        public List<mPerfil> listaDeUsuarios = new List<mPerfil>();
        public mObservaciones observacionAAgregar = new mObservaciones();
        public string mensajeError;
        protected override async Task OnParametersSetAsync()
        {
            if(!string.IsNullOrEmpty(esquema) && !string.IsNullOrEmpty(consecutivo))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ObservacionesService.ObtenerListaDeObservacionesDeInforme(consecutivo, esquema);
                if(ObservacionesService.ListaDeObservacionesDeInforme != null)
                {
                    listaDeObservaciones = ObservacionesService.ListaDeObservacionesDeInforme;
                }
            }
        }

        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }

        private void CambioObservacion(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                observacionAAgregar.observacion = e.Value.ToString();
            }
        }
        private async Task AgregarObservacionDeInforme()
        {
            mensajeError = null;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                if (observacionAAgregar.observacion != null)
                {
                    observacionAAgregar.usuario = usuarioActual;
                    observacionAAgregar.consecutivo_informe = consecutivo;
                    await ObservacionesService.AgregarObservacionDeInforme(observacionAAgregar, esquema);
                    observacionAAgregar = new mObservaciones();
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ObservacionesService.ObtenerListaDeObservacionesDeInforme(consecutivo, esquema);
                    if (ObservacionesService.ListaDeObservacionesDeInforme != null)
                    {
                        listaDeObservaciones = ObservacionesService.ListaDeObservacionesDeInforme;
                    }
                }
            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
                observacionAAgregar = new mObservaciones();
            }

        }
    }
}
