using BSP.POS.UTILITARIOS.Correos;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.CorreosService
{
    public class CorreosService : ICorreosInterface
    {
        public void EnviarCorreo(U_Correo datos, string token, string esquema)
        {
            var correo = new MimeMessage();
            string cuerpo = GenerarCuerpo(token, esquema);
            correo.From.Add(MailboxAddress.Parse("jose.sanchez.bsp@gmail.com"));
            correo.To.Add(MailboxAddress.Parse(datos.para));
            correo.Subject = "Esto es una prueba";
            correo.Body = new TextPart(TextFormat.Html) { Text = cuerpo };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("jose.sanchez.bsp@gmail.com", "bgstfdotixqgcuba");
            smtp.Send(correo);
            smtp.Disconnect(true);

        }

        public string GenerarCuerpo(string token, string esquema)
        {
            string cuerpo = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <title>Gestor de servicios BSP Recuperación de contraseña</title>\r\n</head>\r\n<body style=\"font-family: Arial, sans-serif; background-color: #f0f0f0;\">\r\n    <table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" align=\"center\" style=\"max-width: 600px; margin: 0 auto; background-color: #ffffff;\">\r\n        <tr>\r\n            <td style=\"padding: 20px;\">\r\n                <h1 style=\"color: #198754; text-align: center;\">Gestor de servicios BSP Recuperación de contraseña</h1>\r\n                <p style=\"font-size: 16px; line-height: 1.6; text-align: center;\">Hemos recibido una solicitud para recuperar tu contraseña. Haz clic en el botón de abajo para cambiar tu contraseña.</p>\r\n                <p style=\"font-size: 16px; line-height: 1.6; text-align: center;\"><strong>Si no solicitaste este cambio, puedes ignorar este correo electrónico.</strong></p>\r\n                <table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" style=\"margin-top: 30px;\">\r\n                    <tr>\r\n                        <td style=\"border-radius: 4px; background-color: #198754; text-align: center;\">\r\n                            <a href=\"https://localhost:7200/RecuperarClave/" + token + "/" + esquema + "\" target=\"_blank\" style=\"display: inline-block; padding: 15px 30px; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 4px;\">Recuperar Contraseña</a>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>\r\n";
            return cuerpo;
        }
    }
}
