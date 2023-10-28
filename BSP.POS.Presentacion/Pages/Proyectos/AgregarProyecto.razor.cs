﻿using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Services.Clientes;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class AgregarProyecto: ComponentBase
    {
        public string esquema = string.Empty;
        public mProyectos proyecto = new mProyectos();
        public List<mItemsCliente> listaCentrosDeCosto = new List<mItemsCliente>();
        public List<mClientes> listaDeClientes = new List<mClientes>();
        List<string> permisos;

        private bool cargaInicial = false;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                listaDeClientes = ClientesService.ListaClientes;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ItemsClienteService.ObtenerListaDeCentrosDeCosto(esquema);
            if (ItemsClienteService.listaCentrosDeCosto != null)
            {
                listaCentrosDeCosto = ItemsClienteService.listaCentrosDeCosto;
            }
            cargaInicial = true;
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
                if (nombreConsultor != null)
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
        private async Task DescartarCambios()
        {
            await AlertasService.SwalAvisoNuevoDescartado("Se han cancelado los cambios", "Proyectos");
        }
        private async Task AgregarProyectoNuevo()
        {
            bool resultadoProyecto = false;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                resultadoProyecto = await ProyectosService.AgregarProyecto(proyecto, esquema);
                if(resultadoProyecto) {
                    await AlertasService.SwalExitoNuevo("Se ha agregado el Proyecto", "Proyectos");
                }
                else
                {
                    await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
                }
            }
            catch (Exception)
            {

                await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
            }

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
