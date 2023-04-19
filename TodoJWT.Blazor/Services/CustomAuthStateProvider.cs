using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace TodoJWT.Blazor.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
        {
            _localStorage = localStorage;
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await _localStorage.GetItemAsStringAsync("token");

            var identity = new ClaimsIdentity();
            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            }

            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            AuthenticationState state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            /*
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
            */

            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            List<Claim> claims = new List<Claim>();

            foreach (var item in keyValuePairs)
            {
                if (item.Key == ClaimTypes.Role)
                {
                    if (item.Value.ToString().Contains(','))
                    {
                        string[] roles = JsonSerializer.Deserialize<string[]>(item.Value.ToString());

                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(item.Key, role));
                        }
                    }
                    else
                    {
                        claims.Add(new Claim(item.Key, item.Value.ToString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(item.Key, item.Value.ToString()));
                }
            }

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
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
