using BSP.POS.DATOS.Tiempos;
using BSP.POS.UTILITARIOS.Tiempos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Tiempos
{
    public class N_Tiempos
    {
        D_Tiempos objTiempo = new D_Tiempos();
        public string ListarTiempos(String pEsquema)
        {
            try
            {
                List<U_ListaTiempos> list = new List<U_ListaTiempos>();

                list = objTiempo.ListaTiempos(pEsquema);

                string tiempos = JsonConvert.SerializeObject(list);
                return tiempos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarListaDeTiempos(List<U_ListaTiempos> pTiempos)
        {
            string mensaje = string.Empty;
            mensaje = objTiempo.ActualizarListaDeTiempos(pTiempos);
            return mensaje;
        }
    }
}
