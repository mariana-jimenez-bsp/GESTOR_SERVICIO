using BSP.POS.DATOS.Informes;
using BSP.POS.UTILITARIOS.Informes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Informes
{
    public class N_Informes
    {
        D_Informes objInforme = new D_Informes();
        public string ListarInformesAsociados(String pEsquema, String pCliente)
        {
            try
            {
                List<U_ListaInformesAsociados> list = new List<U_ListaInformesAsociados>();

                list = objInforme.ListaInformesAsociados(pEsquema, pCliente);

                string informe = JsonConvert.SerializeObject(list);
                return informe;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ObtenerInformeAsociado(String pEsquema, String pInforme)
        {
            try
            {
                U_InformeAsociado informeAso = new U_InformeAsociado();

                informeAso = objInforme.ObtenerInformeAsociado(pEsquema, pInforme);

                string informe = JsonConvert.SerializeObject(informeAso);
                return informe;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarInformeAsociado(U_InformeAsociado pInforme, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objInforme.ActualizarInformeAsociado(pInforme, esquema);
            return mensaje;
        }
        public string CambiarEstadoDeInforme(U_InformeAsociado pInforme, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objInforme.CambiarEstadoDeInforme(pInforme, esquema);
            return mensaje;
        }

        public string EliminarInforme(string consecutivo, string esquema)
        {
            try
            {
                string mensaje = string.Empty;
                mensaje = objInforme.EliminarInforme(consecutivo, esquema);

                return mensaje;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error " + ex.Message, ex.InnerException.InnerException);
            }


        }

        public U_TokenAprobacionInforme EnviarTokenDeAprobacionDeInforme(string pCodigo, string pEsquema)
        {

            U_TokenAprobacionInforme tokenAprobacion = new U_TokenAprobacionInforme();
            tokenAprobacion = objInforme.EnviarTokenDeAprobacionDeInforme(pCodigo, pEsquema);
            if (tokenAprobacion != null)
            {
                return tokenAprobacion;
            }
            return new U_TokenAprobacionInforme();

        }

    }
}
