using BSP.POS.DATOS.Informes;
using BSP.POS.UTILITARIOS.Informes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public U_TokenRecibidoInforme EnviarTokenDeRecibidoDeInforme(string pCodigo, string pConsecutivo, string pEsquema)
        {
            string token = GenerarTokenDeRecibido();
            U_TokenRecibidoInforme tokenAprobacion = new U_TokenRecibidoInforme();
            tokenAprobacion = objInforme.EnviarTokenDeRecibidoDeInforme(pCodigo, pConsecutivo, pEsquema, token);
            if (tokenAprobacion != null)
            {
                return tokenAprobacion;
            }
            return new U_TokenRecibidoInforme();

        }

        public string ValidarTokenRecibidoInforme(string pEsquema, string pToken)
        {

            U_TokenRecibidoInforme tokenRecibido = new U_TokenRecibidoInforme();
            tokenRecibido = objInforme.ValidarTokenRecibidoInforme(pEsquema, pToken);
            if (tokenRecibido != null)
            {
                string tokenRecibidoJson = JsonConvert.SerializeObject(tokenRecibido);
                return tokenRecibidoJson;
            }

            return JsonConvert.SerializeObject(new U_TokenRecibidoInforme());

        }

        public string ActivarRecibidoInforme(U_TokenRecibidoInforme pInforme, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objInforme.ActivarRecibidoInforme(pInforme, esquema);
            return mensaje;
        }
        public string AgregarInformeAsociado(string pCliente, string esquema)
        {
            string consecutivo = string.Empty;
            consecutivo = objInforme.AgregarInformeAsociado(pCliente, esquema);
            return consecutivo;
        }

        public string ValidarExistenciaConsecutivoInforme(string pEsquema, string pConsecutivo)
        {
            string consecutivo = objInforme.ValidarExistenciaConsecutivoInforme(pEsquema, pConsecutivo);
            return consecutivo;
        }
        public string GenerarTokenDeRecibido()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
