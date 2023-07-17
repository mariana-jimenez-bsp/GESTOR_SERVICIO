﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Informes
{
    public class U_ListaInformesAsociados
    {
        public string consecutivo { get; set; }
        public string fecha_actualizacion { get; set; }
        public string cliente { get; set; }
        public string estado { get; set; }



        public U_ListaInformesAsociados(string pConsecutivo, string pFecha_actualizacion, string pCliente, string pEstado)
        {
            consecutivo = pConsecutivo;
            fecha_actualizacion = pFecha_actualizacion;
            cliente = pCliente;
            estado = pEstado;
        }
        public U_ListaInformesAsociados() { }
    }
}
