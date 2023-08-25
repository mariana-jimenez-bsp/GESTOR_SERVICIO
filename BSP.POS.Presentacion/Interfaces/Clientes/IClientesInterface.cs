﻿using BSP.POS.Presentacion.Models.Clientes;

namespace BSP.POS.Presentacion.Interfaces.Clientes
{
    public interface IClientesInterface
    {
        List<mClientes> ListaClientes { get; set; }
        Task ObtenerListaClientes(string esquema);
        List<mClientes> ListaClientesRecientes { get; set; }
        Task ObtenerListaClientesRecientes(string esquema);
        mClienteAsociado ClienteAsociado { get; set; }
        Task<mClienteAsociado?> ObtenerClienteAsociado(string cliente, string esquema);
        Task ActualizarListaDeClientes(List<mClientes> listaClientes, string esquema);

        List<mClienteContado> ListaClientesCorporaciones { get; set; }
        Task ObtenerListaClientesCorporaciones(string esquema);

        Task AgregarCliente(mAgregarCliente cliente, string esquema, string usuario);
    }
}
