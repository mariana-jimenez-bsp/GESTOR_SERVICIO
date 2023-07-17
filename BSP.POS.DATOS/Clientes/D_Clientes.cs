﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.Clientes;
using BSP.POS.DATOS.POSDataSetTableAdapters;

namespace BSP.POS.DATOS.Clientes
{
    public class D_Clientes
    {
        public List<U_ListaClientes> ListaClientes(String pEsquema)
        {
            var LstClientes = new List<U_ListaClientes>();

            ListarClientesTableAdapter sp = new ListarClientesTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ListaClientes cliente = new U_ListaClientes(item.CLIENTE, item.NOMBRE, item.ALIAS, item.CONTACTO, item.CARGO, item.DIRECCION, item.TELEFONO1,
                                                     item.CONTRIBUYENTE, item.MONEDA, item.NIVEL_PRECIO, item.PAIS, item.ZONA,
                                                     item.EXENTO_IMPUESTOS, item.E_MAIL, item.CODIGO_IMPUESTO, item.DIVISION_GEOGRAFICA1,
                                                     item.DIVISION_GEOGRAFICA2, item.DIVISION_GEOGRAFICA3, item.DIVISION_GEOGRAFICA4,
                                                     item.OTRAS_SENAS, item.RecordDate);

                    LstClientes.Add(cliente);
                }
                return LstClientes;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }


    }
}

