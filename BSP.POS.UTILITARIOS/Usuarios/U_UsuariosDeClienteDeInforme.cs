﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_UsuariosDeClienteDeInforme
    {
        public string id { get; set; }
        public string consecutivo_informe { get; set; }
        public string codigo_usuario_cliente { get; set; }
        public string recibido { get; set; }



        public U_UsuariosDeClienteDeInforme(string pId, string pConsecutivo_informe, string pCodigo_usuario_cliente, string pRecibido)
        {
            id = pId;
            consecutivo_informe = pConsecutivo_informe;
            codigo_usuario_cliente = pCodigo_usuario_cliente;
            recibido = pRecibido;
        }
        public U_UsuariosDeClienteDeInforme() { }
    }
}
