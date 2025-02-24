using Microsoft.AspNetCore.Http.Features;
using MongoDB.Driver;
using ProductCatalogService.Endpoints;
using ProductCatalogService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddStorageService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });

}
app.UseAntiforgery();

app.UseHttpsRedirection();
app.MapProductServiceEndpoints();

app.Run();
