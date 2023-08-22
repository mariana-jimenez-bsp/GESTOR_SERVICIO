﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Usuarios
{
    public class U_UsuariosParaEditar
    {
        public String id { get; set; }
        public String codigo { get; set; }
        public String cod_cliente { get; set; }

        public String usuario { get; set; }
        public String correo { get; set; }

        public String clave { get; set; }
        public String nombre { get; set; }
        public String rol { get; set; }
        public String telefono { get; set; }
        public String departamento { get; set; }
        public byte[] imagen { get; set; }
        public String esquema { get; set; }

        public U_UsuariosParaEditar(String pId, String pCodigo, String pCod_cliente, String pUsuario, String pCorreo, String pClave, String pNombre, String pRol, String pTelefono, string pDepartamento, byte[] pImagen,  String pEsquema)
        {
            id = pId;
            codigo = pCodigo;
            cod_cliente = pCod_cliente;
            usuario = pUsuario;
            correo = pCorreo;
            clave = pClave;
            nombre = pNombre;
            rol = pRol;
            telefono = pTelefono;
            departamento = pDepartamento;
            imagen = pImagen;
            esquema = pEsquema;
        }
        public U_UsuariosParaEditar()
        {
        }
    }
}