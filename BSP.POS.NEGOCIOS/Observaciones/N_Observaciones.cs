using BSP.POS.DATOS.Observaciones;
using BSP.POS.UTILITARIOS.Observaciones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Observaciones
{
    public class N_Observaciones
    {
        D_Observaciones objObservacion = new D_Observaciones();

        public string ListarDatosObservacionesDeInforme(String pEsquema, String pConsecutivo)
        {
            try
            {
                List<U_DatosObservaciones> list = new List<U_DatosObservaciones>();

                list = objObservacion.ListarDatosObservacionesDeInforme(pConsecutivo, pEsquema);

                string observacionesJson = JsonConvert.SerializeObject(list);
                return observacionesJson;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string AgregarObservacionDeInforme(U_Observaciones pObservacion, string esquema)
        {
            string mensaje = string.Empty;
            mensaje = objObservacion.AgregarObservacionDeInforme(pObservacion, esquema);
            return mensaje;
        }
    }
}
