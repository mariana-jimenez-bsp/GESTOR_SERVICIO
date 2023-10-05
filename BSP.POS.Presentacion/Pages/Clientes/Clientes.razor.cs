using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Services.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Reflection.Metadata;
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
        private string codigoClienteActual;
        private string codigoEditarUsuario;
        private bool estadoClienteNuevo = false;
        private bool estadoClienteCancelado = false;
        private bool estadoUsuarioNuevo = false;
        private bool estadoUsuarioActualizado = false;
        private bool estadoUsuarioNuevoCancelado = false;
        private bool estadoUsuarioActualizadoCancelado = false;
         
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
                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        await ObtenerUsuariosDeCliente(clienteCod);
                    }
                }
                else
                {
                    cliente.IsOpen = false;
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
                await RefrescarListaClientes();
                mensajeActualizar = "Clientes Actualizados";
                
               
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
            await RefrescarListaClientes();
            mensajeDescartar = "Se han descartado los cambios";
            
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
                await RefrescarListaClientes();
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
        bool actividarModalAgregarUsuario = false;
        public async Task EnviarCodigoClienteNuevoUsuario(bool activar, string codigo)
        {
            codigoClienteActual = codigo;
            await ClickHandlerAgregarUsuario(activar);
        }

        public async Task EnviarCodigoEditarUsuario(bool activar, string codigoUsuario, string codigoCliente)
        {
            codigoEditarUsuario = codigoUsuario;
            codigoClienteActual = codigoCliente;
            await ClickHandlerEditarUsuario(activar);
        }


        async Task ClickHandlerAgregarUsuario(bool activar)
        {
            actividarModalAgregarUsuario = activar;
            if (!activar)
            {
                if (!string.IsNullOrEmpty(codigoClienteActual))
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ObtenerUsuariosDeCliente(codigoClienteActual);
                    codigoClienteActual = string.Empty;
                    
                }
            }
            if (activar)
            {
                estadoUsuarioNuevo = false;
                estadoUsuarioNuevoCancelado = false;
            }
            StateHasChanged();
        }
        bool actividarModalEditarUsuario = false;

        async Task ClickHandlerEditarUsuario(bool activar)
        {


            if (!activar)
            {
                if (!string.IsNullOrEmpty(codigoClienteActual))
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ObtenerUsuariosDeCliente(codigoClienteActual);
                    codigoClienteActual = string.Empty;
                    codigoEditarUsuario = string.Empty;
                }
            }
            if (activar)
            {
                estadoUsuarioActualizado = false;
                estadoUsuarioActualizadoCancelado = false;
            }
            actividarModalEditarUsuario = activar;
            StateHasChanged();
        }

        public void CambiarEstadoUsuarioNuevo(bool estado)
        {
            estadoUsuarioNuevo = estado;
        }

        public void CambiarEstadoUsuarioActualizado(bool estado)
        {
            estadoUsuarioActualizado = estado;
        }
        public void CambiarEstadoUsuarioNuevoCancelado(bool estado)
        {
            estadoUsuarioNuevoCancelado = estado;
        }
        public void CambiarEstadoUsuarioActualizadoCancelado(bool estado)
        {
            estadoUsuarioActualizadoCancelado = estado;
        }

        private async Task ActivarScrollBarDeErrores()
        {
            StateHasChanged();
            await Task.Delay(100);
            var isValid = await JS.InvokeAsync<bool>("HayErroresValidacion", ".validation-message");

            if (!isValid)
            {
                // Si hay errores de validación, activa el scrollbar
                await JS.InvokeVoidAsync("ActivarScrollViewValidacion", ".validation-message");
            }
        }
    }
}
