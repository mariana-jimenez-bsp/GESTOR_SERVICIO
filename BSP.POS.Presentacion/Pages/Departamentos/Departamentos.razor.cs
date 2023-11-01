using BSP.POS.Presentacion.Models.Departamentos;
using BSP.POS.Presentacion.Pages.Actividades;
using BSP.POS.Presentacion.Services.Departamentos;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Departamentos
{
    public partial class Departamentos : ComponentBase
    {
        public string esquema = string.Empty;
        public List<mDepartamentos> departamentos = new List<mDepartamentos>();
        public bool cargaInicial = false;
        public string rol = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            await DepartamentosService.ObtenerListaDeDepartamentos(esquema);
            if (DepartamentosService.listaDepartamentos != null)
            {
                departamentos = DepartamentosService.listaDepartamentos;

            }


            cargaInicial = true;
        }

        private void CambioDepartamentoNombre(ChangeEventArgs e, int departamentoId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var departamento in departamentos)
                {
                    if (departamento.Id == departamentoId)
                    {
                        departamento.Departamento = e.Value.ToString();
                    }
                }
            }
        }
        private async Task DescartarCambios()
        {
            await AlertasService.SwalAvisoCancelado("Se han Descartado los cambios");
        }

        private async Task<bool> ActualizarListaDepartamentos()
        {
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool seActualizo = await DepartamentosService.ActualizarListaDeDepartamentos(departamentos, esquema);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await DepartamentosService.ObtenerListaDeDepartamentos(esquema);
                if (DepartamentosService.listaDepartamentos != null)
                {
                    departamentos = DepartamentosService.listaDepartamentos;
                }
                if (seActualizo)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }

        [Parameter]
        public string textoRecibido { get; set; } = string.Empty;

        private Task RecibirTexto(string texto)
        {
            textoRecibido = texto;
            return Task.CompletedTask;
        }


        private async Task ActivarScrollBarDeErrores()
        {
            StateHasChanged();
            await Task.Delay(100);
            var isValid = await JSRuntime.InvokeAsync<bool>("HayErroresValidacion", ".validation-message");

            if (!isValid)
            {
                // Si hay errores de validación, activa el scrollbar
                await JSRuntime.InvokeVoidAsync("ActivarScrollViewValidacion", ".validation-message");
            }
        }

        private void IrAAgregarDepartamento()
        {
            navigationManager.NavigateTo($"configuraciones/departamento/agregar");
        }

        private async Task SwalActualizandoDepartamentos()
        {

            bool resultadoActualizar = false;
            await Swal.FireAsync(new SweetAlertOptions
            {
                Icon = SweetAlertIcon.Info,
                Title = "Actualizando...",
                ShowCancelButton = false,
                ShowConfirmButton = false,
                AllowOutsideClick = false,
                AllowEscapeKey = false,
                DidOpen = new SweetAlertCallback(async () =>
                {
                    resultadoActualizar = await ActualizarListaDepartamentos();
                    await Swal.CloseAsync();

                }),
                WillClose = new SweetAlertCallback(Swal.CloseAsync)

            });

            if (resultadoActualizar)
            {
                await AlertasService.SwalExito("Departamentos Actualizados");
            }
            else
            {
                await AlertasService.SwalError("Ocurrió un error. Vuelva a intentarlo.");
            }


        }
    }
}
