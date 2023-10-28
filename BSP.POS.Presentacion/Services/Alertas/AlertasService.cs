using Blazored.LocalStorage;
using BSP.POS.Presentacion.Interfaces.Alertas;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Services.Alertas
{
    public class AlertasService : IAlertasInterface
    {
        private readonly SweetAlertService Swal;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;
        public AlertasService(SweetAlertService alertasService, NavigationManager navigationManager, ILocalStorageService localStorageService)
        {
            Swal = alertasService;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
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

        public async Task SwalAvisoNuevoDescartado(string mensajeAlerta, string accion)
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
                    if (accion == "Proyectos")
                    {
                        _navigationManager.NavigateTo($"proyectos");
                    }
                    else if (accion == "Clientes")
                    {
                        _navigationManager.NavigateTo($"clientes");
                    }
                    else if (accion == "Actividades")
                    {
                        _navigationManager.NavigateTo($"actividades");
                    }
                    else if (accion == "Departamentos")
                    {
                        _navigationManager.NavigateTo($"configuraciones/departamentos");
                    }
                    else if (accion == "Usuarios")
                    {
                        _navigationManager.NavigateTo($"configuraciones/usuarios");
                    }
                    else if (accion == "Login")
                    {
                        _navigationManager.NavigateTo($"login");
                    }
                }
            });
        }
        public async Task SwalExitoNuevo(string mensajeAlerta, string accion)
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
                    if (accion == "Proyectos")
                    {
                        _navigationManager.NavigateTo($"proyectos");
                    }else if(accion == "Clientes")
                    {
                        _navigationManager.NavigateTo($"clientes");
                    }
                    else if (accion == "Actividades")
                    {
                        _navigationManager.NavigateTo($"actividades");
                    }
                    else if (accion == "Departamentos")
                    {
                        _navigationManager.NavigateTo($"configuraciones/departamentos");
                    }
                    else if (accion == "Usuarios")
                    {
                        _navigationManager.NavigateTo($"configuraciones/usuarios");
                    }
                    else if (accion == "Login")
                    {
                        await _localStorageService.RemoveItemAsync("token");
                        _navigationManager.NavigateTo($"login");
                    }
                }
            });
        }

    }
}
