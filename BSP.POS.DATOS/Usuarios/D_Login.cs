using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Clientes;
using BSP.POS.UTILITARIOS.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace BSP.POS.DATOS.Usuarios
{
    public class D_Login
    {
        public U_LoginToken Login(U_Login pLogin)
        {
            POSDataSet.LoginUsuarioDataTable _tabla = new POSDataSet.LoginUsuarioDataTable();
            LoginUsuarioTableAdapter _tablaUsuario = new LoginUsuarioTableAdapter();

            U_LoginToken login = null;
            string token = GenerateToken(pLogin.usuario, "U", pLogin.key);
            var j = _tablaUsuario.GetData(pLogin.usuario, pLogin.contrasena, pLogin.esquema, token).ToList();

            foreach (POSDataSet.LoginUsuarioRow item in j)
            {
                login = new U_LoginToken(item.TOKEN, item.ESQUEMA, item.USUARIO);
            }
            if (j.Count == 0)
            {
                login = new U_LoginToken("", "", "");
            }

            return login;
        }


        public U_LoginUsuario VerificarUsuarioAdministrador(string pEsquema, string pUsuario)
        {
            VerificarUsuarioConsultorTableAdapter _tablaUsuario = new VerificarUsuarioConsultorTableAdapter();

            U_LoginUsuario login = null;

            var result = _tablaUsuario.GetData(pEsquema, pUsuario).ToList();

            foreach (POSDataSet.VerificarUsuarioConsultorRow item in result)
            {
                login = new U_LoginUsuario(item.USUARIO, item.CLAVE, "");
            }
            if (result.Count == 0)
            {
                login = new U_LoginUsuario("", "", "");
            }
            return login;
        }

        public string GenerateToken(string usuario, string rol, string claveSecreta)
        {
            // Configurar la información del encabezado
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveSecreta));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configurar la información del cuerpo (payload)
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, usuario),
            new Claim(ClaimTypes.Role, rol)
        };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60), // Duración del token en minutos
                SigningCredentials = signingCredentials
            };

            // Crear el token JWT
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }


    }
}
