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
        public String codigo_departamento { get; set; }
        public String correo { get; set; }
        public String telefono { get; set; }

        public U_ListaDeUsuariosDeCliente(String pId, String pCodigo, String pCod_cliente, String pUsuario, String pNombre, String pCodigo_departamento, String pCorreo, String pTelefono)
        {
            id = pId;
            codigo = pCodigo;
            cod_cliente = pCod_cliente;
            usuario = pUsuario;
            nombre = pNombre;
            codigo_departamento = pCodigo_departamento;
            correo = pCorreo;
            telefono = pTelefono;

        }
        public U_ListaDeUsuariosDeCliente()
        {
        }
    }
}
