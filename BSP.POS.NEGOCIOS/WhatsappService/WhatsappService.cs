using BSP.POS.DATOS.CodigoTelefonoPais;
using BSP.POS.DATOS.Usuarios;
using BSP.POS.UTILITARIOS.Actividades;
using BSP.POS.UTILITARIOS.CodigoTelefonoPais;
using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using BSP.POS.UTILITARIOS.Observaciones;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BSP.POS.NEGOCIOS.WhatsappService
{
    public class WhatsappService : IWhatsappInterface
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        D_CodigoTelefonoPais _codigoTelefonoPais = new D_CodigoTelefonoPais();
        D_Usuarios _usuarios = new D_Usuarios();
        public WhatsappService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task EnviarWhatsappAprobarInforme(mObjetosParaCorreoAprobacion objetosParaAprobacion, string token, string idTelefono, string tipoInicio)
        {
            try
            {
                string pathToJson = "";
                if (tipoInicio == "debug")
                {
                    pathToJson = "../BSP.POS.NEGOCIOS/WhatsappService/MensajesJson/AprobarInforme.json";
                }
                else
                {
                    pathToJson = Path.Combine(_hostingEnvironment.ContentRootPath, "WhatsappService", "MensajesJson", "AprobarInforme.json");
                }
                
                
                // Reemplaza el marcador de posición con el valor real

                foreach (var item in objetosParaAprobacion.listadeUsuariosDeClienteDeInforme)
                {
                    string jsonString = File.ReadAllText(pathToJson);
                    if (item.aceptacion == "0")
                    {
                        U_CodigoTelefonoPaisUsuarios codigoTelefono = new U_CodigoTelefonoPaisUsuarios();
                        U_UsuariosParaEditar usuarioActual = new U_UsuariosParaEditar();
                        usuarioActual = _usuarios.ObtenerUsuarioParaEditar(objetosParaAprobacion.esquema, item.codigo_usuario_cliente);
                        codigoTelefono = _codigoTelefonoPais.ObtenerDatosCodigoTelefonoPaisDeUsuariosPorUsuario(objetosParaAprobacion.esquema, item.codigo_usuario_cliente);
                        //Nuestro telefono
                        string telefono = codigoTelefono.CodigoTelefono + usuarioActual.telefono;
                        string usuarios = "";
                        string actividades = "";
                        string observaciones = "";
                        U_DatosUsuariosDeClienteDeInforme ultimoUsuario = new U_DatosUsuariosDeClienteDeInforme();
                        U_DatosActividadesAsociadas ultimaActividad = new U_DatosActividadesAsociadas();
                        U_DatosObservaciones ultimaObservacion = new U_DatosObservaciones();
                        if (objetosParaAprobacion.listadeUsuariosDeClienteDeInforme.Any())
                        {
                           ultimoUsuario = objetosParaAprobacion.listadeUsuariosDeClienteDeInforme[objetosParaAprobacion.listadeUsuariosDeClienteDeInforme.Count - 1];
                        }

                        if (objetosParaAprobacion.listaActividadesAsociadas.Any())
                        {
                            ultimaActividad = objetosParaAprobacion.listaActividadesAsociadas[objetosParaAprobacion.listaActividadesAsociadas.Count - 1];
                        }
                        if (objetosParaAprobacion.listaDeObservaciones.Any())
                        {
                            ultimaObservacion = objetosParaAprobacion.listaDeObservaciones[objetosParaAprobacion.listaDeObservaciones.Count - 1];
                        }
                        foreach (var itemUsuario in objetosParaAprobacion.listadeUsuariosDeClienteDeInforme)
                        {
                            usuarios += "Nombre: " + itemUsuario.nombre_usuario + " - Departamento: " + itemUsuario.departamento_usuario
                                + " - Rol: " + itemUsuario.rol_usuario + " - Correo: " + itemUsuario.correo_usuario;
                            if(itemUsuario.codigo_usuario_cliente != ultimoUsuario.codigo_usuario_cliente)
                            {
                                usuarios += ", ";
                            }

                        }
                        foreach (var itemActividad in objetosParaAprobacion.listaActividadesAsociadas)
                        {
                            actividades += "Actividad: " + itemActividad.nombre_actividad + " - Horas Cobradas: " + itemActividad.horas_cobradas + " - Horas no Cobradas: " + itemActividad.horas_no_cobradas;
                            if (itemActividad.Id != ultimaActividad.Id)
                            {
                                actividades += ", ";
                            }
                        }
                        foreach (var itemObservacion in objetosParaAprobacion.listaDeObservaciones)
                        {
                            observaciones += "Usuario: " + itemObservacion.nombre_usuario + " - Observación: " + itemObservacion.observacion;
                            if (itemObservacion.Id != ultimaObservacion.Id)
                            {
                                observaciones += ", ";
                            }
                        }
                        jsonString = jsonString.Replace("{telefono}", telefono);
                        jsonString = jsonString.Replace("{token}", item.token)
                                    .Replace("{consecutivo}", objetosParaAprobacion.informe.consecutivo)
                                    .Replace("{esquema}", objetosParaAprobacion.esquema)
                                    .Replace("{Fecha}", objetosParaAprobacion.informe.fecha_consultoria)
                                    .Replace("{Hora_Inicio}", objetosParaAprobacion.informe.hora_inicio.Substring(0, 5))
                                    .Replace("{Modalidad}", objetosParaAprobacion.informe.modalidad_consultoria)
                                    .Replace("{Hora_Fin}", objetosParaAprobacion.informe.hora_final.Substring(0, 5))
                                    .Replace("{Cliente}", objetosParaAprobacion.ClienteAsociado.NOMBRE)
                                    .Replace("{Total_Horas_Cobradas}", objetosParaAprobacion.total_horas_cobradas.ToString())
                                    .Replace("{Total_Horas_No_Cobradas}", objetosParaAprobacion.total_horas_no_cobradas.ToString())
                                    .Replace("{Usuarios_Cliente}", !usuarios.IsNullOrEmpty() ? usuarios : "Sin Usuarios")
                                    .Replace("{Actividades}", !actividades.IsNullOrEmpty() ? actividades : "Sin Actividades")
                                    .Replace("{Observaciones}", !observaciones.IsNullOrEmpty() ? observaciones : "Sin Observaciones")
                                    .Replace("{linkAprobar}", "POS_Prueba_Web_Gestor_Servicios/ValidarAprobacionInforme/" + item.token + "/" + objetosParaAprobacion.esquema)
                                    .Replace("{linkRechazar}", "POS_Prueba_Web_Gestor_Servicios/ValidarRechazoInforme/" + item.token + "/" + objetosParaAprobacion.esquema);

                        JObject jsonObject = JObject.Parse(jsonString);

                        HttpClientHandler clientHandler = new HttpClientHandler();
                        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                        HttpClient client = new HttpClient(clientHandler);
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v17.0/" + idTelefono + "/messages");
                        request.Headers.Add("Authorization", "Bearer " + token);
                        request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = await client.SendAsync(request);
                        //response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        if (tipoInicio == "deploy")
                        {
                            string pathError = Path.Combine(_hostingEnvironment.ContentRootPath, "WhatsappService", "MensajesJson", "TextError.txt");
                            File.WriteAllText(pathError, responseBody);
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if(tipoInicio == "deploy")
                {
                    string pathError = Path.Combine(_hostingEnvironment.ContentRootPath, "WhatsappService", "MensajesJson", "TextError.txt");
                    File.WriteAllText(pathError, ex.ToString());
                    if (ex.InnerException != null)
                    {
                        File.AppendAllText(pathError, "\nInner Exception: " + ex.InnerException.ToString());
                    }
                }

                throw;
            }
          
        }
        
    }
}
