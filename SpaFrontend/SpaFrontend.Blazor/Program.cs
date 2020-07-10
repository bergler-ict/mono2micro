using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SpaFrontend.Blazor.Clients;
using SpaFrontend.Blazor.Services;
using System;
using System.Threading.Tasks;
using Polly.Extensions.Http;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using BulletTrain;

namespace SpaFrontend.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBlazoredLocalStorage();

            var salesServiceUri = builder.Configuration["SalesServiceBaseUrl"];
            BulletTrainConfiguration configuration = new BulletTrainConfiguration()
            {
                ApiUrl = builder.Configuration["BulletTrainApiUrl"],
                EnvironmentKey = builder.Configuration["BulletTrainEnviromentKey"]
            };
            BulletTrainClient bulletClient = new BulletTrainClient(configuration);
            var hasCartNewImplementation = await bulletClient.HasFeatureFlag("cart_new_implementation");
            if (hasCartNewImplementation) {
                salesServiceUri = builder.Configuration["SalesServiceNewImplementation"];
            }
            Console.WriteLine($"featureflag: {salesServiceUri}");

            builder.Services.AddHttpClient<IShoppingBasketClient, ShoppingBasketClient>(client =>
            {
                client.BaseAddress = new Uri(salesServiceUri);
            }).AddPolicyHandler(request => request.Method == HttpMethod.Get ? HttpClientResiliency.RetryPolicy() : HttpClientResiliency.NoOpPolicy());

            builder.Services.AddHttpClient<IProductClient, ProductClient>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ProductServiceBaseUrl"]);
            }).AddPolicyHandler(request => request.Method == HttpMethod.Get ? HttpClientResiliency.RetryPolicy() : HttpClientResiliency.NoOpPolicy());

            // ShoppingBasketService must be a singleton, but the ILocalStorage dependency
            // is scoped, which doesn't work together. Fortunately for us, AddScoped() in
            // Blazor Wasm is actually the same as singleton.
            // Refer to: https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-3.1#service-lifetime
            builder.Services.AddScoped<IShoppingBasketService, ShoppingBasketService>(); 
            builder.Services.AddScoped<IProductsService, ProductsService>();



            //builder.Services.AddSingleton(bulletClient);
            await builder.Build().RunAsync();
        }
    }
}
