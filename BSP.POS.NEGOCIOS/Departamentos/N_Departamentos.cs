using BSP.POS.DATOS.Departamentos;
using BSP.POS.UTILITARIOS.Departamentos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Departamentos
{
    public class N_Departamentos
    {
        D_Departamentos _departamentos = new D_Departamentos();

        public string ObtenerListaDeDepartamentos(string esquema)
        {
            List<U_Departamentos> listaDepartamentos = new List<U_Departamentos>();
            listaDepartamentos = _departamentos.ObtenerListaDeDepartamentos(esquema);
            string listaJson = JsonConvert.SerializeObject(listaDepartamentos);
            return listaJson;
        }

        public void ActualizarListaDepartamentos(List<U_Departamentos> listaDepartamentos, string esquema)
        {
            _departamentos.ActualizarListaDepartamentos(listaDepartamentos, esquema);
        }
        public void AgregarDepartamento(U_Departamentos departamento, string esquema)
        {
            _departamentos.AgregarDepartamento(departamento, esquema);
        }
        public void EliminarDepartamento(string esquema, string codigo)
        {
            _departamentos.EliminarDepartamento(esquema, codigo);
        }
    }
}
