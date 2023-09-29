﻿using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Services.Actividades;
using Microsoft.AspNetCore.Components;
using System;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class Proyectos: ComponentBase
    {
        public string esquema = string.Empty;
        public List<mProyectos> proyectos = new List<mProyectos>();
        public List<mItemsCliente> listaCentrosDeCosto = new List<mItemsCliente>();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        List<string> permisos;
        public string mensajeActualizar;
        public string mensajeDescartar;
        public string mensajeError;
        private bool estadoProyectoNuevo = false;
        private bool estadoProyectoCancelado = false;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await ProyectosService.ObtenerListaDeProyectos(esquema);
            if (ProyectosService.ListaProyectos != null)
            {
                proyectos = ProyectosService.ListaProyectos;
               
            }

            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ItemsClienteService.ObtenerListaDeCentrosDeCosto(esquema);
            if(ItemsClienteService.listaCentrosDeCosto != null)
            {
                listaCentrosDeCosto = ItemsClienteService.listaCentrosDeCosto;
            }
            cargaInicial = true;
        }





        private void CambioNombreConsultor(ChangeEventArgs e, string proyectoId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.nombre_consultor = e.Value.ToString();
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

        private void CambioEmpresa(ChangeEventArgs e, string proyectoId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var proyecto in proyectos)
                {
                    if (proyecto.Id == proyectoId)
                    {
                        proyecto.empresa = e.Value.ToString();
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
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ProyectosService.ObtenerListaDeProyectos(esquema);
            if (ProyectosService.ListaProyectos != null)
            {
                proyectos = ProyectosService.ListaProyectos;
                mensajeDescartar = "Se han Descartado los cambios";
            }
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
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ProyectosService.ObtenerListaDeProyectos(esquema);
                if (ProyectosService.ListaProyectos != null)
                {
                    proyectos = ProyectosService.ListaProyectos;
                    mensajeActualizar = "Proyectos Actualizados";
                }
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
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ProyectosService.ObtenerListaDeProyectos(esquema);
                if (ProyectosService.ListaProyectos != null)
                {
                    proyectos = ProyectosService.ListaProyectos;
                }
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
    }
}
