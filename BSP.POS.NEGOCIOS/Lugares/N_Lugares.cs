using BSP.POS.DATOS.Lugares;
using BSP.POS.UTILITARIOS.Lugares;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Lugares
{
    public class N_Lugares
    {
        private D_Lugares objLugares = new D_Lugares();
        public string ObtenerPaises(string pEsquema)
        {
                var Lst = new List<U_Lugares>();
                Lst = objLugares.ObtenerPaises(pEsquema);
                string LstJson = JsonConvert.SerializeObject(Lst);
                return LstJson;
        }

        public string ObtenerProvinciasPorPais(string pEsquema, string pPais)
        {
            var Lst = new List<U_Lugares>();
            Lst = objLugares.ObtenerProvinciasPorPais(pEsquema, pPais);
            string LstJson = JsonConvert.SerializeObject(Lst);
            return LstJson;
        }

        public string ObtenerCantonesPorProvincia(string pEsquema, string pPais, string pProvincia)
        {
            var Lst = new List<U_Lugares>();
            Lst = objLugares.ObtenerCantonesPorProvincia(pEsquema, pPais, pProvincia);
            string LstJson = JsonConvert.SerializeObject(Lst);
            return LstJson;
        }

        public string ObtenerDistritosPorCanton(string pEsquema, string pPais, string pProvincia, string pCanton)
        {
            var Lst = new List<U_Lugares>();
            Lst = objLugares.ObtenerDistritosPorCanton(pEsquema, pPais, pProvincia, pCanton);
            string LstJson = JsonConvert.SerializeObject(Lst);
            return LstJson;
        }

        public string ObtenerBarriosPorDistrito(string pEsquema, string pPais, string pProvincia, string pCanton, string pDistrito)
        {
            var Lst = new List<U_Lugares>();
            Lst = objLugares.ObtenerBarriosPorDistrito(pEsquema, pPais, pProvincia, pCanton, pDistrito);
            string LstJson = JsonConvert.SerializeObject(Lst);
            return LstJson;
        }
    }
}
