using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Clientes;
using Microsoft.JSInterop;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class ModalAgregarProyecto: ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        public string esquema = string.Empty;
        public mProyectos proyecto = new mProyectos();
        public List<mItemsCliente> listaCentrosDeCosto = new List<mItemsCliente>();
        public List<mClientes> listaDeClientes = new List<mClientes>();
        public string mensajeError;
        [Parameter] public EventCallback<bool> proyectoAgregado { get; set; }
        [Parameter] public EventCallback<bool> proyectoCancelado { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if(ClientesService.ListaClientes != null)
            {
                listaDeClientes = ClientesService.ListaClientes;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ItemsClienteService.ObtenerListaDeCentrosDeCosto(esquema);
            if (ItemsClienteService.listaCentrosDeCosto != null)
            {
                listaCentrosDeCosto = ItemsClienteService.listaCentrosDeCosto;
            }
        }

        private void OpenModal()
        {
            ActivarModal = true;
        }
        private async Task CancelarCambios()
        {
            await proyectoCancelado.InvokeAsync(true);
            await CloseModal();
        }
        private async Task CloseModal()
        {
            proyecto = new mProyectos();
            await OnClose.InvokeAsync(false);

        }
        private void CambioFechaInicial(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                        proyecto.fecha_inicial = e.Value.ToString();

            }
        }

        private void CambioFechaFinal(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                        proyecto.fecha_final = e.Value.ToString();
            }
        }

        private void CambioHorasTotales(ChangeEventArgs e)
        {

                        proyecto.horas_totales = e.Value.ToString();

            
        }

        private void CambioCodigoCliente(ChangeEventArgs e)
        {
            proyecto.nombre_consultor = string.Empty;
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                 proyecto.codigo_cliente = e.Value.ToString();
                 string nombreConsultor = listaDeClientes.Where(c => c.CLIENTE == proyecto.codigo_cliente).Select(c => c.CONTACTO).First();
                 if(nombreConsultor != null)
                {
                    proyecto.nombre_consultor = nombreConsultor;
                }
            }
        }

        private void CambioCentroCosto(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                        proyecto.centro_costo = e.Value.ToString();
            }
        }

        private void CambioNombreProyecto(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

              proyecto.nombre_proyecto = e.Value.ToString();
            }
        }

        private async Task AgregarProyecto()
        {
            mensajeError = null;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ProyectosService.AgregarProyecto(proyecto, esquema);
                await proyectoAgregado.InvokeAsync(true);
                await CloseModal();
            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }
            
        }

        private async Task SalirConLaX()
        {
            await OnClose.InvokeAsync(false);
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
    }
}
