using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Clientes;
using BSP.POS.Presentacion.Services.Clientes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ListaClientes : ComponentBase
    {
        public List<mDatosProyectos> proyectosDeCliente = new List<mDatosProyectos>();
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();
        public List<mClientes> Clientes = new List<mClientes>();
        public List<mClientes> ClientesRecientes = new List<mClientes>();
        public List<mInformesDeProyecto> listaInformesDeProyecto { get; set; } = new List<mInformesDeProyecto>();
        public mPerfil PerfilActual = new mPerfil();
        private List<mInformesDeProyecto> listaTodosLosInformes { get; set; } = new List<mInformesDeProyecto>();
        public List<mDatosProyectos> proyectos { get; set; } = new List<mDatosProyectos>();
        private DateTime fechaInicioDateTime = DateTime.MinValue;
        private DateTime fechaFinalDateTime = DateTime.MinValue;
        private ListaInformes listaInformesComponente;

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
                await RefrescarTodosLosInformes();
                ActualizarFiltroFechas();
                cargaInicial = true;
                StateHasChanged();
            }
           


        }
        private async Task RefrescarTodosLosInformes()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerPerfil(usuarioActual, esquema);
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ProyectosService.ObtenerDatosDeProyectosActivosDeCliente(esquema, UsuariosService.Perfil.cod_cliente);
            proyectos = ProyectosService.ListaDatosProyectosActivosDeCliente;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await InformesService.ObtenerListaDeInformes(esquema);
            if (InformesService.ListaInformes != null)
            {
                if (rol == "Admin")
                {
                    listaTodosLosInformes = InformesService.ListaInformes;
                }
                else
                {
                    listaTodosLosInformes = InformesService.ListaInformes.Where(i => proyectos.Any(p => p.numero == i.numero_proyecto)).ToList();
                }
            }
            StateHasChanged();
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

        private async Task EnviarProyectosDeCliente(string cliente)
        {
            RefrescarDatosInformes();
            clienteActual = cliente;
            proyectosDeCliente = new List<mDatosProyectos>();
            ClienteAsociado = new mClienteAsociado();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ProyectosService.ObtenerDatosDeProyectosActivosDeCliente(esquema, cliente);
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            ClientesService.ClienteAsociado = await ClientesService.ObtenerClienteAsociado(cliente, esquema);
            if (ProyectosService.ListaDatosProyectosActivosDeCliente != null)
            {
                proyectosDeCliente = ProyectosService.ListaDatosProyectosActivosDeCliente;
                if (proyectosDeCliente.Count == 1)
                {
                    await listaInformesComponente.ActualizarListaInformessActuales();
                }
                
            }
            if (ClientesService.ClienteAsociado != null)
            {
                ClienteAsociado = ClientesService.ClienteAsociado;
            }
           
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
            }else if(filtroRecibido == "informes" && listaTodosLosInformes.Any())
            {
                fechaInicioDateTime = listaTodosLosInformes.OrderBy(i => i.FechaActualizacionDateTime).Select(i => i.FechaActualizacionDateTime).First();
                fechaFinalDateTime = listaTodosLosInformes.OrderByDescending(i => i.FechaActualizacionDateTime).Select(i => i.FechaActualizacionDateTime).First();
            }
            StateHasChanged();
        }

        private void RefrescarDatosInformes()
        {
            proyectosDeCliente = new List<mDatosProyectos>();
            listaInformesDeProyecto = new List<mInformesDeProyecto>();
            listaInformesComponente.RefrescarDatos();
        }
        //public async Task RefrescaListaInformes(bool estado)
        //{
        //    if (estado)
        //    {
        //        await AuthenticationStateProvider.GetAuthenticationStateAsync();
        //        await InformesService.ObtenerListaDeInformesDeProyecto(proyectoEs, esquema);
        //        if (InformesService.ListaInformesAsociados != null)
        //        {
        //            InformesAsociados = InformesService.ListaInformesAsociados;
        //        }
        //    }
           
        //}
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

        private void RecibirListaInformes(List<mInformesDeProyecto> informes)
        {
            listaInformesDeProyecto = informes;
        }
    }
}
