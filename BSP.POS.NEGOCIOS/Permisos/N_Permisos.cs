using BSP.POS.DATOS.Permisos;

using BSP.POS.UTILITARIOS.Permisos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSP.POS.NEGOCIOS.Permisos
{
    public class N_Permisos
    {
        D_Permisos objetoPermiso = new D_Permisos();

        public string ObtenerListaDePermisos(String pEsquema)
        {
            try
            {
                List<U_Permisos> list = new List<U_Permisos>();

                list = objetoPermiso.ObtenerListaDePermisos(pEsquema);

                string permisos = JsonConvert.SerializeObject(list);
                return permisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }
        public string ObtenerListaDePermisosDeUsuario(String pEsquema, string pCodigo)
        {
            try
            {
                List<U_DatosPermisosDeUsuarios> list = new List<U_DatosPermisosDeUsuarios>();

                list = objetoPermiso.ListaDatosDePermisosDeUsuario(pEsquema, pCodigo);

                string permisos = JsonConvert.SerializeObject(list);
                return permisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ObtenerListaDeSubPermisos(String pEsquema)
        {
            try
            {
                List<U_SubPermisos> list = new List<U_SubPermisos>();

                list = objetoPermiso.ObtenerListaDeSubPermisos(pEsquema);

                string permisos = JsonConvert.SerializeObject(list);
                return permisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ObtenerListaDeSubPermisosDeUsuario(String pEsquema, string pCodigo)
        {
            try
            {
                List<U_DatosSubPermisosDeUsuario> list = new List<U_DatosSubPermisosDeUsuario>();

                list = objetoPermiso.ListaDatosDeSubPermisosDeUsuario(pEsquema, pCodigo);

                string permisos = JsonConvert.SerializeObject(list);
                return permisos;
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error ", ex.InnerException.InnerException);
            }
        }

        public string ActualizarPermisosDeUsuario(List<string> listaPermisos, string codigo, string esquema)
        {
            try
            {
                string mensaje = string.Empty;
                var listaPermisosActuales = objetoPermiso.ListaDatosDePermisosDeUsuario(esquema, codigo);
                var listaSubPermisosActuales = objetoPermiso.ListaDatosDeSubPermisosDeUsuario(esquema, codigo);
                if (listaSubPermisosActuales.Any())
                {
                    objetoPermiso.EliminarSubPermisosDeUsuario(codigo, esquema);
                }
                if (listaPermisosActuales.Any()){
                    objetoPermiso.EliminarPermisosDeUsuario(codigo, esquema);
                }
                if (listaPermisos.Any())
                {
                    List<string> IdPermisos = listaPermisos
                .Select(item => item.Split('-')[0]) // Divide cada cadena en función del guion y toma la parte antes del guion.
                .Distinct() // Elimina elementos duplicados.
                .ToList();
                    foreach (var id in IdPermisos)
                    {
                        string IdPermisoUsuario = objetoPermiso.AgregarPermisoDeUsuario(id, codigo, esquema);
                        foreach (var IdUnico in listaPermisos)
                        {

                            var ids = IdUnico.Split('-');
                            var permisoId = ids[0];
                            var subpermisoId = ids[1];
                            if (id == permisoId)
                            {
                                objetoPermiso.AgregarSubPermisoDeUsuario(IdPermisoUsuario, subpermisoId, esquema);
                            }
                        }
                    }
                }
                
                
                return "Exito";
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error " + ex.Message, ex.InnerException.InnerException);
            }


        }
    }
}
