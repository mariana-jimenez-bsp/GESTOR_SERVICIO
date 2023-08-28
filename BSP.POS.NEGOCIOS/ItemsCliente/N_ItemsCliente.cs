using BSP.POS.DATOS.ItemsCliente;
using BSP.POS.UTILITARIOS.ItemsCliente;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.ItemsCliente
{
    public class N_ItemsCliente
    {
        D_ItemsCliente objItems = new D_ItemsCliente();

        public string ObtenerCondicionesDePago(string pEsquema)
        {
                List<U_ItemsCliente> list = new List<U_ItemsCliente>();

                list = objItems.ObtenerCondicionesDePago(pEsquema);

                string condiciones = JsonConvert.SerializeObject(list);
                return condiciones;

        }

        public string ObtenerNivelesPrecio(string pEsquema, string pMoneda)
        {
            List<U_ItemsCliente> list = new List<U_ItemsCliente>();

            list = objItems.ObtenerNivelesPrecio(pEsquema, pMoneda);

            string niveles = JsonConvert.SerializeObject(list);
            return niveles;

        }

        public string ObtenerTiposDeImpuestos(string pEsquema)
        {
            List<U_ItemsCliente> list = new List<U_ItemsCliente>();

            list = objItems.ObtenerTiposDeImpuestos(pEsquema);

            string tipos = JsonConvert.SerializeObject(list);
            return tipos;

        }

        public string ObtenerTiposDeTarifasDeImpuesto(string pEsquema, string pImpuesto)
        {
            List<U_Tarifa> list = new List<U_Tarifa>();

            list = objItems.ObtenerTiposDeTarifasDeImpuesto(pEsquema, pImpuesto);

            string tipos = JsonConvert.SerializeObject(list);
            return tipos;

        }
        public decimal ObtenerPorcentajeTarifa(string esquema, string impuesto, string tipoTarifa)
        {
            decimal porcentaje = 0;

            porcentaje = objItems.ObtenerPorcentajeTarifa(esquema, impuesto, tipoTarifa);

            return porcentaje;
        }

        public string ObtenerSiguienteCodigoDeCliente(string esquema, string letra)
        {
            string codigo = string.Empty;

            codigo = objItems.ObtenerSiguienteCodigoDeCliente(esquema, letra);

            return codigo;
        }

        public string ObtenerTiposDeNit(string pEsquema)
        {
            List<U_ItemsCliente> list = new List<U_ItemsCliente>();

            list = objItems.ObtenerTiposDeNit(pEsquema);

            string tipos = JsonConvert.SerializeObject(list);
            return tipos;

        }

        public string ObtenerListaDeCentrosDeCosto(string pEsquema)
        {
            List<U_ItemsCliente> list = new List<U_ItemsCliente>();

            list = objItems.ObtenerListaDeCentrosDeCosto(pEsquema);

            string centros = JsonConvert.SerializeObject(list);
            return centros;

        }
    }
}
