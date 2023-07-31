using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Clientes;
using BSP.POS.UTILITARIOS.Informes;
using System;
using System.Collections.Generic;
using System.Linq;
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

                foreach (var item in response)
                {
                    U_ListaInformesAsociados informe = new U_ListaInformesAsociados(item.consecutivo, item.fecha_actualizacion, item.cliente, item.estado);

                    LstInformes.Add(informe);
                }
                if(LstInformes != null)
                {
                    return LstInformes;
                }
                return new List<U_ListaInformesAsociados>();

        }

        public U_InformeAsociado ObtenerInformeAsociado(String pEsquema, String pConsecutivo)
        {
            var informeAso = new U_InformeAsociado();

            ObtenerInformeAsociadoTableAdapter sp = new ObtenerInformeAsociadoTableAdapter();

            var response = sp.GetData(pEsquema, pConsecutivo).ToList();

                foreach (var item in response)
                {
                    U_InformeAsociado informe = new U_InformeAsociado(item.consecutivo, item.fecha_consultoria, item.hora_inicio, item.hora_final, item.modalidad_consultoria, item.cliente, item.estado);
                    informeAso = informe;
                }
            if (informeAso != null)
            {
                return informeAso;
            }
            return new U_InformeAsociado();

        }
    }
}
