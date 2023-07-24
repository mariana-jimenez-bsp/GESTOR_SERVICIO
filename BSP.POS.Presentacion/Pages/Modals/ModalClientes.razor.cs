using Microsoft.AspNetCore.Components;
using BSP.POS.Presentacion.Models;
using System.Runtime.InteropServices;

namespace BSP.POS.Presentacion.Pages.Modals
{
    public partial class ModalClientes : ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        private bool IsCollapseOpen = false;
        public List<mClientes> clientes = new List<mClientes>();
        protected override async Task OnInitializedAsync()
        {

            await ClientesService.ObtenerListaClientes();
            if (ClientesService.ListaClientes != null)
            {
                clientes = ClientesService.ListaClientes;
                // Asegurar que las listas desplegables y cambioColores tengan la misma cantidad de elementos que la lista de clientes
                desplegables = clientes.Select(cliente => new DesplegableInfo()).ToList();
                cambioColores = clientes.Select(cliente => new color()).ToList();
            }
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;

        }
        public class DesplegableInfo
        {
            public bool IsOpen { get; set; } = false;
            // Agrega aquí las otras propiedades necesarias para tu desplegable
        }

        public class color
        {
            public string bg_color { get; set; } = "bg-light";
            // Agrega aquí las otras propiedades necesarias para tu desplegable
        }
        private void OpenModal()
        {
            ActivarModal = true;
        }

        private async Task CloseModal()
        {
            await ClientesService.ObtenerListaClientes();
            clientes = ClientesService.ListaClientes;
            await OnClose.InvokeAsync(false);



        }
        private List<DesplegableInfo> desplegables = new List<DesplegableInfo>();
        private List<color> cambioColores = new List<color>();

        private void ToggleCollapse(int index)
        {
            // Cambiar el estado del desplegable con el índice especificado

            for (int i = 0; i < desplegables.Count; i++)
            {
                if (i == index)
                {
                    desplegables[i].IsOpen = !desplegables[i].IsOpen;

                    cambiarColor(i);

                }
                else
                {
                    desplegables[i].IsOpen = false;
                    cambiarColor(i);
                }
            }


        }
        private void cambiarColor(int index)
        {
            if (desplegables[index].IsOpen)
            {
                cambioColores[index].bg_color = "bg-primary-subtle";
            }
            else
            {
                cambioColores[index].bg_color = "bg-light";
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

        private void CambioTelefono(ChangeEventArgs e, string clienteCod)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var cliente in clientes)
                {
                    if (cliente.CLIENTE == clienteCod)
                    {
                        cliente.TELEFONO = e.Value.ToString();
                    }
                }
            }
        }

        private async Task ActualizarListaClientes()
        {

            await ClientesService.ActualizarListaDeClientes(clientes);
            await ClientesService.ObtenerListaClientes();
            if (ClientesService.ListaClientes != null)
            {
                clientes = ClientesService.ListaClientes;
            }
            await CloseModal();
        }
    }
}
