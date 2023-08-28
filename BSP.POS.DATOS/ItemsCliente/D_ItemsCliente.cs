using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.ItemsCliente;

namespace BSP.POS.DATOS.ItemsCliente
{
    public class D_ItemsCliente
    {
        public List<U_ItemsCliente> ObtenerCondicionesDePago(string pEsquema)
        {
            try
            {
                var Lst = new List<U_ItemsCliente>();

                ObtenerCondicionesDePagoTableAdapter sp = new ObtenerCondicionesDePagoTableAdapter();

                var response = sp.GetData(pEsquema).ToList();

                foreach (var item in response)
                {
                    U_ItemsCliente dropDownList = new U_ItemsCliente(item.CONDICION_PAGO, item.DESCRIPCION);
                    Lst.Add(dropDownList);
                }
                return Lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error", ex.InnerException.InnerException);
            }
        }

        public List<U_ItemsCliente> ObtenerNivelesPrecio(string pEsquema, string pMoneda)
        {
            try
            {
                var Lst = new List<U_ItemsCliente>();

                lISTAR_NIVEL_PRECIO_APROBADOTableAdapter sp = new lISTAR_NIVEL_PRECIO_APROBADOTableAdapter();

                var response = sp.GetData(pEsquema, pMoneda).ToList();

                foreach (var item in response)
                {
                    U_ItemsCliente dropDownList = new U_ItemsCliente(item.NIVEL_PRECIO, item.NIVEL_PRECIO);
                    Lst.Add(dropDownList);
                }
                return Lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error", ex.InnerException.InnerException);
            }
        }

        public List<U_ItemsCliente> ObtenerTiposDeImpuestos(string pEsquema)
        {
            try
            {
                var Lst = new List<U_ItemsCliente>();

                SELECT_TIPO_IMPUESTOTableAdapter sp = new SELECT_TIPO_IMPUESTOTableAdapter();

                var response = sp.GetData(pEsquema).ToList();

                foreach (var item in response)
                {
                    U_ItemsCliente dropDownList = new U_ItemsCliente(item.TIPO_IMPUESTO, item.DESCRIPCION);
                    Lst.Add(dropDownList);
                }
                return Lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error", ex.InnerException.InnerException);
            }
        }

        public List<U_Tarifa> ObtenerTiposDeTarifasDeImpuesto(string pEsquema, string pImpuesto)
        {
            try
            {
                var Lst = new List<U_Tarifa>();

                SELECT_TIPO_TARIFATableAdapter sp = new SELECT_TIPO_TARIFATableAdapter();

                var response = sp.GetData(pEsquema, pImpuesto).ToList();

                foreach (var item in response)
                {
                    U_Tarifa dropDownList = new U_Tarifa(item.TIPO_TARIFA, item.DESCRIPCION, item.PORCENTAJE);
                    Lst.Add(dropDownList);
                }
                return Lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error", ex.InnerException.InnerException);
            }
        }

        public decimal ObtenerPorcentajeTarifa(string esquema, string impuesto, string tipoTarifa)
        {
            decimal porcentaje = 0;

            SELECT_PORCJ_TARIFATableAdapter sp = new SELECT_PORCJ_TARIFATableAdapter();

            var collection = sp.GetData(esquema, impuesto, tipoTarifa);

            foreach (var item in collection)
            {
                porcentaje = item.PORCENTAJE;
            }

            return porcentaje;
        }

        public string ObtenerSiguienteCodigoDeCliente(string esquema, string letra)
        {
            string codigo = string.Empty;

            GenerarSiguienteCodigoDeClienteTableAdapter sp = new GenerarSiguienteCodigoDeClienteTableAdapter();

            var collection = sp.GetData(esquema, letra);

            foreach (var item in collection)
            {
                codigo = item.nuevoCodigo;
            }

            return codigo;
        }

        public List<U_ItemsCliente> ObtenerTiposDeNit(string pEsquema)
        {
            try
            {
                var Lst = new List<U_ItemsCliente>();

                ObtenerTiposDeNitTableAdapter sp = new ObtenerTiposDeNitTableAdapter();

                var response = sp.GetData(pEsquema).ToList();

                foreach (var item in response)
                {
                    U_ItemsCliente dropDownList = new U_ItemsCliente(item.TIPO_NIT, item.DESCRIPCION);
                    Lst.Add(dropDownList);
                }
                return Lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error", ex.InnerException.InnerException);
            }
        }

        public List<U_ItemsCliente> ObtenerListaDeCentrosDeCosto(string pEsquema)
        {
            try
            {
                var Lst = new List<U_ItemsCliente>();

                ListarCentroCostoTableAdapter sp = new ListarCentroCostoTableAdapter();

                var response = sp.GetData(pEsquema).ToList();

                foreach (var item in response)
                {
                    U_ItemsCliente dropDownList = new U_ItemsCliente(item.CENTRO_COSTO, item.DESCRIPCION);
                    Lst.Add(dropDownList);
                }
                return Lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error", ex.InnerException.InnerException);
            }
        }
    }
}
