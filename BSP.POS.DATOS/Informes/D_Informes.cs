using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Informes;
using BSP.POS.UTILITARIOS.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.DATOS.Informes
{
    public class D_Informes
    {
        public List<U_ListaInformesAsociados> ListaInformesAsociados(String pEsquema, String pCliente)
        {
            var LstInformes = new List<U_ListaInformesAsociados>();

            ListarInformesAsociadosTableAdapter sp = new ListarInformesAsociadosTableAdapter();

            var response = sp.GetData(pEsquema, pCliente).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ListaInformesAsociados informe = new U_ListaInformesAsociados(item.consecutivo, item.fecha_actualizacion, item.cliente, item.estado);

                    LstInformes.Add(informe);
                }
                return LstInformes;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public U_InformeAsociado ObtenerInformeAsociado(String pEsquema, String pConsecutivo)
        {
            var informeAso = new U_InformeAsociado();

            ObtenerInformeAsociadoTableAdapter sp = new ObtenerInformeAsociadoTableAdapter();

            var response = sp.GetData(pEsquema, pConsecutivo).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_InformeAsociado informe = new U_InformeAsociado(item.consecutivo, item.fecha_consultoria, item.hora_inicio, item.hora_final, item.modalidad_consultoria, item.cliente, item.estado);
                    informeAso = informe;
                }
                return informeAso;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarInformeAsociado(U_InformeAsociado pInforme, string esquema)
        {
            POSDataSet.ActualizarInformeAsociadoDataTable bTabla = new POSDataSet.ActualizarInformeAsociadoDataTable();
            ActualizarInformeAsociadoTableAdapter sp = new ActualizarInformeAsociadoTableAdapter();
            try
            {
                var response = sp.GetData(pInforme.consecutivo, pInforme.fecha_consultoria, pInforme.hora_inicio, pInforme.hora_final, pInforme.modalidad_consultoria, pInforme.cliente, pInforme.estado, esquema);

                
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public string CambiarEstadoDeInforme(U_InformeAsociado pInforme, string esquema)
        {
            POSDataSet.CambiarEstadoDeInformeDataTable bTabla = new POSDataSet.CambiarEstadoDeInformeDataTable();
            CambiarEstadoDeInformeTableAdapter sp = new CambiarEstadoDeInformeTableAdapter();
            try
            {
                var response = sp.GetData(pInforme.consecutivo, pInforme.estado, esquema);


                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
        public string EliminarInforme(string consecutivo, string esquema)
        {
            POSDataSet.EliminarInformeDataTable bTabla = new POSDataSet.EliminarInformeDataTable();
            EliminarInformeTableAdapter sp = new EliminarInformeTableAdapter();
            try
            {
                var response = sp.GetData(consecutivo, esquema);

                return "Exito";
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }



        }

        public U_TokenAprobacionInforme EnviarTokenDeAprobacionDeInforme(string pCodigo, string pEsquema)
        {
            GenerarTokenAprobacionDeInformeTableAdapter sp = new GenerarTokenAprobacionDeInformeTableAdapter();
            string token = GenerarTokenDeAprobacion();
            DateTime expira = DateTime.Now.AddDays(3);
            try
            {
                var response = sp.GetData(pCodigo, token, expira, pEsquema).ToList();
                U_TokenAprobacionInforme TokenAprobacion = new U_TokenAprobacionInforme();

                foreach (var item in response)
                {
                    U_TokenAprobacionInforme tokenAprobacion = new U_TokenAprobacionInforme(item.token_aprobacion, pEsquema, item.codigo_usuario_cliente, item.fecha_expiracion_TA.ToString());
                    TokenAprobacion = tokenAprobacion;
                }
                if (TokenAprobacion != null)
                {
                    return TokenAprobacion;
                }
                return new U_TokenAprobacionInforme();
            }
            catch (Exception)
            {

                return new U_TokenAprobacionInforme();
            }




        }
        public U_TokenAprobacionInforme ValidarTokenAprobacionInforme(String pEsquema, String pToken)
        {
            var tokenAprobacion = new U_TokenAprobacionInforme();

            ObtenerFechaTokenDeAprobacionInformeTableAdapter sp = new ObtenerFechaTokenDeAprobacionInformeTableAdapter();

            var response = sp.GetData(pEsquema, pToken).ToList();

            foreach (var item in response)
            {
                U_TokenAprobacionInforme tok = new U_TokenAprobacionInforme(item.token_aprobacion, pEsquema, "", item.fecha_expiracion_TA);
                tokenAprobacion = tok;
            }
            if (tokenAprobacion.token_aprobacion != null)
            {
                DateTime fechaAprobacion = DateTime.Parse(tokenAprobacion.fecha_expiracion);
                if (fechaAprobacion < DateTime.UtcNow)
                {

                    return new U_TokenAprobacionInforme();
                }
                else
                {
                    return tokenAprobacion;
                }

            }
            return new U_TokenAprobacionInforme();


        }

        public string AprobarInforme(U_TokenAprobacionInforme pInforme, string esquema)
        {
            POSDataSet.AprobarInformeDataTable bTabla = new POSDataSet.AprobarInformeDataTable();
            AprobarInformeTableAdapter sp = new AprobarInformeTableAdapter();
            try
            {
                var response = sp.GetData(pInforme.token_aprobacion, true, esquema);


                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
        public string GenerarTokenDeAprobacion()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }


    }
}
