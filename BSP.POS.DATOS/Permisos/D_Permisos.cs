using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.DATOS.POSDataSetTableAdapters;

using BSP.POS.UTILITARIOS.Permisos;

namespace BSP.POS.DATOS.Permisos
{
    public class D_Permisos
    {
        public List<U_PermisosAsociados> ListaPermisosAsociados(String pEsquema, String pId_Usuario)
        {
            var LstPermisos = new List<U_PermisosAsociados>();

            ListarPermisosAsociadosTableAdapter sp = new ListarPermisosAsociadosTableAdapter();

            var response = sp.GetData(pEsquema, pId_Usuario).ToList();

                foreach (var item in response)
                {
                    U_PermisosAsociados permiso = new U_PermisosAsociados(item.Id, item.id_permiso);

                    LstPermisos.Add(permiso);
                }
                if(LstPermisos != null)
                {
                    return LstPermisos;
                }
                return new List<U_PermisosAsociados>();
            
        }

        public List<U_Permisos> ListaPermisos(String pEsquema)
        {
            var LstPermisos = new List<U_Permisos>();

            ListarPermisosTableAdapter sp = new ListarPermisosTableAdapter();

            var response = sp.GetData(pEsquema).ToList();

                foreach (var item in response)
                {
                    U_Permisos permiso = new U_Permisos(item.Id, item.permiso);

                    LstPermisos.Add(permiso);
                }
                if(LstPermisos != null)
                {
                    return LstPermisos;
                }
                return new List<U_Permisos>();

        }
        public string EliminarPermisosAsociadosAntiguos(string pIdUsuario, string esquema)
        {
            POSDataSet.EliminarPermisosAsociadosDataTable bTabla = new POSDataSet.EliminarPermisosAsociadosDataTable();
            EliminarPermisosAsociadosTableAdapter sp = new EliminarPermisosAsociadosTableAdapter();
            try
            {         
                    var response = sp.GetData(pIdUsuario, esquema);

                return "Exito";
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }



        }


        public string AgregarPermisosNuevosAsociados(List<U_PermisosAsociados> pPermisos, string pIdUsuaurio, string pEsquema)
        {
            try
            {
                AgregarPermisosAsociadosTableAdapter sp = new AgregarPermisosAsociadosTableAdapter();
                foreach (var permiso in pPermisos)
                {
                    var response = sp.GetData(pEsquema, int.Parse(pIdUsuaurio), int.Parse(permiso.id_permiso));
                }

                return "Exito";
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }
        }
    }
}
