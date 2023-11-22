using BSP.POS.Presentacion.Models.Reportes;
using BSP.POS.Presentacion.Services.Reportes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BSP.POS.Presentacion.Pages.Informes.VerInforme
{
    public partial class EnviarCorreoInforme : ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public EventCallback<List<string>> enviarListaCorreosExtras { get; set; }
        public List<string> listaCorreosExtras { get; set; } = new List<string>();
        public string esquema = string.Empty;
        private string mensajeError;
        private mAgregarCorreo correoExtraActual = new mAgregarCorreo();

        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
        }
        private void CambioCorreoExtra(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                correoExtraActual.correo = e.Value.ToString();

            }
        }

        private void AgregarCorreoExtra()
        {
            if (!string.IsNullOrEmpty(correoExtraActual.correo))
            {
                listaCorreosExtras.Add(correoExtraActual.correo);
                correoExtraActual = new mAgregarCorreo();
            }
           
        }        
        private async Task EnviarCorreo()
        {
            await OnClose.InvokeAsync(false);
            await enviarListaCorreosExtras.InvokeAsync(listaCorreosExtras);
            correoExtraActual = new mAgregarCorreo();
            listaCorreosExtras = new List<string>();
        }
        private async Task SalirConLaX()
        {
            await OnClose.InvokeAsync(false);
        }

        private async Task DescartarCambios()
        {
            correoExtraActual = new mAgregarCorreo();
            listaCorreosExtras = new List<string>();
            await CloseModal();
        }

        private async Task CloseModal()
        {
            await OnClose.InvokeAsync(false);

        }
    }
}
