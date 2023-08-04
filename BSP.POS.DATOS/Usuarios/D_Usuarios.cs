using BSP.POS.DATOS.POSDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSP.POS.UTILITARIOS.Usuarios;
using System.Security.Cryptography;



namespace BSP.POS.DATOS.Usuarios
{
    public class D_Usuarios
    {
        public U_Perfil ObtenerPefil(String pEsquema, String pUsuario)
        {
            var perfil = new U_Perfil();

            ObtenerUsuarioPorNombreTableAdapter sp = new ObtenerUsuarioPorNombreTableAdapter();

            var response = sp.GetData(pEsquema, pUsuario).ToList();

                foreach (var item in response)
                {
                    U_Perfil perf = new U_Perfil(item.ID, item.USUARIO, item.CORREO, item.CLAVE, item.NOMBRE, item.ROL, item.TELEFONO, item.ESQUEMA);
                    perfil = perf;
                }
                if (perfil != null)
                {
                   return perfil;
                }
               return  new U_Perfil();

        }

        public string ActualizarPerfil(U_Perfil pPerfil)
        {
            POSDataSet.ActualizarPerfilDataTable bTabla = new POSDataSet.ActualizarPerfilDataTable();
            ActualizarPerfilTableAdapter sp = new ActualizarPerfilTableAdapter();
            try
            {
                if(string.IsNullOrEmpty(pPerfil.clave))
                {
                    U_Perfil perf= ObtenerUsuarioPorId(pPerfil.esquema, pPerfil.id);
                    pPerfil.clave = perf.clave;
                }
                    var response = sp.GetData(pPerfil.id, pPerfil.usuario, pPerfil.correo, pPerfil.clave, pPerfil.nombre, pPerfil.rol, pPerfil.telefono, pPerfil.esquema);

                
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public U_Perfil ObtenerUsuarioPorId(String pEsquema, String pId)
        {
            var perfil = new U_Perfil();

            ObtenerUsuarioPorIdTableAdapter sp = new ObtenerUsuarioPorIdTableAdapter();

            var response = sp.GetData(pEsquema, pId).ToList();

                foreach (var item in response)
                {
                    U_Perfil perf = new U_Perfil(item.ID, item.USUARIO, item.CORREO, item.CLAVE, item.NOMBRE, item.ROL, item.TELEFONO, item.ESQUEMA);
                    perfil = perf;
                }
                

                if(perfil != null)
                {
                    return perfil;
                }

                return new U_Perfil() ;
                
            
        }
        public List<U_Perfil> ListarUsuarios(String pEsquema)
        {
            var LstUsuarios = new List<U_Perfil>();

            ListarUsuariosTableAdapter sp = new ListarUsuariosTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_Perfil usuario = new U_Perfil(item.ID, item.USUARIO, item.CORREO, "", item.NOMBRE, item.ROL, item.TELEFONO, item.ESQUEMA);

                    LstUsuarios.Add(usuario);
                }
                return LstUsuarios;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public List<U_ListaDeUsuariosDeCliente> ListaDeUsuariosDeClienteAsociados(String pEsquema, string pCliente)
        {
            var LstUsuarios = new List<U_ListaDeUsuariosDeCliente>();

            ListarUsuariosDeClienteAsociadosTableAdapter sp = new ListarUsuariosDeClienteAsociadosTableAdapter();

            var response = sp.GetData(pEsquema, pCliente).ToList();

                foreach (var item in response)
                {
                    U_ListaDeUsuariosDeCliente usuario = new U_ListaDeUsuariosDeCliente(item.Id, item.codigo, item.cod_cliente, item.usuario, item.departamento, item.correo, item.telefono);

                    LstUsuarios.Add(usuario);
                }
                if(LstUsuarios != null)
                {
                    return LstUsuarios;
                }
                return new List<U_ListaDeUsuariosDeCliente>();
            

        }

        public U_TokenRecuperacion EnviarTokenRecuperacion(string pCorreo, string pEsquema)
        {
            GenerarTokenRecuperacionTableAdapter sp = new GenerarTokenRecuperacionTableAdapter();
            string token = GenerarTokenRecuperacion();
            DateTime expira = DateTime.Now.AddDays(1);
            try
            {
                var response = sp.GetData(pCorreo, token, expira, pEsquema).ToList();
                U_TokenRecuperacion TokenRecuperacion = new U_TokenRecuperacion();

                foreach (var item in response)
                {
                    U_TokenRecuperacion tokeRecuperacion = new U_TokenRecuperacion(item.TOKEN_RECUPERACION, pEsquema, item.CORREO, item.FECHA_EXPIRACION_TR.ToString());
                    TokenRecuperacion = tokeRecuperacion;
                }
                if (TokenRecuperacion != null)
                {
                    return TokenRecuperacion;
                }
                return new U_TokenRecuperacion();
            }
            catch (Exception)
            {

                return new U_TokenRecuperacion();
            }
            
           


        }

        public U_TokenRecuperacion ValidarTokenRecuperacion(String pEsquema, String pToken)
        {
            var tokenRecuperacion = new U_TokenRecuperacion();

            ObtenerFechaTokenDeRecuperacionTableAdapter sp = new ObtenerFechaTokenDeRecuperacionTableAdapter();

            var response = sp.GetData(pEsquema, pToken).ToList();

                foreach (var item in response)
                {
                    U_TokenRecuperacion tok = new U_TokenRecuperacion(item.TOKEN_RECUPERACION, pEsquema, "", item.FECHA_EXPIRACION_TR);
                    tokenRecuperacion = tok;
                }
                if (tokenRecuperacion.token_recuperacion != null)
                {
                    DateTime fechaRecuperacion = DateTime.Parse(tokenRecuperacion.fecha_expiracion);
                    if (fechaRecuperacion < DateTime.UtcNow)
                    {

                        return new U_TokenRecuperacion();
                    }
                    else
                    {
                        return tokenRecuperacion;
                    }
                  
                }
                return new U_TokenRecuperacion();

            
        }
        public string ActualizarClaveDeUsuario(U_UsuarioNuevaClave pUsuario)
        {
            POSDataSet.ActualizarClaveDeUsuarioDataTable bTabla = new POSDataSet.ActualizarClaveDeUsuarioDataTable();
            ActualizarClaveDeUsuarioTableAdapter sp = new ActualizarClaveDeUsuarioTableAdapter();
            try
            {
                var response = sp.GetData(pUsuario.token_recuperacion, pUsuario.clave, pUsuario.esquema);


                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
        public string ValidarCorreoExistenteCambioClave(String pEsquema, String pCorreo)
        {

            ValidarCorreoExistenteTableAdapter sp = new ValidarCorreoExistenteTableAdapter();

            var response = sp.GetData(pEsquema, pCorreo).ToList();
            string correo = null;
            foreach (var item in response)
            {
                correo = item.CORREO;
            }


            if (correo != null)
            {
                return correo;
            }

            return null;


        }

        public List<U_UsuariosDeClienteDeInforme> ListaUsuariosDeClienteDeInforme(String pEsquema, String pConsecutivo)
        {
            var LstInformes = new List<U_UsuariosDeClienteDeInforme>();

            ListarUsuariosDeClienteDeInformeTableAdapter sp = new ListarUsuariosDeClienteDeInformeTableAdapter();

            var response = sp.GetData(pEsquema, pConsecutivo).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_UsuariosDeClienteDeInforme informe = new U_UsuariosDeClienteDeInforme(item.Id, item.consecutivo_informe, item.codigo_usuario_cliente, item.aceptacion);

                    LstInformes.Add(informe);
                }
                return LstInformes;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string AgregarUsuarioDeClienteDeInforme(U_UsuariosDeClienteDeInforme pUsuario, string esquema)
        {
            POSDataSet.AgregarUsuarioDeClienteDeInformeDataTable bTabla = new POSDataSet.AgregarUsuarioDeClienteDeInformeDataTable();
            AgregarUsuarioDeClienteDeInformeTableAdapter sp = new AgregarUsuarioDeClienteDeInformeTableAdapter();
            try
            {
                var response = sp.GetData(esquema, pUsuario.consecutivo_informe, pUsuario.codigo_usuario_cliente);


                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
        public string EliminarUsuarioDeClienteDeInforme(string pIdUsuario, string esquema)
        {
            POSDataSet.EliminarUsuarioDeClienteDeInformeDataTable bTabla = new POSDataSet.EliminarUsuarioDeClienteDeInformeDataTable();
            EliminarUsuarioDeClienteDeInformeTableAdapter sp = new EliminarUsuarioDeClienteDeInformeTableAdapter();
            try
            {
                var response = sp.GetData(pIdUsuario, esquema);

                return "Exito";
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error: ", ex.InnerException);
            }



        }
        public string GenerarTokenRecuperacion()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}

