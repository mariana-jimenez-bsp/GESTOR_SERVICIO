using BSP.POS.DATOS.Usuarios;
using BSP.POS.UTILITARIOS.Permisos;
using BSP.POS.UTILITARIOS.Usuarios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Usuarios
{
    public class N_Usuarios
    {
        D_Login objetoLogin = new D_Login();
        D_Usuarios objetoUsuario = new D_Usuarios();
        public string Login(U_Login pLogin)
        {
            U_LoginToken login = new U_LoginToken();
            login = objetoLogin.Login(pLogin);
            string usuario = JsonConvert.SerializeObject(login);
            return usuario;
        }

        public string ValidarToken(string token)
        {
            string login;
            login = objetoLogin.ValidarToken(token);
            return login;
        }


        public U_LoginUsuario VerificarUsuarioAdministrador(string pEsquema, string pUsuario)
        {
            U_LoginUsuario login = new U_LoginUsuario();
            login = objetoLogin.VerificarUsuarioAdministrador(pEsquema, pUsuario);
            return login;
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

        public string ActualizarPerfil(U_Perfil pPerfil)
        {
            string mensaje = string.Empty;
            mensaje = objetoUsuario.ActualizarPerfil(pPerfil);
            return mensaje;
        }

        
    }
}
