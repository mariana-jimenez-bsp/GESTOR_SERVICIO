using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_DatosUsuariosDeClienteDeInforme
    {
        public string id { get; set; }
        public string consecutivo_informe { get; set; }
        public string codigo_usuario_cliente { get; set; }
        public string recibido { get; set; }
        public string nombre_usuario { get; set; }
        public string departamento_usuario { get; set; }
        public string rol_usuario { get; set; }
        public string correo_usuario { get; set; }
        public string token { get; set; } = string.Empty;

        public U_DatosUsuariosDeClienteDeInforme(string pId, string pConsecutivo_informe, string pCodigo_usuario_cliente, string pRecibido, string pNombreUsuario, string pDepartamento, string pRol, string pCorreo)
        {
            id = pId;
            consecutivo_informe = pConsecutivo_informe;
            codigo_usuario_cliente = pCodigo_usuario_cliente;
            recibido = pRecibido;
            nombre_usuario = pNombreUsuario;
            departamento_usuario = pDepartamento;
            rol_usuario = pRol;
            correo_usuario = pCorreo;
        }
        public U_DatosUsuariosDeClienteDeInforme() { }
    }
}
