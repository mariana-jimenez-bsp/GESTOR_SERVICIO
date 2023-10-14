using BSP.POS.DATOS.Licencias;
using BSP.POS.UTILITARIOS.Licencias;
using clSeguridad;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;

namespace BSP.POS.NEGOCIOS.Licencias
{
    public class N_Licencias
    {
        D_Licencias licencias = new D_Licencias();
        Cryptografia _Cryptografia = new Cryptografia();
        public string ObtenerDatosDeLicencia()
        {
            try
            {
                U_Licencia licencia = new U_Licencia();
                U_DatosLicencia datosLicencia = new U_DatosLicencia();
                licencia = licencias.ObtenerDatosDeLicencia();
                string formato = "M/d/yyyy h:mm:ss tt";
                string FechaInicioTemp = _Cryptografia.DecryptString(licencia.FechaInicio, "BSP");
                string FechaFinTemp = _Cryptografia.DecryptString(licencia.FechaFin, "BSP");
                string FechaAvisoTemp = _Cryptografia.DecryptString(licencia.FechaAviso, "BSP");
                string cantidadUsuariosTemp = _Cryptografia.DecryptString(licencia.CantidadUsuarios, "BSP");
                datosLicencia.FechaInicio = DateTime.ParseExact(FechaInicioTemp, formato, CultureInfo.InvariantCulture, DateTimeStyles.None);
                datosLicencia.FechaFin = DateTime.ParseExact(FechaFinTemp, formato, CultureInfo.InvariantCulture, DateTimeStyles.None);
                datosLicencia.FechaAviso = DateTime.ParseExact(FechaAvisoTemp, formato, CultureInfo.InvariantCulture, DateTimeStyles.None);
                datosLicencia.CantidadUsuarios = int.Parse(cantidadUsuariosTemp);
                string MacAddress = _Cryptografia.DecryptString(licencia.MacAddress, "BSP");
                datosLicencia.Pais = _Cryptografia.DecryptString(licencia.Pais, "BSP");
                datosLicencia.CedulaJuridica = _Cryptografia.DecryptString(licencia.CedulaJuridica, "BSP");
                datosLicencia.NombreCliente = _Cryptografia.DecryptString(licencia.NombreCliente, "BSP");
                string MacAddressActual = GetMacAddress();
                if(MacAddress == MacAddressActual)
                {
                    datosLicencia.MacAddressIguales = true;
                }
                string licenciaJson = JsonConvert.SerializeObject(datosLicencia);
                return licenciaJson;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string ActualizarDatosLicencia(U_ActualizarDatosLicencia datosLicencia)
        {
            bool resultado = licencias.ActualizarDatosLicencia(datosLicencia);
            string resultadoString = resultado.ToString();
            return resultadoString;
        }
        public string ObtenerCodigoDeLicenciaYProducto() {
            U_CodigoLicenciaYProducto codigoLicenciaYProducto = new U_CodigoLicenciaYProducto();
            codigoLicenciaYProducto = licencias.ObtenerCodigoDeLicenciaYProducto();
            string codigoLicenciaJson = JsonConvert.SerializeObject(codigoLicenciaYProducto);
            return codigoLicenciaJson;
        }

        public string ObtenerCodigoDeLicenciaDescencriptado()
        {
            string codigoLicencia;
            codigoLicencia = licencias.ObtenerCodigoDeLicenciaDesencriptado();
            return codigoLicencia;
        }

        public int EnviarXMLLicencia(string textoXML)
        {
            int resultado = licencias.EnviarXMLLicencia(textoXML);
            return resultado;
        }

        public string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
        }
    }
}
