using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_ImagenUsuario
    {
        public byte[] imagen { get; set; }

        public U_ImagenUsuario(byte[] pImagen)
        {
            imagen = pImagen;
        }

        public U_ImagenUsuario() { }
    }
}
