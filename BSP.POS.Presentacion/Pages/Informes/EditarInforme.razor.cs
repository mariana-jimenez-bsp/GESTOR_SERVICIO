using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Usuarios;
using Microsoft.AspNetCore.Components;

namespace BSP.POS.Presentacion.Pages.Informes
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
        public List<mUsuariosDeClienteDeInforme> listadeUsuariosDeClienteDeInforme = new List<mUsuariosDeClienteDeInforme>();
        public decimal total_horas_cobradas = 0;
        public decimal total_horas_no_cobradas = 0;
        public string hora_inicio_reducida { get; set; } = string.Empty;
        public string hora_final_reducida { get; set; } = string.Empty;
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
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
                    hora_inicio_reducida = informe.hora_inicio.Substring(0, 5);
                    hora_final_reducida = informe.hora_final.Substring(0, 5);
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
                            total_horas_cobradas = listaActividadesAsociadas.Sum(act => decimal.Parse(act.horas_cobradas));
                            total_horas_no_cobradas = listaActividadesAsociadas.Sum(act => decimal.Parse(act.horas_no_cobradas));
                        }
                        catch
                        {
                            total_horas_cobradas = 0;
                            total_horas_no_cobradas = 0;
                        }
                        foreach (var actividad in listaActividadesAsociadas)
                        {
                            actividad.nombre_actividad = listaActividades.Where(a => a.codigo == actividad.codigo_actividad).Select(c => c.Actividad).First();
                        }
                    }

                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    listaDeUsuariosDeCliente = await UsuariosService.ObtenerListaDeUsuariosDeClienteAsociados(esquema, ClienteAsociado.CLIENTE);
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerListaUsuariosDeClienteDeInforme(informe.consecutivo, esquema);
                    if(UsuariosService.ListaUsuariosDeClienteDeInforme != null)
                    {
                        listadeUsuariosDeClienteDeInforme = UsuariosService.ListaUsuariosDeClienteDeInforme;
                        foreach (var usuario in listadeUsuariosDeClienteDeInforme)
                        {
                            usuario.nombre_cliente = listaDeUsuariosDeCliente.Where(u => u.codigo == usuario.codigo_usuario_cliente).Select(c => c.usuario).First();
                            usuario.departamento_cliente = listaDeUsuariosDeCliente.Where(u => u.codigo == usuario.codigo_usuario_cliente).Select(c => c.departamento).First();
                        }
                    }




                }
            }
        }
    }
}
