using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.ItemsCliente;
using BSP.POS.Presentacion.Models.Proyectos;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Proyectos
{
    public partial class ProyectosTerminados: ComponentBase
    {
        public string esquema = string.Empty;
        public List<mProyectos> proyectos = new List<mProyectos>();
        public List<mItemsCliente> listaCentrosDeCosto = new List<mItemsCliente>();
        public List<mClientes> listaDeClientes = new List<mClientes>();
        public bool cargaInicial = false;
        public string rol = string.Empty;
        List<string> permisos;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            permisos = user.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ClientesService.ObtenerListaClientes(esquema);
            if (ClientesService.ListaClientes != null)
            {
                listaDeClientes = ClientesService.ListaClientes;
            }
            

            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ItemsClienteService.ObtenerListaDeCentrosDeCosto(esquema);
            if (ItemsClienteService.listaCentrosDeCosto != null)
            {
                listaCentrosDeCosto = ItemsClienteService.listaCentrosDeCosto;
            }
            await RefrescarListaDeProyectos();
            cargaInicial = true;
        }

        private async Task RefrescarListaDeProyectos()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ProyectosService.ObtenerListaDeProyectosTerminados(esquema);
            if (ProyectosService.ListaProyectosTerminados != null)
            {
                proyectos = ProyectosService.ListaProyectosTerminados;

            }
            foreach (var proyecto in proyectos)
            {
                string nombreConsultor = listaDeClientes.Where(c => c.CLIENTE == proyecto.codigo_cliente).Select(c => c.CONTACTO).First();
                if (nombreConsultor != null)
                {
                    proyecto.nombre_consultor = nombreConsultor;
                }
                string nombreCliente = listaDeClientes.Where(c => c.CLIENTE == proyecto.codigo_cliente).Select(c => c.NOMBRE).First();
                if (nombreCliente != null)
                {
                    proyecto.nombre_cliente = nombreCliente;
                }
                string descripcionCentroCosto = listaCentrosDeCosto.Where(c => c.valor == proyecto.centro_costo).Select(c => c.descripcion).First();
                if (descripcionCentroCosto != null)
                {
                    proyecto.descripcion_centro_costo = descripcionCentroCosto;
                }
            }
        }
        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }

        private void IrAProyectosIniciados()
        {

            navigationManager.NavigateTo($"proyectos");
        }
    }
}
