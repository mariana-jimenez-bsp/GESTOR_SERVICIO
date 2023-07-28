﻿using BSP.POS.Presentacion.Models;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ListaClientes : ComponentBase
    {
        public List<mInformes> InformesAsociados = new List<mInformes>();
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            usuarioActual = user.Identity.Name;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await ClientesService.ObtenerListaClientes(esquema);
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientesRecientes(esquema);


        }
        private string activeTab = "recent"; // Pestaña activa inicialmente

        private void ChangeTab(string tabId)
        {
            activeTab = tabId;
        }

        private string GetTabLinkClass(string tabId)
        {
            return activeTab == tabId ? "active" : "";
        }

        private async Task EnviarInformesAsociados(string cliente)
        {
            await InformesService.ObtenerListaDeInformesAsociados(cliente, esquema);
            ClientesService.ClienteAsociado = await ClientesService.ObtenerClienteAsociado(cliente, esquema);
            if (ClientesService.ClienteAsociado != null)
            {
                ClienteAsociado = ClientesService.ClienteAsociado;
            }

            if (InformesService.ListaInformesAsociados != null)
            {
                InformesAsociados = InformesService.ListaInformesAsociados;
            }
        }

        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }
    }
}
