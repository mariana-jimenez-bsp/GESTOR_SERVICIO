using BSP.POS.DATOS.Permisos;
using BSP.POS.DATOS.Usuarios;
using BSP.POS.UTILITARIOS.Permisos;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Usuarios
{
    public class N_Login
    {
        D_Login objetoLogin = new D_Login();
        D_Usuarios objetoUsuario = new D_Usuarios();

        public string Login(U_Login pLogin)
        {
            string usuarioJson;
            U_LoginToken login = new U_LoginToken();
            string usuario = objetoLogin.ObtenerUsuarioPorCorreo(pLogin.esquema, pLogin.correo);
            string claveActual = objetoLogin.ConsultarClaveUsuario(pLogin, usuario);
            if (CompararClaves(pLogin.contrasena, claveActual))
            {
                D_Permisos datosPermisos = new D_Permisos();
                D_Usuarios datosUsuarios = new D_Usuarios();
                string rol = objetoLogin.ObtenerRol(pLogin.esquema, usuario);
                U_Perfil perfil = new U_Perfil();
                perfil = datosUsuarios.ObtenerPefil(pLogin.esquema, usuario);
                List<U_PermisosAsociados> permisos = new List<U_PermisosAsociados>();
                List<U_Permisos> todosLosPermisos = new List<U_Permisos>();
                todosLosPermisos = datosPermisos.ListaPermisos(pLogin.esquema);
                permisos = datosPermisos.ListaPermisosAsociados(pLogin.esquema, perfil.id);
                string token = GenerateJWT(usuario, pLogin.key, rol, permisos, todosLosPermisos, pLogin.esquema);
                login = objetoLogin.Login(pLogin, usuario, token);
                usuarioJson = JsonConvert.SerializeObject(login);
                return usuarioJson;
            }
            login = new U_LoginToken("", "", "", "");
            usuarioJson = JsonConvert.SerializeObject(login);
            return usuarioJson;
        }
        public string ValidarToken(string token, string esquema)
        {
            string login;
            login = objetoLogin.ValidarToken(token, esquema);
            if (login != null)
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
            return null;
        }

        public U_TokenRecuperacion EnviarTokenRecuperacion(string pCorreo, string pEsquema)
        {

            U_TokenRecuperacion tokenRecuperacion = new U_TokenRecuperacion();
            string token = GenerarTokenRecuperacion();
            DateTime expira = DateTime.Now.AddDays(1);
            tokenRecuperacion = objetoLogin.EnviarTokenRecuperacion(pCorreo, pEsquema, token, expira);
            if (tokenRecuperacion != null)
            {
                return tokenRecuperacion;
            }
            return new U_TokenRecuperacion();

        }
        public string ValidarTokenRecuperacion(string pEsquema, string pToken)
        {

            U_TokenRecuperacion tokenRecuperacion = new U_TokenRecuperacion();
            tokenRecuperacion = objetoLogin.ValidarTokenRecuperacion(pEsquema, pToken);
            if (tokenRecuperacion != null)
            {
                string tokenRecuperacionJson = JsonConvert.SerializeObject(tokenRecuperacion);
                return tokenRecuperacionJson;
            }

            return JsonConvert.SerializeObject(new U_TokenRecuperacion());

        }
        public string ActualizarClaveDeUsuario(U_UsuarioNuevaClave pUsuario)
        {
            string mensaje = string.Empty;
            mensaje = objetoLogin.ActualizarClaveDeUsuario(pUsuario);
            return mensaje;
        }

        public string AumentarIntentosDeLogin(string esquema, string correo)
        {
            string mensaje = string.Empty;
            mensaje = objetoLogin.AumentarIntentosDeLogin(esquema, correo);
            return mensaje;
        }

        public int ObtenerIntentosDeLogin(string esquema, string correo)
        {
            int intentos = 0;
            intentos = objetoLogin.ObtenerIntentosDeLogin(esquema, correo);
            return intentos;
        }

        public bool CompararClaves(string claveIngresada, string claveGuardada)
        {

            return claveIngresada == claveGuardada;

        }

        private string GenerateJWT(string username, string key, string rol, List<U_PermisosAsociados> permisos, List<U_Permisos> todoLosPermisos, string esquema)
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
            if (todoLosPermisos != null)
            {
                foreach (var permiso in todoLosPermisos)
                {
                    if (permisos.Any(p => p.id_permiso == permiso.Id))
                    {
                        claims.Add(new Claim("permission", permiso.permiso));
                    }
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

        public JwtSecurityToken DecodificarToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Decodificar el token
            JwtSecurityToken tokenDecodificado = tokenHandler.ReadJwtToken(token);

            return tokenDecodificado;
        }

        public string GenerarTokenRecuperacion()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
