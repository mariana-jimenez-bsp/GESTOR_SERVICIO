
using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing.Template;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.CorreosService
{
    public class CorreosService : ICorreosInterface
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public CorreosService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void EnviarCorreoRecuperarClave(U_Correo datos, string token, string esquema)
        {
            string PathHtml = "../BSP.POS.NEGOCIOS/CorreosService/CuerposHtml/RecuperarClave.html";
            //string PathHtml = Path.Combine(_hostingEnvironment.ContentRootPath, "CorreosService", "CuerposHtml", "RecuperarClave.html");
            string CuerpoHtml = File.ReadAllText(PathHtml);
            CuerpoHtml = CuerpoHtml.Replace("{{token}}", token)
                           .Replace("{{esquema}}", esquema);
            var correo = new MimeMessage();
            correo.From.Add(MailboxAddress.Parse(datos.correoUsuario));
            correo.To.Add(MailboxAddress.Parse(datos.para));
            correo.Subject = "Solcitud de Recuperación de clave";
            correo.Body = new TextPart(TextFormat.Html) { Text = CuerpoHtml };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(datos.correoUsuario, datos.claveUsuario);
            smtp.Send(correo);
            smtp.Disconnect(true);

        }
        public void EnviarCorreoAprobarInforme(U_Correo datos, mObjetosParaCorreoAprobacion objetosParaAprobacion)
        {

            foreach (var item in objetosParaAprobacion.listadeUsuariosDeClienteDeInforme)
            {
                if (item.aceptacion == "0")
                {
                    var correo = new MimeMessage();

                correo.From.Add(MailboxAddress.Parse(datos.correoUsuario));
                correo.To.Add(MailboxAddress.Parse("juanramirez1881@gmail.com"));
                correo.Subject = "Solicitud de Aprobación de Informe #" + objetosParaAprobacion.informe.consecutivo;
                string usuarios = "";
                string actividades = "";
                string observaciones = "";
                foreach (var itemUsuario in objetosParaAprobacion.listadeUsuariosDeClienteDeInforme)
                {
                    usuarios += "<tr>\r\n <td>" + itemUsuario.nombre_usuario + "</td>\r\n <td>" + itemUsuario.departamento_usuario + "</td>\r\n </tr> \r\n";
                }
                foreach (var itemActividad in objetosParaAprobacion.listaActividadesAsociadas)
                {
                    actividades += "<tr>\r\n <td>" + itemActividad.nombre_actividad + "</td>\r\n <td>" + itemActividad.horas_cobradas + "</td>\r\n <td>" + itemActividad.horas_no_cobradas + "</td>\r\n </tr> \r\n";
                }
                foreach (var itemObservacion in objetosParaAprobacion.listaDeObservaciones)
                {
                    observaciones += "<tr>\r\n <td>" + itemObservacion.usuario + "</td>\r\n <td>" + itemObservacion.observacion + "</td>\r\n </tr> \r\n";
                }
                    string PathHtml = "../BSP.POS.NEGOCIOS/CorreosService/CuerposHtml/AprobarInforme.html";
                    //string PathHtml = Path.Combine(_hostingEnvironment.ContentRootPath, "CorreosService", "CuerposHtml", "AprobarInforme.html");
                    string CuerpoHtml = File.ReadAllText(PathHtml);
                    CuerpoHtml = CuerpoHtml.Replace("{{token}}", item.token)
                           .Replace("{{esquema}}", objetosParaAprobacion.esquema)
                           .Replace("{{Fecha}}", objetosParaAprobacion.informe.fecha_consultoria)
                           .Replace("{{Hora_Inicio}}", objetosParaAprobacion.informe.hora_inicio.Substring(0, 5))
                           .Replace("{{Modalidad}}", objetosParaAprobacion.informe.modalidad_consultoria)
                           .Replace("{{Hora_Fin}}", objetosParaAprobacion.informe.hora_final.Substring(0, 5))
                           .Replace("{{Cliente}}", objetosParaAprobacion.ClienteAsociado.NOMBRE)
                           .Replace("{{Total_Horas_Cobradas}}", objetosParaAprobacion.total_horas_cobradas.ToString())
                           .Replace("{{Total_Horas_No_Cobradas}}", objetosParaAprobacion.total_horas_no_cobradas.ToString())
                           .Replace("{{Usuarios_Cliente}}", usuarios)
                           .Replace("{{Actividades}}", actividades)
                           .Replace("{{Observaciones}}", observaciones);
                correo.Body = new TextPart(TextFormat.Html) { Text = CuerpoHtml };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(datos.correoUsuario, datos.claveUsuario);
                smtp.Send(correo);
                smtp.Disconnect(true);
                break;
                }
            }
           
        }
    }
}
