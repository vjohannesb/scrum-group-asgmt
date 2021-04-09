﻿using SharedLibrary.Models.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp.Services
{
    public interface IAPIService
    {
        // Properties (samla alla URL:s på ett ställe)
        public string BaseUrl { get; }
        public string CustomersUrl { get; }
        public string SignInUrl { get; }

        // TOKEN
        // TOKEN

        // API
        /// <summary>
        /// Allomfattande funktion vi kan använda på alla sidor för att skicka requests till API.
        /// </summary>
        /// <param name="method">En HttpMethod som anger POST/GET/PUT/PATCH/DELETE. Ex. HttpMethod.Post</param>
        /// <param name="url">URL:en vi vill skicka requesten till. Ex. https://localhost:44306/api/customers/signin </param>
        /// <param name="serializeContent">Om det är något objekt vi vill skicka med så gör vi det här, exempelvis _signInModel, så konverteras det och läggs till korrekt i HTTP-Requesten</param>
        /// <param name="auth">Om requesten kräver autentisering (detta ska läggas till senare med Token)</param>
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
    }
}