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
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();


        public string Consecutivo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        protected override void OnParametersSet()
        {
            if (informesAsociados.Count > 0)
            {
                InformesService.ListaInformesAsociados = informesAsociados;
            }
            if(!informesAsociados.Where(i => i.consecutivo == Consecutivo).Any())
            {
                Consecutivo = string.Empty;
                Estado = string.Empty;
            }
            if (clienteAsociado != null)
            {
                ClienteAsociado = clienteAsociado;

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

        private void IrACrear()
        {
            if (!string.IsNullOrEmpty(ClienteAsociado.CLIENTE))
            {
                navigationManager.NavigateTo($"Informe/Crear/{ClienteAsociado.CLIENTE}");
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
