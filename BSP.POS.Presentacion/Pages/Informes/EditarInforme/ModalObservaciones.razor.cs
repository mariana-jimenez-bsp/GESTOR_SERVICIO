using BSP.POS.Presentacion.Models.Observaciones;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.Usuarios;
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
        [Parameter] public EventCallback<bool> observacionAgregada { get; set; }
        [Parameter] public EventCallback<bool> observacionCancelada { get; set; }

        public mObservaciones observacionAAgregar = new mObservaciones();
        public string mensajeError;
        

        private async Task CancelarCambios()
        {
            await observacionCancelada.InvokeAsync(true);
            await CloseModal();
        }

        private async Task CloseModal()
        {
            observacionAAgregar = new mObservaciones();
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
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerPerfil(usuarioActual, esquema);
                    if(UsuariosService.Perfil != null)
                    {
                        observacionAAgregar.codigo_usuario = UsuariosService.Perfil.codigo;
                    }
                    observacionAAgregar.consecutivo_informe = consecutivo;
                    bool resultadoObservaciones = await ObservacionesService.AgregarObservacionDeInforme(observacionAAgregar, esquema);
                    if (resultadoObservaciones)
                    {
                        await observacionAgregada.InvokeAsync(true);
                        await CloseModal();
                    }
                    else
                    {
                        mensajeError = "Ocurrió un error vuelva a intentarlo";
                    }
                    
 
                   
                }
            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
                observacionAAgregar = new mObservaciones();
            }

        }
        private async Task SalirConLaX()
        {
            await OnClose.InvokeAsync(false);
        }
    }
}
