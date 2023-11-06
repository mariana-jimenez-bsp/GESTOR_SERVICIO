using BSP.POS.DATOS.Permisos;

using BSP.POS.UTILITARIOS.Permisos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Permisos
{
    public class N_Permisos
    {
        D_Permisos objetoPermiso = new D_Permisos();

        public string ListarPermisosAsociados(String pEsquema, String pId_Usuario)
        {
            try
            {
                List<U_PermisosAsociados> list = new List<U_PermisosAsociados>();

                list = objetoPermiso.ListaPermisosAsociados(pEsquema, pId_Usuario);

                string permisos = JsonConvert.SerializeObject(list);
                return permisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ListarPermisos(String pEsquema)
        {
            try
            {
                List<U_Permisos> list = new List<U_Permisos>();

                list = objetoPermiso.ListaPermisos(pEsquema);

                string permisos = JsonConvert.SerializeObject(list);
                return permisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarPermisosAsociados(List<U_PermisosAsociados> pPermisos, string pIdUsuario, string esquema)
        {
            try
            {
                string mensaje = string.Empty;
                mensaje = objetoPermiso.EliminarPermisosAsociadosAntiguos(pIdUsuario, esquema);
                mensaje = objetoPermiso.AgregarPermisosNuevosAsociados(pPermisos,pIdUsuario, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error " + ex.Message, ex.InnerException.InnerException);
            }
            
           
        }

        public string ObtenerListaDePermisos(String pEsquema)
        {
            try
            {
                List<U_Permisos> list = new List<U_Permisos>();

                list = objetoPermiso.ObtenerListaDePermisos(pEsquema);

                string permisos = JsonConvert.SerializeObject(list);
                return permisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string ObtenerListaDePermisosDeUsuario(String pEsquema, string pCodigo)
        {
            try
            {
                List<U_DatosPermisosDeUsuarios> list = new List<U_DatosPermisosDeUsuarios>();

                list = objetoPermiso.ListaDatosDePermisosDeUsuario(pEsquema, pCodigo);

                string permisos = JsonConvert.SerializeObject(list);
                return permisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ObtenerListaDeSubPermisos(String pEsquema)
        {
            try
            {
                List<U_SubPermisos> list = new List<U_SubPermisos>();

                list = objetoPermiso.ObtenerListaDeSubPermisos(pEsquema);

                string permisos = JsonConvert.SerializeObject(list);
                return permisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ObtenerListaDeSubPermisosDeUsuario(String pEsquema, string pCodigo)
        {
            try
            {
                List<U_DatosSubPermisosDeUsuario> list = new List<U_DatosSubPermisosDeUsuario>();

                list = objetoPermiso.ListaDatosDeSubPermisosDeUsuario(pEsquema, pCodigo);

                string permisos = JsonConvert.SerializeObject(list);
                return permisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
    }
}
