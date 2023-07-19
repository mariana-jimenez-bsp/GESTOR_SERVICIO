﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_LoginToken
    {
        public String token { get; set; } 
        public String esquema { get; set; }
        public String usuario { get; set; }

        public U_LoginToken(String pToken, String pEsquema, String pUsuario)
        {
            usuario = pUsuario;
            esquema = pEsquema;
            token = pToken;
        }
        public U_LoginToken()
        {
        }
    }
}
