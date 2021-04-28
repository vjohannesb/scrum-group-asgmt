using SharedLibrary.Models.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    public interface IAPIService
    {
        // Properties (samla alla URL:s på ett ställe)
        public string BaseUrl { get; }

        // Customer URLs
        public string CustomersUrl { get; }
        public string SignInUrl { get; }
        public string RegisterUrl { get; }
        public string ValidateTokenUrl { get; }

        // Product URLs
        public string ProductsUrl { get; }
        public string MultipleProductsUrl { get; }
        public string ProductUrl(int productId);
        public string RelatedProductsUrl(int productId);
        public string TopRatedProductsUrl(int take);
        public string TopSoldProductsUrl(int take);

        // Wishlist URLs
        public string WishlistUrl { get; }
        public string ProductInWishlistUrl(int productId);

        // Order URLs
        public string OrdersUrl { get; }
        public string ShippingMethodsUrl { get; }
        public string PaymentMethodsUrl { get; }

        // Review URLs
        public string ReviewUrl { get; }

        // LocalStorage
        public Task SaveToLocalStorageAsync(string key, string value);
        public Task SaveToLocalStorageAsync<T>(string key, T data);

        public Task<string> GetFromLocalStorageAsync(string key);
        public Task<T> GetFromLocalStorageAsync<T>(string key);


        // API
        /// <summary>
        /// Allomfattande funktion vi kan använda på alla sidor för att skicka requests till API.
        /// </summary>
        /// <param name="method">En HttpMethod som anger POST/GET/PUT/PATCH/DELETE. Ex. HttpMethod.Post</param>
        /// <param name="url">URL:en vi vill skicka requesten till. Ex. https://localhost:44306/api/customers/signin </param>
        /// <param name="serializeContent">Om det är något objekt vi vill skicka med så gör vi det här, exempelvis _signInModel, så konverteras det och läggs till korrekt i HTTP-Requesten</param>
        /// <param name="auth">Om requesten kräver autentisering så lägger den till en Authorization-header med den Token som är sparad i LocalStorage</param>
        /// <example>
        /// <code>
        ///     var response = SendToApiAsync(HttpMethod.Post, SignInUrl, _signInModel);
        ///     if (response.IsSuccessStatusCode) doStuff();
        /// </code>
        /// </example>
        /// <returns><see cref="HttpResponseMessage"/></returns>
        public Task<HttpResponseMessage> SendToAPIAsync(HttpMethod method, string url, object serializeContent = null, bool auth = false);

        // Identity / Customer
        public Task<HttpResponseMessage> SignInAsync(SignInModel model);

        public Task<HttpResponseMessage> RegisterAsync(RegisterModel model);
    }
}
