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
            try
            {
                foreach (var item in response)
                {
                    U_PermisosAsociados permiso = new U_PermisosAsociados(item.Id, item.id_permiso);

                    LstPermisos.Add(permiso);
                }
                return LstPermisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public List<U_Permisos> ListaPermisos(String pEsquema)
        {
            var LstPermisos = new List<U_Permisos>();

            ListarPermisosTableAdapter sp = new ListarPermisosTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_Permisos permiso = new U_Permisos(item.Id, item.permiso);

                    LstPermisos.Add(permiso);
                }
                return LstPermisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
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

        public List<U_DatosPermisosDeUsuarios> ListaDatosDePermisosDeUsuario(String pEsquema, String pCodigo)
        {
            var LstPermisos = new List<U_DatosPermisosDeUsuarios>();

            ListarDatosPermiso_UsuariosTableAdapter sp = new ListarDatosPermiso_UsuariosTableAdapter();

            var response = sp.GetData(pEsquema, pCodigo).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_DatosPermisosDeUsuarios permiso = new U_DatosPermisosDeUsuarios(item.Id, item.id_permiso, item.permiso);

                    LstPermisos.Add(permiso);
                }
                return LstPermisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public List<U_DatosSubPermisosDeUsuario> ListaDatosDeSubPermisosDeUsuario(String pEsquema, String pCodigo)
        {
            var LstPermisos = new List<U_DatosSubPermisosDeUsuario>();

            ListarDatosPermiso_Usuarios_SubPermisoTableAdapter sp = new ListarDatosPermiso_Usuarios_SubPermisoTableAdapter();

            var response = sp.GetData(pEsquema, pCodigo).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_DatosSubPermisosDeUsuario permiso = new U_DatosSubPermisosDeUsuario(item.Id, item.id_permiso_usuario, item.id_sub_permiso, item.sub_permiso);

                    LstPermisos.Add(permiso);
                }
                return LstPermisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
    }
}
