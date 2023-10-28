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
            await AlertasService.SwalAvisoNuevoDescartado("Se han cancelado los cambios", "Departamentos");
        }
        private async Task AgregarDepartamentoNuevo()
        {
            bool resultadoDepartamento = false;
            try
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                resultadoDepartamento = await DepartamentosService.AgregarDepartamento(departamentoNuevo, esquema);
                if (resultadoDepartamento)
                {
                    await AlertasService.SwalExitoNuevo("El departamento se ha agregado", "Departamentos");
                }
                else
                {
                    await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
                }

            }
            catch (Exception)
            {

                await AlertasService.SwalError("Ocurrío un Error vuelva a intentarlo");
            }

        }
    }
}
