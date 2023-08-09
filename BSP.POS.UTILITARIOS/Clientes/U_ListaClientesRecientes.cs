using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.UTILITARIOS.Clientes
{
    public class U_ListarClientesRecientes
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
        public string CreateDate { get; set; }
        public byte[] IMAGEN { get; set; }


        public U_ListarClientesRecientes(string pCLIENTE, string pNOMBRE, string pALIAS, string pCONTACTO, string pCARGO, string pDIRECCION, string pTELEFONO1 , string pTELEFONO2, string pCONTRIBUYENTE,
                string pMONEDA, string pNIVEL_PRECIO, string pPAIS, string pZONA, string pEXENTO_IMPUESTOS, string pE_MAIL,
                string pCODIGO_IMPUESTO, string pDIVISION_GEOGRAFICA1, string pDIVISION_GEOGRAFICA2, string pDIVISION_GEOGRAFICA3,
                string pDIVISION_GEOGRAFICA4, string pOTRAS_SENAS, string pRecordDate, string pCreateDate, byte[] pIMAGEN)
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
            CreateDate = pCreateDate;
            IMAGEN = pIMAGEN;
        }
        public U_ListarClientesRecientes() { }
    }
}