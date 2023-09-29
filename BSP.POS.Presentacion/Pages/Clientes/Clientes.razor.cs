using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Clientes
{
    public partial class Clientes: ComponentBase
    {
        public List<mClientes> clientes = new List<mClientes>();
        public string esquema = string.Empty;
        public bool cargaInicial = false;
        public string rol = string.Empty;
        List<string> permisos;
        public string mensajeActualizar;
        public string mensajeDescartar;
        public string mensajeError;
        private bool estadoClienteNuevo = false;
        private bool estadoClienteCancelado = false;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await RefrescarListaClientes();
            cargaInicial = true;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

                // Inicializa los tooltips de Bootstrap
                try
                {
                    await JS.InvokeVoidAsync("initTooltips");
                }
                catch (Exception ex)
                {

                    string error = ex.ToString();
                    Console.WriteLine(error);
                }
                
            
        }
        private async Task RefrescarListaClientes()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                clientes = ClientesService.ListaClientes;
                // Asegurar que las listas desplegables y cambioColores tengan la misma cantidad de elementos que la lista de clientes

            }
        }

        private async Task ObtenerUsuariosDeCliente(string clienteObtenido)
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            foreach (var cliente in clientes)
            {
                if (cliente.CLIENTE == clienteObtenido)
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    cliente.listaDeUsuarios = await UsuariosService.ObtenerListaDeUsuariosDeClienteAsociados(esquema, cliente.CLIENTE);
                }
            }
        }

        private async Task ToggleCollapse(string clienteCod)
        {
            foreach (var cliente in clientes)
            {
                if (cliente.CLIENTE == clienteCod)
                {
                    cliente.IsOpen = !cliente.IsOpen;
                    if (cliente.IsOpen)
                    {
                        await ObtenerUsuariosDeCliente(clienteCod);
                    }
                }
                else
                {
                    cliente.listaDeUsuarios = new List<mUsuariosDeCliente>();
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
            mensajeError = null;
            mensajeDescartar = null;
            mensajeActualizar = null;
            estadoClienteNuevo = false;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ClientesService.ActualizarListaDeClientes(clientes, esquema);
                if (ClientesService.ListaClientes != null)
                {
                    clientes = ClientesService.ListaClientes;
                    mensajeActualizar = "Clientes Actualizados";
                }
               
            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }

        }
        private bool esElUltimoUsuario(mClientes cliente, mUsuariosDeCliente usuario)
        {
            // Compara el elemento actual con el último elemento de la lista
            return cliente.listaDeUsuarios.IndexOf(usuario) == cliente.listaDeUsuarios.Count - 1;
        }

        private async Task DescartarCambios()
        {
            mensajeActualizar = null;
            estadoClienteNuevo = false;
            mensajeDescartar = null;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                clientes = ClientesService.ListaClientes;
                mensajeDescartar = "Se han descartado los cambios";
            }
        }
        

        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }

        bool actividarModalAgregarCliente = false;

        async Task ClickHandlerAgregarCliente(bool activar)
        {
            actividarModalAgregarCliente = activar;
            if (!activar)
            {
                mensajeActualizar = null;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await ClientesService.ObtenerListaClientes(esquema);
                if (ClientesService.ListaClientes != null)
                {
                    clientes = ClientesService.ListaClientes;
                }
            }
            if (activar)
            {
                estadoClienteNuevo = false;
                estadoClienteCancelado = false;
            }
            StateHasChanged();
        }

        
        public void CambiarEstadoClienteNuevo(bool estado)
        {
            estadoClienteNuevo = estado;
        }
        public void CambiarEstadoClienteCancelado(bool estado)
        {
            estadoClienteCancelado = estado;
        }
    }
}
