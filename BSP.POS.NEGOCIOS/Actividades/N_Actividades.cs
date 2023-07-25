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
    }
}
