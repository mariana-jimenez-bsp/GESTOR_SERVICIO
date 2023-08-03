using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;

namespace BSP.POS.Presentacion.Pages.Informes.EditarInforme
{
    public partial class EditarInforme : ComponentBase
    {
        [Parameter]
        public string Consecutivo { get; set; } = string.Empty;
        public mInformeAsociado informe { get; set; } = new mInformeAsociado();
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();
        public List<mActividades> listaActividades = new List<mActividades>();
        public List<mActividadesAsociadas> listaActividadesAsociadas = new List<mActividadesAsociadas>();
        public List<mUsuariosDeCliente> listaDeUsuariosDeCliente = new List<mUsuariosDeCliente>();
        public List<mUsuariosDeCliente> listaDeUsuariosParaAgregar = new List<mUsuariosDeCliente>();
        public List<mUsuariosDeClienteDeInforme> listadeUsuariosDeClienteDeInforme = new List<mUsuariosDeClienteDeInforme>();
        public mUsuariosDeClienteDeInforme usuarioAAgregar = new mUsuariosDeClienteDeInforme();
        public int total_horas_cobradas = 0;
        public int total_horas_no_cobradas = 0;
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
        private ElementReference actividadesButton;
        private ElementReference informeButton;
        private string successMessage;

        private async Task SubmitActividades()
        {
            await JS.InvokeVoidAsync("clickButton", actividadesButton);
        }

        private async Task SubmitInforme()
        {
            await JS.InvokeVoidAsync("clickButton", informeButton);
        }

        private async Task TodosLosBotonesSubmit()
        {
            
            await SubmitInforme();
            await SubmitActividades();
           
        }
        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            usuarioActual = user.Identity.Name;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            if (!string.IsNullOrEmpty(Consecutivo))
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                InformesService.InformeAsociado = await InformesService.ObtenerInformeAsociado(Consecutivo, esquema);
                if (InformesService.InformeAsociado != null)
                {
                    informe = InformesService.InformeAsociado;
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    ClientesService.ClienteAsociado = await ClientesService.ObtenerClienteAsociado(informe.cliente, esquema);
                    if (ClientesService.ClienteAsociado != null)
                    {
                        ClienteAsociado = ClientesService.ClienteAsociado;
                    }
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ActividadesService.ObtenerListaDeActividades(esquema);
                    if (ActividadesService.ListaActividades != null)
                    {
                        listaActividades = ActividadesService.ListaActividades;

                    }
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await ActividadesService.ObtenerListaDeActividadesAsociadas(Consecutivo, esquema);
                    if (ActividadesService.ListaActividadesAsociadas != null)
                    {
                        listaActividadesAsociadas = ActividadesService.ListaActividadesAsociadas;
                       
                        try
                        {
                            total_horas_cobradas = listaActividadesAsociadas.Sum(act => int.Parse(act.horas_cobradas));
                            total_horas_no_cobradas = listaActividadesAsociadas.Sum(act => int.Parse(act.horas_no_cobradas));
                        }
                        catch
                        {
                            total_horas_cobradas = 0;
                            total_horas_no_cobradas = 0;
                        }
                    }

                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    listaDeUsuariosDeCliente = await UsuariosService.ObtenerListaDeUsuariosDeClienteAsociados(esquema, ClienteAsociado.CLIENTE);
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerListaUsuariosDeClienteDeInforme(informe.consecutivo, esquema);
                    if (UsuariosService.ListaUsuariosDeClienteDeInforme != null)
                    {
                        listadeUsuariosDeClienteDeInforme = UsuariosService.ListaUsuariosDeClienteDeInforme;
                        foreach (var usuario in listadeUsuariosDeClienteDeInforme)
                        {
                            usuario.nombre_usuario = listaDeUsuariosDeCliente.Where(u => u.codigo == usuario.codigo_usuario_cliente).Select(c => c.usuario).First();
                            usuario.departamento_usuario = listaDeUsuariosDeCliente.Where(u => u.codigo == usuario.codigo_usuario_cliente).Select(c => c.departamento).First();
                        }
                    }
                    listaDeUsuariosParaAgregar = listaDeUsuariosDeCliente.Where(usuario => !listadeUsuariosDeClienteDeInforme.Any(usuarioDeInforme => usuarioDeInforme.codigo_usuario_cliente == usuario.codigo)).ToList();
                }
            }
        }

        private void CambioHorasCobradas(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var actividad in listaActividadesAsociadas)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.horas_cobradas = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioHorasNoCobradas(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var actividad in listaActividadesAsociadas)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.horas_no_cobradas = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioActividad(ChangeEventArgs e, string actividadId)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                foreach (var actividad in listaActividadesAsociadas)
                {
                    if (actividad.Id == actividadId)
                    {
                        actividad.codigo_actividad = e.Value.ToString();
                    }
                }
            }
        }

        private void CambioFechaConsultoria(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                 informe.fecha_consultoria = e.Value.ToString();

            }
        }

        private void CambioHoraInicio(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                informe.hora_inicio = e.Value.ToString();
              
            }
        }

        private void CambioHoraFinal(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                informe.hora_final = e.Value.ToString();
                

            }
        }

        private async Task ActualizarActividadesAsociadas()
        {
            successMessage = null;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ActividadesService.ActualizarListaDeActividadesAsociadas(listaActividadesAsociadas, esquema);
            await ActividadesService.ObtenerListaDeActividadesAsociadas(Consecutivo, esquema);
            listaActividadesAsociadas = ActividadesService.ListaActividadesAsociadas;
            try
            {
                total_horas_cobradas = listaActividadesAsociadas.Sum(act => int.Parse(act.horas_cobradas));
                total_horas_no_cobradas = listaActividadesAsociadas.Sum(act => int.Parse(act.horas_no_cobradas));
            }
            catch
            {
                total_horas_cobradas = 0;
                total_horas_no_cobradas = 0;
            }
            successMessage = "Informe Actualizado";
        }

        private async Task ActualizarInformeAsociado()
        {

            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await InformesService.ActualizarInformeAsociado(informe, esquema);
            await InformesService.ObtenerInformeAsociado(Consecutivo, esquema);
            informe = InformesService.InformeAsociado;
            
        }
        private void CambioCodigoDeUsuario(ChangeEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                usuarioAAgregar.codigo_usuario_cliente = e.Value.ToString();

            }
        }
        private async Task AgregarUsuarioDeClienteDeInforme()
        {
            if(usuarioAAgregar.codigo_usuario_cliente != null)
            {
                usuarioAAgregar.consecutivo_informe = Consecutivo;
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await UsuariosService.AgregarUsuarioDeClienteDeInforme(usuarioAAgregar, esquema);
            }
        }
    }
}
