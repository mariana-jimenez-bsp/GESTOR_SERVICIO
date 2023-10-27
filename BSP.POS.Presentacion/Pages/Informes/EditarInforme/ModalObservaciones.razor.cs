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
        [Parameter] public EventCallback observacionAgregada { get; set; }
        [Parameter] public EventCallback observacionCancelada { get; set; }

        public mObservaciones observacionAAgregar = new mObservaciones();
        public string mensajeError;
        

        private async Task CancelarCambios()
        {
            await CloseModal();
            await observacionCancelada.InvokeAsync();
        }

        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);
            observacionAAgregar = new mObservaciones();

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
                        await CloseModal();
                        await observacionAgregada.InvokeAsync();
                       
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
