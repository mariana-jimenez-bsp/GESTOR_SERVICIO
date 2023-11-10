using BSP.POS.Presentacion.Models.Proyectos;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class EscogerProyecto: ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public EventCallback<mDatosProyectos> EnviarProyecto { get; set; }
        [Parameter] public List<mDatosProyectos> datosProyectos { get; set; } = new List<mDatosProyectos>();
        private string mensajeError;
        public mDatosProyectos proyectoEscogido { get; set; } = new mDatosProyectos();



        private void CambioNumeroProyecto(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                proyectoEscogido.numero = e.Value.ToString();
                proyectoEscogido = datosProyectos.Where(p => p.numero == proyectoEscogido.numero).First();

            }
        }

        private async Task EscogerProyectoNuevo()
        {
            mensajeError = null;
            if (!string.IsNullOrEmpty(proyectoEscogido.numero))
            {
                await EnviarProyecto.InvokeAsync(proyectoEscogido);
                await CloseModal();
            }
            else
            {
                await Task.Delay(100);
                StateHasChanged();
                mensajeError = "Debe Escoger un proyecto";
            }
        }

        private async Task SalirConLaX()
        {
            await OnClose.InvokeAsync(false);
        }

        private async Task DescartarCambios()
        {
            await CloseModal();
        }

        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }
    }
}
