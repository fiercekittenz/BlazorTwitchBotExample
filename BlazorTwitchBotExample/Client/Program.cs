// Blazor Twitch Bot Example by twitch.tv/fiercekittenz
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorTwitchBotExample.Client
{
   public class Program
   {
      public static async Task Main(string[] args)
      {
         //
         // This is mostly boilerplate code that comes with the default project setup.
         // You should not need to change or add anything here unless you are going
         // to use other kinds of systems or components that require setup.
         //

         var builder = WebAssemblyHostBuilder.CreateDefault(args);
         builder.RootComponents.Add<App>("app");

         builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

         await builder.Build().RunAsync();
      }
   }
}
