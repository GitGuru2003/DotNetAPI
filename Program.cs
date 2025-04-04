using DotnetAPI.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

var builder = WebApplication.CreateBuilder(args); // This builds the builder to create a new server
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Boilerplate code for CORS
builder.Services.AddCors((options) =>
    {
        options.AddPolicy("DevCors", (corsBuilder) =>
            {
                // :4200 port is for angular, :3000 is for react, :8000 is for vue. This allows the  server to accept requests
                // from these ports. This is only for development purposes. 
                corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
                    .AllowAnyMethod() // We allow any method to be used in the request (GET, POST, PUT, DELETE, etc.)
                    .AllowAnyHeader() // We allow any header to be used in the request (Content-Type, Authorization, etc.)
                    .AllowCredentials(); // We allow credentials to be passed in the request (cookies, etc.)
            });
        options.AddPolicy("ProdCors", (corsBuilder) =>
            {
                // This is for production. We only allow requests from our production site.
                // We can also allow requests from multiple sites by adding more WithOrigins() methods.
                corsBuilder.WithOrigins("https://myProductionSite.com")
                    .AllowAnyMethod() // We allow any method to be used in the request (GET, POST, PUT, DELETE, etc.)
                    .AllowAnyHeader() // We allow any header to be used in the request (Content-Type, Authorization, etc.)
                    .AllowCredentials(); // We allow credentials to be passed in the request (cookies, etc.)
            });
    });

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors"); // We select the DevCors cors policy from our builder above
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors"); // We select the ProdCors cors policy from our builder above
    app.UseHttpsRedirection();
}

app.MapControllers();

// app.MapGet("/weatherforecast", () =>
// {

// })
// .WithName("GetWeatherForecast");

app.Run();


