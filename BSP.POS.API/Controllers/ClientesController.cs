﻿using Microsoft.AspNetCore.Mvc;
using BSP.POS.NEGOCIOS.Clientes;
using BSP.POS.UTILITARIOS.Clientes;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using BSP.POS.API.Models.Clientes;
using BSP.POS.UTILITARIOS.Usuarios;
using System.Reflection.PortableExecutable;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSP.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private N_Clientes clientes;
        public ClientesController()
        {
            clientes = new N_Clientes();

        }
        // GET: api/<ClientesController>
        [HttpGet("ObtengaLaListaDeClientes/{esquema}")]
        public string ObtengaLaListaDeClientes(string esquema)
        {
            try
            {
                string listaClientesJson = clientes.ListarClientes(esquema);
                return listaClientesJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        [HttpGet("ObtengaLaListaDeClientesRecientes/{esquema}")]
        public string ObtengaLaListaDeClientesRecientes(string esquema)
        {
            try
            {
                string listaClientesRecientesJson = clientes.ListarClientesRecientes(esquema);
                return listaClientesRecientesJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
           
        }

        [HttpGet("ObtengaElClienteAsociado/{cliente}/{esquema}")]
        public string ObtengaElClienteAsociado(string cliente, string esquema)
        {
            try
            {
                var clienteAsociadoJson = clientes.ObtenerClienteAsociado(esquema, cliente);
                return clienteAsociadoJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        [HttpPost("ActualizaListaDeClientes")]
        public string ActualizaListaDeClientes([FromBody] List<mClientes> datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];

                List<U_ListaClientes> listaClientes = new List<U_ListaClientes>();
                foreach (var item in datos)
                {
                    U_ListaClientes cliente = new U_ListaClientes();
                    cliente.CLIENTE = item.CLIENTE;
                    cliente.NOMBRE = item.NOMBRE;
                    cliente.ALIAS = item.ALIAS;
                    cliente.CONTRIBUYENTE = item.CONTRIBUYENTE;
                    cliente.TELEFONO1 = item.TELEFONO1;
                    cliente.TELEFONO2 = item.TELEFONO2;
                    cliente.E_MAIL = item.E_MAIL;
                    listaClientes.Add(cliente);
                }

                string mensaje = clientes.ActualizarListaDeClientes(listaClientes, esquema);
                return mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpGet("ObtengaLaListaDeClientesCorporaciones/{esquema}")]
        public string ObtengaLaListaDeClientesCorporaciones(string esquema)
        {
            try
            {
                string listaClientesJson = clientes.ObtenerListaClientesCorporaciones(esquema);
                return listaClientesJson;
            }

            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [HttpPost("AgregaCliente")]
        public Task AgregaCliente([FromBody] mAgregarCliente datos)
        {
            try
            {
                string esquema = Request.Headers["X-Esquema"];
                string usuario = Request.Headers["X-Usuario"];

                U_AgregarCliente cliente = new U_AgregarCliente();

                
                cliente.CLIENTE = datos.CLIENTE;
                cliente.NOMBRE = datos.NOMBRE;
                cliente.ALIAS = datos.ALIAS;
                cliente.CONTACTO = datos.CONTACTO;
                cliente.CARGO = datos.CARGO;
                cliente.DIRECCION = datos.DIRECCION;
                cliente.TELEFONO1 = datos.TELEFONO1;
                cliente.TELEFONO2 = datos.TELEFONO2;
                cliente.CONTRIBUYENTE = datos.CONTRIBUYENTE;
                cliente.TIPO_NIT = datos.TIPO_NIT;
                cliente.MONEDA = datos.MONEDA;
                cliente.CONDICION_PAGO = datos.CONDICION_PAGO;
                cliente.NIVEL_PRECIO = datos.NIVEL_PRECIO;
                cliente.PAIS = datos.PAIS;
                cliente.ZONA = datos.ZONA;
                cliente.E_MAIL = datos.E_MAIL;
                cliente.DIVISION_GEOGRAFICA1 = datos.DIVISION_GEOGRAFICA1;
                cliente.DIVISION_GEOGRAFICA2 = datos.DIVISION_GEOGRAFICA2;
                cliente.USUARIO_CREACION = usuario;
                cliente.DIVISION_GEOGRAFICA3 = datos.DIVISION_GEOGRAFICA3;
                cliente.DIVISION_GEOGRAFICA4 = datos.DIVISION_GEOGRAFICA4;
                cliente.OTRAS_SENAS = datos.OTRAS_SENAS;
                cliente.DOC_A_GENERAR = datos.DOC_A_GENERAR;
                cliente.EXENTO_IMPUESTOS = datos.EXENTO_IMPUESTOS;
                cliente.EXENCION_IMP1 = datos.EXENCION_IMP1;
                cliente.EXENCION_IMP2 = datos.EXENCION_IMP2;
                cliente.DESCUENTO = datos.DESCUENTO;
                cliente.ES_CORPORACION = datos.ES_CORPORACION;
                cliente.CLI_CORPORAC_ASOC = datos.CLI_CORPORAC_ASOC;
                cliente.TIPO_IMPUESTO = datos.TIPO_IMPUESTO;
                cliente.TIPO_TARIFA = datos.TIPO_TARIFA;
                cliente.PORC_TARIFA = datos.PORC_TARIFA;
                cliente.TIPIFICACION_CLIENTE = datos.TIPIFICACION_CLIENTE;
                cliente.AFECTACION_IVA = datos.AFECTACION_IVA;
                cliente.IMAGEN = datos.IMAGEN;




                clientes.AgregarCliente(cliente, esquema);
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }

        }


    }
}
