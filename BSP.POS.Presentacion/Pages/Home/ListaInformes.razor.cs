using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Proyectos;
using BSP.POS.Presentacion.Pages.Clientes;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ListaInformes: ComponentBase
    {
        [Parameter]
        public List<mDatosProyectos> proyectosDeCliente { get; set; } = new List<mDatosProyectos>();
        [Parameter]
        public mClienteAsociado clienteAsociado { get; set; } = new mClienteAsociado();
        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;
        [Parameter]
        public string filtroRecibido { get; set; } = string.Empty;
        [Parameter]
        public string esquema { get; set; } = string.Empty;
        [Parameter] public EventCallback<List<mInformesDeProyecto>> EnviarListaDeInformes { get; set; }
        private bool EsClienteNull = false;
        [Parameter]
        public DateTime fechaInicioDateTime { get; set; } = DateTime.MinValue;
        [Parameter]
        public DateTime fechaFinalDateTime { get; set; } = DateTime.MinValue;
        private List<mInformesDeProyecto> listaInformesDeProyecto { get; set; } = new List<mInformesDeProyecto>();
        private mDatosProyectos proyectoEscogido { get; set; } = new mDatosProyectos();
        private string[] elementos = new string[]{ ".el-layout", ".cliente-asociado", ".consecutivo-informe", ".header-col-left" };
        public string Consecutivo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        protected override void OnParametersSet()
        {
            if (!listaInformesDeProyecto.Where(i => i.consecutivo == Consecutivo).Any())
            {
                Consecutivo = string.Empty;
                Estado = string.Empty;
            }

        }
        private async Task RecibirProyectoEscogido(mDatosProyectos proyecto)
        {
            proyectoEscogido = proyecto;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await InformesService.ObtenerListaDeInformesDeProyecto(proyectoEscogido.codigo_cliente, esquema);
            if(InformesService.ListaInformesDeProyecto != null)
            {
                listaInformesDeProyecto = InformesService.ListaInformesDeProyecto;
                await EnviarListaDeInformes.InvokeAsync(listaInformesDeProyecto);
            }
        }

        public void EnviarConsecutivo(string consecutivo, string estado)
        {

            if (!string.IsNullOrEmpty(consecutivo) && !string.IsNullOrEmpty(estado))
            {
                Consecutivo = consecutivo;
                Estado = estado;
            }
        }

        private ProyectoAsociado proyectoAsociadoComponente; 
        public void RefrescarDatos()
        {
            Consecutivo = string.Empty;
            Estado = string.Empty;

            listaInformesDeProyecto = new List<mInformesDeProyecto>();
            proyectosDeCliente = new List<mDatosProyectos>();
            proyectoEscogido = new mDatosProyectos();
            proyectoAsociadoComponente.LimpiarProyecto();
            StateHasChanged();
        }

        private async Task CrearInforme()
        {
            EsClienteNull = false;
            StateHasChanged();
            await Task.Delay(100);
            if (!string.IsNullOrEmpty(proyectoEscogido.numero))
            {
                if (!string.IsNullOrEmpty(esquema))
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    Consecutivo = await InformesService.AgregarInformeAsociado(proyectoEscogido.numero, esquema);
                    if (!string.IsNullOrEmpty(Consecutivo))
                    {
                        navigationManager.NavigateTo($"Informe/Editar/{Consecutivo}");
                    }
                }
                
            }
            else
            {
                EsClienteNull = true;
            }
        }

        public async Task RefrescaListaInformes(bool estado)
        {
            if (estado)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await InformesService.ObtenerListaDeInformesDeProyecto(proyectoEscogido.codigo_cliente, esquema);
                if (InformesService.ListaInformesDeProyecto != null)
                {
                    listaInformesDeProyecto = InformesService.ListaInformesDeProyecto;
                    await EnviarListaDeInformes.InvokeAsync(listaInformesDeProyecto);
                }
            }
        }
    }
}
