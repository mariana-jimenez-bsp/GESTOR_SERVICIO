using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.CorreosModels.Models;
using BSP.POS.UTILITARIOS.Observaciones;

namespace BSP.POS.UTILITARIOS.CorreosModels
{
    public class mObjetosParaCorreoAprobacion
    {
        public mInformeAsociado informe { get; set; } = new mInformeAsociado();
        public string esquema { get; set; } = string.Empty;
        public mClienteAsociado ClienteAsociado { get; set; } = new mClienteAsociado();
        public List<mActividadesAsociadas> listaActividadesAsociadas { get; set; } = new List<mActividadesAsociadas>();
        public List<mUsuariosDeClienteDeInforme> listadeUsuariosDeClienteDeInforme { get; set; } = new List<mUsuariosDeClienteDeInforme>();
        public int total_horas_cobradas { get; set; } = 0;
        public int total_horas_no_cobradas { get; set; } = 0;
        public List<mObservaciones> listaDeObservaciones { get; set; } = new List<mObservaciones>();
    }
}
