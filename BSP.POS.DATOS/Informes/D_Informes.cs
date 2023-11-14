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
        public List<U_InformesDeProyecto> ListaInformesDeProyecto(String pEsquema, String pNumero)
        {
            var LstInformes = new List<U_InformesDeProyecto>();

            ListarInformesDeProyectoTableAdapter sp = new ListarInformesDeProyectoTableAdapter();
            var response = sp.GetData(pEsquema, pNumero).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_InformesDeProyecto informe = new U_InformesDeProyecto(item.consecutivo, item.fecha_actualizacion, item.fecha_consultoria, item.numero_proyecto, item.estado);

                    LstInformes.Add(informe);
                }
                return LstInformes;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public List<U_InformesDeProyecto> ListaInformes(String pEsquema)
        {
            var LstInformes = new List<U_InformesDeProyecto>();

            ListarInformesTableAdapter sp = new ListarInformesTableAdapter();
            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_InformesDeProyecto informe = new U_InformesDeProyecto(item.consecutivo, item.fecha_actualizacion, item.fecha_consultoria, item.numero_proyecto, item.estado);

                    LstInformes.Add(informe);
                }
                return LstInformes;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public List<U_InformesDeProyecto> ListaInformesDeCliente(String pEsquema, String pCliente)
        {
            var LstInformes = new List<U_InformesDeProyecto>();

            ObtenerListaDeInformesDeClienteTableAdapter sp = new ObtenerListaDeInformesDeClienteTableAdapter();
            var response = sp.GetData(pEsquema, pCliente).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_InformesDeProyecto informe = new U_InformesDeProyecto(item.consecutivo, item.fecha_actualizacion, item.fecha_consultoria, item.numero_proyecto, item.estado);

                    LstInformes.Add(informe);
                }
                return LstInformes;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public U_Informe ObtenerInforme(String pEsquema, String pConsecutivo)
        {
            var informeAso = new U_Informe();

            ObtenerInformeTableAdapter sp = new ObtenerInformeTableAdapter();

            var response = sp.GetData(pEsquema, pConsecutivo).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_Informe informe = new U_Informe(item.consecutivo, item.fecha_consultoria, item.hora_inicio, item.hora_final, item.modalidad_consultoria, item.numero_proyecto, item.estado);
                    informeAso = informe;
                }
                return informeAso;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarInforme(U_Informe pInforme, string esquema)
        {
            POSDataSet.ActualizarInformeDataTable bTabla = new POSDataSet.ActualizarInformeDataTable();
            ActualizarInformeTableAdapter sp = new ActualizarInformeTableAdapter();
            try
            {
                var response = sp.GetData(pInforme.consecutivo, pInforme.fecha_consultoria, pInforme.hora_inicio, pInforme.hora_final, pInforme.modalidad_consultoria, pInforme.numero_proyecto, pInforme.estado, esquema);

                
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public string CambiarEstadoDeInforme(U_Informe pInforme, string esquema)
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

        public U_TokenRecibidoInforme EnviarTokenDeRecibidoDeInforme(string pCodigo, string pConsecutivo, string pEsquema, string token)
        {
            GenerarTokenRecibidoDeInformeTableAdapter sp = new GenerarTokenRecibidoDeInformeTableAdapter();
           
            DateTime expira = DateTime.Now.AddMinutes(15);
            try
            {
                var response = sp.GetData(pCodigo, pConsecutivo, token, expira, pEsquema).ToList();
                U_TokenRecibidoInforme TokenRecibido = new U_TokenRecibidoInforme();

                foreach (var item in response)
                {
                    U_TokenRecibidoInforme tokenRecibido = new U_TokenRecibidoInforme(item.token_recibido, pEsquema, item.codigo_usuario, item.fecha_expiracion_TA.ToString());
                    TokenRecibido = tokenRecibido;
                }
                if (TokenRecibido != null)
                {
                    return TokenRecibido;
                }
                return new U_TokenRecibidoInforme();
            }
            catch (Exception)
            {

                return new U_TokenRecibidoInforme();
            }




        }
        public U_TokenRecibidoInforme ValidarTokenRecibidoInforme(String pEsquema, String pToken)
        {
            var tokenRecibido = new U_TokenRecibidoInforme();

            ObtenerFechaTokenDeRecibidoInformeTableAdapter sp = new ObtenerFechaTokenDeRecibidoInformeTableAdapter();

            var response = sp.GetData(pEsquema, pToken).ToList();

            foreach (var item in response)
            {
                U_TokenRecibidoInforme tok = new U_TokenRecibidoInforme(item.token_recibido, pEsquema, "", item.fecha_expiracion_TA);
                tokenRecibido = tok;
            }
            if (tokenRecibido.token_recibido != null)
            {
                DateTime fechaAprobacion = DateTime.Parse(tokenRecibido.fecha_expiracion);
                if (fechaAprobacion < DateTime.Now)
                {

                    return new U_TokenRecibidoInforme();
                }
                else
                {
                    return tokenRecibido;
                }

            }
            return new U_TokenRecibidoInforme();


        }

        public string ActivarRecibidoInforme(U_TokenRecibidoInforme pInforme, string esquema)
        {
            POSDataSet.ActivarRecibidoInformeDataTable bTabla = new POSDataSet.ActivarRecibidoInformeDataTable();
            ActivarRecibidoInformeTableAdapter sp = new ActivarRecibidoInformeTableAdapter();
            try
            {
                var response = sp.GetData(pInforme.token_recibido, true, esquema);


                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
        

        public string AgregarInformeAsociado(string pNumero, string esquema)
        {
            POSDataSet.AgregarInformeAsociadoDataTable bTabla = new POSDataSet.AgregarInformeAsociadoDataTable();
            AgregarInformeAsociadoTableAdapter sp = new AgregarInformeAsociadoTableAdapter();
            try
            {
                var response = sp.GetData(pNumero, esquema).ToList();
                string consecutivo = string.Empty;
                foreach (var item in response)
                {
                    consecutivo = item.consecutivo;
                }
                if (!string.IsNullOrEmpty(consecutivo))
                {
                    return consecutivo;
                }
                return "";
            }
            catch (Exception)
            {

                return "";
            }



        }

        public string ValidarExistenciaConsecutivoInforme(string pEsquema, string pConsecutivo)
        {
            ValidarExistenciaConsecutivoInformeTableAdapter sp = new ValidarExistenciaConsecutivoInformeTableAdapter();
            string consecutivo = null;

            var response = sp.GetData(pEsquema, pConsecutivo).ToList();

            foreach (var item in response)
            {
                consecutivo = item.consecutivo;
            }

            return consecutivo;
        }

    }
}
