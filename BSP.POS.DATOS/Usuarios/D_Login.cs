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

        public U_LoginToken Login(U_Login pLogin, string token)
        {
            POSDataSet.RealizarLoginDataTable _tabla = new POSDataSet.RealizarLoginDataTable();
            RealizarLoginTableAdapter _tablaUsuario = new RealizarLoginTableAdapter();

            U_LoginToken login = null;

                var j = _tablaUsuario.GetData(pLogin.correo, pLogin.contrasena, pLogin.esquema, token).ToList();
                foreach (POSDataSet.RealizarLoginRow item in j)
                {
                    login = new U_LoginToken(item.token, item.esquema, item.correo);
                }
                if (j.Count == 0)
                {
                    login = new U_LoginToken("", "", "");
                }


            return login;
        }

        public string ConsultarClaveUsuario(U_Login pLogin)
        {
            ObtenerClaveUsuarioTableAdapter _claveUsuario = new ObtenerClaveUsuarioTableAdapter();
            var consultaClave = _claveUsuario.GetData(pLogin.esquema, pLogin.usuario);
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
        public string ActualizarClaveDeUsuario(U_UsuarioNuevaClave pUsuario)
        {
            POSDataSet.ActualizarClaveDeUsuarioDataTable bTabla = new POSDataSet.ActualizarClaveDeUsuarioDataTable();
            ActualizarClaveDeUsuarioTableAdapter sp = new ActualizarClaveDeUsuarioTableAdapter();
            try
            {
                var response = sp.GetData(pUsuario.token_recuperacion, pUsuario.clave, pUsuario.esquema);


                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public U_TokenRecuperacion EnviarTokenRecuperacion(string pCorreo, string pEsquema, string token, DateTime expira)
        {
            GenerarTokenRecuperacionTableAdapter sp = new GenerarTokenRecuperacionTableAdapter();

            try
            {
                var response = sp.GetData(pCorreo, token, expira, pEsquema).ToList();
                U_TokenRecuperacion TokenRecuperacion = new U_TokenRecuperacion();

                foreach (var item in response)
                {
                    U_TokenRecuperacion tokeRecuperacion = new U_TokenRecuperacion(item.token_recuperacion, pEsquema, item.correo, item.fecha_expiracion_TR.ToString());
                    TokenRecuperacion = tokeRecuperacion;
                }
                if (TokenRecuperacion != null)
                {
                    return TokenRecuperacion;
                }
                return new U_TokenRecuperacion();
            }
            catch (Exception)
            {

                return new U_TokenRecuperacion();
            }




        }

        public U_TokenRecuperacion ValidarTokenRecuperacion(String pEsquema, String pToken)
        {
            var tokenRecuperacion = new U_TokenRecuperacion();

            ObtenerFechaTokenDeRecuperacionTableAdapter sp = new ObtenerFechaTokenDeRecuperacionTableAdapter();

            var response = sp.GetData(pEsquema, pToken).ToList();

            foreach (var item in response)
            {
                U_TokenRecuperacion tok = new U_TokenRecuperacion(item.token_recuperacion, pEsquema, "", item.fecha_expiracion_TR);
                tokenRecuperacion = tok;
            }
            if (tokenRecuperacion.token_recuperacion != null)
            {
                DateTime fechaRecuperacion = DateTime.Parse(tokenRecuperacion.fecha_expiracion);
                if (fechaRecuperacion < DateTime.Now)
                {

                    return new U_TokenRecuperacion();
                }
                else
                {
                    return tokenRecuperacion;
                }

            }
            return new U_TokenRecuperacion();


        }
        public string AumentarIntentosDeLogin(string esquema, string correo)
        {
            AumentarIntentosLoginTableAdapter sp = new AumentarIntentosLoginTableAdapter();
            try
            {
                var response = sp.GetData(correo, esquema).ToList();
                return "Éxito";
            }
            catch (Exception ex)
            {

                return "Error: " + ex.Message;
            }
            

        }

        public int ObtenerIntentosDeLogin(string esquema, string correo)
        {
            ObtenerIntentosDeLoginTableAdapter sp = new ObtenerIntentosDeLoginTableAdapter();
                int intentos = 0;
                var response = sp.GetData(esquema, correo).ToList();
                foreach(var item in response)
                {
                    intentos = item.maximo_intentos;
                }
                return intentos;
           

        }

    }
}
