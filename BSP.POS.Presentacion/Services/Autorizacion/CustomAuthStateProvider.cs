
using static System.Net.WebRequestMethods;
using System.Net;
using System.Text.Json;
using System.Text;
using BSP.POS.Presentacion.Models;
using Blazored.LocalStorage;
using BSP.POS.Presentacion.Services.Usuarios;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;

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
            string tokenSinComillas = token.Trim('"');
            var identify = new ClaimsIdentity();

            
            if (token != null)
            {
                string validarToken = await ValidarToken(token);

                if (validarToken != null)
                {
                    identify = new ClaimsIdentity(ParseClaimFromJwt(tokenSinComillas), "jwt");


                }
            }
            var user = new ClaimsPrincipal(identify);
            var state = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }
        public async Task<string> ValidarToken(string token)
        {
            mLogin enviarUsuario = new mLogin();
            enviarUsuario.token = token;
            string url = "https://localhost:7032/api/Usuarios/ValidarToken";
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

           return await _localStorageService.GetItemAsStringAsync("token");
            
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
