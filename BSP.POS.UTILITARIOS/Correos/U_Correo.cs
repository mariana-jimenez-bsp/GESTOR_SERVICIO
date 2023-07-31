﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Correos
{
    public class U_Correo
    {
        public string para { get; set; }
        public string tema { get; set; }

        public string cuerpo { get; set; }

        public U_Correo(string pPara, string pTema, string pCuerpo) {
            para = pPara;
            tema = pTema;
            cuerpo = pCuerpo;
        }

        public U_Correo() { }
    }
}
