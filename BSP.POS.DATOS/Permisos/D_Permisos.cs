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
            try
            {
                var response = sp.GetData(pEsquema, pCodigo).ToList();
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

        public List<U_Permisos> ObtenerListaDePermisos(String pEsquema)
        {
            var LstPermisos = new List<U_Permisos>();

            ObtenerListaPermisosTableAdapter sp = new ObtenerListaPermisosTableAdapter();
            try
            {
                var response = sp.GetData(pEsquema).ToList();
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

        public List<U_SubPermisos> ObtenerListaDeSubPermisos(String pEsquema)
        {
            var LstSubPermisos = new List<U_SubPermisos>();

            ObtenerListaSubPermisosTableAdapter sp = new ObtenerListaSubPermisosTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_SubPermisos subpermiso = new U_SubPermisos(item.Id, item.sub_permiso);

                    LstSubPermisos.Add(subpermiso);
                }
                return LstSubPermisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string EliminarPermisosDeUsuario(string pCodigo, string esquema)
        {
            POSDataSet.EliminarPermisosDeUsuarioDataTable bTabla = new POSDataSet.EliminarPermisosDeUsuarioDataTable();
            EliminarPermisosDeUsuarioTableAdapter sp = new EliminarPermisosDeUsuarioTableAdapter();
            try
            {
                var response = sp.GetData(pCodigo, esquema);
                return "Exito";
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }



        }

        public string EliminarSubPermisosDeUsuario(string pCodigo, string esquema)
        {
            POSDataSet.EliminarSubPermisosDeUsuarioDataTable bTabla = new POSDataSet.EliminarSubPermisosDeUsuarioDataTable();
            EliminarSubPermisosDeUsuarioTableAdapter sp = new EliminarSubPermisosDeUsuarioTableAdapter();
            try
            {
                var response = sp.GetData(pCodigo, esquema);

                return "Exito";
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }

        }

        public string AgregarPermisoDeUsuario(string IdPermiso, string Codigo, string pEsquema)
        {
            try
            {
                AgregarPermisoDeUsuarioTableAdapter sp = new AgregarPermisoDeUsuarioTableAdapter();
                var response = sp.GetData(pEsquema, Codigo, int.Parse(IdPermiso));
                string Id = "";
                foreach (var item in response)
                {
                    Id = item.Id;
                }

                return Id;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }
        }

        public string AgregarSubPermisoDeUsuario(string IdPermisoUsuario, string IdSupPermiso, string pEsquema)
        {
            try
            {
                AgregarSubPermisoDeUsuarioTableAdapter sp = new AgregarSubPermisoDeUsuarioTableAdapter();
                var response = sp.GetData(pEsquema, int.Parse(IdPermisoUsuario), int.Parse(IdSupPermiso));

                return "Exito";
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }
        }
    }
}
