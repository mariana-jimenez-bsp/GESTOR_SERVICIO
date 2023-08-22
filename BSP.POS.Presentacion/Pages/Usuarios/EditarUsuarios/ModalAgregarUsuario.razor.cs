//using BSP.POS.Presentacion.Services.Proyectos;
//using Microsoft.AspNetCore.Components;

//namespace BSP.POS.Presentacion.Pages.Usuarios.EditarUsuarios
//{
//    public partial class ModalAgregarUsuario: ComponentBase
//    {
//        [Parameter] public bool ActivarModal { get; set; } = false;
//        [Parameter] public EventCallback<bool> OnClose { get; set; }
//        public string esquema = string.Empty;
//        public mProyectos proyecto = new mProyectos();
//        protected override async Task OnInitializedAsync()
//        {
//            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
//            var user = authenticationState.User;
//            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();

//        }

//        private void OpenModal()
//        {
//            ActivarModal = true;
//        }

//        private async Task CloseModal()
//        {
//            proyecto = new mProyectos();
//            await OnClose.InvokeAsync(false);

//        }

//        private void CambioNombreConsultor(ChangeEventArgs e)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {
//                proyecto.nombre_consultor = e.Value.ToString();
//            }
//        }

//        private void CambioFechaInicial(ChangeEventArgs e)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {
//                proyecto.fecha_inicial = e.Value.ToString();

//            }
//        }

//        private void CambioFechaFinal(ChangeEventArgs e)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {

//                proyecto.fecha_final = e.Value.ToString();
//            }
//        }

//        private void CambioHorasTotales(ChangeEventArgs e)
//        {

//            proyecto.horas_totales = e.Value.ToString();


//        }

//        private void CambioEmpresa(ChangeEventArgs e)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {

//                proyecto.empresa = e.Value.ToString();
//            }
//        }

//        private void CambioCentroCosto(ChangeEventArgs e)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {

//                proyecto.centro_costo = e.Value.ToString();
//            }
//        }

//        private void CambioNombreProyecto(ChangeEventArgs e)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {

//                proyecto.nombre_proyecto = e.Value.ToString();
//            }
//        }

//        private async Task AgregarProyecto()
//        {
//            await AuthenticationStateProvider.GetAuthenticationStateAsync();
//            await ProyectosService.AgregarProyecto(proyecto, esquema);
//            await CloseModal();
//        }
//    }
//}
