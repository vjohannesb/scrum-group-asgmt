using Blazored.LocalStorage;
using Newtonsoft.Json;
using SharedLibrary.Models.ViewModels;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    public class APIService : IAPIService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public string BaseUrl => "https://localhost:44306/api";

        public string CustomersUrl => $"{BaseUrl}/customers";
        public string SignInUrl => $"{CustomersUrl}/signin";
        public string RegisterUrl => $"{CustomersUrl}/register";

        public string ProductsUrl => $"{BaseUrl}/products";
        public string ProductModelsUrl => $"{ProductsUrl}/models";

        public APIService(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }


        /* Allomfattande funktion vi kan använda på alla sidor för att skicka requests till API. */
        public async Task<HttpResponseMessage> SendToAPIAsync(HttpMethod method, string url, object serializeContent = null, bool auth = false)
        {
            try
            {
                var request = new HttpRequestMessage(method, url);

                // Lägger till objekt i requesten om vi angett serializeContent
                if (serializeContent != null)
                    request.Content = new StringContent(JsonConvert.SerializeObject(serializeContent), Encoding.UTF8, "application/json");

                // Token
                // if (auth) { ... }

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return response;
            }

            // Behöver förbättras i ett senare skede
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Unhandled exception. " + ex.Message)
                };
            }
        }

        // Helper för att logga in "snyggare"
        public async Task<HttpResponseMessage> SignInAsync(SignInModel model)
        {
            var response = await SendToAPIAsync(HttpMethod.Post, SignInUrl, model);
            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<ResponseModel>();
                await SaveTokenAsync(payload.Result);
            }
            return response;
        }

        private async Task SaveTokenAsync(string token)
            => await _localStorage.SetItemAsync("accessToken", token);

        public async Task<HttpResponseMessage> RegisterAsync(RegisterModel model)
        {
            var response = await SendToAPIAsync(HttpMethod.Post, RegisterUrl, model);
            if (response.IsSuccessStatusCode)
            {
                //var payload = await response.Content.ReadFromJsonAsync<ResponseModel>();
            }
            return response;
        }
    }
}



