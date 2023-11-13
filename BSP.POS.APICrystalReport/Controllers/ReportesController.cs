using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using CrystalDecisions.Shared;
using System.Data.Odbc;
using System.Web.Hosting;
using BSP.POS.APICrystalReport.Models.Reportes;
using System.Web.UI.WebControls;

namespace BSP.POS.APICrystalReport.Controllers
{
    public class ReportesController : ApiController
    {
        Prueba_Gestor_ServiciosEntities5 ModelDb = new Prueba_Gestor_ServiciosEntities5();
        [HttpGet]
        [Route("Api/GenerarReporte/{esquema}/{consecutivo}")]
        public HttpResponseMessage GenerarReporte(string esquema, string consecutivo)
        {

            try
            {
                var resultado = ModelDb.GenerarReporteDeInformePorConsecutivo(consecutivo, esquema).ToList();
                ReportDocument report = new ReportDocument();

                string path = HostingEnvironment.MapPath("~/Reportes/ReporteInforme.rpt");
                report.Load(path);
                report.SetDataSource(resultado);

                Stream stream = report.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "ReporteInforme.pdf";
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                return response;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
