// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using testapp.consumer;
using testapp.db;

Console.WriteLine("Starting App...");

var configPath = Path.Combine(Directory.GetCurrentDirectory(), "kafkasettings.properties");

var cloudConfig = File.ReadAllLines(configPath)
        .Where(line => !line.StartsWith("#"))
        .ToDictionary(
            line => line.Substring(0, line.IndexOf('=')),
            line => line.Substring(line.IndexOf('=') + 1));

var clientConfig = new ClientConfig(cloudConfig);
clientConfig.ClientId = "mypc";

await new HostBuilder()
.ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<SequentialConsumerService > (_ => new SequentialConsumerService(clientConfig));
})
.RunConsoleAsync();
