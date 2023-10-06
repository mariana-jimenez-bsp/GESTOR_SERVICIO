using BSP.POS.Presentacion.Models.Actividades;
using BSP.POS.Presentacion.Models.Clientes;
using BSP.POS.Presentacion.Models.Informes;
using BSP.POS.Presentacion.Models.Observaciones;
using BSP.POS.Presentacion.Models.Usuarios;
using BSP.POS.Presentacion.Pages.Usuarios.Usuarios;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BSP.POS.Presentacion.Pages.Informes.VerInforme
{
    public partial class VerInforme: ComponentBase
    {
        [Parameter]
        public string Consecutivo { get; set; } = string.Empty;
        public mInformeAsociado informe { get; set; } = new mInformeAsociado();
        public mClienteAsociado ClienteAsociado = new mClienteAsociado();
        public List<mActividades> listaActividades = new List<mActividades>();
        public List<mActividadesAsociadas> listaActividadesAsociadas = new List<mActividadesAsociadas>();
        public List<mActividades> listaActividadesParaAgregar = new List<mActividades>();
        public List<mUsuariosDeCliente> listaDeUsuariosDeCliente = new List<mUsuariosDeCliente>();
        public List<mUsuariosDeCliente> listaDeUsuariosParaAgregar = new List<mUsuariosDeCliente>();
        public List<mUsuariosDeClienteDeInforme> listadeUsuariosDeClienteDeInforme = new List<mUsuariosDeClienteDeInforme>();
        public List<mUsuariosParaEditar> listaTodosLosUsuarios = new List<mUsuariosParaEditar>();
        public mUsuariosDeClienteDeInforme usuarioAAgregar = new mUsuariosDeClienteDeInforme();
        public mActividadAsociadaParaAgregar actividadAAgregar = new mActividadAsociadaParaAgregar();
        public List<mObservaciones> listaDeObservaciones = new List<mObservaciones>();
        public int total_horas_cobradas = 0;
        public int total_horas_no_cobradas = 0;
        public string usuarioActual { get; set; } = string.Empty;
        public string esquema = string.Empty;
        private string successMessage;
        private string correoEnviado;
        private bool cargaInicial = false;
        private string mensajeConsecutivo;
        private bool estadoObseracionNueva = false;
        private bool estadoObservacionCancelada = false;
        private bool todosLosUsuariosAprobados = false;
        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;
            usuarioActual = user.Identity.Name;
            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
            if (await VerificarValidezDeConsecutivo())
            {
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
                        await RefrescarListaDeActividadesAsociadas();

                        await AuthenticationStateProvider.GetAuthenticationStateAsync();
                        listaDeUsuariosDeCliente = await UsuariosService.ObtenerListaDeUsuariosDeClienteAsociados(esquema, ClienteAsociado.CLIENTE);
                        await RefrescarListaDeUsuariosDeInforme();
                        await RefrescarLaListaDeObservaciones(Consecutivo);
                    }
                }
            }
            else
            {
                mensajeConsecutivo = "El consecutivo no existe o no es válido";
            }
            cargaInicial = true;
        }

        private async Task<bool> VerificarAprobacionesUsuarios()
        {
            bool aprobado = false;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaUsuariosDeClienteDeInforme(Consecutivo, esquema);
            if (UsuariosService.ListaUsuariosDeClienteDeInforme.Any())
            {
                foreach (var usuario in UsuariosService.ListaUsuariosDeClienteDeInforme)
                {
                    if (usuario.aceptacion == "1")
                    {
                        aprobado = true;
                    }
                    else
                    {
                        aprobado = false;
                        break;
                    }
                }
            }
            else
            {
                aprobado = true;
            }
            return aprobado;
        }
        public async Task<bool> VerificarValidezDeConsecutivo()
        {
            if (Consecutivo.Length > 5)
            {
                return false;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            string consecutivoValidar = await InformesService.ValidarExistenciaConsecutivoInforme(esquema, Consecutivo);

            if (!string.IsNullOrEmpty(consecutivoValidar))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task RefrescarLaListaDeObservaciones(string consecutivo)
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await ObservacionesService.ObtenerListaDeObservacionesDeInforme(consecutivo, esquema);
            if (ObservacionesService.ListaDeObservacionesDeInforme != null)
            {
                listaDeObservaciones = ObservacionesService.ListaDeObservacionesDeInforme;
                foreach (var observacion in listaDeObservaciones)
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    await UsuariosService.ObtenerElUsuarioParaEditar(esquema, observacion.codigo_usuario);
                    if (UsuariosService.UsuarioParaEditar != null)
                    {
                        observacion.nombre_usuario = UsuariosService.UsuarioParaEditar.nombre;

                    }


                }
            }
        }
        private async Task RefrescarListaDeActividadesAsociadas()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
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
            listaActividadesParaAgregar = listaActividades.Where(actividad => !listaActividadesAsociadas.Any(actividadAsociada => actividadAsociada.codigo_actividad == actividad.codigo)).ToList();
        }
        private async Task RefrescarListaDeUsuariosDeInforme()
        {
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaDeUsuariosParaEditar(esquema);
            if (UsuariosService.ListaDeUsuariosParaEditar != null)
            {
                listaTodosLosUsuarios = UsuariosService.ListaDeUsuariosParaEditar;
            }
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await UsuariosService.ObtenerListaUsuariosDeClienteDeInforme(informe.consecutivo, esquema);
            if (UsuariosService.ListaUsuariosDeClienteDeInforme != null)
            {
                listadeUsuariosDeClienteDeInforme = UsuariosService.ListaUsuariosDeClienteDeInforme;
                foreach (var usuario in listadeUsuariosDeClienteDeInforme)
                {
                    usuario.nombre_usuario = listaTodosLosUsuarios.Where(u => u.codigo == usuario.codigo_usuario_cliente).Select(c => c.nombre).First();
                    usuario.departamento_usuario = listaDeUsuariosDeCliente.Where(u => u.codigo == usuario.codigo_usuario_cliente).Select(c => c.departamento).First();
                }
            }
            listaDeUsuariosParaAgregar = listaDeUsuariosDeCliente.Where(usuario => !listadeUsuariosDeClienteDeInforme.Any(usuarioDeInforme => usuarioDeInforme.codigo_usuario_cliente == usuario.codigo)).ToList();
        }
       

        
        bool activarModalObservaciones = false;

        bool activarModalEliminarInforme = false;

       

      
        private async Task EnviarCorreosAClientes()
        {
            correoEnviado = null;
            todosLosUsuariosAprobados = false;
            bool verificarAprobacion = false;
            verificarAprobacion = await VerificarAprobacionesUsuarios();
            if (!verificarAprobacion)
            {
                mObjetosParaCorreoAprobacion objetoParaCorreo = new mObjetosParaCorreoAprobacion();
                objetoParaCorreo.informe = informe;
                objetoParaCorreo.total_horas_cobradas = total_horas_cobradas;
                objetoParaCorreo.total_horas_no_cobradas = total_horas_no_cobradas;
                objetoParaCorreo.listaActividadesAsociadas = listaActividadesAsociadas;
                foreach (var actividad in objetoParaCorreo.listaActividadesAsociadas)
                {
                    actividad.nombre_actividad = listaActividades.Where(a => a.codigo == actividad.codigo_actividad).Select(c => c.Actividad).First();
                }
                objetoParaCorreo.listadeUsuariosDeClienteDeInforme = listadeUsuariosDeClienteDeInforme;
                objetoParaCorreo.ClienteAsociado = ClienteAsociado;
                objetoParaCorreo.esquema = esquema;
                objetoParaCorreo.listaDeObservaciones = listaDeObservaciones;
                foreach (var observacion in objetoParaCorreo.listaDeObservaciones)
                {
                    if (UsuariosService.UsuarioParaEditar != null)
                    {
                        observacion.nombre_usuario = listaTodosLosUsuarios.Where(u => u.codigo == observacion.codigo_usuario).Select(c => c.nombre).First();
                    }

                }
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                bool validar = await InformesService.EnviarCorreoDeAprobacionDeInforme(objetoParaCorreo);
                if (validar)
                {
                    correoEnviado = "Correo Enviado";
                }
                else
                {
                    correoEnviado = "Error";
                }
            }
            else
            {
                todosLosUsuariosAprobados = true;
            }
            
        }


      

        async Task ClickHandlerObservaciones(bool activar)
        {
            activarModalObservaciones = activar;
            StateHasChanged();
            if (!activar)
            {
                await RefrescarLaListaDeObservaciones(Consecutivo);
            }
            if (activar)
            {
                estadoObservacionCancelada = false;
                estadoObseracionNueva = false;
            }
        }
      
        void ClickHandlerEliminarInforme(bool activar)
        {
            activarModalEliminarInforme = activar;
            StateHasChanged();
        }

        private bool EsLaPrimeraObservacion(mObservaciones observacion)
        {

            if (listaDeObservaciones.IndexOf(observacion) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool EsLaUltmaObservacion(mObservaciones observacion)
        {

            if (listaDeObservaciones.IndexOf(observacion) == listaDeObservaciones.Count - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void CambiarEstadoObservacionNueva(bool estado)
        {
            estadoObseracionNueva = estado;

        }
        public void CambiarEstadoObservacionCancelada(bool estado)
        {
            estadoObservacionCancelada = estado;
        }


        public bool advertenciaFinalizarInforme = false;
        private async Task ActivarAdvertenciaFinalizar()
        {
            advertenciaFinalizarInforme = false;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            advertenciaFinalizarInforme = true;
            StateHasChanged();
        }

        public bool advertenciaGuardarInforme = false;
        private async Task ActivarAdvertenciaGuardar()
        {
            advertenciaGuardarInforme = false;
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            advertenciaGuardarInforme = true;
            StateHasChanged();
        }
    }
}
