using BSP.POS.DATOS.Esquemas;
using BSP.POS.UTILITARIOS.Esquemas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Esquemas
{
    public class N_Esquemas
    {
        D_Esquemas _esquemas = new D_Esquemas();

        public string ObtenerListaDeEsquemas()
        {
            List<U_Esquemas> esquemas = _esquemas.ObtenerListaDeEsquemas();
            string esquemasJson = JsonConvert.SerializeObject(esquemas);
            return esquemasJson;
        }

        public string ObtenerListaDeEsquemasDeUsuario(string codigo)
        {
            List<U_DatosEsquemasDeUsuario> esquemas = _esquemas.ObtenerListaDeEsquemasDeUsuario(codigo);
            string esquemasJson = JsonConvert.SerializeObject(esquemas);
            return esquemasJson;
        }

        public void ActualizarEsquemasDeUsuario(List<string> listaDeEsquemas, string codigo)
        {
            try
            {
                var listaDeEsquemasActuales = _esquemas.ObtenerListaDeEsquemasDeUsuario(codigo);
                if (listaDeEsquemasActuales.Any())
                {
                    _esquemas.EliminarEsquemasDeUsuario(codigo);
                }

                if (listaDeEsquemas.Any())
                {
                    foreach (var idEsquema in listaDeEsquemas)
                    {
                        _esquemas.AgregarEsquemaDeUsuario(codigo, idEsquema);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
    }
}
