using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Pages.Clientes;
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
        [Parameter]
        public string esquema { get; set; } = string.Empty;
        [Parameter] public EventCallback<bool> RefrescarListaInformes { get; set; }
        private bool EsClienteNull = false;
        private string[] elementos = new string[]{ ".el-layout", ".cliente-asociado", ".consecutivo-informe", ".header-col-left" };
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

        
        public void RefrescarDatos()
        {
            Consecutivo = string.Empty;
            Estado = string.Empty;
            StateHasChanged();
        }

        private async Task CrearInforme()
        {
            EsClienteNull = false;
            StateHasChanged();
            await Task.Delay(100);
            if (!string.IsNullOrEmpty(clienteAsociado.CLIENTE))
            {
                if (!string.IsNullOrEmpty(esquema))
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    Consecutivo = await InformesService.AgregarInformeAsociado(clienteAsociado.CLIENTE, esquema);
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
                await RefrescarListaInformes.InvokeAsync(true);
            }
        }
    }
}
