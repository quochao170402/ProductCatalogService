using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using ProductCatalogService.Configurations;
using ProductCatalogService.Extensions;
using ProductCatalogService.Middlewares;
using ProductCatalogService.Profiles;
using ProductCatalogService.Services.Grpc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddValidator();
builder.Services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddStorageService(builder.Configuration);
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddAutoMapperProfiles();
builder.Services.AddGrpc();
builder.Services.AddOpenApi();

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

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAntiforgery();
app.MapGrpcService<GrpcProductServiceImpl>();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
