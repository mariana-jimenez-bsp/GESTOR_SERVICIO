using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Departamentos;

namespace BSP.POS.DATOS.Departamentos
{
    public class D_Departamentos
    {
        public List<U_Departamentos> ObtenerListaDeDepartamentos(string esquema)
        {
            List<U_Departamentos> listaDepartamentos = new List<U_Departamentos>();
            ListarDepartamentosTableAdapter sp = new ListarDepartamentosTableAdapter();
            
            var response = sp.GetData(esquema).ToList();

            foreach (var item in response)
            {
                U_Departamentos departamento = new U_Departamentos();
                departamento.Id = int.Parse(item.Id);
                departamento.Departamento = item.Departamento;
                departamento.codigo = item.codigo;
                listaDepartamentos.Add(departamento);
            }
            return listaDepartamentos;
        }

        public void ActualizarListaDepartamentos(List<U_Departamentos> listaDepartamentos, string esquema)
        {
            try
            {
                ActualizarDepartamentosTableAdapter sp = new ActualizarDepartamentosTableAdapter();
                foreach (var item in listaDepartamentos)
                {
                    var response = sp.GetData(item.Id, item.codigo, item.Departamento, esquema);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AgregarDepartamento(U_Departamentos departamento, string esquema)
        {
            try
            {
                AgregarDepartamentoTableAdapter sp = new AgregarDepartamentoTableAdapter();
                var response = sp.GetData(departamento.Departamento, esquema);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void EliminarDepartamento(string esquema, string codigo)
        {
            try
            {
                EliminarDepartamentoTableAdapter sp = new EliminarDepartamentoTableAdapter();
                var response = sp.GetData(codigo, esquema).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
    }
}
