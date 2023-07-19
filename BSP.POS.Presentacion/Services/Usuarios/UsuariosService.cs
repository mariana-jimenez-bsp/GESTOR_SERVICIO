using BSP.POS.Presentacion.Interfaces.Usuarios;
using System.Net;
using System.Net.Http.Json;

namespace BSP.POS.Presentacion.Services.Usuarios
{
    public class UsuariosService : IUsuariosInterface
    {
        private readonly HttpClient _http;
        public UsuariosService(HttpClient htpp)
        {
            _http = htpp;
        }
        public async Task<string> RealizarLogin(string USUARIO, string CLAVE)
        {
            var token = await _http.GetAsync("https://localhost:7032/api/Clientes/ObtengaElClienteAsociado/" + USUARIO);
            if (token.StatusCode == HttpStatusCode.OK)
            {
                return await token.Content.ReadFromJsonAsync<string?>();
            }
            return null;
        }
        }
    }

