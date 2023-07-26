using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.Data;
using BSP.POS.UTILITARIOS.Informes;

namespace BSP.POS.DATOS.Usuarios
{
    public class D_Login
    {

        public U_LoginToken Login(U_Login pLogin)
        {
            POSDataSet.LoginUsuarioDataTable _tabla = new POSDataSet.LoginUsuarioDataTable();
            LoginUsuarioTableAdapter _tablaUsuario = new LoginUsuarioTableAdapter();
            ObtenerClaveUsuarioTableAdapter _claveUsuario = new ObtenerClaveUsuarioTableAdapter();
            var consultaClave = _claveUsuario.GetData(pLogin.esquema, pLogin.usuario);
            string claveActual = string.Empty;
            foreach (POSDataSet.ObtenerClaveUsuarioRow item in consultaClave)
            {
                claveActual = item.CLAVE;
            }
            
            U_LoginToken login = null;

            if (CompararClaves(pLogin.contrasena, claveActual))
            {
               string rol = ObtenerRol(pLogin.esquema, pLogin.usuario);
                List<U_PermisosAsociados> permisos = new List<U_PermisosAsociados>();
                permisos = ListaPermisosAsociados(pLogin.esquema, pLogin.usuario);
               string token = GenerateJWT(pLogin.usuario, pLogin.key, rol, permisos, pLogin.esquema);
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
                return null;
            }
            else
            {
                JwtSecurityToken tokenDecodificado = DecodificarToken(token);
                DateTime fecha = tokenDecodificado.ValidTo;
                if (tokenDecodificado.ValidTo < DateTime.UtcNow)
                {
                    return null;
                }
                else
                {
                    return token;
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

        private string GenerateJWT(string username, string key, string rol, List<U_PermisosAsociados> permisos, string esquema)
        {
            // Also consider using AsymmetricSecurityKey if you want the client to be able to validate the token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };
            if (!string.IsNullOrEmpty(rol))
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
            if (!string.IsNullOrEmpty(esquema))
            {
                claims.Add(new Claim("esquema", esquema));
            }
            if (permisos != null)
            {
                foreach (var permiso in permisos)
                {
                    claims.Add(new Claim("permission", permiso.id_permiso));
                }
            }

            var token = new JwtSecurityToken(
                "BSP",
                "Usuarios",
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public string ObtenerRol(String pEsquema, String pUsuario)
        {
            string rol = string.Empty;

            ObtenerRolDeUsuarioTableAdapter sp = new ObtenerRolDeUsuarioTableAdapter();

            var response = sp.GetData(pEsquema, pUsuario).ToList();
            try
            {
                foreach (var item in response)
                {
                    string r = item.ROL;
                    rol = r;
                }
                return rol;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public List<U_PermisosAsociados> ListaPermisosAsociados(String pEsquema, String pId_Usuario)
        {
            var LstPermisos = new List<U_PermisosAsociados>();

            ListarPermisosAsociadosTableAdapter sp = new ListarPermisosAsociadosTableAdapter();

            var response = sp.GetData(pEsquema, pId_Usuario).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_PermisosAsociados permiso = new U_PermisosAsociados(item.Id, item.id_permiso);

                    LstPermisos.Add(permiso);
                }
                return LstPermisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public List<U_Permisos> ListaPermisos(String pEsquema)
        {
            var LstPermisos = new List<U_Permisos>();

            ListarPermisosTableAdapter sp = new ListarPermisosTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_Permisos permiso = new U_Permisos(item.Id, item.permiso);

                    LstPermisos.Add(permiso);
                }
                return LstPermisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
    }
}
