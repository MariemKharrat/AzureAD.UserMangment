using AzureAD.UserMangment.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureAD.UserMangment
{
    public class GraphClient
    {

        public string clientId;
        public string clientSecret;
        public string tenant;
        private readonly AuthenticationContext authContext;
        private readonly ClientCredential credential;

        public GraphClient(string clientId, string clientSecret, string tenant)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.tenant = tenant;
            this.authContext = new AuthenticationContext("https://login.microsoftonline.com/" + tenant);
            this.credential = new ClientCredential(clientId, clientSecret);

        }
        public async Task<string> GetAllUsers(string query = "")
        {
            var rest = await SendGraphGetRequest("/users", query);
            return rest;
        }

        public async Task<string> CreateUser(User u)
        {
            var json = JsonConvert.SerializeObject(u);
            return await SendGraphPostRequest("/users", json);
        }
        public async Task<string> EnableUser(string objectId)
        {
            string json = "{\n  \"accountEnabled\":\"true\"\n}";
            return await SendGraphPatchRequest("/users/" + objectId, json);
        }
        public async Task<string> DisableUser(string objectId)
        {
            string json = "{\n  \"accountEnabled\":\"false\"\n}";
            return await SendGraphPatchRequest("/users/" + objectId, json);
        }

        public async Task<string> UpdateUser(string objectId, string json)
        {
            return await SendGraphPatchRequest("/users/" + objectId, json);
        }
        public async Task<string> DeleteUser(string objectId)
        {
            return await SendGraphDeleteRequest("/users/" + objectId);
        }


        private async Task<string> SendGraphDeleteRequest(string api)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Globals.aadGraphResourceId, credential);
            HttpClient http = new HttpClient();
            string url = Globals.aadGraphEndpoint + tenant + api + "?" + Globals.aadGraphVersion;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage response = await http.SendAsync(request);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("DELETE " + url);
            Console.WriteLine("Authorization: Bearer " + result.AccessToken.Substring(0, 80) + "...");
            Console.WriteLine("");

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            Console.WriteLine("");

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> SendGraphPatchRequest(string api, string json)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Globals.aadGraphResourceId, credential);
            HttpClient http = new HttpClient();
            string url = Globals.aadGraphEndpoint + tenant + api + "?" + Globals.aadGraphVersion;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("PATCH " + url);
            Console.WriteLine("Authorization: Bearer " + result.AccessToken.Substring(0, 80) + "...");
            Console.WriteLine("Content-Type: application/json");
            Console.WriteLine("");
            Console.WriteLine(json);
            Console.WriteLine("");

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            Console.WriteLine("");

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> SendGraphPostRequest(string api, string json)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            AuthenticationResult result = await authContext.AcquireTokenAsync(Globals.aadGraphResourceId, credential);
            HttpClient http = new HttpClient();
            string url = Globals.aadGraphEndpoint + tenant + api + "?" + Globals.aadGraphVersion;


            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            Console.WriteLine("");

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> SendGraphGetRequest(string api, string query)
        {
            // First, use ADAL to acquire a token using the app's identity (the credential)
            // The first parameter is the resource we want an access_token for; in this case, the Graph API.
            AuthenticationResult result = await authContext.AcquireTokenAsync("https://graph.windows.net", credential);

            // For B2C user managment, be sure to use the 1.6 Graph API version.
            HttpClient http = new HttpClient();
            string url = "https://graph.windows.net/" + tenant + api + "?" + Globals.aadGraphVersion;
            if (!string.IsNullOrEmpty(query))
            {
                url += "&$filter=userPrincipalName eq '" + query + "'";
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("GET " + url);
            Console.WriteLine("Authorization: Bearer " + result.AccessToken.Substring(0, 80) + "...");
            Console.WriteLine("");

            // Append the access token for the Graph API to the Authorization header of the request, using the Bearer scheme.
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            Console.WriteLine("");
            var res = await response.Content.ReadAsStringAsync();
            var users = new List<User>();
            

            GraphResponse user = JsonConvert.DeserializeObject<GraphResponse>(res);
            var a = JsonConvert.SerializeObject(user.value);
            users = JsonConvert.DeserializeObject<List<User>>(a);
            var b = JsonConvert.SerializeObject(users);
            return b;


        }


    }

}
