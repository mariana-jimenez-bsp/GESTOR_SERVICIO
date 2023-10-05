using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ListaInformes: ComponentBase
    {
        [Parameter]
        public List<mInformes> informesAsociados { get; set; } = new List<mInformes>();
        [Parameter]
        public mClienteAsociado clienteAsociado { get; set; } = new mClienteAsociado();
        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;
        [Parameter]
        public string filtroRecibido { get; set; } = string.Empty;
        private bool EsClienteNull = false;


        public string Consecutivo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        protected override void OnParametersSet()
        {
            if(!informesAsociados.Where(i => i.consecutivo == Consecutivo).Any())
            {
                Consecutivo = string.Empty;
                Estado = string.Empty;
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

        private async Task IrACrear()
        {
            EsClienteNull = false;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (!string.IsNullOrEmpty(clienteAsociado.CLIENTE))
            {
                navigationManager.NavigateTo($"Informe/Crear/{clienteAsociado.CLIENTE}");
            }
            else
            {
                EsClienteNull = true;
            }
        }
        public void RefrescarDatos()
        {
            Consecutivo = string.Empty;
            Estado = string.Empty;
            StateHasChanged();
        }
    }
}
