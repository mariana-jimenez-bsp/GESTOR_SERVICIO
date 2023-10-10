
using static System.Net.WebRequestMethods;
using System.Net;
using System.Text.Json;
using System.Text;
using Blazored.LocalStorage;
using BSP.POS.Presentacion.Services.Usuarios;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Net.Http.Headers;
using BSP.POS.Presentacion.Models.Usuarios;

namespace BSP.POS.Presentacion.Services.Autorizacion
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;
        public CustomAuthStateProvider(HttpClient htpp , ILocalStorageService localStorageService)
        {
            _http = htpp;
            _localStorageService = localStorageService; 
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
   
            string token = await ObtenerToken();
            string esquema = await ObtenerEsquema();

            var identify = new ClaimsIdentity();
            _http.DefaultRequestHeaders.Authorization = null;
            
            if (!string.IsNullOrEmpty(token))
            {
                string tokenSinComillas = token.Trim('"');
                string validarToken = await ValidarToken(token, esquema);

                if (!string.IsNullOrEmpty(validarToken))
                {
                    identify = new ClaimsIdentity(ParseClaimFromJwt(tokenSinComillas), "jwt");
                    _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenSinComillas);
  

                }
            }
            var user = new ClaimsPrincipal(identify);
            var state = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }
        public async Task<string> ValidarToken(string token, string esquema)
        {
            mLogin enviarUsuario = new mLogin();
            enviarUsuario.token = token;
            enviarUsuario.esquema = esquema;
            string url = "Login/ValidarToken";
            string jsonData = JsonSerializer.Serialize(enviarUsuario);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var usuario = await _http.PostAsync(url, content);
            if (usuario.StatusCode == HttpStatusCode.OK)
            {
                await usuario.Content.ReadAsStringAsync();
                return await usuario.Content.ReadAsStringAsync();
            }
            return null;
        }

        private async Task<string> ObtenerToken()
        {
            string token = await _localStorageService.GetItemAsStringAsync("token");
            if(token != null)
            {
                return token;
            }
            else
            {
                return null;
            }
           
            
        }

        private async Task<string> ObtenerEsquema()
        {
            string esquema = await _localStorageService.GetItemAsStringAsync("esquema");
            if (esquema != null)
            {
                return esquema;
            }
            else
            {
                return null;
            }


        }


        public static IEnumerable<Claim> ParseClaimFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        public static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break; 
            }
            return Convert.FromBase64String(base64);
        }

    }


}
