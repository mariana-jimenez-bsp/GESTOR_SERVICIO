﻿using BSP.POS.Presentacion.Models.Actividades;

namespace BSP.POS.Presentacion.Interfaces.Actividades
{
    public interface IActividadesInterface
    {
        List<mActividadesAsociadas> ListaActividadesAsociadas { get; set; }
        Task ObtenerListaDeActividadesAsociadas(string consecutivo, string esquema);

        List<mActividades> ListaActividades { get; set; }
        Task ObtenerListaDeActividades(string esquema);

        Task ActualizarListaDeActividades(List<mActividades> listaActividades, string esquema);
    }
}
