using System;
using System.Collections.Generic;
using System.Linq;
using BSP.POS.DATOS.POSDataSetTableAdapters;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.Lugares;

namespace BSP.POS.DATOS.Lugares
{
    public class D_Lugares
    {
        public List<U_Lugares> ObtenerPaises(string pEsquema)
        {
            try
            {
                var Lst = new List<U_Lugares>();

                ObtenerPaisesTableAdapter sp = new ObtenerPaisesTableAdapter();

                var response = sp.GetData(pEsquema).ToList();

                foreach (var item in response)
                {
                    U_Lugares dropDownList = new U_Lugares(item.PAIS, item.DESCRIPCION);
                    Lst.Add(dropDownList);
                }
                return Lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error", ex.InnerException.InnerException);
            }
        }

        public List<U_Lugares> ObtenerProvinciasPorPais(string pEsquema, string pPais)
        {
            try
            {
                var Lst = new List<U_Lugares>();

                ObtenerProvinciasTableAdapter sp = new ObtenerProvinciasTableAdapter();

                var response = sp.GetData(pEsquema, pPais).ToList();

                foreach (var item in response)
                {
                    U_Lugares dropDownList = new U_Lugares(item.PROVINCIA, item.NOMBRE);
                    Lst.Add(dropDownList);
                }
                return Lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error", ex.InnerException.InnerException);
            }
        }

        public List<U_Lugares> ObtenerCantonesPorProvincia(string pEsquema, string pPais, string pProvincia)
        {
            try
            {
                var Lst = new List<U_Lugares>();

                ObtenerCantonesTableAdapter sp = new ObtenerCantonesTableAdapter();

                var response = sp.GetData(pEsquema, pPais, pProvincia).ToList();

                foreach (var item in response)
                {
                    U_Lugares dropDownList = new U_Lugares(item.DIVISION_GEOGRAFICA2, item.CANTON);
                    Lst.Add(dropDownList);
                }
                return Lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error", ex.InnerException.InnerException);
            }
        }

        public List<U_Lugares> ObtenerDistritosPorCanton(string pEsquema, string pPais, string pProvincia, string pCanton)
        {
            try
            {
                var Lst = new List<U_Lugares>();

                ObtenerDistritosTableAdapter sp = new ObtenerDistritosTableAdapter();

                var response = sp.GetData(pEsquema, pPais, pProvincia, pCanton).ToList();

                foreach (var item in response)
                {
                    U_Lugares dropDownList = new U_Lugares(item.DIVISION_GEOGRAFICA3, item.DISTRITO);
                    Lst.Add(dropDownList);
                }
                return Lst;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error", ex.InnerException.InnerException);
            }
        }

        public List<U_Lugares> ObtenerBarriosPorDistrito(string pEsquema, string pPais, string pProvincia, string pCanton, string pDistrito)
        {
            try
            {
                var Lst = new List<U_Lugares>();

                ObtenerBarriosTableAdapter sp = new ObtenerBarriosTableAdapter();

                var response = sp.GetData(pEsquema, pPais, pProvincia, pCanton, pDistrito).ToList();

                foreach (var item in response)
                {
                    U_Lugares dropDownList = new U_Lugares(item.DIVISION_GEOGRAFICA4, item.BARRIO);
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
