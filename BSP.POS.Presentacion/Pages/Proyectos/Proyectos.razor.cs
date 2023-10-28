using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.Presentacion.Services.Actividades;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class Proyectos: ComponentBase
    {
        public string esquema = string.Empty;
        public List<mProyectos> proyectos = new List<mProyectos>();
        public List<mItemsCliente> listaCentrosDeCosto = new List<mItemsCliente>();
        public List<mClientes> listaDeClientes = new List<mClientes>();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        public string numeroActual = string.Empty;
        List<string> permisos;


        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                listaDeClientes = ClientesService.ListaClientes;
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




        private async Task RefrescarListaDeProyectos()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ProyectosService.ObtenerListaDeProyectosIniciados(esquema);
            if (ProyectosService.ListaProyectosIniciados != null)
            {
                proyectos = ProyectosService.ListaProyectosIniciados;

            }
            foreach (var proyecto in proyectos)
            {
                string nombreConsultor = listaDeClientes.Where(c => c.CLIENTE == proyecto.codigo_cliente).Select(c => c.CONTACTO).First();
                if (nombreConsultor != null)
                {
                    proyecto.nombre_consultor = nombreConsultor;
                }
            }
        }
        private void CambioCodigoCliente(ChangeEventArgs e, string proyectoId)
        {
            foreach (var proyecto in proyectos)
            {
                if (proyecto.Id == proyectoId)
                {
                    proyecto.nombre_consultor = string.Empty;
                }
            }
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.codigo_cliente = e.Value.ToString();
                        string nombreConsultor = listaDeClientes.Where(c => c.CLIENTE == proyecto.codigo_cliente).Select(c => c.CONTACTO).First();
                        if(nombreConsultor != null)
                        {
                            proyecto.nombre_consultor = nombreConsultor;
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
        
        private async Task DescartarCambios()
        {

            await RefrescarListaDeProyectos();
            await AlertasService.SwalAviso("Se han Descartado los cambios");
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
        private async Task TerminarProyecto(string numero)
        {
            if (!string.IsNullOrEmpty(numero) && !string.IsNullOrEmpty(esquema))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool resultadoTerminar = await ProyectosService.TerminarProyecto(numero, esquema);
                if (resultadoTerminar)
                {
                    await SwalAvisoProyectos("Se ha cambiado el estado del proyecto #" + numero + " a Terminado");
                }
                else
                {
                    await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
                }
               
                
            }
            
        }
        private async Task SwalTerminarProyecto(string mensajeAlerta, string numero)
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
                    await TerminarProyecto(numero);
                }
            });
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
}
