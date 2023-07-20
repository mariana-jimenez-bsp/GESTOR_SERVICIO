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
using clSeguridad;
using System.Security.Cryptography;

namespace BSP.POS.DATOS.Usuarios
{
    public class D_Login
    {
        Cryptografia _Cryptografia = new Cryptografia();
        public U_LoginToken Login(U_Login pLogin)
        {
            POSDataSet.LoginUsuarioDataTable _tabla = new POSDataSet.LoginUsuarioDataTable();
            LoginUsuarioTableAdapter _tablaUsuario = new LoginUsuarioTableAdapter();
            ObtenerClaveUsuarioTableAdapter _claveUsuario = new ObtenerClaveUsuarioTableAdapter();
            var consultaClave = _claveUsuario.GetData("BSP", pLogin.usuario);
            string claveActual = string.Empty;
            foreach (POSDataSet.ObtenerClaveUsuarioRow item in consultaClave)
            {
                claveActual = item.CLAVE;
            }
            
            U_LoginToken login = null;

            if (CompararClaves(pLogin.contrasena, claveActual))
            {
               
                string token = GenerateToken(pLogin.usuario, "U", pLogin.key);
               var j = _tablaUsuario.GetData(pLogin.usuario, claveActual, pLogin.esquema, token).ToList();
                foreach (POSDataSet.LoginUsuarioRow item in j)
                {
                    login = new U_LoginToken(item.TOKEN, item.ESQUEMA, item.USUARIO);
                }
                if (j.Count == 0)
                {
                    login = new U_LoginToken("", "", "");
                }
            }
            else
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

        // Genera un hash seguro con salting para la contraseña dada
        public string EncriptarClave(string clave)
        {
            byte[] data = Encoding.UTF8.GetBytes(clave);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(data);
                return Convert.ToBase64String(hash);
            }
        }


        public bool CompararClaves(string claveIngresada, string claveGuardada)
        {

                return claveIngresada == claveGuardada;
            
        }

        public string ValidarToken (string token)
        {
            ObtenerUsuarioPorTokenTableAdapter _usuario = new ObtenerUsuarioPorTokenTableAdapter();
            var consultaUsuario = _usuario.GetData("BSP", token);
            string usuario = null;
            foreach (POSDataSet.ObtenerUsuarioPorTokenRow item in consultaUsuario)
            {
                usuario = item.USUARIO;
            }
            if(usuario == null)
            {
                return "El Token no es válido";
            }
            else
            {
                JwtSecurityToken tokenDecodificado = DecodificarToken(token);
                if (tokenDecodificado.ValidTo < DateTime.UtcNow)
                {
                    return "El Token ha expirado";
                }
                else
                {
                    return "Token válido";
                }
            }
            
        }
        public JwtSecurityToken DecodificarToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Decodificar el token
            JwtSecurityToken tokenDecodificado = tokenHandler.ReadJwtToken(token);

            return tokenDecodificado;
        }

    }
}
