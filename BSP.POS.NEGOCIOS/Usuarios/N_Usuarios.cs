using BSP.POS.DATOS.Usuarios;
using BSP.POS.UTILITARIOS.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Usuarios
{
    public class N_Login
    {
        D_Login objetoLogin = new D_Login();
        public U_LoginToken Login(U_Login pLogin)
        {
            U_LoginToken login = new U_LoginToken();
            login = objetoLogin.Login(pLogin);
            return login;
        }


        public U_LoginUsuario VerificarUsuarioAdministrador(string pEsquema, string pUsuario)
        {
            U_LoginUsuario login = new U_LoginUsuario();
            login = objetoLogin.VerificarUsuarioAdministrador(pEsquema, pUsuario);
            return login;
        }
    }
}
