using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Clientes;
using BSP.POS.Presentacion.Services.Clientes;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ListaClientes : ComponentBase
    {
        public List<mInformes> InformesAsociados = new List<mInformes>();
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();
        public List<mClientes> Clientes = new List<mClientes>();
        public List<mClientes> ClientesRecientes = new List<mClientes>();
        public mPerfil PerfilActual = new mPerfil();
        private DateTime fechaInicioDateTime = DateTime.MinValue;
        private DateTime fechaFinalDateTime = DateTime.MinValue;
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
        public string rol = string.Empty;
        public string clienteActual = string.Empty;
        public bool cargaInicial = false;
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            if (user.Identity != null && !string.IsNullOrEmpty(user.Identity.Name))
            {
                rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
                usuarioActual = user.Identity.Name;
                esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await UsuariosService.ObtenerPerfil(user.Identity.Name, esquema);
                if(UsuariosService.Perfil != null)
                {
                    PerfilActual = UsuariosService.Perfil;
                }
                await RefrescarListaClientes();
                ActualizarFiltroFechas();
                cargaInicial = true;
                StateHasChanged();
            }
           


        }
        private async Task RefrescarListaClientes()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                if (rol == "Admin")
                {
                    Clientes = ClientesService.ListaClientes;
                }
                else
                {
                    Clientes = ClientesService.ListaClientes.Where(c => c.CLIENTE == PerfilActual.cod_cliente).ToList();
                }

            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientesRecientes(esquema);
            if (ClientesService.ListaClientesRecientes != null)
            {

                if (rol == "Admin")
                {
                    ClientesRecientes = ClientesService.ListaClientesRecientes;
                }
                else
                {
                    ClientesRecientes = Clientes;
                }
            }
        }
        private string activeTab = "all"; // Pestaña activa inicialmente

        private void ChangeTab(string tabId)
        {
            activeTab = tabId;
            ActualizarFiltroFechas();
        }

        private string GetTabLinkClass(string tabId)
        {
            return activeTab == tabId ? "active" : "";
        }

        private async Task EnviarInformesAsociados(string cliente)
        {
            clienteActual = cliente;
            InformesAsociados = new List<mInformes>();
            ClienteAsociado = new mClienteAsociado();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await InformesService.ObtenerListaDeInformesAsociados(cliente, esquema);
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            ClientesService.ClienteAsociado = await ClientesService.ObtenerClienteAsociado(cliente, esquema);
            if (InformesService.ListaInformesAsociados != null)
            {
                InformesAsociados = InformesService.ListaInformesAsociados;
            }
            if (ClientesService.ClienteAsociado != null)
            {
                ClienteAsociado = ClientesService.ClienteAsociado;
            }
            RefrescarDatosInformes();
            ActualizarFiltroFechas();
        }

        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;
        [Parameter]
        public string filtroRecibido { get; set; } = "clientes";

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }
        private Task RecibirFiltro(string texto)
        {
            filtroRecibido = texto;
            ActualizarFiltroFechas();
            return Task.CompletedTask;
        }
        private void ActualizarFiltroFechas()
        {
            if (activeTab == "all" && filtroRecibido == "clientes")
            {
                if (Clientes.Any())
                {
                    fechaInicioDateTime = Clientes.OrderBy(i => i.RecordDateDateTime).Select(i => i.RecordDateDateTime).First();
                    fechaFinalDateTime = Clientes.OrderByDescending(i => i.RecordDateDateTime).Select(i => i.RecordDateDateTime).First();
                }
            }
            else if (activeTab == "recent" && filtroRecibido == "clientes")
            {
                if (ClientesRecientes.Any())
                {
                    fechaInicioDateTime = ClientesRecientes.OrderBy(i => i.RecordDateDateTime).Select(i => i.RecordDateDateTime).First();
                    fechaFinalDateTime = ClientesRecientes.OrderByDescending(i => i.RecordDateDateTime).Select(i => i.RecordDateDateTime).First();
                }
            }else if(filtroRecibido == "informes" && InformesAsociados.Any())
            {
                fechaInicioDateTime = InformesAsociados.OrderBy(i => i.FechaActualizacionDateTime).Select(i => i.FechaActualizacionDateTime).First();
                fechaFinalDateTime = InformesAsociados.OrderByDescending(i => i.FechaActualizacionDateTime).Select(i => i.FechaActualizacionDateTime).First();
            }
            StateHasChanged();
        }
        private ListaInformes listaInformesComponente;

        private void RefrescarDatosInformes()
        {
            listaInformesComponente.RefrescarDatos();
        }
        public async Task RefrescaListaInformes(bool estado)
        {
            if (estado)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await InformesService.ObtenerListaDeInformesAsociados(clienteActual, esquema);
                if (InformesService.ListaInformesAsociados != null)
                {
                    InformesAsociados = InformesService.ListaInformesAsociados;
                }
            }
           
        }
        public void ActualizarFechaInicio(DateTime fechaInicio)
        {
            fechaInicioDateTime = fechaInicio;
            StateHasChanged();
        }
        public void ActualizarFechaFin(DateTime fechaFin)
        {
            fechaFinalDateTime = fechaFin;
            StateHasChanged();
        }
    }
}
