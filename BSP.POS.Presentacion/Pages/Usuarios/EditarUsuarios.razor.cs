//using BSP.POS.Presentacion.Models.Proyectos;
//using BSP.POS.Presentacion.Models.Usuarios;
//using BSP.POS.Presentacion.Services.Actividades;
//using Microsoft.AspNetCore.Components;
//using System.Security.Claims;

//namespace BSP.POS.Presentacion.Pages.Usuarios
//{
//    public partial class EditarUsuarios: ComponentBase
//    {
//        public string esquema = string.Empty;
//        public List<mUsuariosParaEditar> usuarios = new List<mUsuariosParaEditar>();
//        public bool cargaInicial = false;
//        public string rol = string.Empty;
//        protected override async Task OnInitializedAsync()
//        {
//            cargaInicial = false;
//            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
//            var user = authenticationState.User;
//            rol = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).First();
//            esquema = user.Claims.Where(c => c.Type == "esquema").Select(c => c.Value).First();
//            await UsuariosService.ObtenerListaDeUsuariosParaEditar(esquema);
//            if (UsuariosService.ListaDeUsuariosParaEditar != null)
//            {
//                usuarios = UsuariosService.ListaDeUsuariosParaEditar;
//                cargaInicial = true;
//            }

//        }



//        private void VolverAlHome()
//        {

//            navigationManager.NavigateTo($"Index", forceLoad: true);

//        }

//        private void CambioCliente(ChangeEventArgs e, string usuarioId)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {
//                foreach (var usuario in usuarios)
//                {
//                    if (usuario.id == usuarioId)
//                    {
//                        usuario.cod_cliente = e.Value.ToString();
//                    }
//                }
//            }
//        }

//        private void CambioUsuario(ChangeEventArgs e, string usuarioId)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {
//                foreach (var usuario in usuarios)
//                {
//                    if (usuario.id == usuarioId)
//                    {
//                        usuario.usuario = e.Value.ToString();
//                    }
//                }
//            }
//        }

//        private void CambioCorreo(ChangeEventArgs e, string usuarioId)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {
//                foreach (var usuario in usuarios)
//                {
//                    if (usuario.id == usuarioId)
//                    {
//                        usuario.correo = e.Value.ToString();
//                    }
//                }
//            }
//        }

//        private void CambioClave(ChangeEventArgs e, string usuarioId)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {
//                foreach (var usuario in usuarios)
//                {
//                    if (usuario.id == usuarioId)
//                    {
//                        usuario.clave = e.Value.ToString();
//                    }
//                }
//            }
//        }

//        private void CambioNombre(ChangeEventArgs e, string usuarioId)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {
//                foreach (var usuario in usuarios)
//                {
//                    if (usuario.id == usuarioId)
//                    {
//                        usuario.nombre = e.Value.ToString();
//                    }
//                }
//            }
//        }

//        private void CambioTelefono(ChangeEventArgs e, string usuarioId)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {
//                foreach (var usuario in usuarios)
//                {
//                    if (usuario.id == usuarioId)
//                    {
//                        usuario.telefono = e.Value.ToString();
//                    }
//                }
//            }
//        }


//        private void CambioDepartamento(ChangeEventArgs e, string usuarioId)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {
//                foreach (var usuario in usuarios)
//                {
//                    if (usuario.id == usuarioId)
//                    {
//                        usuario.departamento = e.Value.ToString();
//                    }
//                }
//            }
//        }
//        private void CambioImagen(ChangeEventArgs e, string usuarioId)
//        {
//            if (!string.IsNullOrEmpty(e.Value.ToString()))
//            {
//                foreach (var usuario in usuarios)
//                {
//                    if (usuario.id == usuarioId)
//                    {
//                        usuario.usuario = e.Value.ToString();
//                    }
//                }
//            }
//        }

//        private async Task ActualizarListaProyectos()
//        {
//            await AuthenticationStateProvider.GetAuthenticationStateAsync();
//            await ProyectosService.ActualizarListaDeProyectos(proyectos, esquema);
//            if (ProyectosService.ListaProyectos != null)
//            {
//                proyectos = ProyectosService.ListaProyectos;
//            }
//            VolverAlHome();
//        }

//        [Parameter]
//        public string textoRecibido { get; set; } = string.Empty;

//        private Task RecibirTexto(string texto)
//        {
//            textoRecibido = texto;
//            return Task.CompletedTask;
//        }

//        bool actividadModalAgregarProyecto = false;

//        async Task ClickHandlerAgregarProyecto(bool activar)
//        {
//            actividadModalAgregarProyecto = activar;
//            if (!activar)
//            {
//                await AuthenticationStateProvider.GetAuthenticationStateAsync();
//                await ProyectosService.ObtenerListaDeProyectos(esquema);
//                if (ProyectosService.ListaProyectos != null)
//                {
//                    proyectos = ProyectosService.ListaProyectos;
//                }
//            }
//            StateHasChanged();
//        }
//    }
//}
