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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Usuarios
{
    public class N_Usuarios
    {
        D_Usuarios objetoUsuario = new D_Usuarios();
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

        public string ObtenerUsuarioParaEditar(string pEsquema, string pCodigo)
        {
            try
            {
                U_UsuariosParaEditar usuario = new U_UsuariosParaEditar();

                usuario = objetoUsuario.ObtenerUsuarioParaEditar(pEsquema, pCodigo);

                string usuarioJson = JsonConvert.SerializeObject(usuario);
                return usuarioJson;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string ActualizarUsuario(U_UsuariosParaEditar pUsuario, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objetoUsuario.ActualizarUsuario(pUsuario, esquema);
            return mensaje;
        }

        public string ValidarExistenciaEsquema(string pEsquema)
        {
            string esquema = objetoUsuario.ValidarExistenciaEsquema(pEsquema);
            return esquema;
        }
    }
}
