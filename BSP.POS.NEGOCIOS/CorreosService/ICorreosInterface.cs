﻿
using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using BSP.POS.UTILITARIOS.Reportes;
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
        void EnviarCorreosInformes(U_Correo datos, mObjetoParaCorreoInforme objetosParaInforme, string urlWeb, string tipoInicio, U_ObjetoReporte objetoReporte);
        mObjetoParaCorreoInforme CrearObjetoDeCorreo(string esquema, string consecutivo);

    }
}
