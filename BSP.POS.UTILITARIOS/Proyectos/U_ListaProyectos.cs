using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Proyectos
{
    public class U_ListaProyectos
    {
        public string Id { get; set; }

        public string numero { get; set; }
        public string codigo_cliente { get; set; }
        public string fecha_inicial { get; set; }
        public string fecha_final { get; set; }
        public string horas_totales { get; set; }
        public string centro_costo { get; set; }
        public string nombre_proyecto { get; set; }
        public string estado { get; set; }

        public U_ListaProyectos(string pId, string pNumero, string pCodigo_cliente, string pFecha_Inicial, string pFecha_Final, string pHoras_totales, string pCentro_costo, string pNombre_proyecto, string pEstado)
        {
            Id = pId;
            numero = pNumero;
            codigo_cliente = pCodigo_cliente;
            fecha_inicial = pFecha_Inicial;
            fecha_final = pFecha_Final;
            horas_totales = pHoras_totales;
            centro_costo = pCentro_costo;
            nombre_proyecto = pNombre_proyecto;
            estado = pEstado;
        }

        public U_ListaProyectos()
        {
        }
    }
}
