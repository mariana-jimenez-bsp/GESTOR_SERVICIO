using Microsoft.AspNetCore.Components;
using BSP.POS.Presentacion.Models;
namespace BSP.POS.Presentacion.Pages.Modals
{
    public partial class ModalTiempos: ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        public List<mTiempos> tiempos = new List<mTiempos>();
        protected override async Task OnInitializedAsync()
        {
            //await TiemposService.ObtenerListaTIempos();
            //if (TiemposService.ObtenerListaTIempos() != null)
            //{
            //    tiempos = TiemposService.ListaTiempos;
            //}
        }
        private void CambioTiempo(ChangeEventArgs e, string id)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var tiempo in tiempos)
                {
                    if (tiempo.Id == id)
                    {
                        tiempo.horas = e.Value.ToString();
                    }
                }
            }
        }
        private void OpenModal()
        {
            ActivarModal = true;

        }
        private async Task ActualizarListaTiempos()
        {

            await TiemposService.ActualizarListaDeTiempo(tiempos);
            await TiemposService.ObtenerListaTIempos();
            if (TiemposService.ObtenerListaTIempos() != null)
            {
                tiempos = TiemposService.ListaTiempos;
            }
            await CloseModal();
        }
        private async Task CloseModal()
        {
            await TiemposService.ObtenerListaTIempos();
            tiempos = TiemposService.ListaTiempos;
            await OnClose.InvokeAsync(false);

        }
    }
}
