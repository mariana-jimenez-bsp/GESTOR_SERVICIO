using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Esquemas;

namespace BSP.POS.DATOS.Esquemas
{
    public class D_Esquemas
    {
        public List<U_Esquemas> ObtenerListaDeEsquemas()
        {
            var LstEsquemas = new List<U_Esquemas>();

            ObtenerListaEsquemasTableAdapter sp = new ObtenerListaEsquemasTableAdapter();
            try
            {
                var response = sp.GetData().ToList();
                foreach (var item in response)
                {
                    U_Esquemas nuevoEsquema = new U_Esquemas(item.Id, item.esquema);
                    LstEsquemas.Add(nuevoEsquema);
                }
                return LstEsquemas;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
            
        }

        public List<U_DatosEsquemasDeUsuario> ObtenerListaDeEsquemasDeUsuario(string codigo)
        {
            var LstEsquemas = new List<U_DatosEsquemasDeUsuario>();

            ListarDatosEsquemas_UsuariosTableAdapter sp = new ListarDatosEsquemas_UsuariosTableAdapter();
            try
            {
                var response = sp.GetData(codigo).ToList();
                foreach (var item in response)
                {
                    U_DatosEsquemasDeUsuario nuevoEsquema = new U_DatosEsquemasDeUsuario(item.Id, item.id_esquema, item.esquema);
                    LstEsquemas.Add(nuevoEsquema);
                }
                return LstEsquemas;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }

        }

        public void AgregarEsquemaDeUsuario(string codigo, string idEsquema)
        {
            AgregarEsquemaDeUsuarioTableAdapter sp = new AgregarEsquemaDeUsuarioTableAdapter();
            try
            {
                var response = sp.GetData(codigo, int.Parse(idEsquema)).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }
        }

        public void EliminarEsquemasDeUsuario(string codigo)
        {
            EliminarEsquemasDeUsuarioTableAdapter sp = new EliminarEsquemasDeUsuarioTableAdapter();
            try
            {
                var response = sp.GetData(codigo).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }
        }
    }
}
