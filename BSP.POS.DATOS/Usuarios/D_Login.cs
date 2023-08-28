using BSP.POS.DATOS.Permisos;
using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Permisos;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BSP.POS.DATOS.Usuarios
{
    public class D_Login
    {

        public U_LoginToken Login(U_Login pLogin, string usuario, string token)
        {
            POSDataSet.LoginUsuarioDataTable _tabla = new POSDataSet.LoginUsuarioDataTable();
            LoginUsuarioTableAdapter _tablaUsuario = new LoginUsuarioTableAdapter();

            U_LoginToken login = null;

                var j = _tablaUsuario.GetData(pLogin.correo, pLogin.contrasena, pLogin.esquema, token).ToList();
                foreach (POSDataSet.LoginUsuarioRow item in j)
                {
                    login = new U_LoginToken(item.token, item.esquema, usuario, item.correo);
                }
                if (j.Count == 0)
                {
                    login = new U_LoginToken("", "", "", "");
                }


            return login;
        }

        public string ConsultarClaveUsuario(U_Login pLogin, string usuario)
        {
            ObtenerClaveUsuarioTableAdapter _claveUsuario = new ObtenerClaveUsuarioTableAdapter();
            var consultaClave = _claveUsuario.GetData(pLogin.esquema, usuario);
            string claveActual = string.Empty;
            foreach (POSDataSet.ObtenerClaveUsuarioRow item in consultaClave)
            {
                claveActual = item.clave;
            }
            return claveActual;
        }
        

        public string ValidarToken(string token, string esquema)
        {
            ObtenerUsuarioPorTokenTableAdapter _usuario = new ObtenerUsuarioPorTokenTableAdapter();
            var consultaUsuario = _usuario.GetData(esquema, token);
            string usuario = null;
            foreach (POSDataSet.ObtenerUsuarioPorTokenRow item in consultaUsuario)
            {
                usuario = item.usuario;
            }
            if (usuario == null)
            {
                return null;
            }
            else
            {
                return token;
            }

        }
        public string ObtenerUsuarioPorCorreo(String pEsquema, String pCorreo)
        {
            string usuario = string.Empty;

            ObtenerUsuarioPorCorreoTableAdapter sp = new ObtenerUsuarioPorCorreoTableAdapter();

            var response = sp.GetData(pEsquema, pCorreo).ToList();

            foreach (var item in response)
            {
                string u = item.usuario;
                usuario = u;
            }
            if (usuario != null)
            {
                return usuario;
            }
            return string.Empty;

        }
        public string ObtenerRol(String pEsquema, String pUsuario)
        {
            string rol = string.Empty;

            ObtenerRolDeUsuarioTableAdapter sp = new ObtenerRolDeUsuarioTableAdapter();

            var response = sp.GetData(pEsquema, pUsuario).ToList();

            foreach (var item in response)
            {
                string r = item.rol;
                rol = r;
            }
            if (rol != null)
            {
                return rol;
            }
            return string.Empty;

        }


    }
}
