using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testapp.consumer;
using testapp.db;
using testapp.producer;
using testapp.web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<SqliteContext>();

var configPath = Path.Combine(Directory.GetCurrentDirectory(), "kafkasettings.properties");

var cloudConfig = File.ReadAllLines(configPath)
        .Where(line => !line.StartsWith("#"))
        .ToDictionary(
            line => line.Substring(0, line.IndexOf('=')),
            line => line.Substring(line.IndexOf('=') + 1));

var clientConfig = new ClientConfig(cloudConfig);
clientConfig.ClientId = "mypc";

//builder.Services.AddSingleton<ParallelProducer>(new ParallelProducer(clientConfig));
builder.Services.AddSingleton<SequentialProducer>(new SequentialProducer(clientConfig));

var app = builder.Build();

app.MapGet("/getMessages", ([FromServices] SqliteContext db) =>
{
    var messages = db.Messages.ToList();

    return Results.Json(messages);
});

app.MapPost("/sendMessage", (
    [FromBody] int messagesSent,
    [FromServices] SqliteContext db) =>
{
    var sequentialProducer = app.Services.GetRequiredService<SequentialProducer>();
    sequentialProducer.Send("SequentialTopic", $"Message {messagesSent}");
    var messages = db.Messages.ToList();
    var message = messages.LastOrDefault();

    return Results.Json(message);
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
