using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Clientes
{
    public class U_AgregarCliente
    {
        public string CLIENTE { get; set; }
        public string NOMBRE { get; set; }
        public string ALIAS { get; set; }
        public string CONTACTO { get; set; }
        public string CARGO { get; set; }
        public string DIRECCION { get; set; }
        public string TELEFONO1 { get; set; }
        public string TELEFONO2 { get; set; }
        public string CONTRIBUYENTE { get; set; }
        public string MONEDA { get; set; }
        public string PAIS { get; set; }
        public string ZONA { get; set; }
        public string E_MAIL { get; set; }
        public string DIVISION_GEOGRAFICA1 { get; set; }
        public string DIVISION_GEOGRAFICA2 { get; set; }
        public string DIVISION_GEOGRAFICA3 { get; set; }
        public string DIVISION_GEOGRAFICA4 { get; set; }
        public string OTRAS_SENAS { get; set; }
        public string NIVEL_PRECIO { get; set; }
        public string EXENTO_IMPUESTOS { get; set; }
        public string CODIGO_IMPUESTO { get; set; }
        public string RecordDate { get; set; }
        public byte[] IMAGEN { get; set; }
        public string CONDICION_PAGO { get; set; }
        public string DOC_A_GENERAR { get; set; }
        public string EXENCION_IMP1 { get; set; }
        public string EXENCION_IMP2 { get; set; }
        public string DESCUENTO { get; set; }
        public string ES_CORPORACION { get; set; }
        public string CLI_CORPORAC_ASOC { get; set; }
        public string TIPO_IMPUESTO { get; set; }
        public string TIPO_TARIFA { get; set; }
        public string PORC_TARIFA { get; set; }
        public string TIPIFICACION_CLIENTE { get; set; }
        public string AFECTACION_IVA { get; set; }
        public string TIPO_NIT { get; set; }
        public string MONEDA_NIVEL { get; set; }
        public string USUARIO_CREACION { get; set; }
        


        public U_AgregarCliente(string pCLIENTE, string pNOMBRE, string pALIAS, string pCONTACTO, string pCARGO, string pDIRECCION, string pTELEFONO1, string pTELEFONO2, string pCONTRIBUYENTE,
                string pMONEDA, string pNIVEL_PRECIO, string pPAIS, string pZONA, string pEXENTO_IMPUESTOS, string pE_MAIL,
                string pCODIGO_IMPUESTO, string pDIVISION_GEOGRAFICA1, string pDIVISION_GEOGRAFICA2, string pDIVISION_GEOGRAFICA3,
                string pDIVISION_GEOGRAFICA4, string pOTRAS_SENAS, string pRecordDate, byte[] pIMAGEN, string pCONDICION_PAGO,
                string pDOC_A_GENERAR, string pEXENCION_IMP1, string pEXENCION_IMP2, string pDESCUENTO, string pES_CORPORACION,
                string pCLI_CORPORAC_ASOC, string pTIPO_IMPUESTO, string pTIPO_TARIFA, string pPORC_TARIFA, string pTIPIFICACION_CLIENTE,
                string pAFECTACION_IVA, string pTIPO_NIT, string pMONEDA_NIVEL, string pUSUARIO_CREACION)
        {
            CLIENTE = pCLIENTE;
            NOMBRE = pNOMBRE;
            CARGO = pCARGO;
            CONTACTO = pCONTACTO;
            ALIAS = pALIAS;
            DIRECCION = pDIRECCION;
            TELEFONO1 = pTELEFONO1;
            TELEFONO2 = pTELEFONO2;
            CONTRIBUYENTE = pCONTRIBUYENTE;
            MONEDA = pMONEDA;
            NIVEL_PRECIO = pNIVEL_PRECIO;
            PAIS = pPAIS;
            ZONA = pZONA;
            EXENTO_IMPUESTOS = pEXENTO_IMPUESTOS;
            E_MAIL = pE_MAIL;
            CODIGO_IMPUESTO = pCODIGO_IMPUESTO;
            DIVISION_GEOGRAFICA1 = pDIVISION_GEOGRAFICA1;
            DIVISION_GEOGRAFICA2 = pDIVISION_GEOGRAFICA2;
            DIVISION_GEOGRAFICA3 = pDIVISION_GEOGRAFICA3;
            DIVISION_GEOGRAFICA4 = pDIVISION_GEOGRAFICA4;
            OTRAS_SENAS = pOTRAS_SENAS;
            RecordDate = pRecordDate;
            IMAGEN = pIMAGEN;
            CONDICION_PAGO = pCONDICION_PAGO;
            DOC_A_GENERAR = pDOC_A_GENERAR;
            EXENCION_IMP1 = pEXENCION_IMP1;
            EXENCION_IMP2 = pEXENCION_IMP2;
            DESCUENTO = pDESCUENTO;
            ES_CORPORACION = pES_CORPORACION;
            CLI_CORPORAC_ASOC = pCLI_CORPORAC_ASOC;
            TIPO_IMPUESTO = pTIPO_IMPUESTO;
            TIPO_TARIFA = pTIPO_TARIFA;
            PORC_TARIFA = pPORC_TARIFA;
            TIPIFICACION_CLIENTE = pTIPIFICACION_CLIENTE;
            AFECTACION_IVA = pAFECTACION_IVA;
            TIPO_NIT = pTIPO_NIT;
            MONEDA_NIVEL = pMONEDA_NIVEL;
            USUARIO_CREACION = pUSUARIO_CREACION;
    }
        public U_AgregarCliente() { }
    }
}
