﻿using BSP.POS.UTILITARIOS.Correos;
using BSP.POS.UTILITARIOS.CorreosModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.WhatsappService
{
    public class WhatsappService : IWhatsappInterface
    {
        public async Task EnviarWhatsappAprobarInforme(mObjetosParaCorreoAprobacion objetosParaAprobacion)
        {
            try
            {
                string pathToJson = "../BSP.POS.NEGOCIOS/WhatsappService/MensajesJson/AprobarInforme.json";
                string jsonString = File.ReadAllText(pathToJson);

                // Reemplaza el marcador de posición con el valor real
               
               
                string token = "EAAC76rdPRMABOwt6p5ry4kMHWTDdWnvwXjsdzm1RcjTWkZBZBzEYeGVTV5La6yRHHk1ZAEZCl94Pdy97Udvq1HtUmPGEI0dlNDLgtFn7HXO94e719p2ZBrcopJTAbyUtOKoYbJA3ZAdCfZB95RNclIk19jlL6MRWTTqOXKfXpZAmeGZAAmrXo3wou3mZBZAo1Sti5KcRKTeZA2Hg6qbCigQ5XP9DPM70iqZAiMAZDZD";
                //Identificador de número de teléfono
                string idTelefono = "110251098843177";
                foreach (var item in objetosParaAprobacion.listadeUsuariosDeClienteDeInforme)
                {
                    if (item.aceptacion == "0")
                    {
                        //Nuestro telefono
                        string telefono = "50671417642";
                        string usuarios = "";
                        string actividades = "";
                        string observaciones = "";
                        foreach (var itemUsuario in objetosParaAprobacion.listadeUsuariosDeClienteDeInforme)
                        {
                            usuarios += "\\nNombre: " + itemUsuario.nombre_usuario + " - Departamento: " + itemUsuario.departamento_usuario;
                        }
                        foreach (var itemActividad in objetosParaAprobacion.listaActividadesAsociadas)
                        {
                            actividades += "\\nActividad: " + itemActividad.nombre_actividad + " - Horas Cobradas: " + itemActividad.horas_cobradas + " - Horas no Cobradas: " + itemActividad.horas_no_cobradas;
                        }
                        foreach (var itemObservacion in objetosParaAprobacion.listaDeObservaciones)
                        {
                            observaciones += "\\nUsuario: " + itemObservacion.usuario + " - Observación: " + itemObservacion.observacion;
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
                                    .Replace("{Usuarios_Cliente}", usuarios)
                                    .Replace("{Actividades}", actividades)
                                    .Replace("{Observaciones}", observaciones);

                        JObject jsonObject = JObject.Parse(jsonString);
                        HttpClient client = new HttpClient();
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v17.0/" + idTelefono + "/messages");
                        request.Headers.Add("Authorization", "Bearer " + token);
                        request.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        HttpResponseMessage response = await client.SendAsync(request);
                        //response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        break;
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
