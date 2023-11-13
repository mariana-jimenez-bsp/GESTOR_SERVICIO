﻿using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Actividades;
using BSP.POS.UTILITARIOS.Proyectos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.DATOS.Proyectos
{
    public class D_Proyectos
    {

        public List<U_ListaProyectos> ListarProyectosActivos(String pEsquema)
        {
            var LstProyectos = new List<U_ListaProyectos>();

            ListarProyectosActivosTableAdapter sp = new ListarProyectosActivosTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ListaProyectos proyecto = new U_ListaProyectos(item.Id, item.numero, item.codigo_cliente, item.fecha_inicial, item.fecha_final, item.horas_totales, item.centro_costo, item.nombre_proyecto, item.estado);

                    LstProyectos.Add(proyecto);
                }
                return LstProyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public U_ListaProyectos ObtenerProyecto(string pEsquema, string pNumero)
        {
            ObtenerProyectoTableAdapter sp = new ObtenerProyectoTableAdapter();
            U_ListaProyectos proyecto = new U_ListaProyectos();
            var response = sp.GetData(pEsquema, pNumero).ToList();
            try
            {
                foreach (var item in response)
                {
                    proyecto = new U_ListaProyectos(item.Id, item.numero, item.codigo_cliente, item.fecha_inicial, item.fecha_final, item.horas_totales, item.centro_costo, item.nombre_proyecto, item.estado);
                   
                }
                return proyecto;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public List<U_DatosProyectos> ListarDatosProyectosActivos(String pEsquema)
        {
            var LstProyectos = new List<U_DatosProyectos>();

            ListarDatosProyectosActivosTableAdapter sp = new ListarDatosProyectosActivosTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_DatosProyectos proyecto = new U_DatosProyectos(item.Id, item.numero, item.codigo_cliente, item.fecha_inicial, item.fecha_final, item.horas_totales, item.centro_costo, item.nombre_proyecto, item.estado, item.nombre_cliente, item.contacto, item.cargo, item.imagen);

                    LstProyectos.Add(proyecto);
                }
                return LstProyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public List<U_DatosProyectos> ListarDatosProyectosActivosDeCliente(String pEsquema, string pCliente)
        {
            var LstProyectos = new List<U_DatosProyectos>();

            ListarDatosProyectosActivosDeClienteTableAdapter sp = new ListarDatosProyectosActivosDeClienteTableAdapter();

            var response = sp.GetData(pEsquema, pCliente).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_DatosProyectos proyecto = new U_DatosProyectos(item.Id, item.numero, item.codigo_cliente, item.fecha_inicial, item.fecha_final, item.horas_totales, item.centro_costo, item.nombre_proyecto, item.estado, item.nombre_cliente, item.contacto, item.cargo, item.imagen);

                    LstProyectos.Add(proyecto);
                }
                return LstProyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public List<U_ListaProyectos> ListarProyectosTerminados(String pEsquema)
        {
            var LstProyectos = new List<U_ListaProyectos>();

            ListarProyectosTerminadosTableAdapter sp = new ListarProyectosTerminadosTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_ListaProyectos proyecto = new U_ListaProyectos(item.Id, item.numero, item.codigo_cliente, item.fecha_inicial, item.fecha_final, item.horas_totales, item.centro_costo, item.nombre_proyecto, item.estado);

                    LstProyectos.Add(proyecto);
                }
                return LstProyectos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarListaDeProyectos(List<U_ListaProyectos> pProyectos, string esquema)
        {
            POSDataSet.ActualizarActividadesDataTable bTabla = new POSDataSet.ActualizarActividadesDataTable();
            ActualizarListaProyectosTableAdapter sp = new ActualizarListaProyectosTableAdapter();
            try
            {
                foreach (var proyecto in pProyectos)
                {
                    var response = sp.GetData(proyecto.Id,proyecto.numero, proyecto.codigo_cliente, proyecto.fecha_inicial, proyecto.fecha_final, proyecto.horas_totales, proyecto.centro_costo, proyecto.nombre_proyecto, esquema);

                }
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
        public string TerminarProyecto(string numero, string esquema)
        {
            POSDataSet.TerminarProyectoDataTable bTabla = new POSDataSet.TerminarProyectoDataTable();
            TerminarProyectoTableAdapter sp = new TerminarProyectoTableAdapter();
            try
            {

                var response = sp.GetData(numero, true, esquema);

                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public string AgregarProyecto(U_ListaProyectos pProyecto, string esquema)
        {
            POSDataSet.AgregarProyectoDataTable bTabla = new POSDataSet.AgregarProyectoDataTable();
            AgregarProyectoTableAdapter sp = new AgregarProyectoTableAdapter();
            try
            {
                    var response = sp.GetData(pProyecto.codigo_cliente, pProyecto.fecha_inicial, pProyecto.fecha_final, int.Parse(pProyecto.horas_totales), pProyecto.centro_costo, pProyecto.nombre_proyecto, esquema);

                
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
    }
}
