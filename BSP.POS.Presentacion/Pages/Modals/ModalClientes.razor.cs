using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices;
using BSP.POS.Presentacion.Pages.Home;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Models.Clientes;

namespace BSP.POS.Presentacion.Pages.Modals
{
    public partial class ModalClientes : ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        private bool IsCollapseOpen = false;
        public List<mClientes> clientes = new List<mClientes>();
        public string esquema = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                clientes = ClientesService.ListaClientes;
                // Asegurar que las listas desplegables y cambioColores tengan la misma cantidad de elementos que la lista de clientes
               
            }
            foreach (var cliente in clientes)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                cliente.listaDeUsuarios = await UsuariosService.ObtenerListaDeUsuariosDeClienteAsociados(esquema, cliente.CLIENTE);
            }

           
        }




        private void OpenModal()
        {
            ActivarModal = true;
        }

        private async Task CloseModal()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            await OnClose.InvokeAsync(false);
            clientes = ClientesService.ListaClientes;
          
            foreach (var cliente in clientes)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                cliente.listaDeUsuarios = await UsuariosService.ObtenerListaDeUsuariosDeClienteAsociados(esquema, cliente.CLIENTE);
            }
           



        }
    
  

        private void ToggleCollapse(string clienteCod)
        {
            foreach (var cliente in clientes)
            {
                if (cliente.CLIENTE == clienteCod)
                {
                    cliente.IsOpen = !cliente.IsOpen;
                }
            }

        }



        private void CambioNombre(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.NOMBRE = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioAlias(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.ALIAS = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioContribuyente(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.CONTRIBUYENTE = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioTelefono1(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.TELEFONO1 = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioTelefono2(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.TELEFONO2 = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioCorreo(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.E_MAIL = e.Value.ToString();
                    }
                }
            }
        }

        private async Task ActualizarListaClientes()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ActualizarListaDeClientes(clientes, esquema);
            if (ClientesService.ListaClientes != null)
            {
                clientes = ClientesService.ListaClientes;
            }
            await CloseModal();
        }

        private bool esElUltimoUsuario(mClientes cliente ,mUsuariosDeCliente usuario)
        {
            // Compara el elemento actual con el último elemento de la lista
            return cliente.listaDeUsuarios.IndexOf(usuario) == cliente.listaDeUsuarios.Count - 1;
        }

        private string activeTab = "lista"; // Pestaña activa inicialmente

        private void ChangeTab(string tabId)
        {
            activeTab = tabId;
        }

        private string GetTabLinkClass(string tabId)
        {
            return activeTab == tabId ? "active" : "";
        }
    }
}
