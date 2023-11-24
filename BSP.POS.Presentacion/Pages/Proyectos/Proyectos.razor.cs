using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Permisos;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.Presentacion.Services.Actividades;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class Proyectos: ComponentBase
    {
        public string esquema = string.Empty;
        public List<mProyectos> proyectos = new List<mProyectos>();
        public List<mItemsCliente> listaCentrosDeCosto = new List<mItemsCliente>();
        public List<mClientes> listaDeClientes = new List<mClientes>();
        public List<mUsuariosParaEditar> listaUsuariosConsultores = new List<mUsuariosParaEditar>();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        public string numeroActual = string.Empty;
        List<mObjetoPermiso> permisos = new List<mObjetoPermiso>();


        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            var PermisosClaim = user.Claims.FirstOrDefault(c => c.Type == "permisos");
            if (PermisosClaim != null)
            {
                permisos = JsonConvert.DeserializeObject<List<mObjetoPermiso>>(PermisosClaim.Value);
            }
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                listaDeClientes = ClientesService.ListaClientes;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaDeUsuariosConsultores(esquema);
            if (UsuariosService.ListaDeUsuariosConsultores != null)
            {
                listaUsuariosConsultores = UsuariosService.ListaDeUsuariosConsultores;
            }
            await RefrescarListaDeProyectos();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ItemsClienteService.ObtenerListaDeCentrosDeCosto(esquema);
            if(ItemsClienteService.listaCentrosDeCosto != null)
            {
                listaCentrosDeCosto = ItemsClienteService.listaCentrosDeCosto;
            }
            
            cargaInicial = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("initTooltips");
                if (permisos.Any(p => p.permiso == "Proyectos" && p.subpermisos.Contains("Ver Lista") && !p.subpermisos.Contains("Editar")))
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


        private async Task RefrescarListaDeProyectos()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ProyectosService.ObtenerListaDeProyectosActivos(esquema);
            if (ProyectosService.ListaProyectosActivos != null)
            {
                proyectos = ProyectosService.ListaProyectosActivos;

            }
            foreach (var proyecto in proyectos)
            {
                string nombreResponsable = listaDeClientes.Where(c => c.CLIENTE == proyecto.codigo_cliente).Select(c => c.CONTACTO).First();
                if (nombreResponsable != null)
                {
                    proyecto.nombre_responsable = nombreResponsable;
                }
            }
        }
        private void CambioCodigoCliente(ChangeEventArgs e, string proyectoId)
        {
            foreach (var proyecto in proyectos)
            {
                if (proyecto.Id == proyectoId)
                {
                    proyecto.nombre_responsable = string.Empty;
                }
            }
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.codigo_cliente = e.Value.ToString();
                        string nombreResponsable = listaDeClientes.Where(c => c.CLIENTE == proyecto.codigo_cliente).Select(c => c.CONTACTO).First();
                        if(nombreResponsable != null)
                        {
                            proyecto.nombre_responsable = nombreResponsable;
                        }
                        
                    }
                }
            }
        }

        private void CambioFechaInicial(ChangeEventArgs e, string proyectoId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.fecha_inicial = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioFechaFinal(ChangeEventArgs e, string proyectoId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.fecha_final = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioHorasTotales(ChangeEventArgs e, string proyectoId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.horas_totales = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioCentroCosto(ChangeEventArgs e, string proyectoId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.centro_costo = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioNombreProyecto(ChangeEventArgs e, string proyectoId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.nombre_proyecto = e.Value.ToString();
                    }
                }
            }
        }
        private void CambioConsultor(ChangeEventArgs e, string proyectoId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.codigo_consultor = e.Value.ToString();
                    }
                }
            }
        }
        private void CambioActivarEditar(bool activar, string proyectoId)
        { 
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.activar_editar = activar;
                    }
                }
        }


        private async Task DescartarCambios()
        {
            await AlertasService.SwalAvisoCancelado("Se han Descartado los cambios");
        }
        private async Task<bool> ActualizarListaProyectos()
        {
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoActualizar = await ProyectosService.ActualizarListaDeProyectos(proyectos, esquema);
                if (resultadoActualizar)
                {
                    await RefrescarListaDeProyectos();
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

        private void IrAProyectosTerminados()
        {

            navigationManager.NavigateTo($"proyectos/terminados");
        }

        private void IrAAgregarProyecto()
        {
            navigationManager.NavigateTo($"proyecto/agregar");
        }
        private async Task CambiarEstadoProyecto(string numero, string estado)
        {
            if (!string.IsNullOrEmpty(numero) && !string.IsNullOrEmpty(esquema))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoTerminar = await ProyectosService.CambiarEstadoProyecto(numero, estado, esquema);
                if (resultadoTerminar)
                {
                    await SwalAvisoProyectos("Se ha cambiado el estado del proyecto #" + numero + " a" + estado);
                }
                else
                {
                    await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
                }
               
                
            }
            
        }
        private async Task SwalCambiarEstadoProyecto(string mensajeAlerta, string numero, string estado)
        {
            if (permisos.Any(p => p.permiso == "Proyectos" && !p.subpermisos.Contains("Terminar")))
            {
                await AlertasService.SwalAdvertencia("No tienes permisos para realizar esta acción");
            }
            else
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "Advertencia!",
                    Text = mensajeAlerta,
                    Icon = SweetAlertIcon.Warning,
                    ShowCancelButton = true,
                    ConfirmButtonText = "Aceptar",
                    CancelButtonText = "Cancelar"
                }).ContinueWith(async swalTask =>
                {
                    SweetAlertResult result = swalTask.Result;
                    if (result.IsConfirmed)
                    {
                        await CambiarEstadoProyecto(numero, estado);
                    }
                });
            }
            
        }

        private async Task SwalAvisoProyectos(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Aviso!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Info,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(async swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    await RefrescarListaDeProyectos();
                    StateHasChanged();
                }
            });
        }

        private async Task SwalActualizandoProyectos()
        {
            bool resultadoActualizar = false;
            if (permisos.Any(p => p.permiso == "Proyectos" && !p.subpermisos.Contains("Editar")))
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
                        resultadoActualizar = await ActualizarListaProyectos();
                        await Swal.CloseAsync();

                    }),
                    WillClose = new SweetAlertCallback(Swal.CloseAsync)

                });

                if (resultadoActualizar)
                {
                    await AlertasService.SwalExito("Proyectos Actualizados");
                }
                else
                {
                    await AlertasService.SwalError("Ocurrió un error. Vuelva a intentarlo.");
                }

            }


        }
        void ActivarVerInformes(bool activar, string numero)
        {
            numeroActual = numero;
            ClickHandlerVerInformes(activar);
        }
        bool activarModalVerInformes = false;
        void ClickHandlerVerInformes(bool activar)
        {
            activarModalVerInformes = activar;
            StateHasChanged();
        }

        private async Task SwalEliminarProyecto(string mensajeAlerta, string numeroProyecto)
        {
            if (permisos.Any(p => p.permiso == "Proyectos" && !p.subpermisos.Contains("Eliminar")))
            {
                await AlertasService.SwalAdvertencia("No tienes permisos para eliminar proyectos");
            }
            else
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = mensajeAlerta,
                    Icon = SweetAlertIcon.Question,
                    ShowCancelButton = true,
                    ConfirmButtonText = "Eliminar",
                    CancelButtonText = "Cancelar"
                }).ContinueWith(async swalTask =>
                {
                    SweetAlertResult result = swalTask.Result;
                    if (result.IsConfirmed)
                    {
                        bool resultadoEliminar = await EliminarProyecto(numeroProyecto);
                        if (resultadoEliminar)
                        {
                            await AlertasService.SwalExito("Se ha eliminado el proyecto #" + numeroProyecto);
                            await RefrescarListaDeProyectos();
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

        private async Task<bool> EliminarProyecto(string numero)
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            bool resultadoEliminar = await ProyectosService.EliminarProyecto(esquema, numero);
            return resultadoEliminar;
        }
    }
}
