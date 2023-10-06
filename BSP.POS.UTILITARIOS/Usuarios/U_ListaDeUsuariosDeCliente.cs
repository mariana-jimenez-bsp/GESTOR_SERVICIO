using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_ListaDeUsuariosDeCliente
    {
        public String id { get; set; }

        public String codigo { get; set; }
        public String cod_cliente { get; set; }

        public String usuario { get; set; }
        public String nombre { get; set; }
        public String departamento { get; set; }
        public String correo { get; set; }
        public String telefono { get; set; }

        public U_ListaDeUsuariosDeCliente(String pId, String pCodigo, String pCod_cliente, String pUsuario, String pNombre, String pDepartamento, String pCorreo, String pTelefono)
        {
            id = pId;
            codigo = pCodigo;
            cod_cliente = pCod_cliente;
            usuario = pUsuario;
            nombre = pNombre;
            departamento = pDepartamento;
            correo = pCorreo;
            telefono = pTelefono;

        }
        public U_ListaDeUsuariosDeCliente()
        {
        }
    }
}
