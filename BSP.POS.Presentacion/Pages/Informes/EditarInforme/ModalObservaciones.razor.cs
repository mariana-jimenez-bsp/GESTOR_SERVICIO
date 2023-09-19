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
        public List<mObservaciones> listaDeObservaciones = new List<mObservaciones>();
        
        public mObservaciones observacionAAgregar = new mObservaciones();
        public string mensajeError;
        protected override async Task OnParametersSetAsync()
        {
            if(!string.IsNullOrEmpty(esquema) && !string.IsNullOrEmpty(consecutivo))
            {
                await RefrescarListaDeObservaciones();
            }
        }

        public async Task RefrescarListaDeObservaciones()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ObservacionesService.ObtenerListaDeObservacionesDeInforme(consecutivo, esquema);
            if (ObservacionesService.ListaDeObservacionesDeInforme != null)
            {
                listaDeObservaciones = ObservacionesService.ListaDeObservacionesDeInforme;
                foreach (var observacion in listaDeObservaciones)
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerElUsuarioParaEditar(esquema, observacion.codigo_usuario);
                    if (UsuariosService.UsuarioParaEditar != null)
                    {
                        observacion.nombre_usuario = UsuariosService.UsuarioParaEditar.nombre;
                    }
                       
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
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerPerfil(usuarioActual, esquema);
                    if(UsuariosService.Perfil != null)
                    {
                        observacionAAgregar.codigo_usuario = UsuariosService.Perfil.codigo;
                    }
                    observacionAAgregar.consecutivo_informe = consecutivo;
                    await ObservacionesService.AgregarObservacionDeInforme(observacionAAgregar, esquema);
                    observacionAAgregar = new mObservaciones();
                    await RefrescarListaDeObservaciones();
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
