using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Security.Claims;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class AgregarActividadInforme : ComponentBase
    {
        [Parameter] public bool ActivarModal { get; set; } = false;
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter]public List<mActividades> listaActividadesParaAgregar { get; set; } = new List<mActividades>();
        [Parameter] public string Consecutivo { get; set; } = string.Empty;
        [Parameter]public List<mUsuariosParaEditar> listaTodosLosUsuarios { get; set; } = new List<mUsuariosParaEditar>();
        public mActividadAsociadaParaAgregar actividadAAgregar = new mActividadAsociadaParaAgregar();
        public string esquema = string.Empty;
        public string rol = string.Empty;
        private string fechaInicio = string.Empty;
        private string fechaFinal = string.Empty;
        private string usuarioFiltro = string.Empty;
        private string tipoFiltro = "Fecha";
        private DateTime fechaInicioDateTime = DateTime.MinValue;
        private DateTime fechaFinalDateTime = DateTime.MinValue;
        private string mensajeError;

        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
            if (listaActividadesParaAgregar.Any())
            {
                fechaInicioDateTime = listaActividadesParaAgregar.OrderBy(i => i.FechaActualizacionDateTime).Select(i => i.FechaActualizacionDateTime).First();
                fechaFinalDateTime = listaActividadesParaAgregar.OrderByDescending(i => i.FechaActualizacionDateTime).Select(i => i.FechaActualizacionDateTime).First();
            }
        }

        private async Task AgregarActividadDeInforme()
        {
            mensajeError = null;
            try
            {
                    string horas = listaActividadesParaAgregar.Where(actividad => actividad.codigo == actividadAAgregar.codigo_actividad).First().horas;
                    actividadAAgregar.consecutivo_informe = Consecutivo;
                    actividadAAgregar.horas_cobradas = horas;
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ActividadesService.AgregarActividadDeInforme(actividadAAgregar, esquema);
                    actividadAAgregar = new mActividadAsociadaParaAgregar();
                    await OnClose.InvokeAsync(false);
            }
            catch (Exception)
            {

                mensajeError = "Ocurrió un error vuelva a intentarlo";
            }
            
        }

        private void CambioCodigoActividad(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                actividadAAgregar.codigo_actividad = e.Value.ToString();

            }
        }

        private void CambioFechaInicio(ChangeEventArgs e)
        {
            fechaInicio = e.Value.ToString();
            if (!string.IsNullOrEmpty(fechaInicio))
            {
                fechaInicioDateTime = DateTime.ParseExact(fechaInicio, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (fechaInicioDateTime > fechaFinalDateTime)
                {
                    fechaFinal = fechaInicio;
                    fechaFinalDateTime = fechaInicioDateTime;
                }
            }
        }

        private void CambioFechaFinal(ChangeEventArgs e)
        {
            fechaFinal = e.Value.ToString();
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fechaFinalDateTime = DateTime.ParseExact(fechaFinal, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (fechaFinalDateTime < fechaInicioDateTime)
                {
                    fechaInicio = fechaFinal;
                    fechaInicioDateTime = fechaFinalDateTime;
                }
            }
        }

        private void CambioUsuarioFiltro(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuarioFiltro = e.Value.ToString();
            }
            else
            {
                usuarioFiltro = string.Empty;
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
