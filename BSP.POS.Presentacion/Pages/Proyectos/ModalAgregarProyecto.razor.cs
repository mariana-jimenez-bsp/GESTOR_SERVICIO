﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.ItemsCliente;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class ModalAgregarProyecto: ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        public string esquema = string.Empty;
        public mProyectos proyecto = new mProyectos();
        public List<mItemsCliente> listaCentrosDeCosto = new List<mItemsCliente>();
        public string mensajeError;
        [Parameter] public EventCallback<bool> proyectoAgregado { get; set; }
        [Parameter] public EventCallback<bool> proyectoCancelado { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
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

        private void CambioNombreConsultor(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                        proyecto.nombre_consultor = e.Value.ToString();
            }
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

        private void CambioEmpresa(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                        proyecto.empresa = e.Value.ToString();
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
    }
}
