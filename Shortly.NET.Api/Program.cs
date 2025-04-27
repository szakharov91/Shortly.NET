using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Shortly.NET.Api.Requests;
using Shortly.NET.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ILinkStore, InMemoryJsonLinkStore>();
builder.Services.AddCors(
    o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseFileServer();

app.UseCors();

var store = app.Services.GetRequiredService<ILinkStore>();

app.MapGet("/links", () => Results.Json(store.GetAll()));

app.MapPost("/shorten", (LinkRequest req) =>
{
    if(!Uri.TryCreate(req.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("Invalid URL");
    }

    var link = store.Create(req.Url);

    return Results.Created($"/{link.Id}", link);
});

app.MapGet("/{id}", (string id, HttpContext ctx) => 
{ 
    if(!store.TryGet(id, out var link))
    {
        return Results.NotFound();
    }

    store.IncrementHints(id);

    return Results.Redirect(link.OriginalUrl);
});

app.MapPost("/batch-shorten", ([FromBody] IEnumerable<LinkRequest> batch) => 
{
    var result = batch.Select(r => store.Create(r.Url));
    return Results.Ok(result);
});

app.MapFallbackToFile("index.html");

app.Run();
