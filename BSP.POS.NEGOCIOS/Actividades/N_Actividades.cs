using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.Actividades;
using BSP.POS.DATOS.Actividades;
using Newtonsoft.Json;

namespace BSP.POS.NEGOCIOS.Actividades
{
    public class N_Actividades
    {
        D_Actividades objActividad = new D_Actividades();

        public string ListarActividadesAsociadas(String pEsquema, String pConsecutivo)
        {
            try
            {
                List<U_ListaActividadesAsociadas> list = new List<U_ListaActividadesAsociadas>();

                list = objActividad.ListaActividadesAsociadas(pEsquema, pConsecutivo);

                string actividades = JsonConvert.SerializeObject(list);
                return actividades;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ListarActividades(String pEsquema)
        {
            try
            {
                List<U_ListaActividades> list = new List<U_ListaActividades>();

                list = objActividad.ListaActividades(pEsquema);

                string actividades = JsonConvert.SerializeObject(list);
                return actividades;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ListarActividadesActivas(String pEsquema)
        {
            try
            {
                List<U_ListaActividades> list = new List<U_ListaActividades>();

                list = objActividad.ListaActividadesActivas(pEsquema);

                string actividades = JsonConvert.SerializeObject(list);
                return actividades;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string ListarActividadesPorUsuario(String pEsquema, string pCodigo)
        {
            try
            {
                List<U_ListaActividades> list = new List<U_ListaActividades>();

                list = objActividad.ListaActividadesPorUsuario(pEsquema, pCodigo);

                string actividades = JsonConvert.SerializeObject(list);
                return actividades;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ListarActividadesActivasPorUsuario(String pEsquema, string pCodigo)
        {
            try
            {
                List<U_ListaActividades> list = new List<U_ListaActividades>();

                list = objActividad.ListaActividadesActivasPorUsuario(pEsquema, pCodigo);

                string actividades = JsonConvert.SerializeObject(list);
                return actividades;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string ActualizarListaDeActividades(List<U_ListaActividades> pActividades, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objActividad.ActualizarListaDeActividades(pActividades, esquema);
            return mensaje;
        }
        public string ActualizarListaDeActividadesAsociadas(List<U_ListaActividadesAsociadas> pActividades, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objActividad.ActualizarListaDeActividadesAsociadas(pActividades, esquema);
            return mensaje;
        }

        public string AgregarActividadDeInforme(U_ListaActividadesAsociadas pActividad, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objActividad.AgregarActividadDeInforme(pActividad, esquema);
            return mensaje;
        }

        public string EliminarActividadDeInforme(string pIdActividad, string esquema)
        {
            try
            {
                string mensaje = string.Empty;
                mensaje = objActividad.EliminarActividadDeInforme(pIdActividad, esquema);

                return mensaje;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error " + ex.Message, ex.InnerException.InnerException);
            }


        }

        public string AgregarActividad(U_ListaActividades pActividad, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objActividad.AgregarActividad(pActividad, esquema);
            return mensaje;
        }

        public void EliminarActividad(string esquema, string codigo)
        {
            objActividad.EliminarActividad(esquema, codigo);
        }
        public void CambiarEstadoActividad(string esquema, string codigo, string estado)
        {
            objActividad.CambiarEstadoActividad(esquema, codigo, estado);
        }
        public string ValidarActivadAsociadaInforme(string esquema, string codigo)
        {
            string resultado = objActividad.ValidarActividadAsociadaInforme(esquema, codigo);
            return resultado;
        }
    }
}
