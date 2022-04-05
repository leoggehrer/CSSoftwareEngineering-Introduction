using System;

namespace Introduction.WebApiClient
{
    internal class Program
    {
        private static string BaseUri = "https://localhost:7211/api/";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Access to a RESTful-Service");
            Console.WriteLine();

            await PrintGetResult(BaseUri, $"{nameof(Models.Student)}s");
        }

        public static async Task PrintGetResult(string baseUri, string controller)
        {
            var clientAccess = new RestApi.ClientAccess();
            var models = await clientAccess.GetAsync<Models.Student>(baseUri, controller);

            foreach (var item in models)
            {
                Console.WriteLine($"{item.LastName} {item.FirstName}");
            }
        }
    }
}