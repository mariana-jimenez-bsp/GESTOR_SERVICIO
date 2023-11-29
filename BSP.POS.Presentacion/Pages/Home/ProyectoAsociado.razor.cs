using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Proyectos;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BSP.POS.Presentacion.Pages.Home
{
    public partial class ProyectoAsociado : ComponentBase
    {
        [Parameter]
        public List<mDatosProyectos> datosDeProyectos { get; set; } = new List<mDatosProyectos>();
        [Parameter] public EventCallback<mDatosProyectos> EnviarProyecto { get; set; }
        public mDatosProyectos proyectoEscogido { get; set; } = new mDatosProyectos();
        [Parameter] public mClienteAsociado ClienteActual { get; set; } = new mClienteAsociado();
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            // Inicializa los tooltips de Bootstrap
            try
            {
                await JS.InvokeVoidAsync("initTooltips");
            }
            catch (Exception ex)
            {

                string error = ex.ToString();
                Console.WriteLine(error);
            }


        }
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
        async Task ClickHandlerEscogerProyecto(bool activar)
        {
            if (activar)
            {
                if (!string.IsNullOrEmpty(ClienteActual.CLIENTE))
                {
                    activarModalEscogerProyecto = activar;
                    StateHasChanged();
                }
                else
                {
                    await AlertasService.SwalAdvertencia("Debe seleccionar un cliente");
                }
            }
            else
            {
                activarModalEscogerProyecto = activar;
                StateHasChanged();
            }
           
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
