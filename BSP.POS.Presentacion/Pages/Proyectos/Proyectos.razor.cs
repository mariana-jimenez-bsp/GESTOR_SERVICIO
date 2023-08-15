﻿using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Services.Actividades;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class Proyectos: ComponentBase
    {
        public string esquema = string.Empty;
        public List<mProyectos> proyectos = new List<mProyectos>();
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await ProyectosService.ObtenerListaDeProyectos(esquema);
            if (ProyectosService.ListaProyectos != null)
            {
                proyectos = ProyectosService.ListaProyectos;
            }

        }



        private void VolverAlHome()
        {

            navigationManager.NavigateTo($"Index", forceLoad: true);

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

        private async Task ActualizarListaProyectos()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ProyectosService.ActualizarListaDeProyectos(proyectos, esquema);
            if (ProyectosService.ListaProyectos != null)
            {
                proyectos = ProyectosService.ListaProyectos;
            }
            VolverAlHome();
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
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ProyectosService.ObtenerListaDeProyectos(esquema);
                if (ProyectosService.ListaProyectos != null)
                {
                    proyectos = ProyectosService.ListaProyectos;
                }
            }
            StateHasChanged();
        }
    }
}