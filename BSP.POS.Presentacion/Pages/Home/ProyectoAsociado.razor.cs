using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Proyectos;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ProyectoAsociado : ComponentBase
    {
        [Parameter]
        public List<mDatosProyectos> datosDeProyectos { get; set; } = new List<mDatosProyectos>();
        [Parameter] public EventCallback<mDatosProyectos> EnviarProyecto { get; set; }
        public mDatosProyectos proyectoEscogido { get; set; } = new mDatosProyectos();
        protected override void OnParametersSet()
        {
            if (datosDeProyectos != null)
            {
               if(datosDeProyectos.Count == 1)
                {
                    proyectoEscogido = datosDeProyectos[0]; 
                }

            }
        }

        bool activarModalEscogerProyecto = false;
        void ClickHandlerEscogerProyecto(bool activar)
        {
            
            activarModalEscogerProyecto = activar;
            StateHasChanged();
        }

        private async Task RecibirProyectoEscogido(mDatosProyectos proyecto)
        {
            proyectoEscogido = proyecto;
            await EnviarProyecto.InvokeAsync(proyectoEscogido);
        }

        public void LimpiarProyecto()
        {
            proyectoEscogido = new mDatosProyectos();
        }
    }
}
