using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.Presentacion.Services.Actividades;
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
        public string mensajeActualizar;
        public string mensajeDescartar;
        public string mensajeError;
        private bool estadoProyectoNuevo = false;
        private bool estadoProyectoCancelado = false;
        private bool estadoProyectoTerminado = false;
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
            mensajeActualizar = null;
            mensajeDescartar = null;
            actividadModalAgregarProyecto = false;
            await RefrescarListaDeProyectos();
            mensajeDescartar = "Se han Descartado los cambios";
            
        }
        private async Task ActualizarListaProyectos()
        {
            mensajeActualizar = null;
            mensajeError = null;
            mensajeDescartar = null;
            estadoProyectoNuevo = false;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ProyectosService.ActualizarListaDeProyectos(proyectos, esquema);
                await RefrescarListaDeProyectos();
                mensajeActualizar = "Proyectos Actualizados";

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

        bool actividadModalAgregarProyecto = false;

        async Task ClickHandlerAgregarProyecto(bool activar)
        {
            actividadModalAgregarProyecto = activar;
            if (!activar)
            {
                mensajeActualizar = null;
                await RefrescarListaDeProyectos();
            }
            if (activar)
            {
                estadoProyectoNuevo = false;
                estadoProyectoCancelado = false;
            }
            StateHasChanged();
        }
        public void CambiarEstadoProyectoNuevo(bool estado)
        {
            estadoProyectoNuevo = estado;
        }
        public void CambiarEstadoProyectoCancelado(bool estado)
        {
            estadoProyectoCancelado = estado;
        }
        bool actividarModalTerminarProyecto = false;
        async Task ClickHandlerTerminarProyecto(bool activar)
        {
            actividarModalTerminarProyecto = activar;
            if (!activar)
            {
                mensajeActualizar = null;
                await RefrescarListaDeProyectos();
            }
            if (activar)
            {
                estadoProyectoTerminado = false;
            }
            StateHasChanged();
        }

        async Task EnviarNumeroTerminarProyecto(bool activar, string numero)
        {
            numeroActual = numero;
            await ClickHandlerTerminarProyecto(activar);
        }

        void CambiarEstadoProyectoTerminado(bool estado)
        {
            estadoProyectoTerminado = estado;
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
    }
}
