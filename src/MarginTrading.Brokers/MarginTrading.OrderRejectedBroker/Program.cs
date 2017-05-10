﻿using Microsoft.AspNetCore.Hosting;

namespace MarginTrading.OrderRejectedBroker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:5003")
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}