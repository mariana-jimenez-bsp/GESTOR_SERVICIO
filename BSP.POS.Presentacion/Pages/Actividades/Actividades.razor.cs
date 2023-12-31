﻿using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.Proyectos;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
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
        List<mObjetoPermiso> permisos = new List<mObjetoPermiso>();

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            usuarioActual = user.Identity.Name;
            var PermisosClaim = user.Claims.FirstOrDefault(c => c.Type == "permisos");
            if (PermisosClaim != null)
            {
                permisos = JsonConvert.DeserializeObject<List<mObjetoPermiso>>(PermisosClaim.Value);
            }
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (permisos.Any(p => p.permiso == "Actividades" && p.subpermisos.Contains("Ver Lista") && !p.subpermisos.Contains("Editar")))
                {
                    await JSRuntime.InvokeVoidAsync("DesactivarElementos");
                    await AlertasService.SwalAdvertencia("No tienes permisos de edición, solo puedes visualizar");
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                Console.WriteLine(error);
            }
        }
        private async Task RefrescarListaActividades()
        {
           
            if (rol == "Admin")
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ActividadesService.ObtenerListaDeActividadesActivas(esquema);
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
                    await ActividadesService.ObtenerListaDeActividadesActivasPorUsuario(esquema, perfilActual.codigo);
                    if (ActividadesService.ListaActividades != null)
                    {
                        actividades = ActividadesService.ListaActividadesDeUsuario;

                    }
                }
            }
            foreach (var actividad in actividades)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                actividad.ActividadAsociadaInforme = await ActividadesService.ValidarActividadAsociadaInforme(esquema, actividad.codigo);

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
        private async Task InvalidSubmit(EditContext modeloContext)
        {
            await ActivarScrollBarDeErrores();
            var mensajesDeValidaciones = modeloContext.GetValidationMessages();
            string mensaje = mensajesDeValidaciones.First();
            await AlertasService.SwalError(mensaje);
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

        private async Task SwalActualizandoActividades()
        {

            bool resultadoActualizar = false;
            if (permisos.Any(p => p.permiso == "Actividades" && !p.subpermisos.Contains("Editar")))
            {
                await AlertasService.SwalAdvertencia("No tienes permisos de edición, solo puedes visualizar");
            }
            else
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = " <span class=\"spinner-border spinner-border-sm\" aria-hidden=\"true\"></span>\r\n  <span role=\"status\">Actualizando...</span>",
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

        private async Task SwalEliminarActividad(string mensajeAlerta, string codigoActividad)
        {
            if (permisos.Any(p => p.permiso == "Actividades" && !p.subpermisos.Contains("Eliminar")))
            {
                await AlertasService.SwalAdvertencia("No tienes permisos para eliminar actividades");
            }
            else
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = mensajeAlerta,
                    Icon = SweetAlertIcon.Question,
                    ShowCancelButton = true,
                    ConfirmButtonText = "Aceptar",
                    CancelButtonText = "Cancelar"
                }).ContinueWith(async swalTask =>
                {
                    SweetAlertResult result = swalTask.Result;
                    if (result.IsConfirmed)
                    {
                        bool resultadoEliminar = await EliminarActividad(codigoActividad);
                        if (resultadoEliminar)
                        {
                            await AlertasService.SwalExito("Se ha eliminado la actividad #" + codigoActividad);
                            await RefrescarListaActividades();
                            StateHasChanged();
                        }
                        else
                        {
                            await AlertasService.SwalError("Ocurrió un error vuelva a intentarlo");
                        }
                    }
                });
            }

        }

        private async Task SwalInactivarActividad(string mensajeAlerta, string codigoActividad)
        {
            if (permisos.Any(p => p.permiso == "Actividades" && !p.subpermisos.Contains("Eliminar")))
            {
                await AlertasService.SwalAdvertencia("No tienes permisos para inactivar o eliminar actividades");
            }
            else
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = mensajeAlerta,
                    Icon = SweetAlertIcon.Question,
                    ShowCancelButton = true,
                    ConfirmButtonText = "Aceptar",
                    CancelButtonText = "Cancelar"
                }).ContinueWith(async swalTask =>
                {
                    SweetAlertResult result = swalTask.Result;
                    if (result.IsConfirmed)
                    {
                        bool resultadoInactivar = await InactivarActividad(codigoActividad);
                        if (resultadoInactivar)
                        {
                            await AlertasService.SwalExito("Se ha Inactivado la actividad #" + codigoActividad);
                            await RefrescarListaActividades();
                            StateHasChanged();
                        }
                        else
                        {
                            await AlertasService.SwalError("Ocurrió un error vuelva a intentarlo");
                        }
                    }
                });
            }

        }

        private async Task<bool> EliminarActividad(string codigo)
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool resultadoEliminar = await ActividadesService.EliminarActividad(esquema, codigo);
            return resultadoEliminar;
        }

        private async Task<bool> InactivarActividad(string codigo)
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool resultadoInactivar = await ActividadesService.CambiarEstadoActividad(esquema, codigo, "Inactivo");
            return resultadoInactivar;
        }
    }
}
