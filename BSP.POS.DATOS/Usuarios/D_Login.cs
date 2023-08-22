using BSP.POS.DATOS.Permisos;
using BSP.POS.DATOS.POSDataSetTableAdapters;
using BSP.POS.UTILITARIOS.Permisos;
using BSP.POS.UTILITARIOS.Usuarios;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BSP.POS.DATOS.Usuarios
{
    public class D_Login
    {

        public U_LoginToken Login(U_Login pLogin)
        {
            POSDataSet.LoginUsuarioDataTable _tabla = new POSDataSet.LoginUsuarioDataTable();
            LoginUsuarioTableAdapter _tablaUsuario = new LoginUsuarioTableAdapter();
            ObtenerClaveUsuarioTableAdapter _claveUsuario = new ObtenerClaveUsuarioTableAdapter();
            string usuario = ObtenerUsuarioPorCorreo(pLogin.esquema, pLogin.correo);
            var consultaClave = _claveUsuario.GetData(pLogin.esquema, usuario);
            string claveActual = string.Empty;
            foreach (POSDataSet.ObtenerClaveUsuarioRow item in consultaClave)
            {
                claveActual = item.clave;
            }

            U_LoginToken login = null;

            if (CompararClaves(pLogin.contrasena, claveActual))
            {
                D_Permisos datosPermisos = new D_Permisos();
                D_Usuarios datosUsuarios = new D_Usuarios();
                string rol = ObtenerRol(pLogin.esquema, usuario);
                U_Perfil perfil = new U_Perfil();
                perfil = datosUsuarios.ObtenerPefil(pLogin.esquema, usuario);
                List<U_PermisosAsociados> permisos = new List<U_PermisosAsociados>();
                List<U_Permisos> todosLosPermisos = new List<U_Permisos>();
                todosLosPermisos = datosPermisos.ListaPermisos(pLogin.esquema);
                permisos = datosPermisos.ListaPermisosAsociados(pLogin.esquema, perfil.id);
                string token = GenerateJWT(usuario, pLogin.key, rol, permisos, todosLosPermisos, pLogin.esquema);
                var j = _tablaUsuario.GetData(pLogin.correo, claveActual, pLogin.esquema, token).ToList();
                foreach (POSDataSet.LoginUsuarioRow item in j)
                {
                    login = new U_LoginToken(item.token, item.esquema, usuario, item.correo);
                }
                if (j.Count == 0)
                {
                    login = new U_LoginToken("", "", "", "");
                }
            }
            else
            {
                login = new U_LoginToken("", "", "", "");
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

        public string ValidarToken(string token)
        {
            ObtenerUsuarioPorTokenTableAdapter _usuario = new ObtenerUsuarioPorTokenTableAdapter();
            var consultaUsuario = _usuario.GetData("BSP", token);
            string usuario = null;
            foreach (POSDataSet.ObtenerUsuarioPorTokenRow item in consultaUsuario)
            {
                usuario = item.usuario;
            }
            if (usuario == null)
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
        public string ObtenerUsuarioPorCorreo(String pEsquema, String pCorreo)
        {
            string usuario = string.Empty;

            ObtenerUsuarioPorCorreoTableAdapter sp = new ObtenerUsuarioPorCorreoTableAdapter();

            var response = sp.GetData(pEsquema, pCorreo).ToList();

            foreach (var item in response)
            {
                string u = item.usuario;
                usuario = u;
            }
            if (usuario != null)
            {
                return usuario;
            }
            return string.Empty;

        }
        public string ObtenerRol(String pEsquema, String pUsuario)
        {
            string rol = string.Empty;

            ObtenerRolDeUsuarioTableAdapter sp = new ObtenerRolDeUsuarioTableAdapter();

            var response = sp.GetData(pEsquema, pUsuario).ToList();

            foreach (var item in response)
            {
                string r = item.rol;
                rol = r;
            }
            if (rol != null)
            {
                return rol;
            }
            return string.Empty;

        }


    }
}
