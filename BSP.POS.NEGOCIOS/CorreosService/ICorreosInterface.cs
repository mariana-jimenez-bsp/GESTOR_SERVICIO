﻿
using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.CorreosService
{
    public interface ICorreosInterface
    {
        void EnviarCorreoRecuperarClave(U_Correo datos, string token, string esquema, string urlWeb, string tipoInicio);
        Task EnviarCorreosInformes(U_Correo datos, mObjetoParaCorreoInforme objetosParaInforme, string urlWeb, string tipoInicio, string urlApiCristal, byte[] reporte);
        mObjetoParaCorreoInforme CrearObjetoDeCorreo(string esquema, string consecutivo);

    }
}
