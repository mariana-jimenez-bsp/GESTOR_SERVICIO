
using BSP.POS.DATOS.Actividades;
using BSP.POS.DATOS.Clientes;
using BSP.POS.DATOS.Informes;
using BSP.POS.DATOS.Observaciones;
using BSP.POS.DATOS.Proyectos;
using BSP.POS.DATOS.Usuarios;
using BSP.POS.UTILITARIOS.Actividades;
using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using BSP.POS.UTILITARIOS.Proyectos;
using BSP.POS.UTILITARIOS.Usuarios;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing.Template;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.CorreosService
{
    public class CorreosService : ICorreosInterface
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        D_Informes _informes = new D_Informes();
        D_Usuarios _usuarios = new D_Usuarios();
        D_Observaciones _observaciones = new D_Observaciones();
        D_Clientes _clientes = new D_Clientes();
        D_Actividades _actividades = new D_Actividades();
        D_Proyectos _proyectos = new D_Proyectos();
        public CorreosService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void EnviarCorreoRecuperarClave(U_Correo datos, string token, string esquema, string urlWeb, string tipoInicio)
        {
            string PathHtml = "";
            if(tipoInicio == "debug")
            {
                PathHtml = "../BSP.POS.NEGOCIOS/CorreosService/CuerposHtml/RecuperarClave.html";
            }
            else
            {
                PathHtml = Path.Combine(_hostingEnvironment.ContentRootPath, "CorreosService", "CuerposHtml", "RecuperarClave.html");
            }
            string CuerpoHtml = File.ReadAllText(PathHtml);
            CuerpoHtml = CuerpoHtml.Replace("{{token}}", token)
                           .Replace("{{esquema}}", esquema)
                           .Replace("{{UrlWeb}}", urlWeb);
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
        public async Task EnviarCorreosInformes(U_Correo datos, mObjetoParaCorreoInforme objetosParaInforme, string urlWeb, string tipoInicio, string urlApiCristal)
        {
            foreach (var item in objetosParaInforme.listadeUsuariosDeClienteDeInforme)
            {
                if (item.recibido == "0")
                {
                    await EnviarCorreoRecibidoInforme(datos, objetosParaInforme, urlWeb, tipoInicio, urlApiCristal, item);
                }else if (item.recibido == "1")
                {
                    await EnviarCorreoReporteInforme(datos, objetosParaInforme, tipoInicio, urlApiCristal, item);
                }
            }
        }
        public async Task EnviarCorreoRecibidoInforme(U_Correo datos, mObjetoParaCorreoInforme objetosParaInforme, string urlWeb, string tipoInicio, string urlApiCristal, U_DatosUsuariosDeClienteDeInforme usuarioActual)
        {
            byte[] byteReporte = await GenerarReporteDeInforme(objetosParaInforme.esquema, objetosParaInforme.informe.consecutivo, urlApiCristal);
                    var correo = new MimeMessage();

                correo.From.Add(MailboxAddress.Parse(datos.correoUsuario));
                correo.To.Add(MailboxAddress.Parse(usuarioActual.correo_usuario));
                correo.Subject = "Reporte de Informe #" + objetosParaInforme.informe.consecutivo;
                string usuarios = "";
                string actividades = "";
                string observaciones = "";
                foreach (var itemUsuario in objetosParaInforme.listadeUsuariosDeClienteDeInforme)
                {
                    usuarios += "<tr>\r\n <td>" + itemUsuario.nombre_usuario + "</td>\r\n <td>" + itemUsuario.departamento_usuario
                            + "</td>\r\n <td>" + itemUsuario.rol_usuario + "</td>\r\n <td>" + itemUsuario.correo_usuario 
                            + "</td>\r\n </tr> \r\n";
                }
                foreach (var itemActividad in objetosParaInforme.listaActividadesAsociadas)
                {
                    actividades += "<tr>\r\n <td>" + itemActividad.nombre_actividad + "</td>\r\n <td>" + itemActividad.horas_cobradas + "</td>\r\n <td>" + itemActividad.horas_no_cobradas + "</td>\r\n </tr> \r\n";
                }
                foreach (var itemObservacion in objetosParaInforme.listaDeObservaciones)
                {
                    observaciones += "<tr>\r\n <td>" + itemObservacion.nombre_usuario + "</td>\r\n <td>" + itemObservacion.observacion + "</td>\r\n </tr> \r\n";
                }
                    string PathHtml = "";
                    if (tipoInicio == "debug")
                    {
                        PathHtml = "../BSP.POS.NEGOCIOS/CorreosService/CuerposHtml/RecibidoInforme.html";
                    }
                    else
                    {
                        PathHtml = Path.Combine(_hostingEnvironment.ContentRootPath, "CorreosService", "CuerposHtml", "RecibidoInforme.html");
                    }
                   
                    string CuerpoHtml = File.ReadAllText(PathHtml);
                    CuerpoHtml = CuerpoHtml.Replace("{{token}}", usuarioActual.token)
                           .Replace("{{esquema}}", objetosParaInforme.esquema)
                           .Replace("{{consecutivo}}", objetosParaInforme.informe.consecutivo)
                           .Replace("{{Numero_Proyecto}}", objetosParaInforme.numero_proyecto)
                           .Replace("{{Fecha}}", objetosParaInforme.informe.fecha_consultoria)
                           .Replace("{{Hora_Inicio}}", objetosParaInforme.informe.hora_inicio.Substring(0, 5))
                           .Replace("{{Modalidad}}", objetosParaInforme.informe.modalidad_consultoria)
                           .Replace("{{Hora_Fin}}", objetosParaInforme.informe.hora_final.Substring(0, 5))
                           .Replace("{{Cliente}}", objetosParaInforme.ClienteAsociado.NOMBRE)
                           .Replace("{{Total_Horas_Cobradas}}", objetosParaInforme.total_horas_cobradas.ToString())
                           .Replace("{{Total_Horas_No_Cobradas}}", objetosParaInforme.total_horas_no_cobradas.ToString())
                           .Replace("{{Usuarios_Cliente}}", usuarios)
                           .Replace("{{Actividades}}", actividades)
                           .Replace("{{Observaciones}}", observaciones)
                           .Replace("{{UrlWeb}}", urlWeb);
                var cuerpoHtml = new TextPart(TextFormat.Html)
                {
                    Text = CuerpoHtml
                };
                
                var attachment = new MimePart
                {
                    Content = new MimeContent(new MemoryStream(byteReporte), ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "ReporteInforme_" + objetosParaInforme.informe.consecutivo + ".pdf"
                };
                var multipart = new Multipart("mixed");
                multipart.Add(cuerpoHtml); // Agregar el cuerpo HTML
                multipart.Add(attachment); // Agregar el archivo adjunto

                correo.Body = multipart;

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(datos.correoUsuario, datos.claveUsuario);
                smtp.Send(correo);
                smtp.Disconnect(true);

           
        }

        public async Task EnviarCorreoReporteInforme(U_Correo datos, mObjetoParaCorreoInforme objetosParaInforme, string tipoInicio, string urlApiCristal, U_DatosUsuariosDeClienteDeInforme usuarioActual)
        {
            byte[] byteReporte = await GenerarReporteDeInforme(objetosParaInforme.esquema, objetosParaInforme.informe.consecutivo, urlApiCristal);
            
                    var correo = new MimeMessage();

                    correo.From.Add(MailboxAddress.Parse(datos.correoUsuario));
                    correo.To.Add(MailboxAddress.Parse(usuarioActual.correo_usuario));
                    correo.Subject = "Reenvío de Reporte de Informe #" + objetosParaInforme.informe.consecutivo;
                    string usuarios = "";
                    string actividades = "";
                    string observaciones = "";
                    foreach (var itemUsuario in objetosParaInforme.listadeUsuariosDeClienteDeInforme)
                    {
                        usuarios += "<tr>\r\n <td>" + itemUsuario.nombre_usuario + "</td>\r\n <td>" + itemUsuario.departamento_usuario
                                + "</td>\r\n <td>" + itemUsuario.rol_usuario + "</td>\r\n <td>" + itemUsuario.correo_usuario
                                + "</td>\r\n </tr> \r\n";
                    }
                    foreach (var itemActividad in objetosParaInforme.listaActividadesAsociadas)
                    {
                        actividades += "<tr>\r\n <td>" + itemActividad.nombre_actividad + "</td>\r\n <td>" + itemActividad.horas_cobradas + "</td>\r\n <td>" + itemActividad.horas_no_cobradas + "</td>\r\n </tr> \r\n";
                    }
                    foreach (var itemObservacion in objetosParaInforme.listaDeObservaciones)
                    {
                        observaciones += "<tr>\r\n <td>" + itemObservacion.nombre_usuario + "</td>\r\n <td>" + itemObservacion.observacion + "</td>\r\n </tr> \r\n";
                    }
                    string PathHtml = "";
                    if (tipoInicio == "debug")
                    {
                        PathHtml = "../BSP.POS.NEGOCIOS/CorreosService/CuerposHtml/ReporteInforme.html";
                    }
                    else
                    {
                        PathHtml = Path.Combine(_hostingEnvironment.ContentRootPath, "CorreosService", "CuerposHtml", "ReporteInforme.html");
                    }

                    string CuerpoHtml = File.ReadAllText(PathHtml);
                    CuerpoHtml = CuerpoHtml
                           .Replace("{{consecutivo}}", objetosParaInforme.informe.consecutivo)
                           .Replace("{{Numero_Proyecto}}", objetosParaInforme.numero_proyecto)
                           .Replace("{{Fecha}}", objetosParaInforme.informe.fecha_consultoria)
                           .Replace("{{Hora_Inicio}}", objetosParaInforme.informe.hora_inicio.Substring(0, 5))
                           .Replace("{{Modalidad}}", objetosParaInforme.informe.modalidad_consultoria)
                           .Replace("{{Hora_Fin}}", objetosParaInforme.informe.hora_final.Substring(0, 5))
                           .Replace("{{Cliente}}", objetosParaInforme.ClienteAsociado.NOMBRE)
                           .Replace("{{Total_Horas_Cobradas}}", objetosParaInforme.total_horas_cobradas.ToString())
                           .Replace("{{Total_Horas_No_Cobradas}}", objetosParaInforme.total_horas_no_cobradas.ToString())
                           .Replace("{{Usuarios_Cliente}}", usuarios)
                           .Replace("{{Actividades}}", actividades)
                           .Replace("{{Observaciones}}", observaciones);
                    var cuerpoHtml = new TextPart(TextFormat.Html)
                    {
                        Text = CuerpoHtml
                    };

                    var attachment = new MimePart
                    {
                        Content = new MimeContent(new MemoryStream(byteReporte), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = "ReporteInforme_" + objetosParaInforme.informe.consecutivo + ".pdf"
                    };
                    var multipart = new Multipart("mixed");
                    multipart.Add(cuerpoHtml); // Agregar el cuerpo HTML
                    multipart.Add(attachment); // Agregar el archivo adjunto

                    correo.Body = multipart;

                    using var smtp = new SmtpClient();
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate(datos.correoUsuario, datos.claveUsuario);
                    smtp.Send(correo);
                    smtp.Disconnect(true);


        }

        public mObjetoParaCorreoInforme CrearObjetoDeCorreo(string esquema, string consecutivo)
        {
            mObjetoParaCorreoInforme objetoParaCorreo = new mObjetoParaCorreoInforme();
            objetoParaCorreo.informe = _informes.ObtenerInforme(esquema, consecutivo);
            objetoParaCorreo.listaActividadesAsociadas = _actividades.ListaDatosActividadesAsociadas(esquema, consecutivo);
            try
            {
                objetoParaCorreo.total_horas_cobradas = objetoParaCorreo.listaActividadesAsociadas.Sum(act => int.Parse(act.horas_cobradas));
                objetoParaCorreo.total_horas_no_cobradas = objetoParaCorreo.listaActividadesAsociadas.Sum(act => int.Parse(act.horas_no_cobradas));
            }
            catch (Exception)
            {
                objetoParaCorreo.total_horas_cobradas = 0;
                objetoParaCorreo.total_horas_no_cobradas = 0;
            }
            objetoParaCorreo.listadeUsuariosDeClienteDeInforme = _usuarios.ListaDatosUsuariosDeClienteDeInforme(esquema, consecutivo);
            U_ListaProyectos proyecto =  _proyectos.ObtenerProyecto(esquema, objetoParaCorreo.informe.numero_proyecto);
            if(proyecto != null)
            {
                objetoParaCorreo.numero_proyecto = proyecto.numero;
                objetoParaCorreo.codigo_cliente = proyecto.codigo_cliente;
                objetoParaCorreo.ClienteAsociado = _clientes.ClienteAsociado(esquema, objetoParaCorreo.codigo_cliente);
            }
            
            objetoParaCorreo.esquema = esquema;
            objetoParaCorreo.listaDeObservaciones = _observaciones.ListarDatosObservacionesDeInforme(consecutivo, esquema);
            return objetoParaCorreo;
        }

        public async Task<byte[]> GenerarReporteDeInforme(string esquema, string consecutivo, string urlApiCristal)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                using (HttpClient client = new HttpClient(clientHandler))
                {
                    var response = await client.GetAsync(urlApiCristal + "Api/GenerarReporte/" + esquema + "/" + consecutivo);

                    if (response.IsSuccessStatusCode)
                    {
                        var fileBytes = await response.Content.ReadAsByteArrayAsync();


                        return fileBytes;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
