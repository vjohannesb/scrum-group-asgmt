using Blazored.LocalStorage;
using Newtonsoft.Json;
using SharedLibrary.Models.ViewModels;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public string WishlistUrl => $"{BaseUrl}/Wishlist";
        public string SignInUrl => $"{CustomersUrl}/signin";
        public string RegisterUrl => $"{CustomersUrl}/register";

        public string ProductsUrl => $"{BaseUrl}/products";
        public string MultipleProductsUrl => $"{ProductsUrl}/multi";

        public string ShippingMethodsUrl => $"{BaseUrl}/shippingmethods";

        public string AddWishlistUrl => $"{WishlistUrl}/addWishlist";
        public string CheckWishlistUrl => $"{WishlistUrl}/checkWishlist";
        public string DeleteWishlistUrl => $"{WishlistUrl}/deleteWishlist";
        public string ProductModelsWishlistUrl => $"{WishlistUrl}/getWishlist";
        public string reviewModelUrl => $"{ProductsUrl}/registerReview";
        public string changeCustomerNameUrl => $"{ProductsUrl}/ChangeNameCustomer";


        public APIService(ILocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        // Helper för att spara i LocalStorage
        public async Task SaveToLocalStorageAsync(string key, string value)
            => await _localStorage.SetItemAsync(key, value);

        // Overload för objekt, SetItemAsync serializar objektet automagiskt
        public async Task SaveToLocalStorageAsync<T>(string key, T data)
            => await _localStorage.SetItemAsync(key, data);

        // Helper för att hämta från LocalStorage
        public async Task<string> GetFromLocalStorageAsync(string key)
            => await _localStorage.GetItemAsStringAsync(key);

        // Overload för objekt, GetItemAsync de-serializar objektet automagiskt
        public async Task<T> GetFromLocalStorageAsync<T>(string key)
            => await _localStorage.GetItemAsync<T>(key);

        /* Allomfattande funktion vi kan använda på alla sidor för att skicka requests till API. */
        public async Task<HttpResponseMessage> SendToAPIAsync(HttpMethod method, string url, object serializeContent = null, bool auth = false)
        {
            try
            {
                var request = new HttpRequestMessage(method, url);

                // Lägger till objekt i requesten om vi angett serializeContent
                if (serializeContent != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(serializeContent), Encoding.UTF8, "application/json");
                }

                if (auth)
                {
                    var _token = await GetFromLocalStorageAsync("accessToken");
                    if (string.IsNullOrEmpty(_token))
                    {
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
                }

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
                await SaveToLocalStorageAsync("accessToken", payload.Result);
            }
            return response;
        }

        public async Task<HttpResponseMessage> RegisterAsync(RegisterModel model)
        {
            var response = await SendToAPIAsync(HttpMethod.Post, RegisterUrl, model);
            if (response.IsSuccessStatusCode)
            {
                //var payload = await response.Content.ReadFromJsonAsync<ResponseModel>();
            }
            return response;
        }
        //public async Task<HttpResponseMessage> AddToWishlist(int model)
        //{
        //    var response = await SendToAPIAsync(HttpMethod.Post, AddWishlistUrl, model, true);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        //var payload = await response.Content.ReadFromJsonAsync<ResponseModel>();
        //    }
        //    return response;
        //}
        public async Task<bool> checkIfInWishlist(int model)
        {
            var response = await SendToAPIAsync(HttpMethod.Post, CheckWishlistUrl, model);
            if (response.IsSuccessStatusCode)
            {
                return true;
                //var payload = await response.Content.ReadFromJsonAsync<ResponseModel>();
            }
            return false;
        }

        //public async Task<HttpResponseMessage> DeleteFromWishlist(int model)
        //{
        //    var response = await SendToAPIAsync(HttpMethod.Delete, DeleteWishlistUrl, model, true);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        //var payload = await response.Content.ReadFromJsonAsync<ResponseModel>();
        //    }
        //    return response;
        //}
    }
}



