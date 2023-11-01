using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Usuarios;
using CurrieTechnologies.Razor.SweetAlert2;
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
        public List<mUsuariosParaEditar> listaUsuarios = new List<mUsuariosParaEditar>();
        public mPerfil perfilActual = new mPerfil();
        public bool cargaInicial = false;
        public string usuarioActual = string.Empty;
        public string rol = string.Empty;
        List<string> permisos;
       
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            usuarioActual = user.Identity.Name;
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await RefrescarListaActividades();
            
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaDeUsuariosParaEditar(esquema);
            if(UsuariosService.ListaDeUsuariosParaEditar != null)
            {
                listaUsuarios = UsuariosService.ListaDeUsuariosParaEditar;
            }

            cargaInicial = true;
        }
        private async Task RefrescarListaActividades()
        {
           
            if (rol == "Admin")
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.ObtenerListaDeActividades(esquema);
                if (ActividadesService.ListaActividades != null)
                {
                    actividades = ActividadesService.ListaActividades;

                }
            }
            else
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await UsuariosService.ObtenerPerfil(usuarioActual, esquema);
                if(UsuariosService.Perfil != null)
                {
                    perfilActual = UsuariosService.Perfil;
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ActividadesService.ObtenerListaDeActividadesPorUsuario(esquema, perfilActual.codigo);
                    if (ActividadesService.ListaActividades != null)
                    {
                        actividades = ActividadesService.ListaActividadesDeUsuario;

                    }
                }
            }
        }

        private void CambioCodigoUsuario(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()) && rol == "Admin")
            {
                foreach (var actividad in actividades)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.codigo_usuario = e.Value.ToString();
                    }
                }
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

        private async Task DescartarCambios()
        {
            await AlertasService.SwalAvisoCancelado("Se han Descartado los cambios");
        }
        private async Task<bool> ActualizarListaActividades()
        {
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool seActualizo = await ActividadesService.ActualizarListaDeActividades(actividades, esquema);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.ObtenerListaDeActividades(esquema);
                await RefrescarListaActividades();
                if (seActualizo)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                    
                
            }
            catch (Exception)
            {

                return false;
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

        private async Task SwalActualizandoActividades()
        {

            bool resultadoActualizar = false;
            await Swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Info,
                Title = "Actualizando...",
                ShowCancelButton = false,
                ShowConfirmButton = false,
                AllowOutsideClick = false,
                AllowEscapeKey = false,
                DidOpen = new SweetAlertCallback(async () =>
                {
                    resultadoActualizar = await ActualizarListaActividades();
                    await Swal.CloseAsync();

                }),
                WillClose = new SweetAlertCallback(Swal.CloseAsync)

            });

            if (resultadoActualizar)
            {
                await AlertasService.SwalExito("Actividades Actualizadas");
            }
            else
            {
                await AlertasService.SwalError("Ocurrió un error. Vuelva a intentarlo.");
            }


        }
    }
}
