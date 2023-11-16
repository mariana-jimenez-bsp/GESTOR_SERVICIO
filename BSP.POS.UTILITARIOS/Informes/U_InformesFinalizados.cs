using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Informes
{
    public class U_InformesFinalizados
    {
        public string consecutivo { get; set; }
        public string fecha_actualizacion { get; set; }
        public string fecha_consultoria { get; set; }
        public string numero_proyecto { get; set; }
        public string estado { get; set; }
        public string codigo_cliente { get; set; }
        public string nombre_cliente { get; set; }
        public byte[] imagen_cliente { get; set; }
        public string codigo_consultor { get; set; }
        public string nombre_consultor { get; set; }
        public U_InformesFinalizados(
        string pConsecutivo,
        string pFechaActualizacion,
        string pFechaConsultoria,
        string pNumeroProyecto,
        string pEstado,
        string pCodigoCliente,
        string pNombreCliente,
        byte[] pImagenCliente,
        string pCodigoConsultor,
        string pNombreConsultor)
        {
            consecutivo = pConsecutivo;
            fecha_actualizacion = pFechaActualizacion;
            fecha_consultoria = pFechaConsultoria;
            numero_proyecto = pNumeroProyecto;
            estado = pEstado;
            codigo_cliente = pCodigoCliente;
            nombre_cliente = pNombreCliente;
            imagen_cliente = pImagenCliente;
            codigo_consultor = pCodigoConsultor;
            nombre_consultor = pNombreConsultor;
        }
        public U_InformesFinalizados() { }
    }
}
