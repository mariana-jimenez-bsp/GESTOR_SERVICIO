using BSP.POS.Presentacion.Interfaces.Alertas;
using CurrieTechnologies.Razor.SweetAlert2;

namespace BSP.POS.Presentacion.Services.Alertas
{
    public class AlertasService : IAlertasInterface
    {
        private readonly SweetAlertService Swal;
        public AlertasService(SweetAlertService alertasService)
        {
            Swal = alertasService;
        }
        public async Task SwalExito(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Éxito!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Success,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            });
        }
        public async Task SwalError(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Error!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Error,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            });
        }
        public async Task SwalAviso(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Aviso!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Info,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            });
        }

        public async Task SwalAdvertencia(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Aviso!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            });
        }
        
    }
}
