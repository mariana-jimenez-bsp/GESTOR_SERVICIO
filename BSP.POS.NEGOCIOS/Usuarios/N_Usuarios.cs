using BSP.POS.DATOS.Permisos;
using BSP.POS.DATOS.Usuarios;
using BSP.POS.UTILITARIOS.Permisos;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Usuarios
{
    public class N_Usuarios
    {
        D_Login objetoLogin = new D_Login();
        D_Usuarios objetoUsuario = new D_Usuarios();
        string usuarioJson;
        public string Login(U_Login pLogin)
        {
            U_LoginToken login = new U_LoginToken();
            string usuario = objetoLogin.ObtenerUsuarioPorCorreo(pLogin.esquema, pLogin.correo);
            string claveActual = objetoLogin.ConsultarClaveUsuario(pLogin, usuario);
            if (CompararClaves(pLogin.contrasena, claveActual))
            {
                D_Permisos datosPermisos = new D_Permisos();
                D_Usuarios datosUsuarios = new D_Usuarios();
                string rol = objetoLogin.ObtenerRol(pLogin.esquema, usuario);
                U_Perfil perfil = new U_Perfil();
                perfil = datosUsuarios.ObtenerPefil(pLogin.esquema, usuario);
                List<U_PermisosAsociados> permisos = new List<U_PermisosAsociados>();
                List<U_Permisos> todosLosPermisos = new List<U_Permisos>();
                todosLosPermisos = datosPermisos.ListaPermisos(pLogin.esquema);
                permisos = datosPermisos.ListaPermisosAsociados(pLogin.esquema, perfil.id);
                string token = GenerateJWT(usuario, pLogin.key, rol, permisos, todosLosPermisos, pLogin.esquema);
                login = objetoLogin.Login(pLogin, usuario, token);
                usuarioJson = JsonConvert.SerializeObject(login);
                return usuarioJson;
            }
            login = new U_LoginToken("", "", "", "");
            usuarioJson = JsonConvert.SerializeObject(login);
            return usuarioJson;
        }

        public string ValidarToken(string token, string esquema)
        {
            string login;
            login = objetoLogin.ValidarToken(token, esquema);
            if(login != null)
            {
                JwtSecurityToken tokenDecodificado = DecodificarToken(token);
                DateTime fecha = tokenDecodificado.ValidTo;
                if (tokenDecodificado.ValidTo < DateTime.UtcNow)
                {
                    return null;
                }
                else
                {
                    return token;
                }
            }
            return null;
        }


        public string ObtenerPerfil(String pEsquema, String pUsuario)
        {
            try
            {
                U_Perfil perfil = new U_Perfil();

                perfil = objetoUsuario.ObtenerPefil(pEsquema, pUsuario);

                string perf = JsonConvert.SerializeObject(perfil);
                return perf;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string ListarUsuarios(String pEsquema)
        {
            try
            {
                List<U_Perfil> list = new List<U_Perfil>();

                list = objetoUsuario.ListarUsuarios(pEsquema);

                string usuarios = JsonConvert.SerializeObject(list);
                return usuarios;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string ActualizarPerfil(U_Perfil pPerfil)
        {
            string mensaje = string.Empty;
            mensaje = objetoUsuario.ActualizarPerfil(pPerfil);
            return mensaje;
        }

        public string ListarUsuariosDeClienteAsociados(String pEsquema, string pCliente)
        {
            try
            {
                List<U_ListaDeUsuariosDeCliente> list = new List<U_ListaDeUsuariosDeCliente>();

                list = objetoUsuario.ListaDeUsuariosDeClienteAsociados(pEsquema, pCliente);

                string usuarios = JsonConvert.SerializeObject(list);
                return usuarios;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public U_TokenRecuperacion EnviarTokenRecuperacion(string pCorreo, string pEsquema)
        {

                U_TokenRecuperacion tokenRecuperacion = new U_TokenRecuperacion();
                tokenRecuperacion = objetoUsuario.EnviarTokenRecuperacion(pCorreo, pEsquema);
                if (tokenRecuperacion != null)
                {
                    return tokenRecuperacion;
                }
                return new U_TokenRecuperacion();

        }

        public string ValidarTokenRecuperacion(string pEsquema, string pToken)
        {

                U_TokenRecuperacion tokenRecuperacion = new U_TokenRecuperacion();
                tokenRecuperacion = objetoUsuario.ValidarTokenRecuperacion(pEsquema, pToken);
                if (tokenRecuperacion != null)
                {
                string tokenRecuperacionJson = JsonConvert.SerializeObject(tokenRecuperacion);
                return tokenRecuperacionJson;
                }

                return JsonConvert.SerializeObject(new U_TokenRecuperacion());

        }

        public string ActualizarClaveDeUsuario(U_UsuarioNuevaClave pUsuario)
        {
            string mensaje = string.Empty;
            mensaje = objetoUsuario.ActualizarClaveDeUsuario(pUsuario);
            return mensaje;
        }

        public string ValidarCorreoExistente(string pEsquema, string pCorreo)
        {
            string correo = objetoUsuario.ValidarCorreoExistente(pEsquema, pCorreo);
            return correo;
        }
        public string ValidarUsuarioExistente(string pEsquema, string pUsuario)
        {
            string usuario = objetoUsuario.ValidarUsuarioExistente(pEsquema, pUsuario);
            return usuario;
        }
        public string ListarUsuariosDeClienteDeInforme(String pEsquema, String pConsecutivo)
        {
            try
            {
                List<U_UsuariosDeClienteDeInforme> list = new List<U_UsuariosDeClienteDeInforme>();

                list = objetoUsuario.ListaUsuariosDeClienteDeInforme(pEsquema, pConsecutivo);

                string informe = JsonConvert.SerializeObject(list);
                return informe;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string AgregarUsuarioDeClienteDeInforme(U_UsuariosDeClienteDeInforme pUsuario, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objetoUsuario.AgregarUsuarioDeClienteDeInforme(pUsuario, esquema);
            return mensaje;
        }

        public string EliminarUsuarioDeClienteDeInforme(string pIdUsuario, string esquema)
        {
            try
            {
                string mensaje = string.Empty;
                mensaje = objetoUsuario.EliminarUsuarioDeClienteDeInforme(pIdUsuario, esquema);
               
                return mensaje;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error " + ex.Message, ex.InnerException.InnerException);
            }


        }
        public U_ListaDeUsuariosDeCliente ObtenerUsuarioDeClientePorCodigo(String pEsquema, String pCodigo)
        {

                U_ListaDeUsuariosDeCliente usuario = new U_ListaDeUsuariosDeCliente();

                usuario = objetoUsuario.ObtenerUsuarioDeClientePorCodigo(pEsquema, pCodigo);

                if(usuario != null)
                {
                    return usuario;
                }

            return new U_ListaDeUsuariosDeCliente();

        }

        public string ObtenerImagenDeUsuario(String pEsquema, String pUsuario)
        {
            try
            {
                U_ImagenUsuario imagenUsuario = new U_ImagenUsuario();

                imagenUsuario = objetoUsuario.ObtenerImagenDeUsuario(pEsquema, pUsuario);

                string imagen = JsonConvert.SerializeObject(imagenUsuario);
                return imagen;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ObtenerListaDeInformesDeUsuarioDeInforme(String pEsquema, String pCodigo)
        {
            try
            {
                List<U_UsuariosDeClienteDeInforme> list = new List<U_UsuariosDeClienteDeInforme>();

                list = objetoUsuario.ObtenerListaDeInformesDeUsuario(pEsquema, pCodigo);

                string informe = JsonConvert.SerializeObject(list);
                return informe;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ListarUsuariosParaEditar(String pEsquema)
        {
            try
            {
                List<U_UsuariosParaEditar> list = new List<U_UsuariosParaEditar>();

                list = objetoUsuario.ListarUsuariosParaEditar(pEsquema);

                string usuarios = JsonConvert.SerializeObject(list);
                return usuarios;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarListaDeUsuarios(List<U_UsuariosParaEditar> pUsuarios, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objetoUsuario.ActualizarListaDeUsuarios(pUsuarios, esquema);
            return mensaje;
        }
        public string AgregarUsuario(U_UsuariosParaEditar pUsuario, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objetoUsuario.AgregarUsuario(pUsuario, esquema);
            return mensaje;
        }

        public bool CompararClaves(string claveIngresada, string claveGuardada)
        {

            return claveIngresada == claveGuardada;

        }

        private string GenerateJWT(string username, string key, string rol, List<U_PermisosAsociados> permisos, List<U_Permisos> todoLosPermisos, string esquema)
        {
            // Also consider using AsymmetricSecurityKey if you want the client to be able to validate the token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
            if (!string.IsNullOrEmpty(rol))
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
            if (!string.IsNullOrEmpty(esquema))
            {
                claims.Add(new Claim("esquema", esquema));
            }
            if (todoLosPermisos != null)
            {
                foreach (var permiso in todoLosPermisos)
                {
                    if (permisos.Any(p => p.id_permiso == permiso.Id))
                    {
                        claims.Add(new Claim("permission", permiso.permiso));
                    }
                }
            }

            var token = new JwtSecurityToken(
                "BSP",
                "Usuarios",
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public JwtSecurityToken DecodificarToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Decodificar el token
            JwtSecurityToken tokenDecodificado = tokenHandler.ReadJwtToken(token);

            return tokenDecodificado;
        }
    }
}
