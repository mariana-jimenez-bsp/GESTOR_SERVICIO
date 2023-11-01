using Blazored.LocalStorage;
using BSP.POS.Presentacion.Interfaces.Alertas;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BSP.POS.Presentacion.Services.Alertas
{
    public class AlertasService : IAlertasInterface
    {
        private readonly SweetAlertService Swal;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;
        private readonly IJSRuntime _JSRuntime;
        public AlertasService(SweetAlertService alertasService, NavigationManager navigationManager, ILocalStorageService localStorageService, IJSRuntime JSRuntime)
        {
            Swal = alertasService;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _JSRuntime = JSRuntime;
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
                Title = "Advertencia!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            });
        }

        public async Task SwalAvisoCancelado(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Aviso!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Info,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(async swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    await _JSRuntime.InvokeVoidAsync("history.back");
                }
            });
        }

        public async Task SwalAvisoLogin(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Aviso!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Info,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    _navigationManager.NavigateTo($"login");
                }
            });
        }
        public async Task SwalExitoHecho(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Éxito!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Success,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(async swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    await _JSRuntime.InvokeVoidAsync("history.back");
                }
            });
        }

        public async Task SwalExitoLogin(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Éxito!",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Success,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(async swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    await _localStorageService.RemoveItemAsync("token");
                    _navigationManager.NavigateTo($"login");
                }
            });
        }

    }
}
