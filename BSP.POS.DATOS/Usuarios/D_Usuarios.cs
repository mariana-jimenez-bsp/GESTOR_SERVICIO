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
                    U_Perfil perf = new U_Perfil(item.Id, item.codigo, item.cod_cliente, item.usuario, item.correo, item.clave, item.nombre, item.rol, item.telefono, item.esquema);
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
                    var response = sp.GetData(pPerfil.id, pPerfil.codigo, pPerfil.usuario, pPerfil.correo, pPerfil.clave, pPerfil.nombre, pPerfil.rol, pPerfil.telefono, pPerfil.cod_cliente, pPerfil.esquema);

                
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
                    U_Perfil perf = new U_Perfil(item.Id,item.codigo, item.cod_cliente, item.usuario, item.correo, item.clave, item.nombre, item.rol, item.telefono, item.esquema);
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
                    U_Perfil usuario = new U_Perfil(item.Id, item.codigo, item.cod_cliente, item.usuario, item.correo, "", item.nombre, item.rol, item.telefono, item.esquema);

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
                    U_ListaDeUsuariosDeCliente usuario = new U_ListaDeUsuariosDeCliente(item.Id, item.codigo, item.cod_cliente, item.usuario, item.nombre, item.departamento, item.correo, item.telefono);

                    LstUsuarios.Add(usuario);
                }
                if(LstUsuarios != null)
                {
                    return LstUsuarios;
                }
                return new List<U_ListaDeUsuariosDeCliente>();
            

        }

        
        public string ValidarUsuarioExistente(String pEsquema, String pUsuario)
        {

            ValidarUsuarioExistenteTableAdapter sp = new ValidarUsuarioExistenteTableAdapter();

            var response = sp.GetData(pEsquema, pUsuario).ToList();
            string usuario = null;
            foreach (var item in response)
            {
                usuario = item.usuario;
            }


            if (usuario != null)
            {
                return usuario;
            }

            return null;


        }

        public string ValidarExistenciaDeCodigoUsuario(String pEsquema, String pCodigo)
        {

            ValidarExistenciaDeCodigoUsuarioTableAdapter sp = new ValidarExistenciaDeCodigoUsuarioTableAdapter();

            var response = sp.GetData(pEsquema, pCodigo).ToList();
            string codigo = null;
            foreach (var item in response)
            {
                codigo = item.codigo;
            }


            if (codigo != null)
            {
                return codigo;
            }

            return null;


        }

        public string ValidarCorreoExistente(String pEsquema, String pCorreo)
        {

            ValidarCorreoExistenteTableAdapter sp = new ValidarCorreoExistenteTableAdapter();

            var response = sp.GetData(pEsquema, pCorreo).ToList();
            string correo = null;
            foreach (var item in response)
            {
                correo = item.correo;
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
        public U_ListaDeUsuariosDeCliente ObtenerUsuarioDeClientePorCodigo(String pEsquema, String pCodigo)
        {
            var usuario = new U_ListaDeUsuariosDeCliente();

            ObtenerUsuarioDeClientePorCodigoTableAdapter sp = new ObtenerUsuarioDeClientePorCodigoTableAdapter();

            var response = sp.GetData(pEsquema, pCodigo).ToList();

            foreach (var item in response)
            {
                U_ListaDeUsuariosDeCliente user = new U_ListaDeUsuariosDeCliente(item.Id, item.codigo, item.cod_cliente, item.usuario, "", item.departamento, item.correo, item.telefono);
                usuario = user;
            }


            if (usuario != null)
            {
                return usuario;
            }

            return new U_ListaDeUsuariosDeCliente();


        }

        public U_ImagenUsuario ObtenerImagenDeUsuario(String pEsquema, String pUsuario)
        {
            var imagenUsuario = new U_ImagenUsuario();

            ObtenerImagenDeUsuarioTableAdapter sp = new ObtenerImagenDeUsuarioTableAdapter();

            var response = sp.GetData(pEsquema, pUsuario).ToList();

            foreach (var item in response)
            {
                U_ImagenUsuario imagen = new U_ImagenUsuario(item.imagen);
                imagenUsuario = imagen;
            }
            if (imagenUsuario != null)
            {
                return imagenUsuario;
            }
            return new U_ImagenUsuario();

        }

        public List<U_UsuariosDeClienteDeInforme> ObtenerListaDeInformesDeUsuario(String pEsquema, String pCodigo)
        {
            var LstInformes = new List<U_UsuariosDeClienteDeInforme>();

            ObtenerListaDeInformesDeUsuarioDeClienteTableAdapter sp = new ObtenerListaDeInformesDeUsuarioDeClienteTableAdapter();

            var response = sp.GetData(pEsquema, pCodigo).ToList();
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
        public List<U_UsuariosParaEditar> ListarUsuariosParaEditar(String pEsquema)
        {
            var LstUsuarios = new List<U_UsuariosParaEditar>();

            ListarUsuariosParaEditarTableAdapter sp = new ListarUsuariosParaEditarTableAdapter();

            var response = sp.GetData(pEsquema).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_UsuariosParaEditar usuario = new U_UsuariosParaEditar(item.Id, item.codigo, item.cod_cliente, item.usuario, item.correo, item.clave, item.nombre, item.rol, item.telefono, item.departamento, item.imagen, item.esquema);

                    LstUsuarios.Add(usuario);
                }
                return LstUsuarios;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string ActualizarListaDeUsuarios(List<U_UsuariosParaEditar> pUsuarios, string esquema)
        {
            POSDataSet.ActualizarListaDeUsuariosDataTable bTabla = new POSDataSet.ActualizarListaDeUsuariosDataTable();
            ActualizarListaDeUsuariosTableAdapter sp = new ActualizarListaDeUsuariosTableAdapter();
            try
            {
                foreach (var usuario in pUsuarios)
                {
                    if (string.IsNullOrEmpty(usuario.clave))
                    {
                        U_Perfil perf = ObtenerUsuarioPorId(usuario.esquema, usuario.id);
                        usuario.clave = perf.clave;
                    }
                    var response = sp.GetData(esquema, usuario.id, usuario.cod_cliente, usuario.departamento, usuario.usuario, usuario.correo, usuario.clave, usuario.nombre, usuario.rol, usuario.telefono, usuario.imagen);

                }
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }
        public string AgregarUsuario(U_UsuariosParaEditar pUsuario, string esquema)
        {
            POSDataSet.AgregarUsuarioDataTable bTabla = new POSDataSet.AgregarUsuarioDataTable();
            AgregarUsuarioTableAdapter sp = new AgregarUsuarioTableAdapter();
            try
            {
                var response = sp.GetData(pUsuario.cod_cliente, pUsuario.departamento, 
                    pUsuario.usuario, pUsuario.correo, pUsuario.clave, pUsuario.nombre, 
                    pUsuario.rol, pUsuario.telefono, pUsuario.imagen, pUsuario.esquema);


                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public U_UsuariosParaEditar ObtenerUsuarioParaEditar(string pEsquema, string pCodigo)
        {
            var usuarioParEditar = new U_UsuariosParaEditar();

            ObtenerUsuarioParaEditarTableAdapter sp = new ObtenerUsuarioParaEditarTableAdapter();

            var response = sp.GetData(pEsquema, pCodigo).ToList();
            try
            {
                foreach (var item in response)
                {
                    U_UsuariosParaEditar usuario = new U_UsuariosParaEditar(item.Id, item.codigo, item.cod_cliente, item.usuario, item.correo, item.clave, item.nombre, item.rol, item.telefono, item.departamento, item.imagen, item.esquema);

                    usuarioParEditar = usuario;
                }
                return usuarioParEditar;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarUsuario(U_UsuariosParaEditar pUsuario, string esquema)
        {
           
            ActualizarUsuarioTableAdapter sp = new ActualizarUsuarioTableAdapter();
            try
            {

                    if (string.IsNullOrEmpty(pUsuario.clave))
                    {
                        U_Perfil perf = ObtenerUsuarioPorId(pUsuario.esquema, pUsuario.id);
                        pUsuario.clave = perf.clave;
                    }
                    var response = sp.GetData(esquema, pUsuario.id, pUsuario.codigo, pUsuario.cod_cliente, pUsuario.departamento, pUsuario.usuario, pUsuario.correo, pUsuario.clave, pUsuario.nombre, pUsuario.rol, pUsuario.telefono, pUsuario.imagen);

                
                return "Exito";
            }
            catch (Exception)
            {

                return "Error";
            }



        }

        public string ValidarExistenciaEsquema(string pEsquema)
        {
            ValidarExistenciaEsquemaTableAdapter sp = new ValidarExistenciaEsquemaTableAdapter();
            string esquema = null;

            var response = sp.GetData(pEsquema).ToList();

            foreach (var item in response)
            {
                esquema = item.Esquema;
            }

            return esquema;
        }
    }
}

