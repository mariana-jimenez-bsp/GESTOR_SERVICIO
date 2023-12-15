using BSP.POS.UTILITARIOS.Actividades;
using BSP.POS.UTILITARIOS.Clientes;
using BSP.POS.UTILITARIOS.Informes;
using BSP.POS.UTILITARIOS.Observaciones;
using BSP.POS.UTILITARIOS.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.CorreosModels
{
    public class mObjetoParaCorreoInforme
    {
        public U_Informe informe { get; set; } = new U_Informe();
        public string esquema { get; set; } = string.Empty;
        public U_ClienteAsociado ClienteAsociado { get; set; } = new U_ClienteAsociado();
        public List<U_ListaActividadesAsociadas> listaActividadesAsociadas { get; set; } = new List<U_ListaActividadesAsociadas>();
        public List<U_DatosUsuariosDeClienteDeInforme> listadeUsuariosDeClienteDeInforme { get; set; } = new List<U_DatosUsuariosDeClienteDeInforme>();
        public int total_horas_cobradas { get; set; } = 0;
        public int total_horas_no_cobradas { get; set; } = 0;
        public List<U_DatosObservaciones> listaDeObservaciones { get; set; } = new List<U_DatosObservaciones>();
        public string codigo_cliente { get; set; } = string.Empty;
        public string numero_proyecto { get; set; } = string.Empty;
    }
}
