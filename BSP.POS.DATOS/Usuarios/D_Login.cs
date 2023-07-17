using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.DATOS.Usuarios
{
    public class D_Login
    {
        public U_Login Login(U_Login pLogin)
        {
            POSDataSet.LoginUsuarioDataTable _tabla = new POSDataSet.LoginUsuarioDataTable();
            LoginUsuarioTableAdapter _tablaUsuario = new LoginUsuarioTableAdapter();

            U_Login login = null;

            var j = _tablaUsuario.GetData(pLogin.usuario, pLogin.contrasena, pLogin.esquema).ToList();

            foreach (POSDataSet.LoginUsuarioRow item in j)
            {
                login = new U_Login(item.USUARIO, item.CLAVE, item.ROL, item.ESQUEMA);
            }
            if (j.Count == 0)
            {
                login = new U_Login("", "", "", "");
            }

            return login;
        }


        public U_LoginUsuario VerificarUsuarioAdministrador(string pEsquema, string pUsuario)
        {
            VerificarUsuarioConsultorTableAdapter _tablaUsuario = new VerificarUsuarioConsultorTableAdapter();

            U_LoginUsuario login = null;

            var result = _tablaUsuario.GetData(pEsquema, pUsuario).ToList();

            foreach (POSDataSet.VerificarUsuarioConsultorRow item in result)
            {
                login = new U_LoginUsuario(item.USUARIO, item.CLAVE, "");
            }
            if (result.Count == 0)
            {
                login = new U_LoginUsuario("", "", "");
            }
            return login;
        }


    }
}
