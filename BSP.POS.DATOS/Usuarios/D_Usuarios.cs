using BSP.POS.DATOS.POSDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.Usuarios;
using BSP.POS.UTILITARIOS.Tiempos;

namespace BSP.POS.DATOS.Usuarios
{
    public class D_Usuarios
    {
        public U_Perfil ObtenerPefil(String pEsquema, String pUsuario)
        {
            var perfil = new U_Perfil();

            ObtenerUsuarioPorNombreTableAdapter sp = new ObtenerUsuarioPorNombreTableAdapter();

            var response = sp.GetData(pEsquema, pUsuario).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_Perfil perf = new U_Perfil(item.ID, item.USUARIO, item.CORREO, item.CLAVE, item.NOMBRE, item.ROL);
                    perfil = perf;
                }
                return perfil;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarPerfil(U_Perfil pPerfil)
        {
            POSDataSet.ActualizarPerfilDataTable bTabla = new POSDataSet.ActualizarPerfilDataTable();
            ActualizarPerfilTableAdapter sp = new ActualizarPerfilTableAdapter();
            try
            {
                if(pPerfil.clave == null)
                {
                    U_Perfil perf= ObtenerPefil("BSP", pPerfil.usuario);
                    pPerfil.clave = perf.clave;
                }
                    var response = sp.GetData(pPerfil.id, pPerfil.usuario, pPerfil.correo, pPerfil.clave, pPerfil.nombre, pPerfil.rol);

                
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
    }
}
