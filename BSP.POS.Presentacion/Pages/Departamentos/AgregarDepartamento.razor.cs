using BSP.POS.Presentacion.Models.Departamentos;
using BSP.POS.Presentacion.Services.Actividades;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Departamentos
{
    public partial class AgregarDepartamento : ComponentBase
    {
        public string esquema = string.Empty;
        public mDepartamentos departamentoNuevo = new mDepartamentos();

        public string mensajeError;
        private bool cargaInicial = false;
        private string rol = string.Empty;
        
        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            cargaInicial = true;
        }
        private void CambioDepartamentoNombre(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {

                departamentoNuevo.Departamento = e.Value.ToString();

            }

        }

        private async Task DescartarCambios()
        {
            await SwalAviso("Se han cancelado los cambios");
        }
        private async Task AgregarDepartamentoNuevo()
        {
            mensajeError = null;
            bool resultadoDepartamento = false;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                resultadoDepartamento = await DepartamentosService.AgregarDepartamento(departamentoNuevo, esquema);
                if (resultadoDepartamento)
                {
                    await SwalExito("El departamento se ha agregado");
                }
                else
                {
                    mensajeError = "Ocurrío un Error vuelva a intentarlo";
                }

            }
            catch (Exception)
            {

                mensajeError = "Ocurrío un Error vuelva a intentarlo";
            }

        }

        private void IrADepartamentos()
        {
            navigationManager.NavigateTo($"configuraciones/departamentos");
        }

        private async Task SwalExito(string mensajeAlerta)
        {
            await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Éxito",
                Text = mensajeAlerta,
                Icon = SweetAlertIcon.Success,
                ShowCancelButton = false,
                ConfirmButtonText = "Ok"
            }).ContinueWith(swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (result.IsConfirmed || result.IsDismissed)
                {
                    IrADepartamentos();
                }
            });
        }

        private async Task SwalAviso(string mensajeAlerta)
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
                    IrADepartamentos();
                }
            });
        }
    }
}
