using BSP.POS.Presentacion.Models;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes
{
    public partial class EditarInforme: ComponentBase
    {
        [Parameter]
        public string Consecutivo { get; set; } = string.Empty;
        public mInformeAsociado informe { get; set; } = new mInformeAsociado();
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();
        public List<mActividades> listaActividades = new List<mActividades>();
        public decimal total_horas_cobradas = 0;
        public decimal total_horas_no_cobradas = 0;
        public string hora_inicio_reducida { get; set; } = string.Empty;
        public string hora_final_reducida { get; set; } = string.Empty;
        protected override async Task OnInitializedAsync()
        {


            if (!string.IsNullOrEmpty(Consecutivo))
            {

                InformesService.InformeAsociado = await InformesService.ObtenerInformeAsociado(Consecutivo);
                if (InformesService.InformeAsociado != null)
                {
                    informe = InformesService.InformeAsociado;
                    hora_inicio_reducida = informe.hora_inicio.Substring(0, 5);
                    hora_final_reducida = informe.hora_final.Substring(0, 5);
                    ClientesService.ClienteAsociado = await ClientesService.ObtenerClienteAsociado(informe.cliente);
                    if (ClientesService.ClienteAsociado != null)
                    {
                        ClienteAsociado = ClientesService.ClienteAsociado;
                    }
                    await ActividadesService.ObtenerListaDeActividadesAsociadas(Consecutivo);
                    if (ActividadesService.ListaActividadesAsociadas != null)
                    {
                        listaActividades = ActividadesService.ListaActividadesAsociadas;
                        try
                        {
                            total_horas_cobradas = listaActividades.Sum(act => decimal.Parse(act.horas_cobradas));
                            total_horas_no_cobradas = listaActividades.Sum(act => decimal.Parse(act.horas_no_cobradas));
                        }
                        catch
                        {
                            total_horas_cobradas = 0;
                            total_horas_no_cobradas = 0;
                        }

                    }

                }
            }
        }
    }
}
