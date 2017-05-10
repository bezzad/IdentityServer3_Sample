using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using IdentityModel;
using IdentityModel.Extensions;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        const string AuthServerBaseUrl = "http://localhost:5005";
        const string InvalidTokenResponseMsg = "{\"Message\":\"invalid_token\"}";

        static void Main(string[] args)
        {
            var respCarbon = GetUserToken();
            AccessTokenValidation(respCarbon?.AccessToken);
            CallApi(respCarbon);

            Console.WriteLine("\n\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();

            var respSilicon = GetClientToken();
            AccessTokenValidation(respSilicon?.AccessToken);
            CallApi(respSilicon);

            Console.WriteLine("\n\nPlease press any key to exit application!");
            Console.ReadKey();
        }

        //private static TokenResponse RefreshToken(TokenClient token, string refreshToken)
        //{
        //    Console.WriteLine("Using refresh token: {0}", refreshToken);

        //    return token.RequestRefreshTokenAsync(refreshToken).Result;
        //}

        static void CallApi(TokenResponse response)
        {
            var accessToken = response?.AccessToken;
            var client = new HttpClient();
            client.SetBearerToken(accessToken);

            Console.WriteLine("Request for API server by given token...\n");
            Console.Write("Result:  ");
            Console.WriteLine(client.GetStringAsync("http://localhost:14869/test").Result);
            Console.WriteLine();
        }

        static TokenResponse GetClientToken()
        {
            Console.WriteLine("Request to AccessToken for silicon...\n");
            var client = new TokenClient(
                AuthServerBaseUrl + "/connect/token",
                "silicon",
                "F621F470-9731-4A25-80EF-67A6F7C5F4B8");

            var response = client.RequestClientCredentialsAsync("api1").Result;
            ShowResponse(response);
            return response;
        }

        static TokenResponse GetUserToken()
        {
            Console.WriteLine("Request to AccessToken for carbon...\n");
            var client = new TokenClient(
                AuthServerBaseUrl + "/connect/token",
                "carbon",
                "21B5F798-BE55-42BC-8AA8-0025B903DC3B");

            var response = client.RequestResourceOwnerPasswordAsync("bob", "secret", "api1").Result;
            ShowResponse(response);
            return response;
        }

        static void AccessTokenValidation(string accessToken)
        {
            Console.WriteLine("\nValidate Access Token ...\n");
            var client = new HttpClient();
            var validationUrl = AuthServerBaseUrl + "/connect/accesstokenvalidation";
            HttpResponseMessage response = null;
            try
            {
                response = client.PostAsync(validationUrl,
                    new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
                    {
                    new KeyValuePair<string, string>("token", accessToken)
                    })).Result;

                var message = response.Content.ReadAsStringAsync().Result;
                if (message == InvalidTokenResponseMsg)
                {
                    $"\n{message}\n".ConsoleRed();
                }
                else
                {
                    $"\n{message}\n".ConsoleGreen();
                }
            }
            catch (Exception e)
            {
                e.Message.ConsoleRed();
                $"\n{response?.Content?.ReadAsStringAsync()?.Result}\n".ConsoleRed();
            }
        }

        private static void ShowResponse(TokenResponse response)
        {
            if (!response.IsError)
            {
                "Token response:".ConsoleGreen();
                Console.WriteLine(response.Json);

                if (response.AccessToken.Contains("."))
                {
                    "\nAccess Token (decoded):".ConsoleGreen();

                    var parts = response.AccessToken.Split('.');
                    var header = parts[0];
                    var claims = parts[1];

                    Console.WriteLine(JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(header))));
                    Console.WriteLine(JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims))));
                }
            }
            else
            {
                if (response.IsHttpError)
                {
                    "HTTP error: ".ConsoleGreen();
                    Console.WriteLine(response.HttpErrorStatusCode);
                    "HTTP error reason: ".ConsoleGreen();
                    Console.WriteLine(response.HttpErrorReason);
                }
                else
                {
                    "Protocol error response:".ConsoleGreen();
                    Console.WriteLine(response.Json);
                }
            }
        }

    }
}