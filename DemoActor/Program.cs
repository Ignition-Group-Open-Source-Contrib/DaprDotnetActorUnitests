// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using Dapr.Actors.AspNetCore;
using Dapr.Actors.Runtime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DemoActor
{
    /// <summary>
    /// Class for host.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates a IWebHostBuilder.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>IWebHostBuilder instance.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseActors(actorRuntime =>
                        {
                            actorRuntime.RegisterActor<DemoActor>(information => 
                                new ActorService(information, (service, id) => new DemoActor(service,id, new HelloDaprWorld())));
                        });
                });
    }
}
